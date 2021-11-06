using InfinityEngine.Core.Object;
using InfinityEngine.Core.Mathmatics;

namespace InfinityEngine.Graphics.RHI
{
    public enum EUsageType
    {
        Static = 0x1,
        Dynamic = 0x2,
        Staging = 0x4,
        Default = 0x8
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

    public class FRHIResource : FDisposable
    {
        public FRHIResource() { }
    }

    public struct FRHIBufferDescription
    {
        public string name;

        public ulong count;
        public ulong stride;
        public EUsageType flag;
        public EBufferType type;

        public FRHIBufferDescription(in ulong count, in ulong stride, in EUsageType usageFlag, in EBufferType type = EBufferType.Structured) : this()
        {
            this.type = type;
            this.flag = usageFlag;
            this.count = count;
            this.stride = stride;
        }

        public override int GetHashCode()
        {
            return new int3((int)type, count.GetHashCode(), stride.GetHashCode()).GetHashCode();
        }
    }

    public class FRHIBuffer : FRHIResource
    {
        internal FRHIBufferDescription description;

        internal FRHIBuffer(FRHIDevice device, in FRHIBufferDescription description) { }

        public virtual void SetData<T>(params T[] data) where T : struct { }
        public virtual void SetData<T>(FRHICommandList cmdList, params T[] data) where T : struct { }
        public virtual void RequestUpload<T>(FRHICommandList cmdList) where T : struct { }
        public virtual void GetData<T>(T[] data) where T : struct { }
        public virtual void GetData<T>(FRHICommandList cmdList, T[] data) where T : struct { }
        public virtual void RequestReadback<T>(FRHICommandList cmdList) where T : struct { }
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

    public struct FRHITextureDescription
    {
        public string name;

        public int width;
        public int height;
        public int slices;
        public bool sparse;
        public ushort mipLevel;
        public ushort anisoLevel;
        public EUsageType flag;
        public EMSAASample sample;
        public ETextureType type;
        public EGraphicsFormat format;

        public FRHITextureDescription(in int width, in int height, in EUsageType usageFlag, in int slices = 1, in ushort mipLevel = 1, in ushort anisoLevel = 4, in ETextureType type = ETextureType.Tex2D, in EGraphicsFormat format = EGraphicsFormat.R8G8B8A8_UNorm, in EMSAASample msaaSample = EMSAASample.None, in bool sparse = false) : this()
        {
            this.type = type;
            this.width = width;
            this.height = height;
            this.slices = slices;
            this.sparse = sparse;
            this.format = format;
            this.sample = msaaSample;
            this.flag = usageFlag;
            this.mipLevel = mipLevel;
            this.anisoLevel = anisoLevel;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            hashCode += width;
            hashCode += height;
            hashCode += slices;
            hashCode += (int)type;
            hashCode += mipLevel;
            hashCode += anisoLevel;
            return hashCode;
        }
    }

    public class FRHITexture : FRHIResource
    {
        internal FRHITextureDescription description;

        internal FRHITexture(FRHIDevice device, in FRHITextureDescription description) { }
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
}
