﻿using InfinityEngine.Core.Object;
using InfinityEngine.Graphics.RHI;

namespace InfinityEngine.Graphics.RDG
{
    public struct FRDGContext
    {
        public FRDGObjectPool objectPool;
        public FRHICommandList commandList;
        public FRHIGraphicsContext graphicsContext;
    }

    public class FRDGGraphBuilder : UObject
    {
        public string name;

        public FRDGGraphBuilder(string Name)
        {
            name = Name;
        }

        protected override void Disposed()
        {

        }
    }
}
