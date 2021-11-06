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
        public FRHICommandBuffer cmdBuffer;
        public FRHIGraphicsContext graphicsContext;
    }

    internal struct FCompiledPassInfo
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
        public bool allowPassCulling { get { return pass.enablePassCulling; } }
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

    internal struct FCompiledResourceInfo
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

    public class FRDGBuilder : FDisposable
    {
        public string name;
        FRDGResourceFactory m_Resources;
        FRDGResourceScope<FRDGBufferRef> m_BufferScope;
        FRDGResourceScope<FRDGTextureRef> m_TextureScope;
        List<IRDGPass> m_RenderPasses = new List<IRDGPass>(64);

        bool m_ExecutionExceptionWasRaised;
        FRDGObjectPool m_ObjectPool = new FRDGObjectPool();

        Stack<int> m_CullingStack = new Stack<int>();
        TDynamicArray<FCompiledPassInfo> m_CompiledPassInfos = new TDynamicArray<FCompiledPassInfo>();
        TDynamicArray<FCompiledResourceInfo>[] m_CompiledResourcesInfos = new TDynamicArray<FCompiledResourceInfo>[2];

        public FRDGBuilder(string name)
        {
            this.name = name;
            this.m_Resources = new FRDGResourceFactory();
            this.m_BufferScope = new FRDGResourceScope<FRDGBufferRef>();
            this.m_TextureScope = new FRDGResourceScope<FRDGTextureRef>();

            for (int i = 0; i < 2; ++i)
            {
                this.m_CompiledResourcesInfos[i] = new TDynamicArray<FCompiledResourceInfo>();
            }
        }

        protected override void Release()
        {
            m_Resources.Dispose();
            m_BufferScope.Clear();
            m_TextureScope.Clear();
        }

        public void MoveBuffer(in FRDGBufferRef srcBuffer, in FRDGBufferRef dscBuffer)
        {

        }

        public FRDGBufferRef ImportBuffer(FRHIBuffer buffer)
        {
            return m_Resources.ImportBuffer(buffer);
        }

        public FRDGBufferRef CreateBuffer(in FRHIBufferDescription bufferDesc)
        {
            return m_Resources.CreateBuffer(bufferDesc);
        }

        public FRDGBufferRef CreateBuffer(in FRDGBufferRef bufferRef)
        {
            return m_Resources.CreateBuffer(m_Resources.GetBufferResourceDesc(bufferRef.handle));
        }

        public FRHIBufferDescription GetBufferDesc(in FRDGBufferRef bufferRef)
        {
            return m_Resources.GetBufferResourceDesc(bufferRef.handle);
        }

        public FRDGBufferRef ScopeBuffer(in int handle)
        {
            return m_BufferScope.Get(handle);
        }

        public void ScopeBuffer(int handle, in FRDGBufferRef bufferRef)
        {
            m_BufferScope.Set(handle, bufferRef);
        }

        public FRDGBufferRef ScopeBuffer(in int handle, in FRHIBufferDescription bufferDesc)
        {
            FRDGBufferRef bufferRef = CreateBuffer(bufferDesc);
            m_BufferScope.Set(handle, bufferRef);
            return bufferRef;
        }

        public void MoveTexture(in FRDGTextureRef srcTexture, in FRDGTextureRef dscTexture)
        {
   
        }

        public FRDGTextureRef ImportTexture(FRHITexture texture, int shaderProperty = 0)
        {
            return m_Resources.ImportTexture(texture, shaderProperty);
        }

        public FRDGTextureRef CreateTexture(in FRHITextureDescription textureDesc, int shaderProperty = 0)
        {
            return m_Resources.CreateTexture(textureDesc, shaderProperty);
        }

        public FRDGTextureRef CreateTexture(in FRDGTextureRef textureRef, int shaderProperty = 0)
        {
            return m_Resources.CreateTexture(m_Resources.GetTextureResourceDesc(textureRef.handle), shaderProperty);
        }

        public FRDGTextureRef ScopeTexture(in int handle)
        {
            return m_TextureScope.Get(handle);
        }

        public void ScopeTexture(int handle, in FRDGTextureRef textureRef)
        {
            m_TextureScope.Set(handle, textureRef);
        }

        public FRDGTextureRef ScopeTexture(in int handle, in FRHITextureDescription textureDesc)
        {
            FRDGTextureRef textureRef = CreateTexture(textureDesc, handle);
            m_TextureScope.Set(handle, textureRef);
            return textureRef;
        }

        public FRHITextureDescription GetTextureDesc(in FRDGTextureRef textureRef)
        {
            return m_Resources.GetTextureResourceDesc(textureRef.handle);
        }

        public FRDGPassRef AddPass<T>(string passName/*, ProfilingSampler profilerSampler*/) where T : struct
        {
            var renderPass = m_ObjectPool.Get<FRDGPass<T>>();
            renderPass.Clear();
            renderPass.name = passName;
            renderPass.index = m_RenderPasses.Count;
            //renderPass.customSampler = profilerSampler;
            m_RenderPasses.Add(renderPass);
            return new FRDGPassRef(renderPass, m_Resources);
        }

        internal void Execute(FRHIGraphicsContext graphicsContext, FRDGResourceFactory resourceFactory)
        {
            m_ExecutionExceptionWasRaised = false;

            #region ExecuteRenderPass
            try
            {
                m_Resources.BeginRender();
                CompileRenderPass();
                ExecuteRenderPass(graphicsContext, resourceFactory);
            }
            catch (Exception exception)
            {
                //Debug.LogError("Execute error");
                //if (!m_ExecutionExceptionWasRaised)
                    //Debug.LogException(exception);
                m_ExecutionExceptionWasRaised = true;
            }
            finally
            {
                ClearCompiledPass();
                m_Resources.EndRender();
            }
            #endregion //ExecuteRenderPass
        }

        internal TDynamicArray<FCompiledPassInfo> GetCompiledPassInfos() 
        { 
            return m_CompiledPassInfos; 
        }

        internal void ClearCompiledPass()
        {
            ClearRenderPasses();
            m_Resources.Clear();

            for (int i = 0; i < 2; ++i)
                m_CompiledResourcesInfos[i].Clear();

            m_CompiledPassInfos.Clear();
        }

        void InitResourceInfosData(TDynamicArray<FCompiledResourceInfo> resourceInfos, int count)
        {
            resourceInfos.Resize(count);
            for (int i = 0; i < resourceInfos.size; ++i)
                resourceInfos[i].Reset();
        }

        void InitializeCompilationData()
        {
            InitResourceInfosData(m_CompiledResourcesInfos[(int)EResourceType.Buffer], m_Resources.GetBufferResourceCount());
            InitResourceInfosData(m_CompiledResourcesInfos[(int)EResourceType.Texture], m_Resources.GetTextureResourceCount());

            m_CompiledPassInfos.Resize(m_RenderPasses.Count);
            for (int i = 0; i < m_CompiledPassInfos.size; ++i)
                m_CompiledPassInfos[i].Reset(m_RenderPasses[i]);
        }

        void CountReferences()
        {
            for (int passIndex = 0; passIndex < m_CompiledPassInfos.size; ++passIndex)
            {
                ref FCompiledPassInfo passInfo = ref m_CompiledPassInfos[passIndex];

                for (int type = 0; type < 2; ++type)
                {
                    var resourceRead = passInfo.pass.resourceReadLists[type];
                    foreach (var resource in resourceRead)
                    {
                        ref FCompiledResourceInfo info = ref m_CompiledResourcesInfos[type][resource];
                        info.consumers.Add(passIndex);
                        info.refCount++;
                    }

                    var resourceWrite = passInfo.pass.resourceWriteLists[type];
                    foreach (var resource in resourceWrite)
                    {
                        ref FCompiledResourceInfo info = ref m_CompiledResourcesInfos[type][resource];
                        info.producers.Add(passIndex);
                        passInfo.refCount++;

                        // Writing to an imported texture is considered as a side effect because we don't know what users will do with it outside of render graph.
                        if (m_Resources.IsResourceImported(resource))
                            passInfo.hasSideEffect = true;
                    }

                    foreach (int resourceIndex in passInfo.pass.temporalResourceList[type])
                    {
                        ref FCompiledResourceInfo info = ref m_CompiledResourcesInfos[type][resourceIndex];
                        info.refCount++;
                        info.consumers.Add(passIndex);
                        info.producers.Add(passIndex);
                    }
                }
            }
        }

        void CulledOutputlessPasses()
        {
            m_CullingStack.Clear();
            for (int pass = 0; pass < m_CompiledPassInfos.size; ++pass)
            {
                ref FCompiledPassInfo passInfo = ref m_CompiledPassInfos[pass];

                if (passInfo.refCount == 0 && !passInfo.hasSideEffect && passInfo.allowPassCulling)
                {
                    passInfo.culled = true;
                    for (int type = 0; type < 2; ++type)
                    {
                        foreach (var index in passInfo.pass.resourceReadLists[type])
                        {
                            m_CompiledResourcesInfos[type][index].refCount--;

                        }
                    }
                }
            }
        }

        void CulledUnusedPasses()
        {
            for (int type = 0; type < 2; ++type)
            {
                TDynamicArray<FCompiledResourceInfo> resourceUsageList = m_CompiledResourcesInfos[type];

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
                        ref var producerInfo = ref m_CompiledPassInfos[producerIndex];
                        producerInfo.refCount--;
                        if (producerInfo.refCount == 0 && !producerInfo.hasSideEffect && producerInfo.allowPassCulling)
                        {
                            producerInfo.culled = true;

                            foreach (var resourceIndex in producerInfo.pass.resourceReadLists[type])
                            {
                                ref FCompiledResourceInfo resourceInfo = ref resourceUsageList[resourceIndex];
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

        void UpdatePassSynchronization(ref FCompiledPassInfo currentPassInfo, ref FCompiledPassInfo producerPassInfo, int currentPassIndex, int lastProducer, ref int intLastSyncIndex)
        {
            // Current pass needs to wait for pass index lastProducer
            currentPassInfo.syncToPassIndex = lastProducer;
            // Update latest pass waiting for the other pipe.
            intLastSyncIndex = lastProducer;

            // Producer will need a graphics fence that this pass will wait on.
            producerPassInfo.needGraphicsFence = true;
            // We update the producer pass with the index of the smallest pass waiting for it.
            // This will be used to "lock" resource from being reused until the pipe has been synchronized.
            if (producerPassInfo.syncFromPassIndex == -1)
                producerPassInfo.syncFromPassIndex = currentPassIndex;
        }

        void UpdateResourceSynchronization(ref int lastGraphicsPipeSync, ref int lastComputePipeSync, int currentPassIndex, in FCompiledResourceInfo resource)
        {
            int lastProducer = GetLatestProducerIndex(currentPassIndex, resource);
            if (lastProducer != -1)
            {
                ref FCompiledPassInfo currentPassInfo = ref m_CompiledPassInfos[currentPassIndex];

                //If the passes are on different pipes, we need synchronization.
                if (m_CompiledPassInfos[lastProducer].enableAsyncCompute != currentPassInfo.enableAsyncCompute)
                {
                    // Pass is on compute pipe, need sync with graphics pipe.
                    if (currentPassInfo.enableAsyncCompute)
                    {
                        if (lastProducer > lastGraphicsPipeSync)
                        {
                            UpdatePassSynchronization(ref currentPassInfo, ref m_CompiledPassInfos[lastProducer], currentPassIndex, lastProducer, ref lastGraphicsPipeSync);
                        }
                    }
                    else
                    {
                        if (lastProducer > lastComputePipeSync)
                        {
                            UpdatePassSynchronization(ref currentPassInfo, ref m_CompiledPassInfos[lastProducer], currentPassIndex, lastProducer, ref lastComputePipeSync);
                        }
                    }
                }
            }
        }

        int GetLatestProducerIndex(int passIndex, in FCompiledResourceInfo info)
        {
            // We want to know the highest pass index below the current pass that writes to the resource.
            int result = -1;
            foreach (var producer in info.producers)
            {
                // producers are by construction in increasing order.
                if (producer < passIndex)
                    result = producer;
                else
                    return result;
            }

            return result;
        }

        int GetLatestValidReadIndex(in FCompiledResourceInfo info)
        {
            if (info.consumers.Count == 0)
                return -1;

            var consumers = info.consumers;
            for (int i = consumers.Count - 1; i >= 0; --i)
            {
                if (!m_CompiledPassInfos[consumers[i]].culled)
                    return consumers[i];
            }

            return -1;
        }

        int GetFirstValidWriteIndex(in FCompiledResourceInfo info)
        {
            if (info.producers.Count == 0)
                return -1;

            var producers = info.producers;
            for (int i = 0; i < producers.Count; ++i)
            {
                if (!m_CompiledPassInfos[producers[i]].culled)
                    return producers[i];
            }

            return -1;
        }

        int GetLatestValidWriteIndex(in FCompiledResourceInfo info)
        {
            if (info.producers.Count == 0)
                return -1;

            var producers = info.producers;
            for (int i = producers.Count - 1; i >= 0; --i)
            {
                if (!m_CompiledPassInfos[producers[i]].culled)
                    return producers[i];
            }

            return -1;
        }


        void UpdateResourceAllocationAndSynchronization()
        {
            int lastGraphicsPipeSync = -1;
            int lastComputePipeSync = -1;

            // First go through all passes.
            // - Update the last pass read index for each resource.
            // - Add texture to creation list for passes that first write to a texture.
            // - Update synchronization points for all resources between compute and graphics pipes.
            for (int passIndex = 0; passIndex < m_CompiledPassInfos.size; ++passIndex)
            {
                ref FCompiledPassInfo passInfo = ref m_CompiledPassInfos[passIndex];

                if (passInfo.culled)
                    continue;

                for (int type = 0; type < 2; ++type)
                {
                    var resourcesInfo = m_CompiledResourcesInfos[type];
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
                var resourceInfos = m_CompiledResourcesInfos[type];
                // Now push resources to the release list of the pass that reads it last.
                for (int i = 0; i < resourceInfos.size; ++i)
                {
                    FCompiledResourceInfo resourceInfo = resourceInfos[i];

                    // Resource creation
                    int firstWriteIndex = GetFirstValidWriteIndex(resourceInfo);
                    // Index -1 can happen for imported resources (for example an imported dummy black texture will never be written to but does not need creation anyway)
                    if (firstWriteIndex != -1)
                        m_CompiledPassInfos[firstWriteIndex].resourceCreateList[type].Add(i);

                    // Texture release
                    // Sometimes, a texture can be written by a pass after the last pass that reads it.
                    // In this case, we need to extend its lifetime to this pass otherwise the pass would get an invalid texture.
                    int lastReadPassIndex = Math.Max(GetLatestValidReadIndex(resourceInfo), GetLatestValidWriteIndex(resourceInfo));

                    if (lastReadPassIndex != -1)
                    {
                        // In case of async passes, we need to extend lifetime of resource to the first pass on the graphics pipeline that wait for async passes to be over.
                        // Otherwise, if we freed the resource right away during an async pass, another non async pass could reuse the resource even though the async pipe is not done.
                        if (m_CompiledPassInfos[lastReadPassIndex].enableAsyncCompute)
                        {
                            int currentPassIndex = lastReadPassIndex;
                            int firstWaitingPassIndex = m_CompiledPassInfos[currentPassIndex].syncFromPassIndex;
                            // Find the first async pass that is synchronized by the graphics pipeline (ie: passInfo.syncFromPassIndex != -1)
                            while (firstWaitingPassIndex == -1 && currentPassIndex < m_CompiledPassInfos.size)
                            {
                                currentPassIndex++;
                                if (m_CompiledPassInfos[currentPassIndex].enableAsyncCompute)
                                    firstWaitingPassIndex = m_CompiledPassInfos[currentPassIndex].syncFromPassIndex;
                            }

                            // Finally add the release command to the pass before the first pass that waits for the compute pipe.
                            ref FCompiledPassInfo passInfo = ref m_CompiledPassInfos[Math.Max(0, firstWaitingPassIndex - 1)];
                            passInfo.resourceReleaseList[type].Add(i);

                            // Fail safe in case render graph is badly formed.
                            if (currentPassIndex == m_CompiledPassInfos.size)
                            {
                                IRDGPass invalidPass = m_RenderPasses[lastReadPassIndex];
                                throw new InvalidOperationException($"Asynchronous pass {invalidPass.name} was never synchronized on the graphics pipeline.");
                            }
                        } else {
                            ref FCompiledPassInfo passInfo = ref m_CompiledPassInfos[lastReadPassIndex];
                            passInfo.resourceReleaseList[type].Add(i);
                        }
                    }
                }
            }
        }

        internal void CompileRenderPass()
        {
            InitializeCompilationData();
            CountReferences();
            CulledUnusedPasses();
            UpdateResourceAllocationAndSynchronization();
        }

        void ExecuteRenderPass(FRHIGraphicsContext graphicsContext, FRDGResourceFactory resourceFactory)
        {
            FRDGContext graphContext;
            graphContext.cmdBuffer = null;
            graphContext.objectPool = m_ObjectPool;
            graphContext.graphicsContext = graphicsContext;

            for (int passIndex = 0; passIndex < m_CompiledPassInfos.size; ++passIndex)
            {
                ref var passInfo = ref m_CompiledPassInfos[passIndex];
                if (passInfo.culled)
                    continue;

                /*if (!passInfo.pass.HasRenderFunc())
                {
                    throw new InvalidOperationException(string.Format("RenderPass {0} was not provided with an execute function.", passInfo.pass.name));
                }*/

                try
                {
                    //using (new ProfilingScope(m_GraphContext.cmdBuffer, passInfo.pass.customSampler))
                    {
                        PreRenderPassExecute(ref graphContext, passInfo);
                        passInfo.pass.Execute(ref graphContext);
                        PostRenderPassExecute(ref graphContext, ref passInfo);
                    }
                }
                catch (Exception e)
                {
                    m_ExecutionExceptionWasRaised = true;
                    //Debug.LogError($"RenderGraph Execute error at pass {passInfo.pass.name} ({passIndex})");
                    //Debug.LogException(e);
                    throw;
                }
            }
        }

        void PreRenderPassSetRenderTargets(ref FRDGContext graphContext, in FCompiledPassInfo passInfo)
        {
            /*var pass = passInfo.pass;
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

                        mrtArray[i] = m_Resources.GetTexture(colorBuffers[i]);
                    }

                    if (pass.depthBuffer.IsValid())
                    {
                        using (new ProfilingScope(graphContext.cmdBuffer, ProfilingSampler.Get(ERGProfileId.BindRenderTarget)))
                        {
                            CoreUtils.SetRenderTarget(graphContext.cmdBuffer, mrtArray, m_Resources.GetTexture(pass.depthBuffer));
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
                                CoreUtils.SetRenderTarget(graphContext.cmdBuffer, m_Resources.GetTexture(pass.colorBuffers[0]), m_Resources.GetTexture(pass.depthBuffer));
                            }
                        } else {
                            using (new ProfilingScope(graphContext.cmdBuffer, ProfilingSampler.Get(ERGProfileId.BindRenderTarget)))
                            {
                                CoreUtils.SetRenderTarget(graphContext.cmdBuffer, m_Resources.GetTexture(pass.depthBuffer));
                            }
                        }
                    } else {
                        using (new ProfilingScope(graphContext.cmdBuffer, ProfilingSampler.Get(ERGProfileId.BindRenderTarget)))
                        {
                            CoreUtils.SetRenderTarget(graphContext.cmdBuffer, m_Resources.GetTexture(pass.colorBuffers[0]));
                        }
                    }

                }
            }*/
        }

        void PreRenderPassExecute(ref FRDGContext graphContext, in FCompiledPassInfo passInfo)
        {
            // TODO RENDERGRAPH merge clear and setup here if possible
            IRDGPass pass = passInfo.pass;

            foreach (var bufferRef in passInfo.resourceCreateList[(int)EResourceType.Buffer])
            {
                m_Resources.CreateRealBuffer(bufferRef);
            }

            foreach (var textureRef in passInfo.resourceCreateList[(int)EResourceType.Texture])
            {
                m_Resources.CreateRealTexture(textureRef);
            }

            if (pass.enableAsyncCompute)
            {
                graphContext.cmdBuffer = graphContext.graphicsContext.GetCommandBuffer(EContextType.Compute);
            } else {
                graphContext.cmdBuffer = graphContext.graphicsContext.GetCommandBuffer(EContextType.Graphics);
            }

            // Synchronize with graphics or compute pipe if needed.
            if (passInfo.syncToPassIndex != -1)
            {
                graphContext.graphicsContext.WaitFence(EContextType.Graphics, m_CompiledPassInfos[passInfo.syncToPassIndex].fence);
            }

            // Auto bind render target
            PreRenderPassSetRenderTargets(ref graphContext, passInfo);
        }

        void PostRenderPassExecute(ref FRDGContext graphContext, ref FCompiledPassInfo passInfo)
        {
            IRDGPass pass = passInfo.pass;

            if (passInfo.needGraphicsFence) { passInfo.fence = graphContext.graphicsContext.GetFence(); }

            // The command list has been filled. We can kick the async task.
            if (pass.enableAsyncCompute)
            {
                graphContext.graphicsContext.ExecuteCommandBuffer(EContextType.Compute, graphContext.cmdBuffer);
            } else {
                graphContext.graphicsContext.ExecuteCommandBuffer(EContextType.Graphics, graphContext.cmdBuffer);
            }
            
            m_ObjectPool.ReleaseAllTempAlloc();

            foreach (var bufferRef in passInfo.resourceReleaseList[(int)EResourceType.Buffer])
            {
                m_Resources.ReleaseRealBuffer(bufferRef);
            }

            foreach (var textureRef in passInfo.resourceReleaseList[(int)EResourceType.Texture])
            {
                m_Resources.ReleaseRealTexture(textureRef);
            }

        }

        void ClearRenderPasses()
        {
            foreach (var pass in m_RenderPasses)
            {
                pass.Release(m_ObjectPool);
            }
            m_BufferScope.Clear();
            m_TextureScope.Clear();
            m_RenderPasses.Clear();
        }
    }
}
