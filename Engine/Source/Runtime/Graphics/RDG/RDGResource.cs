using InfinityEngine.Graphics.RHI;

namespace InfinityEngine.Graphics.RDG
{
    public enum EDepthAccess
    {
        Read = 1 << 0,
        Write = 1 << 1,
        ReadWrite = Read | Write,
    }

    internal struct FRDGResourceRef
    {
        bool bValid;
        public int index { get; private set; }
        public EResourceType type { get; private set; }
        public int iType { get { return (int)type; } }


        internal FRDGResourceRef(int value, EResourceType type)
        {
            this.type = type;
            this.index = value;
            this.bValid = true;
        }

        public static implicit operator int(FRDGResourceRef handle) => handle.index;

        public bool IsValid() => bValid;
    }

    public struct FRDGBufferRef
    {
        internal FRDGResourceRef handle;


        internal FRDGBufferRef(int handle) 
        { 
            this.handle = new FRDGResourceRef(handle, EResourceType.Buffer); 
        }

        public bool IsValid() => handle.IsValid();

        public static implicit operator FRHIBuffer(FRDGBufferRef bufferRef) => bufferRef.IsValid() ? FRDGResourceFactory.current.GetBuffer(bufferRef) : null;
    }

    public struct FRDGTextureRef
    {
        internal FRDGResourceRef handle;


        internal FRDGTextureRef(int handle) 
        { 
            this.handle = new FRDGResourceRef(handle, EResourceType.Texture); 
        }

        public bool IsValid() => handle.IsValid();

        public static implicit operator FRHITexture(FRDGTextureRef textureRef) => textureRef.IsValid() ? FRDGResourceFactory.current.GetTexture(textureRef) : null;
    }

    internal class IRDGResource
    {
        public bool imported;
        public bool wasReleased;
        public int cachedHash;
        public int shaderProperty;
        public int temporalPassIndex;

        public virtual void Reset()
        {
            imported = false;
            wasReleased = false;
            cachedHash = -1;
            shaderProperty = 0;
            temporalPassIndex = -1;
        }

        public virtual string GetName()
        {
            return "";
        }
    }

    internal class FRDGResource<DescType, ResType> : IRDGResource where DescType : struct where ResType : class
    {
        public DescType desc;
        public ResType resource;

        protected FRDGResource()
        {

        }

        public override void Reset()
        {
            base.Reset();
            resource = null;
        }
    }

    internal class FRDGBuffer : FRDGResource<FRHIBufferDescription, FRHIBuffer>
    {
        public override string GetName()
        {
            return desc.name;
        }
    }

    internal class FRDGTexture : FRDGResource<FRHITextureDescription, FRHITexture>
    {
        public override string GetName()
        {
            return desc.name;
        }
    }
}
