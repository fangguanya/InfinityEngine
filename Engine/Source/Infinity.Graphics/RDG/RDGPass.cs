using System;
using System.Collections.Generic;

namespace InfinityEngine.Graphics.RDG
{
    internal interface IRDGPass
    {
        public void Step(ref FRDGPassBuilder passBuilder);
        public void Execute(FRDGGraphContext graphContext);
        public void Release();
    }

    public delegate void FRDGStepFunc<T>(ref T passData, ref FRDGPassBuilder passBuilder) where T : struct;
    public delegate void FRDGExecuteFunc<T>(ref T passData, FRDGGraphContext graphContext) where T : struct;

    internal sealed class FRDGBasePass<T> : IRDGPass where T : struct
    {
        internal int index;
        internal string name;

        public int refCount;
        public int colorBufferMaxIndex;
        public bool enablePassCulling;
        public bool enableAsyncCompute;
        public FRDGTextureRef depthBuffer;
        public FRDGTextureRef[] colorBuffers;

        internal T passData;
        internal FRDGStepFunc<T> stepFunc;
        internal FRDGExecuteFunc<T> executeFunc;

        public List<FRDGResourceRef>[] resourceReadLists = new List<FRDGResourceRef>[2];
        public List<FRDGResourceRef>[] resourceWriteLists = new List<FRDGResourceRef>[2];
        public List<FRDGResourceRef>[] temporalResourceList = new List<FRDGResourceRef>[2];


        public FRDGBasePass()
        {
            colorBuffers = new FRDGTextureRef[8];
            colorBufferMaxIndex = -1;

            for (int i = 0; i < 2; ++i)
            {
                resourceReadLists[i] = new List<FRDGResourceRef>();
                resourceWriteLists[i] = new List<FRDGResourceRef>();
                temporalResourceList[i] = new List<FRDGResourceRef>();
            }
        }

        public void Step(ref FRDGPassBuilder passBuilder)
        {
            stepFunc(ref passData, ref passBuilder);
        }

        public void Execute(FRDGGraphContext graphContext)
        {
            executeFunc(ref passData, graphContext);
        }

        public void Release()
        {

        }

        public void AddResourceWrite(in FRDGResourceRef res)
        {
            resourceWriteLists[res.iType].Add(res);
        }

        public void AddResourceRead(in FRDGResourceRef res)
        {
            resourceReadLists[res.iType].Add(res);
        }

        public void AddTemporalResource(in FRDGResourceRef res)
        {
            temporalResourceList[res.iType].Add(res);
        }

        public void SetColorBuffer(in FRDGTextureRef resource, int index)
        {
            colorBufferMaxIndex = Math.Max(colorBufferMaxIndex, index);
            colorBuffers[index] = resource;
            AddResourceWrite(resource.handle);
        }

        public void SetDepthBuffer(in FRDGTextureRef resource, in EDepthAccess flags)
        {
            depthBuffer = resource;
            if ((flags & EDepthAccess.Read) != 0)
                AddResourceRead(resource.handle);
            if ((flags & EDepthAccess.Write) != 0)
                AddResourceWrite(resource.handle);
        }

        public void EnableAsyncCompute(in bool value)
        {
            enableAsyncCompute = value;
        }

        public void AllowPassCulling(in bool value)
        {
            enablePassCulling = value;
        }

        public void Clear()
        {
            name = "";
            index = -1;

            for (int i = 0; i < 2; ++i)
            {
                resourceReadLists[i].Clear();
                resourceWriteLists[i].Clear();
                temporalResourceList[i].Clear();
            }

            refCount = 0;
            enablePassCulling = true;
            enableAsyncCompute = false;

            // Invalidate everything
            colorBufferMaxIndex = -1;
            depthBuffer = new FRDGTextureRef();
            for (int i = 0; i < 8; ++i)
            {
                colorBuffers[i] = new FRDGTextureRef();
            }
        }
    }
}
