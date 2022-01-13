using System;
using System.Collections.Generic;
using InfinityEngine.Core.Object;
using InfinityEngine.Graphics.RHI;
using InfinityEngine.Core.Container;

namespace InfinityEngine.Graphics.RDG
{
    public struct FRDGContext
    {
        public FRDGObjectPool objectPool;
        public FRHIContext context;
    }

    internal struct FRDGCompiledPassInfo
    {
        public IRDGPass pass;
        public int refCount;
        public int syncToPassIndex; // Index of the pass that needs to be waited for.
        public int syncFromPassIndex; // Smaller pass index that waits for this pass.
        public bool culled;
        public bool hasSideEffect;
        public bool needGraphicsFence;
        public FRHIFence fence;
        public List<int>[] resourceCreateList;
        public List<int>[] resourceReleaseList;
        public bool enablePassCulling { get { return pass.enablePassCulling; } }
        public bool enableAsyncCompute { get { return pass.enableAsyncCompute; } }

        public void Reset(IRDGPass pass)
        {
            this.pass = pass;

            if (resourceCreateList == null)
            {
                resourceCreateList = new List<int>[2];
                resourceReleaseList = new List<int>[2];
                for (int i = 0; i < 2; ++i)
                {
                    resourceCreateList[i] = new List<int>();
                    resourceReleaseList[i] = new List<int>();
                }
            }

            for (int i = 0; i < 2; ++i)
            {
                resourceCreateList[i].Clear();
                resourceReleaseList[i].Clear();
            }

            refCount = 0;
            culled = false;
            hasSideEffect = false;
            syncToPassIndex = -1;
            syncFromPassIndex = -1;
            needGraphicsFence = false;
        }
    }

    internal struct FRDGCompiledResourceInfo
    {
        public int refCount;
        public bool resourceCreated;
        public List<int> producers;
        public List<int> consumers;

        public void Reset()
        {
            if (producers == null)
                producers = new List<int>();
            if (consumers == null)
                consumers = new List<int>();

            producers.Clear();
            consumers.Clear();
            resourceCreated = false;
            refCount = 0;
        }
    }

    public class FRDGBuilder : FDisposal
    {
        public string name;
        FRDGResourceFactory m_ResourceFactory;
        List<IRDGPass> m_RenderPass = new List<IRDGPass>(64);

        bool m_ExecutionExceptionWasRaised;
        FRDGObjectPool m_ObjectPool = new FRDGObjectPool();

        Stack<int> m_CullingStack = new Stack<int>();
        TDynamicArray<FRDGCompiledPassInfo> m_PassCompileInfos;
        TDynamicArray<FRDGCompiledResourceInfo>[] m_ResourcesCompileInfos;

        public FRDGBuilder(string name)
        {
            this.name = name;
            this.m_ResourceFactory = new FRDGResourceFactory();
            this.m_PassCompileInfos = new TDynamicArray<FRDGCompiledPassInfo>();
            this.m_ResourcesCompileInfos = new TDynamicArray<FRDGCompiledResourceInfo>[2];

            for (int i = 0; i < 2; ++i)
            {
                this.m_ResourcesCompileInfos[i] = new TDynamicArray<FRDGCompiledResourceInfo>();
            }
        }

        public FRDGBufferRef ImportBuffer(FRHIBuffer buffer)
        {
            return m_ResourceFactory.ImportBuffer(buffer);
        }

        public void MoveBuffer(in FRDGBufferRef srcBuffer, in FRDGBufferRef dscBuffer)
        {

        }

        public FRDGBufferRef CreateBuffer(in FBufferDescriptor descriptor)
        {
            return m_ResourceFactory.CreateBuffer(descriptor);
        }

        public FRDGBufferRef CreateBuffer(in FRDGBufferRef bufferRef)
        {
            return m_ResourceFactory.CreateBuffer(m_ResourceFactory.GetBufferDescriptor(bufferRef.handle));
        }

        public FBufferDescriptor GetBufferDescriptor(in FRDGBufferRef bufferRef)
        {
            return m_ResourceFactory.GetBufferDescriptor(bufferRef.handle);
        }

        public FRDGTextureRef ImportTexture(FRHITexture texture, int shaderProperty = 0)
        {
            return m_ResourceFactory.ImportTexture(texture, shaderProperty);
        }

        public void MoveTexture(in FRDGTextureRef srcTexture, in FRDGTextureRef dscTexture)
        {

        }

        public FRDGTextureRef CreateTexture(in FTextureDescriptor descriptor, int shaderProperty = 0)
        {
            return m_ResourceFactory.CreateTexture(descriptor, shaderProperty);
        }

        public FRDGTextureRef CreateTexture(in FRDGTextureRef textureRef, int shaderProperty = 0)
        {
            return m_ResourceFactory.CreateTexture(m_ResourceFactory.GetTextureDescriptor(textureRef.handle), shaderProperty);
        }

        public FTextureDescriptor GetTextureDescriptor(in FRDGTextureRef textureRef)
        {
            return m_ResourceFactory.GetTextureDescriptor(textureRef.handle);
        }

        public FRDGPassRef AddPass<T>(string passName/*, ProfilingSampler profilerSampler*/) where T : struct
        {
            var renderPass = m_ObjectPool.Get<FRDGPass<T>>();
            renderPass.Clear();
            renderPass.name = passName;
            renderPass.index = m_RenderPass.Count;
            //renderPass.customSampler = profilerSampler;
            m_RenderPass.Add(renderPass);
            return new FRDGPassRef(renderPass, m_ResourceFactory);
        }

        internal void Execute(FRHIContext context)
        {
            m_ExecutionExceptionWasRaised = false;

            #region ExecuteRenderPass
            try {
                m_ResourceFactory.BeginRender();
                CompilePass();
                ExecutePass(context);
            } catch (Exception exception) {
                //Debug.LogError("Execute error");
                //if (!m_ExecutionExceptionWasRaised)
                    //Debug.LogException(exception);
                m_ExecutionExceptionWasRaised = true;
            } finally {
                ClearCompiledPass();
                m_ResourceFactory.EndRender();
            }
            #endregion //ExecuteRenderPass
        }

        internal void ClearCompiledPass()
        {
            foreach (var pass in m_RenderPass)
            {
                pass.Release(m_ObjectPool);
            }

            m_RenderPass.Clear();
            m_ResourceFactory.Clear();

            for (int i = 0; i < 2; ++i)
            {
                m_ResourcesCompileInfos[i].Clear();
            }

            m_PassCompileInfos.Clear();
        }

        void InitResourceInfosData(TDynamicArray<FRDGCompiledResourceInfo> resourceInfos, in int count)
        {
            resourceInfos.Resize(count);
            for (int i = 0; i < resourceInfos.size; ++i)
                resourceInfos[i].Reset();
        }

        void InitializeCompileData()
        {
            InitResourceInfosData(m_ResourcesCompileInfos[(int)EResourceType.Buffer], m_ResourceFactory.GetBufferResourceCount());
            InitResourceInfosData(m_ResourcesCompileInfos[(int)EResourceType.Texture], m_ResourceFactory.GetTextureResourceCount());

            m_PassCompileInfos.Resize(m_RenderPass.Count);
            for (int i = 0; i < m_PassCompileInfos.size; ++i)
                m_PassCompileInfos[i].Reset(m_RenderPass[i]);
        }

        void CountPassReference()
        {
            for (int passIndex = 0; passIndex < m_PassCompileInfos.size; ++passIndex)
            {
                ref FRDGCompiledPassInfo passInfo = ref m_PassCompileInfos[passIndex];

                for (int type = 0; type < 2; ++type)
                {
                    var resourceRead = passInfo.pass.resourceReadLists[type];
                    foreach (var resource in resourceRead)
                    {
                        ref FRDGCompiledResourceInfo info = ref m_ResourcesCompileInfos[type][resource];
                        info.consumers.Add(passIndex);
                        info.refCount++;
                    }

                    var resourceWrite = passInfo.pass.resourceWriteLists[type];
                    foreach (var resource in resourceWrite)
                    {
                        ref FRDGCompiledResourceInfo info = ref m_ResourcesCompileInfos[type][resource];
                        info.producers.Add(passIndex);
                        passInfo.refCount++;

                        // Writing to an imported texture is considered as a side effect because we don't know what users will do with it outside of render graph.
                        if (m_ResourceFactory.IsResourceImported(resource))
                            passInfo.hasSideEffect = true;
                    }

                    foreach (int resourceIndex in passInfo.pass.temporalResourceList[type])
                    {
                        ref FRDGCompiledResourceInfo info = ref m_ResourcesCompileInfos[type][resourceIndex];
                        info.refCount++;
                        info.consumers.Add(passIndex);
                        info.producers.Add(passIndex);
                    }
                }
            }
        }

        void CullingUnusedPass()
        {
            for (int type = 0; type < 2; ++type)
            {
                TDynamicArray<FRDGCompiledResourceInfo> resourceUsageList = m_ResourcesCompileInfos[type];

                // Gather resources that are never read.
                m_CullingStack.Clear();
                for (int i = 0; i < resourceUsageList.size; ++i)
                {
                    if (resourceUsageList[i].refCount == 0)
                    {
                        m_CullingStack.Push(i);
                    }
                }

                while (m_CullingStack.Count != 0)
                {
                    var unusedResource = resourceUsageList[m_CullingStack.Pop()];
                    foreach (var producerIndex in unusedResource.producers)
                    {
                        ref var producerInfo = ref m_PassCompileInfos[producerIndex];
                        producerInfo.refCount--;
                        if (producerInfo.refCount == 0 && !producerInfo.hasSideEffect && producerInfo.enablePassCulling)
                        {
                            producerInfo.culled = true;

                            foreach (var resourceIndex in producerInfo.pass.resourceReadLists[type])
                            {
                                ref FRDGCompiledResourceInfo resourceInfo = ref resourceUsageList[resourceIndex];
                                resourceInfo.refCount--;
                                // If a resource is not used anymore, add it to the stack to be processed in subsequent iteration.
                                if (resourceInfo.refCount == 0)
                                    m_CullingStack.Push(resourceIndex);
                            }
                        }
                    }
                }
            }
        }

        void UpdatePassSynchronization(ref FRDGCompiledPassInfo currentPassCompileInfo, ref FRDGCompiledPassInfo producerPassCompileInfo, in int currentPassIndex, in int lastProducer, ref int intLastSyncIndex)
        {
            // Current pass needs to wait for pass index lastProducer
            currentPassCompileInfo.syncToPassIndex = lastProducer;
            // Update latest pass waiting for the other pipe.
            intLastSyncIndex = lastProducer;

            // Producer will need a graphics fence that this pass will wait on.
            producerPassCompileInfo.needGraphicsFence = true;
            // We update the producer pass with the index of the smallest pass waiting for it.
            // This will be used to "lock" resource from being reused until the pipe has been synchronized.
            if (producerPassCompileInfo.syncFromPassIndex == -1)
                producerPassCompileInfo.syncFromPassIndex = currentPassIndex;
        }

        void UpdateResourceSynchronization(ref int lastGraphicsPipeSync, ref int lastComputePipeSync, in int currentPassIndex, in FRDGCompiledResourceInfo resourceCompileInfo)
        {
            int lastProducer = GetLatestProducerIndex(currentPassIndex, resourceCompileInfo);
            if (lastProducer != -1)
            {
                ref FRDGCompiledPassInfo currentPassInfo = ref m_PassCompileInfos[currentPassIndex];

                //If the passes are on different pipes, we need synchronization.
                if (m_PassCompileInfos[lastProducer].enableAsyncCompute != currentPassInfo.enableAsyncCompute)
                {
                    // Pass is on compute pipe, need sync with graphics pipe.
                    if (currentPassInfo.enableAsyncCompute)
                    {
                        if (lastProducer > lastGraphicsPipeSync)
                        {
                            UpdatePassSynchronization(ref currentPassInfo, ref m_PassCompileInfos[lastProducer], currentPassIndex, lastProducer, ref lastGraphicsPipeSync);
                        }
                    }
                    else
                    {
                        if (lastProducer > lastComputePipeSync)
                        {
                            UpdatePassSynchronization(ref currentPassInfo, ref m_PassCompileInfos[lastProducer], currentPassIndex, lastProducer, ref lastComputePipeSync);
                        }
                    }
                }
            }
        }

        int GetLatestProducerIndex(int passIndex, in FRDGCompiledResourceInfo resourceCompileInfo)
        {
            // We want to know the highest pass index below the current pass that writes to the resource.
            int result = -1;
            foreach (var producer in resourceCompileInfo.producers)
            {
                // producers are by construction in increasing order.
                if (producer < passIndex)
                    result = producer;
                else
                    return result;
            }

            return result;
        }

        int GetLatestValidReadIndex(in FRDGCompiledResourceInfo resourceCompileInfo)
        {
            if (resourceCompileInfo.consumers.Count == 0)
                return -1;

            var consumers = resourceCompileInfo.consumers;
            for (int i = consumers.Count - 1; i >= 0; --i)
            {
                if (!m_PassCompileInfos[consumers[i]].culled)
                    return consumers[i];
            }

            return -1;
        }

        int GetFirstValidWriteIndex(in FRDGCompiledResourceInfo resourceCompileInfo)
        {
            if (resourceCompileInfo.producers.Count == 0)
                return -1;

            var producers = resourceCompileInfo.producers;
            for (int i = 0; i < producers.Count; ++i)
            {
                if (!m_PassCompileInfos[producers[i]].culled)
                    return producers[i];
            }

            return -1;
        }

        int GetLatestValidWriteIndex(in FRDGCompiledResourceInfo resourceCompileInfo)
        {
            if (resourceCompileInfo.producers.Count == 0)
                return -1;

            var producers = resourceCompileInfo.producers;
            for (int i = producers.Count - 1; i >= 0; --i)
            {
                if (!m_PassCompileInfos[producers[i]].culled)
                    return producers[i];
            }

            return -1;
        }

        void UpdateResource()
        {
            int lastGraphicsPipeSync = -1;
            int lastComputePipeSync = -1;

            // First go through all passes.
            // - Update the last pass read index for each resource.
            // - Add texture to creation list for passes that first write to a texture.
            // - Update synchronization points for all resources between compute and graphics pipes.
            for (int passIndex = 0; passIndex < m_PassCompileInfos.size; ++passIndex)
            {
                ref FRDGCompiledPassInfo passInfo = ref m_PassCompileInfos[passIndex];

                if (passInfo.culled)
                    continue;

                for (int type = 0; type < 2; ++type)
                {
                    var resourcesInfo = m_ResourcesCompileInfos[type];
                    foreach (int resource in passInfo.pass.resourceReadLists[type])
                    {
                        UpdateResourceSynchronization(ref lastGraphicsPipeSync, ref lastComputePipeSync, passIndex, resourcesInfo[resource]);
                    }

                    foreach (int resource in passInfo.pass.resourceWriteLists[type])
                    {
                        UpdateResourceSynchronization(ref lastGraphicsPipeSync, ref lastComputePipeSync, passIndex, resourcesInfo[resource]);
                    }

                }
            }

            for (int type = 0; type < 2; ++type)
            {
                var resourceInfos = m_ResourcesCompileInfos[type];
                // Now push resources to the release list of the pass that reads it last.
                for (int i = 0; i < resourceInfos.size; ++i)
                {
                    FRDGCompiledResourceInfo resourceInfo = resourceInfos[i];

                    // Resource creation
                    int firstWriteIndex = GetFirstValidWriteIndex(resourceInfo);
                    // Index -1 can happen for imported resources (for example an imported dummy black texture will never be written to but does not need creation anyway)
                    if (firstWriteIndex != -1)
                        m_PassCompileInfos[firstWriteIndex].resourceCreateList[type].Add(i);

                    // Texture release
                    // Sometimes, a texture can be written by a pass after the last pass that reads it.
                    // In this case, we need to extend its lifetime to this pass otherwise the pass would get an invalid texture.
                    int lastReadPassIndex = Math.Max(GetLatestValidReadIndex(resourceInfo), GetLatestValidWriteIndex(resourceInfo));

                    if (lastReadPassIndex != -1)
                    {
                        // In case of async passes, we need to extend lifetime of resource to the first pass on the graphics pipeline that wait for async passes to be over.
                        // Otherwise, if we freed the resource right away during an async pass, another non async pass could reuse the resource even though the async pipe is not done.
                        if (m_PassCompileInfos[lastReadPassIndex].enableAsyncCompute)
                        {
                            int currentPassIndex = lastReadPassIndex;
                            int firstWaitingPassIndex = m_PassCompileInfos[currentPassIndex].syncFromPassIndex;
                            // Find the first async pass that is synchronized by the graphics pipeline (ie: passInfo.syncFromPassIndex != -1)
                            while (firstWaitingPassIndex == -1 && currentPassIndex < m_PassCompileInfos.size)
                            {
                                currentPassIndex++;
                                if (m_PassCompileInfos[currentPassIndex].enableAsyncCompute)
                                    firstWaitingPassIndex = m_PassCompileInfos[currentPassIndex].syncFromPassIndex;
                            }

                            // Finally add the release command to the pass before the first pass that waits for the compute pipe.
                            ref FRDGCompiledPassInfo passInfo = ref m_PassCompileInfos[Math.Max(0, firstWaitingPassIndex - 1)];
                            passInfo.resourceReleaseList[type].Add(i);

                            // Fail safe in case render graph is badly formed.
                            if (currentPassIndex == m_PassCompileInfos.size)
                            {
                                IRDGPass invalidPass = m_RenderPass[lastReadPassIndex];
                                throw new InvalidOperationException($"Asynchronous pass {invalidPass.name} was never synchronized on the graphics pipeline.");
                            }
                        } else {
                            ref FRDGCompiledPassInfo passInfo = ref m_PassCompileInfos[lastReadPassIndex];
                            passInfo.resourceReleaseList[type].Add(i);
                        }
                    }
                }
            }
        }

        internal void CompilePass()
        {
            InitializeCompileData();
            CountPassReference();
            CullingUnusedPass();
            UpdateResource();
        }

        void SetRenderTarget(in FRDGContext graphContext, in FRDGCompiledPassInfo passCompileInfo)
        {
            /*var pass = passCompileInfo.pass;
            if (pass.depthBuffer.IsValid() || pass.colorBufferMaxIndex != -1)
            {
                var mrtArray = graphContext.objectPool.GetTempArray<RenderTargetIdentifier>(pass.colorBufferMaxIndex + 1);
                var colorBuffers = pass.colorBuffers;

                if (pass.colorBufferMaxIndex > 0)
                {
                    for (int i = 0; i <= pass.colorBufferMaxIndex; ++i)
                    {
                        if (!colorBuffers[i].IsValid())
                            throw new InvalidOperationException("MRT setup is invalid. Some indices are not used.");

                        mrtArray[i] = m_ResourceFactory.GetTexture(colorBuffers[i]);
                    }

                    if (pass.depthBuffer.IsValid())
                    {
                        using (new ProfilingScope(graphContext.cmdBuffer, ProfilingSampler.Get(ERGProfileId.BindRenderTarget)))
                        {
                            CoreUtils.SetRenderTarget(graphContext.cmdBuffer, mrtArray, m_ResourceFactory.GetTexture(pass.depthBuffer));
                        }
                    } else {
                        throw new InvalidOperationException("Setting MRTs without a depth buffer is not supported.");
                    }
                } else {
                    if (pass.depthBuffer.IsValid())
                    {
                        if (pass.colorBufferMaxIndex > -1)
                        {
                            using (new ProfilingScope(graphContext.cmdBuffer, ProfilingSampler.Get(ERGProfileId.BindRenderTarget)))
                            {
                                CoreUtils.SetRenderTarget(graphContext.cmdBuffer, m_ResourceFactory.GetTexture(pass.colorBuffers[0]), m_ResourceFactory.GetTexture(pass.depthBuffer));
                            }
                        } else {
                            using (new ProfilingScope(graphContext.cmdBuffer, ProfilingSampler.Get(ERGProfileId.BindRenderTarget)))
                            {
                                CoreUtils.SetRenderTarget(graphContext.cmdBuffer, m_ResourceFactory.GetTexture(pass.depthBuffer));
                            }
                        }
                    } else {
                        using (new ProfilingScope(graphContext.cmdBuffer, ProfilingSampler.Get(ERGProfileId.BindRenderTarget)))
                        {
                            CoreUtils.SetRenderTarget(graphContext.cmdBuffer, m_ResourceFactory.GetTexture(pass.colorBuffers[0]));
                        }
                    }

                }
            }*/
        }

        void PrePassExecute(in FRDGContext graphContext, FRHICommandBuffer cmdBuffer, ref FRDGCompiledPassInfo passCompileInfo)
        {
            // TODO RENDERGRAPH merge clear and setup here if possible
            foreach (var bufferRef in passCompileInfo.resourceCreateList[(int)EResourceType.Buffer]) {
                m_ResourceFactory.CreateRealBuffer(bufferRef);
            }

            foreach (var textureRef in passCompileInfo.resourceCreateList[(int)EResourceType.Texture]) {
                m_ResourceFactory.CreateRealTexture(textureRef);
            }

            // Synchronize with graphics or compute pipe if needed.
            if (passCompileInfo.syncToPassIndex != -1) {
                graphContext.context.WaitForFence(cmdBuffer.contextType, m_PassCompileInfos[passCompileInfo.syncToPassIndex].fence);
            }

            // Auto bind render target
            SetRenderTarget(graphContext, passCompileInfo);
        }

        void PostPassExecute(in FRDGContext graphContext, FRHICommandBuffer cmdBuffer, ref FRDGCompiledPassInfo passCompileInfo)
        {
            IRDGPass pass = passCompileInfo.pass;

            // The command list has been filled. We can kick the async task.
            if (pass.enableAsyncCompute) {
                graphContext.context.ExecuteCommandBuffer(cmdBuffer);
            } else {
                graphContext.context.ExecuteCommandBuffer(cmdBuffer);
            }

            if (passCompileInfo.needGraphicsFence) {
                passCompileInfo.fence = graphContext.context.GetFence(pass.name);
                graphContext.context.WriteToFence(cmdBuffer.contextType, passCompileInfo.fence);
            }

            m_ObjectPool.ReleaseAllTempAlloc();

            foreach (var bufferRef in passCompileInfo.resourceReleaseList[(int)EResourceType.Buffer]) {
                m_ResourceFactory.ReleaseRealBuffer(bufferRef);
            }

            foreach (var textureRef in passCompileInfo.resourceReleaseList[(int)EResourceType.Texture]) {
                m_ResourceFactory.ReleaseRealTexture(textureRef);
            }

        }

        void ExecutePass(FRHIContext context)
        {
            FRDGContext graphContext;
            graphContext.objectPool = m_ObjectPool;
            graphContext.context = context;

            for (int passIndex = 0; passIndex < m_PassCompileInfos.size; ++passIndex)
            {
                ref var passInfo = ref m_PassCompileInfos[passIndex];
                if (passInfo.culled) {
                    continue;
                }

                if (!passInfo.pass.hasExecuteFunc) {
                    throw new InvalidOperationException(string.Format("RenderPass {0} was not provided with an execute function.", passInfo.pass.name));
                }

                try {
                    //using (new ProfilingScope(m_GraphContext.cmdBuffer, passInfo.pass.customSampler))
                    {
                        FRHICommandBuffer cmdBuffer = null;
                        if (passInfo.pass.enableAsyncCompute) {
                            cmdBuffer = graphContext.context.GetCommandBuffer(EContextType.Compute, passInfo.pass.name, true);
                        } else {
                            cmdBuffer = graphContext.context.GetCommandBuffer(EContextType.Graphics, passInfo.pass.name, true);
                        }

                        PrePassExecute(graphContext, cmdBuffer, ref passInfo);
                        passInfo.pass.Execute(graphContext, cmdBuffer);
                        PostPassExecute(graphContext, cmdBuffer, ref passInfo);
                    }
                } catch (Exception e) {
                    m_ExecutionExceptionWasRaised = true;
                    //Debug.LogError($"RenderGraph Execute error at pass {passInfo.pass.name} ({passIndex})");
                    //Debug.LogException(e);
                    throw;
                }
            }
        }

        protected override void Release()
        {
            m_ResourceFactory.Dispose();
        }
    }
}
