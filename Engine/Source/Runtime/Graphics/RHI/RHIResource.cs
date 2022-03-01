using System.Collections.Generic;
using InfinityEngine.Core.Object;
using InfinityEngine.Core.Mathmatics;

namespace InfinityEngine.Graphics.RHI
{
    public enum EUsageType
    {
        Default = 0x1,
        DeptnStencil = 0x2,
        RenderTarget = 0x4,
        UnorderedAccess = 0x8
    };

    public enum EStorageType
    {
        UMA = 0x1,
        Static = 0x2,
        Dynamic = 0x4,
        Staging = 0x8,
        Default = 0x16
    };

    public enum EBufferType
    {
        Index = 0,
        Vertex = 1,
        Append = 2,
        Consume = 3,
        Counter = 4,
        Constant = 5,
        Argument = 6,
        Structured = 7,
        ByteAddress = 8
    };

    public enum ETextureType
    {
        Tex2D = 0,
        Tex2DArray = 1,
        Tex2DSparse = 2,
        TexCube = 3,
        TexCubeArray = 4,
        Tex3D = 5,
        Tex3DSparse = 6,
    };

    public enum EResourceType
    {
        Buffer = 0,
        Texture = 1
    };

    public enum EResourceState
    {
        Vertex_And_Constant_Buffer = 0x1,
        IndexBuffer = 0x2,
        RenderTarget = 0x4,
        UnorderedAccess = 0x8,
        DepthWrite = 0x10,
        DepthRead = 0x20,
        NonPixelShaderResource = 0x40,
        PixelShaderResource = 0x80,
        ShaderResource = 0x40 | 0x80,
        StreamOut = 0x100,
        IndirectArgument = 0x200,
        CopyDest = 0x400,
        CopySrc = 0x800,
        GenericRead = (((((0x1 | 0x2) | 0x40) | 0x80) | 0x200) | 0x800),
        Present = 0x1000,
        Common = 0x2000,
        AcclerationStruct = 0x4000,
        ShadingRateSource = 0x8000
    };

    public enum EDepthBits
    {
        None = 0,
        Depth8 = 8,
        Depth16 = 16,
        Depth24 = 24,
        Depth32 = 32
    }

    public enum EWrapMode
    {
        Clamp = 0,
        Border = 1,
        Repeat = 2,
        Mirror = 3,
        MirrorOnce = 4
    }

    public enum EFilterMode
    {
        Point = 0,
        Bilinear = 1,
        Trilinear = 2,
        Anisotropic = 3
    }

    public enum EMSAASample
    {
        None = 1,
        MSAA2x = 2,
        MSAA4x = 4,
        MSAA8x = 8
    }

    public enum EGraphicsFormat
    {
        None = 0,
        R8_SRGB = 1,
        R8G8_SRGB = 2,
        R8G8B8_SRGB = 3,
        R8G8B8A8_SRGB = 4,
        R8_UNorm = 5,
        R8G8_UNorm = 6,
        R8G8B8_UNorm = 7,
        R8G8B8A8_UNorm = 8,
        R8_SNorm = 9,
        R8G8_SNorm = 10,
        R8G8B8_SNorm = 11,
        R8G8B8A8_SNorm = 12,
        R8_UInt = 13,
        R8G8_UInt = 14,
        R8G8B8_UInt = 15,
        R8G8B8A8_UInt = 16,
        R8_SInt = 17,
        R8G8_SInt = 18,
        R8G8B8_SInt = 19,
        R8G8B8A8_SInt = 20,
        R16_UNorm = 21,
        R16G16_UNorm = 22,
        R16G16B16_UNorm = 23,
        R16G16B16A16_UNorm = 24,
        R16_SNorm = 25,
        R16G16_SNorm = 26,
        R16G16B16_SNorm = 27,
        R16G16B16A16_SNorm = 28,
        R16_UInt = 29,
        R16G16_UInt = 30,
        R16G16B16_UInt = 31,
        R16G16B16A16_UInt = 32,
        R16_SInt = 33,
        R16G16_SInt = 34,
        R16G16B16_SInt = 35,
        R16G16B16A16_SInt = 36,
        R32_UInt = 37,
        R32G32_UInt = 38,
        R32G32B32_UInt = 39,
        R32G32B32A32_UInt = 40,
        R32_SInt = 41,
        R32G32_SInt = 42,
        R32G32B32_SInt = 43,
        R32G32B32A32_SInt = 44,
        R16_SFloat = 45,
        R16G16_SFloat = 46,
        R16G16B16_SFloat = 47,
        R16G16B16A16_SFloat = 48,
        R32_SFloat = 49,
        R32G32_SFloat = 50,
        R32G32B32_SFloat = 51,
        R32G32B32A32_SFloat = 52,
        B8G8R8_SRGB = 56,
        B8G8R8A8_SRGB = 57,
        B8G8R8_UNorm = 58,
        B8G8R8A8_UNorm = 59,
        B8G8R8_SNorm = 60,
        B8G8R8A8_SNorm = 61,
        B8G8R8_UInt = 62,
        B8G8R8A8_UInt = 63,
        B8G8R8_SInt = 64,
        B8G8R8A8_SInt = 65,
        R4G4B4A4_UNormPack16 = 66,
        B4G4R4A4_UNormPack16 = 67,
        R5G6B5_UNormPack16 = 68,
        B5G6R5_UNormPack16 = 69,
        R5G5B5A1_UNormPack16 = 70,
        B5G5R5A1_UNormPack16 = 71,
        A1R5G5B5_UNormPack16 = 72,
        E5B9G9R9_UFloatPack32 = 73,
        B10G11R11_UFloatPack32 = 74,
        A2B10G10R10_UNormPack32 = 75,
        A2B10G10R10_UIntPack32 = 76,
        A2B10G10R10_SIntPack32 = 77,
        A2R10G10B10_UNormPack32 = 78,
        A2R10G10B10_UIntPack32 = 79,
        A2R10G10B10_SIntPack32 = 80,
        RGB_DXT1_SRGB = 96,
        RGBA_DXT1_SRGB = 96,
        RGB_DXT1_UNorm = 97,
        RGBA_DXT1_UNorm = 97,
        RGBA_DXT3_SRGB = 98,
        RGBA_DXT3_UNorm = 99,
        RGBA_DXT5_SRGB = 100,
        RGBA_DXT5_UNorm = 101,
        R_BC4_UNorm = 102,
        R_BC4_SNorm = 103,
        RG_BC5_UNorm = 104,
        RG_BC5_SNorm = 105,
        RGB_BC6H_UFloat = 106,
        RGB_BC6H_SFloat = 107,
        RGBA_BC7_SRGB = 108,
        RGBA_BC7_UNorm = 109,
    }

    public struct FResourceBarrierInfo
    {

    }

    public class FRHIResource : FDisposal
    {
        public FRHIResource() { }
    }

    public struct FBufferDescriptor
    {
        public string name;

        public ulong count;
        public ulong stride;
        public EUsageType usageType;
        public EBufferType bufferType;
        public EStorageType storageType;

        public FBufferDescriptor(in int count, in int stride, in EUsageType usageType, in EStorageType storageType, in EBufferType bufferType = EBufferType.Structured)
        {
            this.name = null;
            this.count = (ulong)count;
            this.stride = (ulong)stride;
            this.usageType = usageType;
            this.bufferType = bufferType;
            this.storageType = storageType;
        }

        public override int GetHashCode()
        {
            return new int3((int)bufferType, count.GetHashCode(), stride.GetHashCode()).GetHashCode();
        }
    }

    public abstract class FRHIBuffer : FRHIResource
    {
        internal FBufferDescriptor descriptor;

        internal FRHIBuffer() { }
        internal FRHIBuffer(FRHIDevice device, in FBufferDescriptor descriptor) { }
        public abstract void SetData<T>(params T[] data) where T : struct;
        public abstract void Upload(FRHICommandBuffer cmdBuffer);
        public abstract void SetData<T>(FRHICommandBuffer cmdBuffer, params T[] data) where T : struct;
        public abstract void GetData<T>(T[] data) where T : struct;
        public abstract void Readback(FRHICommandBuffer cmdBuffer);
        public abstract void GetData<T>(FRHICommandBuffer cmdBuffer, T[] data) where T : struct;
    }

    public struct FRHIBufferRef
    {
        internal int handle;
        public FRHIBuffer buffer;

        internal FRHIBufferRef(int handle, FRHIBuffer buffer) 
        { 
           this.handle = handle; 
           this.buffer = buffer; 
        }
    }

    public struct FTextureDescriptor
    {
        public string name;

        public int width;
        public int height;
        public int slices;
        public bool sparse;
        public ushort mipLevel;
        public ushort anisoLevel;
        public EMSAASample sample;
        public EUsageType usageType;
        public ETextureType textureType;
        public EStorageType storageType;
        public EGraphicsFormat format;

        public FTextureDescriptor(in int width, in int height, in EUsageType usageType, in EStorageType storageType, in int slices = 1, in ushort mipLevel = 1, in ushort anisoLevel = 4, in ETextureType textureType = ETextureType.Tex2D, in EGraphicsFormat format = EGraphicsFormat.R8G8B8A8_UNorm, in EMSAASample sample = EMSAASample.None, in bool sparse = false)
        {
            this.name = null;
            this.width = width;
            this.height = height;
            this.slices = slices;
            this.sparse = sparse;
            this.format = format;
            this.sample = sample;
            this.mipLevel = mipLevel;
            this.usageType = usageType;
            this.anisoLevel = anisoLevel;
            this.textureType = textureType;
            this.storageType = storageType;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            hashCode += width;
            hashCode += height;
            hashCode += slices;
            hashCode += (int)textureType;
            hashCode += mipLevel;
            hashCode += anisoLevel;
            return hashCode;
        }
    }

    public class FRHITexture : FRHIResource
    {
        internal FTextureDescriptor descriptor;

        internal FRHITexture() { }
        internal FRHITexture(FRHIDevice device, in FTextureDescriptor descriptor) { }
    }

    public struct FRHITextureRef
    {
        internal int handle;
        public FRHITexture texture;

        internal FRHITextureRef(int handle, FRHITexture texture) 
        { 
            this.handle = handle; 
            this.texture = texture; 
        }
    }

    internal abstract class FRHIResourceCache<Type> where Type : class
    {
        protected Dictionary<int, List<Type>> m_ResourcePool = new Dictionary<int, List<Type>>(64);
        abstract protected void ReleaseInternalResource(Type res);
        abstract protected string GetResourceName(Type res);
        abstract protected string GetResourceTypeName();

        public bool Pull(in int hashCode, out Type resource)
        {
            if (m_ResourcePool.TryGetValue(hashCode, out var list) && list.Count > 0)
            {
                /*resource = list[0];
                list.RemoveAt(0);*/
                resource = list[list.Count - 1];
                list.RemoveAt(list.Count - 1);
                return true;
            }

            resource = null;
            return false;
        }

        public void Push(in int hash, Type resource)
        {
            if (!m_ResourcePool.TryGetValue(hash, out var list))
            {
                list = new List<Type>();
                m_ResourcePool.Add(hash, list);
            }

            list.Add(resource);
        }

        public void Dispose()
        {
            foreach (var kvp in m_ResourcePool)
            {
                foreach (Type resource in kvp.Value)
                {
                    ReleaseInternalResource(resource);
                }
            }
        }
    }

    internal class FRHIBufferCache : FRHIResourceCache<FRHIBuffer>
    {
        protected override void ReleaseInternalResource(FRHIBuffer buffer)
        {
            buffer.Dispose();
        }

        protected override string GetResourceName(FRHIBuffer buffer)
        {
            return buffer.descriptor.name;
        }

        override protected string GetResourceTypeName()
        {
            return "Buffer";
        }
    }

    internal class FRHITextureCache : FRHIResourceCache<FRHITexture>
    {
        protected override void ReleaseInternalResource(FRHITexture texture)
        {
            texture.Dispose();
        }

        protected override string GetResourceName(FRHITexture texture)
        {
            return texture.descriptor.name;
        }

        override protected string GetResourceTypeName()
        {
            return "Texture";
        }
    }

    public class FRHIResourcePool
    {
        FRHIBufferCache m_BufferPool;
        FRHITextureCache m_TexturePool;
        FRHIContext m_Context;

        internal FRHIResourcePool(FRHIContext context)
        {
            m_Context = context;
            m_BufferPool = new FRHIBufferCache();
            m_TexturePool = new FRHITextureCache();
        }

        public FRHIBufferRef GetBuffer(in FBufferDescriptor descriptor)
        {
            FRHIBuffer buffer;
            int handle = descriptor.GetHashCode();

            if (!m_BufferPool.Pull(handle, out buffer))
            {
                buffer = m_Context.CreateBuffer(descriptor);
            }

            return new FRHIBufferRef(handle, buffer);
        }

        public void ReleaseBuffer(in FRHIBufferRef bufferRef)
        {
            m_BufferPool.Push(bufferRef.handle, bufferRef.buffer);
        }

        public FRHITextureRef GetTexture(in FTextureDescriptor descriptor)
        {
            FRHITexture texture;
            int handle = descriptor.GetHashCode();

            if (!m_TexturePool.Pull(handle, out texture))
            {
                texture = m_Context.CreateTexture(descriptor);
            }

            return new FRHITextureRef(handle, texture);
        }

        public void ReleaseTexture(in FRHITextureRef textureRef)
        {
            m_TexturePool.Push(textureRef.handle, textureRef.texture);
        }

        public void Dispose()
        {
            m_BufferPool.Dispose();
            m_TexturePool.Dispose();
            m_Context = null;
        }
    }
}
