using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityEngine.Graphics.ShaderCompiler
{
    public enum EShaderStage
    {
        None = 0,
        Vertex = 1,
        Hull = 2,
        Domain = 3,
        Geometry = 4,
        Pixel = 5,
        Compute = 6,
    }

    public partial class ShaderBytecode
    {
        public EShaderStage Stage;

        public byte[] Data { get; set; }

        public ShaderBytecode()
        {

        }

        public ShaderBytecode(byte[] data)
        {
            Data = data;
        }

        public ShaderBytecode Clone()
        {
            return (ShaderBytecode)MemberwiseClone();
        }

        public static implicit operator byte[](ShaderBytecode shaderBytecode)
        {
            return shaderBytecode.Data;
        }

        public string GetDataAsString()
        {
            return Encoding.UTF8.GetString(Data, 0, Data.Length);
        }
    }

    internal class FBytecodeResult
    {
        public ShaderBytecode Bytecode { get; set; }

        public string DisassembleText { get; set; }
    }

    internal interface IShaderCompiler
    {
        FBytecodeResult Compile(string shaderSource, string entryPoint, EShaderStage stage, string sourceFilename = null);
    }
}
