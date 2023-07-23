using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Cache;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using Orbis.Internals;
using SharpGLES;

namespace OrbisGL
{
    public static class Shader
    {
        public static bool ExportShaderCache;

        static List<ShaderInfo> ShaderCache = null;
        
#if ORBIS
        static readonly string DefaultShaderCachePath = Path.Combine(IO.GetAppBaseDirectory(), "Shaders.bin");
#endif

        public static int GetShader(int Type, string Source)
        {
#if ORBIS
            if (GetShaderFromCache(Source, out int hProgram))
            {
                return hProgram;
            }
#endif

            if (!GLES20.HasShaderCompiler)
            {
                throw new Exception("Precompiled Shader Not Found");
            }

            int hShader = GLES20.CreateShader(Type);

            if (hShader == 0)
                throw new Exception("Failed to Create the Shader");

            GLES20.ShaderSource(hShader, Source);

            GLES20.CompileShader(hShader);

            GLES20.GetShaderiv(hShader, GLES20.GL_COMPILE_STATUS, out int Status);

            if (Status == 0)
            {
                var Info = GLES20.GetShaderInfoLog(hShader);

                GLES20.DeleteShader(hShader);

                throw new Exception("Failed to Compile the Shader: " + Info);
            }
#if ORBIS
            AddShaderToCache(Type, Source, hShader);
#endif

            return hShader;
        }

#if ORBIS
        public static int GetShader(int Type, byte[] Data)
        {
            int hShader = GLES20.CreateShader(Type);

            if (hShader == 0)
                throw new Exception("Failed to Create the Shader");

            IntPtr Buffer = Marshal.AllocHGlobal(Data.Length);
            Marshal.Copy(Data, 0, Buffer, Data.Length);

            GLES20.ShaderBinary(1, new int[] { hShader }, GLES20.PrecompiledFormat, Buffer, Data.Length);


            GLES20.GetShaderiv(hShader, GLES20.GL_COMPILE_STATUS, out int Status);


            if (Status == 0)
            {
                var Info = GLES20.GetShaderInfoLog(hShader);

                GLES20.DeleteShader(hShader);

                throw new Exception("Failed to Compile the Shader: " + Info);
            }

            Marshal.FreeHGlobal(Buffer);

            return hShader;
        }

        /// <summary>
        /// Compile all shaders embedded with the OrbisGL library
        /// and export to Shaders.bin file in the app root directory (if in a remote debug)
        /// or the /data/ directory
        /// </summary>
        /// <exception cref="Exception">The shader resource names didn't specify the shader type corretly</exception>
        public static void PrecompileShaders()
        {
#if DEBUG
            ExportShaderCache = true;
#endif
            
            var Shaders = ResLoader.ResourcesList
                .Where(x => x.EndsWith(".glsl", StringComparison.InvariantCultureIgnoreCase));
            
            var Fragments = Shaders.Where(x => x.ToLowerInvariant().Contains("frag"));
            var Vertex = Shaders.Where(x => x.ToLowerInvariant().Contains("vert"));
            
            if (Fragments.Count() + Vertex.Count() != Shaders.Count())
            {
                throw new Exception("Unable to detect all shaders type");
            }

            foreach (var Resource in Vertex)
            {
                var Source = ResLoader.GetResource(Resource);
                int hShader = GetShader(GLES20.GL_VERTEX_SHADER, Source);
                GLES20.DeleteShader(hShader);
            }
            
            foreach (var Resource in Fragments)
            {
                var Source = ResLoader.GetResource(Resource);
                int hShader = GetShader(GLES20.GL_FRAGMENT_SHADER, Source);
                GLES20.DeleteShader(hShader);
            }
        }

        public static byte[] GetShaderBinary(int hShader)
        {
            return GLES20.GetShaderBinary(hShader, out _);
        }

        public static byte[] GetShaderBinary(int Type, string Source)
        {
            var hShader = GetShader(Type, Source);

            var Data = GLES20.GetShaderBinary(hShader, out _);
            
            GLES20.DeleteShader(hShader);

            return Data;
        }

        public static int GetProgram(byte[] VertexData, byte[] FragmentData)
        {
            int hVertex = GetShader(GLES20.GL_VERTEX_SHADER, VertexData);
            int hFragment = GetShader(GLES20.GL_FRAGMENT_SHADER, FragmentData);

            return GetProgram(hVertex, hFragment);
        }

        private static byte[] GetSHA256(byte[] Data)
        {
            SHA256 SHA256 = SHA256.Create();
            return SHA256.ComputeHash(Data);
        }
        
        private static bool GetShaderFromCache(string Source, out int hProgram)
        {
            hProgram = -1;

            if (ShaderCache == null)
            {
                ShaderCache = new List<ShaderInfo>();

                if (File.Exists(DefaultShaderCachePath))
                {
                    var CacheData = File.ReadAllBytes(DefaultShaderCachePath);
                    LoadShaderCache(CacheData);
                }
                
                if (ResLoader.FindResource("Shaders.bin") != null)
                {
                    var CacheData = ResLoader.GetResourceData("Shaders.bin");
                    LoadShaderCache(CacheData);
                }
            }

            int ShaderType = -1;
            byte[] ShaderData = null;
            var ShaderHash = GetSHA256(Encoding.UTF8.GetBytes(Source));
            foreach (var Shader in ShaderCache)
            {
                if (!Shader.Hash.SequenceEqual(ShaderHash))
                    continue;

                ShaderData = Shader.Data;
                ShaderType = Shader.Type;
                break;
            }

            if (ShaderData == null)
                return false;

            hProgram = GetShader(ShaderType, ShaderData);

            return true;
        }

        private static void LoadShaderCache(byte[] CacheData)
        {
            using (MemoryStream Stream = new MemoryStream(CacheData))
            using (BinaryReader Reader = new BinaryReader(Stream))
            {
                var ShaderCount = Reader.ReadInt32();
                for (int i = 0; i < ShaderCount; i++)
                {
                    ShaderInfo Shader = new ShaderInfo();

                    Shader.Type = Reader.ReadInt32();
                    Shader.Hash = Reader.ReadBytes(0x20);
                    Shader.Data = new byte[Reader.ReadInt32()];
                    Reader.Read(Shader.Data, 0, Shader.Data.Length);

                    if (!ShaderCache.Any(x => x.Hash.SequenceEqual(Shader.Hash)))
                        ShaderCache.Add(Shader);
                }
            }
        }

        private static void AddShaderToCache(int Type, string Source, int hShader)
        {
            var Data = GetShaderBinary(hShader);
            var Hash = GetSHA256(Encoding.UTF8.GetBytes(Source));

            ShaderInfo Info = new ShaderInfo()
            {
                Data = Data,
                Hash = Hash,
                Type = Type
            };

            if (ShaderCache.Where(x => x.Hash.SequenceEqual(Hash)).Any())
            {
                throw new Exception("Shader Hash Collision: The shader is already in the cache.");
            }

            ShaderCache.Add(Info);

            if (ExportShaderCache)
            {
                Stream Output = null;
                
                try
                {
                    try
                    {
                        Output = File.Create(DefaultShaderCachePath, 1024 * 1024);
                    }
                    catch
                    {
                        Output = File.Create("/data/Shaders.bin");
                    }

                    using (BinaryWriter Writer = new BinaryWriter(Output))
                    {
                        Writer.Write(ShaderCache.Count);

                        foreach (var Shader in ShaderCache)
                        {
                            Writer.Write(Shader.Type);
                            Writer.Write(Shader.Hash, 0, 0x20);
                            Writer.Write(Shader.Data.Length);
                            Writer.Write(Shader.Data, 0, Shader.Data.Length);
                        }
                    }
                }
                finally
                {
                    Output?.Close();
                    Output?.Dispose();
                }
            }
        }

#else
        public static byte[] GetProgramBinary(string VertexSource, string FragmentSource, out int BinaryFormat)
        {
            var hVertexShader = GetShader(GLES20.GL_VERTEX_SHADER, VertexSource);
            var hFragmenetShader = GetShader(GLES20.GL_FRAGMENT_SHADER, FragmentSource);

            var hProgram = GetProgram(hVertexShader, hFragmenetShader);

            var Data = GLES20.GetShaderBinary(hProgram, out BinaryFormat);

            GLES20.DeleteProgram(hProgram);

            return Data;
        }

        public unsafe static int GetProgram(byte[] Program, int Format)
        {
            int hProgram = GLES20.CreateProgram();

            fixed (void* hData = Program) {
                GLES20.ProgramBinary(hProgram, Format, new IntPtr(hData), Program.Length);

                GLES20.GetProgramiv(hProgram, GLES20.GL_LINK_STATUS, out int Status);

                if (Status == 0)
                {
                    var Info = GLES20.GetProgramInfoLog(hProgram);

                    GLES20.DeleteProgram(hProgram);

                    throw new Exception("Failed to Initialize the GL Program: " + Info);
                }

                int LastError = State.GetLastGLError();
                if (LastError != GLES20.GL_NO_ERROR)
                {
                    throw new Exception("OpenGL Error: 0x" + LastError.ToString("X8"));
                }

                return hProgram;
            }
        }

#endif
        public static int GetProgram(string VertexSource, string FragmentSource)
        {
            int Vertex = GetShader(GLES20.GL_VERTEX_SHADER, VertexSource);
            int Fragment = GetShader(GLES20.GL_FRAGMENT_SHADER, FragmentSource);

            return GetProgram(Vertex, Fragment);
        }

        static int GetProgram(int hVertex, int hFragment)
        {
            int Program = GLES20.CreateProgram();

            GLES20.AttachShader(Program, hVertex);
            GLES20.AttachShader(Program, hFragment);

            GLES20.LinkProgram(Program);

            GLES20.DeleteShader(hVertex);
            GLES20.DeleteShader(hFragment);

            GLES20.GetProgramiv(Program, GLES20.GL_LINK_STATUS, out int Status);

            if (Status == 0)
            {
                var Info = GLES20.GetProgramInfoLog(Program);

                GLES20.DeleteProgram(Program);

                throw new Exception("Failed to Initialize the GL Program: " + Info);
            }

            int LastError = State.GetLastGLError();
            if (LastError != GLES20.GL_NO_ERROR)
            {
                throw new Exception("OpenGL Error: 0x" + LastError.ToString("X8"));
            }

            return Program;
        }
    }
}