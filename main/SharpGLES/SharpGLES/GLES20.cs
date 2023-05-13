using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpGLES
{
	public static class GLES20
	{
#if ORBIS
		public static bool HasShaderCompiler { get; internal set; } = false;
#else
        public static bool HasShaderCompiler { get; internal set; } = true;
#endif

        const string Path = @"libGLESv2.dll";

		public const int GL_ES_VERSION_2_0 = 1;
		public const int GL_DEPTH_BUFFER_BIT = 0x00000100;
		public const int GL_STENCIL_BUFFER_BIT = 0x00000400;
		public const int GL_COLOR_BUFFER_BIT = 0x00004000;
		public const int GL_FALSE = 0;
		public const int GL_TRUE = 1;
		public const int GL_POINTS = 0x0000;
		public const int GL_LINES = 0x0001;
		public const int GL_LINE_LOOP = 0x0002;
		public const int GL_LINE_STRIP = 0x0003;
		public const int GL_TRIANGLES = 0x0004;
		public const int GL_TRIANGLE_STRIP = 0x0005;
		public const int GL_TRIANGLE_FAN = 0x0006;
		public const int GL_ZERO = 0;
		public const int GL_ONE = 1;
		public const int GL_SRC_COLOR = 0x0300;
		public const int GL_ONE_MINUS_SRC_COLOR = 0x0301;
		public const int GL_SRC_ALPHA = 0x0302;
		public const int GL_ONE_MINUS_SRC_ALPHA = 0x0303;
		public const int GL_DST_ALPHA = 0x0304;
		public const int GL_ONE_MINUS_DST_ALPHA = 0x0305;
		public const int GL_DST_COLOR = 0x0306;
		public const int GL_ONE_MINUS_DST_COLOR = 0x0307;
		public const int GL_SRC_ALPHA_SATURATE = 0x0308;
		public const int GL_FUNC_ADD = 0x8006;
		public const int GL_BLEND_EQUATION = 0x8009;
		public const int GL_BLEND_EQUATION_RGB = 0x8009;
		public const int GL_BLEND_EQUATION_ALPHA = 0x883D;
		public const int GL_FUNC_SUBTRACT = 0x800A;
		public const int GL_FUNC_REVERSE_SUBTRACT = 0x800B;
		public const int GL_BLEND_DST_RGB = 0x80C8;
		public const int GL_BLEND_SRC_RGB = 0x80C9;
		public const int GL_BLEND_DST_ALPHA = 0x80CA;
		public const int GL_BLEND_SRC_ALPHA = 0x80CB;
		public const int GL_CONSTANT_COLOR = 0x8001;
		public const int GL_ONE_MINUS_CONSTANT_COLOR = 0x8002;
		public const int GL_CONSTANT_ALPHA = 0x8003;
		public const int GL_ONE_MINUS_CONSTANT_ALPHA = 0x8004;
		public const int GL_BLEND_COLOR = 0x8005;
		public const int GL_ARRAY_BUFFER = 0x8892;
		public const int GL_ELEMENT_ARRAY_BUFFER = 0x8893;
		public const int GL_ARRAY_BUFFER_BINDING = 0x8894;
		public const int GL_ELEMENT_ARRAY_BUFFER_BINDING = 0x8895;
		public const int GL_STREAM_DRAW = 0x88E0;
		public const int GL_STATIC_DRAW = 0x88E4;
		public const int GL_DYNAMIC_DRAW = 0x88E8;
		public const int GL_BUFFER_SIZE = 0x8764;
		public const int GL_BUFFER_USAGE = 0x8765;
		public const int GL_CURRENT_VERTEX_ATTRIB = 0x8626;
		public const int GL_FRONT = 0x0404;
		public const int GL_BACK = 0x0405;
		public const int GL_FRONT_AND_BACK = 0x0408;
		public const int GL_TEXTURE_2D = 0x0DE1;
		public const int GL_CULL_FACE = 0x0B44;
		public const int GL_BLEND = 0x0BE2;
		public const int GL_DITHER = 0x0BD0;
		public const int GL_STENCIL_TEST = 0x0B90;
		public const int GL_DEPTH_TEST = 0x0B71;
		public const int GL_SCISSOR_TEST = 0x0C11;
		public const int GL_POLYGON_OFFSET_FILL = 0x8037;
		public const int GL_SAMPLE_ALPHA_TO_COVERAGE = 0x809E;
		public const int GL_SAMPLE_COVERAGE = 0x80A0;
		public const int GL_NO_ERROR = 0;
		public const int GL_INVALID_ENUM = 0x0500;
		public const int GL_INVALID_VALUE = 0x0501;
		public const int GL_INVALID_OPERATION = 0x0502;
		public const int GL_OUT_OF_MEMORY = 0x0505;
		public const int GL_CW = 0x0900;
		public const int GL_CCW = 0x0901;
		public const int GL_LINE_WIDTH = 0x0B21;
		public const int GL_ALIASED_POINT_SIZE_RANGE = 0x846D;
		public const int GL_ALIASED_LINE_WIDTH_RANGE = 0x846E;
		public const int GL_CULL_FACE_MODE = 0x0B45;
		public const int GL_FRONT_FACE = 0x0B46;
		public const int GL_DEPTH_RANGE = 0x0B70;
		public const int GL_DEPTH_WRITEMASK = 0x0B72;
		public const int GL_DEPTH_CLEAR_VALUE = 0x0B73;
		public const int GL_DEPTH_FUNC = 0x0B74;
		public const int GL_STENCIL_CLEAR_VALUE = 0x0B91;
		public const int GL_STENCIL_FUNC = 0x0B92;
		public const int GL_STENCIL_FAIL = 0x0B94;
		public const int GL_STENCIL_PASS_DEPTH_FAIL = 0x0B95;
		public const int GL_STENCIL_PASS_DEPTH_PASS = 0x0B96;
		public const int GL_STENCIL_REF = 0x0B97;
		public const int GL_STENCIL_VALUE_MASK = 0x0B93;
		public const int GL_STENCIL_WRITEMASK = 0x0B98;
		public const int GL_STENCIL_BACK_FUNC = 0x8800;
		public const int GL_STENCIL_BACK_FAIL = 0x8801;
		public const int GL_STENCIL_BACK_PASS_DEPTH_FAIL = 0x8802;
		public const int GL_STENCIL_BACK_PASS_DEPTH_PASS = 0x8803;
		public const int GL_STENCIL_BACK_REF = 0x8CA3;
		public const int GL_STENCIL_BACK_VALUE_MASK = 0x8CA4;
		public const int GL_STENCIL_BACK_WRITEMASK = 0x8CA5;
		public const int GL_VIEWPORT = 0x0BA2;
		public const int GL_SCISSOR_BOX = 0x0C10;
		public const int GL_COLOR_CLEAR_VALUE = 0x0C22;
		public const int GL_COLOR_WRITEMASK = 0x0C23;
		public const int GL_UNPACK_ALIGNMENT = 0x0CF5;
		public const int GL_PACK_ALIGNMENT = 0x0D05;
		public const int GL_MAX_TEXTURE_SIZE = 0x0D33;
		public const int GL_MAX_VIEWPORT_DIMS = 0x0D3A;
		public const int GL_SUBPIXEL_BITS = 0x0D50;
		public const int GL_RED_BITS = 0x0D52;
		public const int GL_GREEN_BITS = 0x0D53;
		public const int GL_BLUE_BITS = 0x0D54;
		public const int GL_ALPHA_BITS = 0x0D55;
		public const int GL_DEPTH_BITS = 0x0D56;
		public const int GL_STENCIL_BITS = 0x0D57;
		public const int GL_POLYGON_OFFSET_UNITS = 0x2A00;
		public const int GL_POLYGON_OFFSET_FACTOR = 0x8038;
		public const int GL_TEXTURE_BINDING_2D = 0x8069;
		public const int GL_SAMPLE_BUFFERS = 0x80A8;
		public const int GL_SAMPLES = 0x80A9;
		public const int GL_SAMPLE_COVERAGE_VALUE = 0x80AA;
		public const int GL_SAMPLE_COVERAGE_INVERT = 0x80AB;
		public const int GL_NUM_COMPRESSED_TEXTURE_FORMATS = 0x86A2;
		public const int GL_COMPRESSED_TEXTURE_FORMATS = 0x86A3;
		public const int GL_DONT_CARE = 0x1100;
		public const int GL_FASTEST = 0x1101;
		public const int GL_NICEST = 0x1102;
		public const int GL_GENERATE_MIPMAP_HINT = 0x8192;
		public const int GL_BYTE = 0x1400;
		public const int GL_UNSIGNED_BYTE = 0x1401;
		public const int GL_SHORT = 0x1402;
		public const int GL_UNSIGNED_SHORT = 0x1403;
		public const int GL_INT = 0x1404;
		public const int GL_UNSIGNED_INT = 0x1405;
		public const int GL_FLOAT = 0x1406;
		public const int GL_FIXED = 0x140C;
		public const int GL_DEPTH_COMPONENT = 0x1902;
		public const int GL_ALPHA = 0x1906;
		public const int GL_RGB = 0x1907;
		public const int GL_RGBA = 0x1908;
		public const int GL_LUMINANCE = 0x1909;
		public const int GL_LUMINANCE_ALPHA = 0x190A;
		public const int GL_UNSIGNED_SHORT_4_4_4_4 = 0x8033;
		public const int GL_UNSIGNED_SHORT_5_5_5_1 = 0x8034;
		public const int GL_UNSIGNED_SHORT_5_6_5 = 0x8363;
		public const int GL_FRAGMENT_SHADER = 0x8B30;
		public const int GL_VERTEX_SHADER = 0x8B31;
		public const int GL_MAX_VERTEX_ATTRIBS = 0x8869;
		public const int GL_MAX_VERTEX_UNIFORM_VECTORS = 0x8DFB;
		public const int GL_MAX_VARYING_VECTORS = 0x8DFC;
		public const int GL_MAX_COMBINED_TEXTURE_IMAGE_UNITS = 0x8B4D;
		public const int GL_MAX_VERTEX_TEXTURE_IMAGE_UNITS = 0x8B4C;
		public const int GL_MAX_TEXTURE_IMAGE_UNITS = 0x8872;
		public const int GL_MAX_FRAGMENT_UNIFORM_VECTORS = 0x8DFD;
		public const int GL_SHADER_TYPE = 0x8B4F;
		public const int GL_DELETE_STATUS = 0x8B80;
		public const int GL_LINK_STATUS = 0x8B82;
		public const int GL_VALIDATE_STATUS = 0x8B83;
		public const int GL_ATTACHED_SHADERS = 0x8B85;
		public const int GL_ACTIVE_UNIFORMS = 0x8B86;
		public const int GL_ACTIVE_UNIFORM_MAX_LENGTH = 0x8B87;
		public const int GL_ACTIVE_ATTRIBUTES = 0x8B89;
		public const int GL_ACTIVE_ATTRIBUTE_MAX_LENGTH = 0x8B8A;
		public const int GL_SHADING_LANGUAGE_VERSION = 0x8B8C;
		public const int GL_CURRENT_PROGRAM = 0x8B8D;
		public const int GL_NEVER = 0x0200;
		public const int GL_LESS = 0x0201;
		public const int GL_EQUAL = 0x0202;
		public const int GL_LEQUAL = 0x0203;
		public const int GL_GREATER = 0x0204;
		public const int GL_NOTEQUAL = 0x0205;
		public const int GL_GEQUAL = 0x0206;
		public const int GL_ALWAYS = 0x0207;
		public const int GL_KEEP = 0x1E00;
		public const int GL_REPLACE = 0x1E01;
		public const int GL_INCR = 0x1E02;
		public const int GL_DECR = 0x1E03;
		public const int GL_INVERT = 0x150A;
		public const int GL_INCR_WRAP = 0x8507;
		public const int GL_DECR_WRAP = 0x8508;
		public const int GL_VENDOR = 0x1F00;
		public const int GL_RENDERER = 0x1F01;
		public const int GL_VERSION = 0x1F02;
		public const int GL_EXTENSIONS = 0x1F03;
		public const int GL_NEAREST = 0x2600;
		public const int GL_LINEAR = 0x2601;
		public const int GL_NEAREST_MIPMAP_NEAREST = 0x2700;
		public const int GL_LINEAR_MIPMAP_NEAREST = 0x2701;
		public const int GL_NEAREST_MIPMAP_LINEAR = 0x2702;
		public const int GL_LINEAR_MIPMAP_LINEAR = 0x2703;
		public const int GL_TEXTURE_MAG_FILTER = 0x2800;
		public const int GL_TEXTURE_MIN_FILTER = 0x2801;
		public const int GL_TEXTURE_WRAP_S = 0x2802;
		public const int GL_TEXTURE_WRAP_T = 0x2803;
		public const int GL_TEXTURE = 0x1702;
		public const int GL_TEXTURE_CUBE_MAP = 0x8513;
		public const int GL_TEXTURE_BINDING_CUBE_MAP = 0x8514;
		public const int GL_TEXTURE_CUBE_MAP_POSITIVE_X = 0x8515;
		public const int GL_TEXTURE_CUBE_MAP_NEGATIVE_X = 0x8516;
		public const int GL_TEXTURE_CUBE_MAP_POSITIVE_Y = 0x8517;
		public const int GL_TEXTURE_CUBE_MAP_NEGATIVE_Y = 0x8518;
		public const int GL_TEXTURE_CUBE_MAP_POSITIVE_Z = 0x8519;
		public const int GL_TEXTURE_CUBE_MAP_NEGATIVE_Z = 0x851A;
		public const int GL_MAX_CUBE_MAP_TEXTURE_SIZE = 0x851C;
		public const int GL_TEXTURE0 = 0x84C0;
		public const int GL_TEXTURE1 = 0x84C1;
		public const int GL_TEXTURE2 = 0x84C2;
		public const int GL_TEXTURE3 = 0x84C3;
		public const int GL_TEXTURE4 = 0x84C4;
		public const int GL_TEXTURE5 = 0x84C5;
		public const int GL_TEXTURE6 = 0x84C6;
		public const int GL_TEXTURE7 = 0x84C7;
		public const int GL_TEXTURE8 = 0x84C8;
		public const int GL_TEXTURE9 = 0x84C9;
		public const int GL_TEXTURE10 = 0x84CA;
		public const int GL_TEXTURE11 = 0x84CB;
		public const int GL_TEXTURE12 = 0x84CC;
		public const int GL_TEXTURE13 = 0x84CD;
		public const int GL_TEXTURE14 = 0x84CE;
		public const int GL_TEXTURE15 = 0x84CF;
		public const int GL_TEXTURE16 = 0x84D0;
		public const int GL_TEXTURE17 = 0x84D1;
		public const int GL_TEXTURE18 = 0x84D2;
		public const int GL_TEXTURE19 = 0x84D3;
		public const int GL_TEXTURE20 = 0x84D4;
		public const int GL_TEXTURE21 = 0x84D5;
		public const int GL_TEXTURE22 = 0x84D6;
		public const int GL_TEXTURE23 = 0x84D7;
		public const int GL_TEXTURE24 = 0x84D8;
		public const int GL_TEXTURE25 = 0x84D9;
		public const int GL_TEXTURE26 = 0x84DA;
		public const int GL_TEXTURE27 = 0x84DB;
		public const int GL_TEXTURE28 = 0x84DC;
		public const int GL_TEXTURE29 = 0x84DD;
		public const int GL_TEXTURE30 = 0x84DE;
		public const int GL_TEXTURE31 = 0x84DF;
		public const int GL_ACTIVE_TEXTURE = 0x84E0;
		public const int GL_REPEAT = 0x2901;
		public const int GL_CLAMP_TO_EDGE = 0x812F;
		public const int GL_MIRRORED_REPEAT = 0x8370;
		public const int GL_FLOAT_VEC2 = 0x8B50;
		public const int GL_FLOAT_VEC3 = 0x8B51;
		public const int GL_FLOAT_VEC4 = 0x8B52;
		public const int GL_INT_VEC2 = 0x8B53;
		public const int GL_INT_VEC3 = 0x8B54;
		public const int GL_INT_VEC4 = 0x8B55;
		public const int GL_BOOL = 0x8B56;
		public const int GL_BOOL_VEC2 = 0x8B57;
		public const int GL_BOOL_VEC3 = 0x8B58;
		public const int GL_BOOL_VEC4 = 0x8B59;
		public const int GL_FLOAT_MAT2 = 0x8B5A;
		public const int GL_FLOAT_MAT3 = 0x8B5B;
		public const int GL_FLOAT_MAT4 = 0x8B5C;
		public const int GL_SAMPLER_2D = 0x8B5E;
		public const int GL_SAMPLER_CUBE = 0x8B60;
		public const int GL_VERTEX_ATTRIB_ARRAY_ENABLED = 0x8622;
		public const int GL_VERTEX_ATTRIB_ARRAY_SIZE = 0x8623;
		public const int GL_VERTEX_ATTRIB_ARRAY_STRIDE = 0x8624;
		public const int GL_VERTEX_ATTRIB_ARRAY_TYPE = 0x8625;
		public const int GL_VERTEX_ATTRIB_ARRAY_NORMALIZED = 0x886A;
		public const int GL_VERTEX_ATTRIB_ARRAY_POINTER = 0x8645;
		public const int GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING = 0x889F;
		public const int GL_IMPLEMENTATION_COLOR_READ_TYPE = 0x8B9A;
		public const int GL_IMPLEMENTATION_COLOR_READ_FORMAT = 0x8B9B;
		public const int GL_COMPILE_STATUS = 0x8B81;
		public const int GL_INFO_LOG_LENGTH = 0x8B84;
		public const int GL_SHADER_SOURCE_LENGTH = 0x8B88;
		public const int GL_SHADER_COMPILER = 0x8DFA;
		public const int GL_SHADER_BINARY_FORMATS = 0x8DF8;
		public const int GL_NUM_SHADER_BINARY_FORMATS = 0x8DF9;
		public const int GL_LOW_FLOAT = 0x8DF0;
		public const int GL_MEDIUM_FLOAT = 0x8DF1;
		public const int GL_HIGH_FLOAT = 0x8DF2;
		public const int GL_LOW_INT = 0x8DF3;
		public const int GL_MEDIUM_INT = 0x8DF4;
		public const int GL_HIGH_INT = 0x8DF5;
		public const int GL_FRAMEBUFFER = 0x8D40;
		public const int GL_RENDERBUFFER = 0x8D41;
		public const int GL_RGBA4 = 0x8056;
		public const int GL_RGB5_A1 = 0x8057;
		public const int GL_RGB565 = 0x8D62;
		public const int GL_DEPTH_COMPONENT16 = 0x81A5;
		public const int GL_STENCIL_INDEX8 = 0x8D48;
		public const int GL_RENDERBUFFER_WIDTH = 0x8D42;
		public const int GL_RENDERBUFFER_HEIGHT = 0x8D43;
		public const int GL_RENDERBUFFER_INTERNAL_FORMAT = 0x8D44;
		public const int GL_RENDERBUFFER_RED_SIZE = 0x8D50;
		public const int GL_RENDERBUFFER_GREEN_SIZE = 0x8D51;
		public const int GL_RENDERBUFFER_BLUE_SIZE = 0x8D52;
		public const int GL_RENDERBUFFER_ALPHA_SIZE = 0x8D53;
		public const int GL_RENDERBUFFER_DEPTH_SIZE = 0x8D54;
		public const int GL_RENDERBUFFER_STENCIL_SIZE = 0x8D55;
		public const int GL_FRAMEBUFFER_ATTACHMENT_OBJECT_TYPE = 0x8CD0;
		public const int GL_FRAMEBUFFER_ATTACHMENT_OBJECT_NAME = 0x8CD1;
		public const int GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_LEVEL = 0x8CD2;
		public const int GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_CUBE_MAP_FACE = 0x8CD3;
		public const int GL_COLOR_ATTACHMENT0 = 0x8CE0;
		public const int GL_DEPTH_ATTACHMENT = 0x8D00;
		public const int GL_STENCIL_ATTACHMENT = 0x8D20;
		public const int GL_NONE = 0;
		public const int GL_FRAMEBUFFER_COMPLETE = 0x8CD5;
		public const int GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENT = 0x8CD6;
		public const int GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT = 0x8CD7;
		public const int GL_FRAMEBUFFER_INCOMPLETE_DIMENSIONS = 0x8CD9;
		public const int GL_FRAMEBUFFER_UNSUPPORTED = 0x8CDD;
		public const int GL_FRAMEBUFFER_BINDING = 0x8CA6;
		public const int GL_RENDERBUFFER_BINDING = 0x8CA7;
		public const int GL_MAX_RENDERBUFFER_SIZE = 0x84E8;
		public const int GL_INVALID_FRAMEBUFFER_OPERATION = 0x0506;

		[DllImport(Path, EntryPoint = "glActiveTexture")]
		public static extern void ActiveTexture(int texture);

		[DllImport(Path, EntryPoint = "glAttachShader")]
		public static extern void AttachShader(int program, int shader);

		[DllImport(Path, EntryPoint = "glBindAttribLocation")]
		public static extern void BindAttribLocation (int program, int index, string name);

		[DllImport(Path, EntryPoint = "glBindBuffer")]
		public static extern void BindBuffer(int target, int buffer);

		[DllImport(Path, EntryPoint = "glBindFramebuffer")]
		public static extern void BindFramebuffer(int target, int framebuffer);

		[DllImport(Path, EntryPoint = "glBindRenderbuffer")]
		public static extern void BindRenderbuffer(int target, int renderbuffer);

		[DllImport(Path, EntryPoint = "glBindTexture")]
		public static extern void BindTexture(int target, int texture);

		[DllImport(Path, EntryPoint = "glBlendColor")]
		public static extern void BlendColor(float red, float green, float blue, float alpha);

		[DllImport(Path, EntryPoint = "glBlendEquation")]
		public static extern void BlendEquation(int mode);

		[DllImport(Path, EntryPoint = "glBlendEquationSeparate")]
		public static extern void BlendEquationSeparate(int modeRGB, int modeAlpha);

		[DllImport(Path, EntryPoint = "glBlendFunc")]
		public static extern void BlendFunc(int sfactor, int dfactor);

		[DllImport(Path, EntryPoint = "glBlendFuncSeparate")]
		public static extern void BlendFuncSeparate(int srcRGB, int dstRGB, int srcAlpha, int dstAlpha);
		
		[DllImport(Path, EntryPoint = "glBufferData")]
		public static extern void BufferData (int target, int size, IntPtr data, int usage);

		[DllImport(Path, EntryPoint = "glBufferSubData")]
		public static extern void BufferSubData (int target, int offset, int size, IntPtr data);

		[DllImport(Path, EntryPoint = "glCheckFramebufferStatus")]
		public static extern int CheckFramebufferStatus(int target);

		[DllImport(Path, EntryPoint = "glClear")]
		public static extern void Clear(uint mask);

		[DllImport(Path, EntryPoint = "glClearColor")]
		public static extern void ClearColor(float red, float green, float blue, float alpha);

		[DllImport(Path, EntryPoint = "glClearDepthf")]
		public static extern void ClearDepthf(float depth);

		[DllImport(Path, EntryPoint = "glClearStencil")]
		public static extern void ClearStencil(int s);

		[DllImport(Path, EntryPoint = "glColorMask")]
		public static extern void ColorMask(bool red, bool green, bool blue, bool alpha);

		[DllImport(Path, EntryPoint = "glCompileShader")]
		public static extern void CompileShader(int shader);

		[DllImport(Path, EntryPoint = "glCompressedTexImage2D")]
		public static extern void CompressedTexImage2D (int target, int level, int internalformat, int width, int height, int border, int imageSize, IntPtr data);

		[DllImport(Path, EntryPoint = "glCompressedTexSubImage2D")]
		public static extern void CompressedTexSubImage2D (int target, int level, int xoffset, int yoffset, int width, int height, int format, int imageSize, IntPtr data);

		[DllImport(Path, EntryPoint = "glCopyTexImage2D")]
		public static extern void CopyTexImage2D (int target, int level, int internalformat, int x, int y, int width, int height, int border);

		[DllImport(Path, EntryPoint = "glCopyTexSubImage2D")]
		public static extern void CopyTexSubImage2D (int target, int level, int xoffset, int yoffset, int x, int y, int width, int height);

		[DllImport(Path, EntryPoint = "glCreateProgram")]
		public static extern int CreateProgram();

		[DllImport(Path, EntryPoint = "glCreateShader")]
		public static extern int CreateShader(int type);

		[DllImport(Path, EntryPoint = "glCullFace")]
		public static extern void CullFace(int mode);

		[DllImport(Path, EntryPoint = "glDeleteBuffers")]
		public static extern void DeleteBuffers (int n, int[] buffers);

		[DllImport(Path, EntryPoint = "glDeleteFramebuffers")]
		public static extern void DeleteFramebuffers (int n, int[] framebuffers);

		[DllImport(Path, EntryPoint = "glDeleteProgram")]
		public static extern void DeleteProgram (int program);

		[DllImport(Path, EntryPoint = "glDeleteRenderbuffers")]
		public static extern void DeleteRenderbuffers (int n, int[] renderbuffers);

		[DllImport(Path, EntryPoint = "glDeleteShader")]
		public static extern void DeleteShader(int shader);

		[DllImport(Path, EntryPoint = "glDeleteTextures")]
		public static extern void DeleteTextures (int n, int[] textures);

		[DllImport(Path, EntryPoint = "glDepthFunc")]
		public static extern void DepthFunc(int func);

		[DllImport(Path, EntryPoint = "glDepthMask")]
		public static extern void DepthMask(bool flag);

		[DllImport(Path, EntryPoint = "glDepthRangef")]
		public static extern void DepthRangef (float zNear, float zFar);

		[DllImport(Path, EntryPoint = "glDetachShader")]
		public static extern void DetachShader(int program, int shader);

		[DllImport(Path, EntryPoint = "glDisable")]
		public static extern void Disable(int cap);

		[DllImport(Path, EntryPoint = "glDisableVertexAttribArray")]
		public static extern void DisableVertexAttribArray (int index);

		[DllImport(Path, EntryPoint = "glDrawArrays")]
		public static extern void DrawArrays(int mode, int first, int count);

		[DllImport(Path, EntryPoint = "glDrawElements")]
		public static extern void DrawElements(int mode, int count, int type, IntPtr indices);

		[DllImport(Path, EntryPoint = "glEnable")]
		public static extern void Enable(int cap);

		[DllImport(Path, EntryPoint = "glEnableVertexAttribArray")]
		public static extern void EnableVertexAttribArray(int index);

		[DllImport(Path, EntryPoint = "glFinish")]
		public static extern void Finish();

		[DllImport(Path, EntryPoint = "glFlush")]
		public static extern void Flush();

		[DllImport(Path, EntryPoint = "glFramebufferRenderbuffer")]
		public static extern void FramebufferRenderbuffer (int target, int attachment, int renderbuffertarget, int renderbuffer);

		[DllImport(Path, EntryPoint = "glFramebufferTexture2D")]
		public static extern void FramebufferTexture2D (int target, int attachment, int textarget, int texture, int level);

		[DllImport(Path, EntryPoint = "glFrontFace")]
		public static extern void FrontFace(int mode);

		public unsafe static int[] GenBuffers(int Count)
		{

			var Arr = new int[Count];

			fixed (int* Ptr = Arr)
			{
				GenBuffers(Count, Ptr);
                var Err = GLES20.GetError();
            }

			return Arr;
		}

		[DllImport(Path, EntryPoint = "glGenBuffers", CallingConvention = CallingConvention.StdCall)]
		public static unsafe extern void GenBuffers (int n, int* buffers);

		[DllImport(Path, EntryPoint = "glGenerateMipmap")]
		public static extern void GenerateMipmap (int target);

		[DllImport(Path, EntryPoint = "glGenFramebuffers")]
		public static extern void GenFramebuffers (int n, int[] framebuffers);

		[DllImport(Path, EntryPoint = "glGenRenderbuffers")]
		public static extern void GenRenderbuffers (int n, int[] renderbuffers);

		[DllImport(Path, EntryPoint = "glGenTextures")]
		public static extern void GenTextures (int n, int[] textures);

		//public static extern void glGetActiveAttrib (int program, int index, int bufsize, int* length, int size, int* type, string name);
		//public static extern void glGetActiveUniform (int program, int index, int bufsize, int* length, int size, int* type, string name);
		//public static extern void glGetAttachedShaders (int program, int maxcount, int* count, int* shaders);

		[DllImport(Path, EntryPoint = "glGetAttribLocation")]
		public static extern int GetAttribLocation(int program, string name);

		/*
		//public static extern void glGetBooleanv (int pname, GLboolean* params);
		//public static extern void glGetBufferParameteriv (int target, int pname, int  params);
		*/

		[DllImport(Path, EntryPoint = "glGetError")]
		public static extern int GetError();

        //public static extern void glGetFloatv (int pname, float params);
        //public static extern void glGetFramebufferAttachmentParameteriv (int target, int attachment, int pname, int params);


        [DllImport(Path, EntryPoint = "glGetIntegerv")]
        public unsafe static extern void GetInteger(int pname, void* Data);


		[DllImport(Path, EntryPoint = "glGetProgramiv")]
		public static extern void GetProgramiv(int program, int pname, int[] parameters);

		[DllImport(Path, EntryPoint = "glGetProgramiv")]
		public static extern void GetProgramiv(int program, int pname, out int parameter);

		[DllImport(Path, EntryPoint = "glGetProgramInfoLog")]
		static extern void GetProgramInfoLog(int program, int bufsize, IntPtr length, byte[] infolog);

		public static string GetProgramInfoLog(int program)
		{
			int infoLogLength;

			GetProgramiv(program, GL_INFO_LOG_LENGTH, out infoLogLength);

			byte[] data = new byte[infoLogLength];

			GetProgramInfoLog(program, infoLogLength, IntPtr.Zero, data);

			return Encoding.ASCII.GetString(data);
		}

		//public static extern void glGetRenderbufferParameteriv (int target, int pname, int params);

		[DllImport(Path, EntryPoint = "glGetShaderiv")]
		public static extern void GetShaderiv(int shader, int pname, int[] parameters);

		[DllImport(Path, EntryPoint = "glGetShaderiv")]
		public static extern void GetShaderiv(int shader, int pname, out int parameter);

		public static string GetShaderInfoLog(int shader)
		{
			int infoLogLength;

			GetShaderiv(shader, GL_INFO_LOG_LENGTH, out infoLogLength);

			byte[] data = new byte[infoLogLength];

			GetShaderInfoLog(shader, infoLogLength, IntPtr.Zero, data);

			return Encoding.ASCII.GetString(data);
		}

		[DllImport(Path, EntryPoint = "glGetShaderInfoLog")]
		static extern void GetShaderInfoLog(int shader, int bufsize, IntPtr length, byte[] infolog);
		
		//public static extern void glGetShaderPrecisionFormat (int shadertype, int precisiontype, int range, int precision);
		//public static extern void glGetShaderSource (int shader, int bufsize, int* length, GLchar* source);
		//GL_APICALL const GLubyte* GL_APIENTRY glGetString (int name);
		//public static extern void glGetTexParameterfv (int target, int pname, float params);
		//public static extern void glGetTexParameteriv (int target, int pname, int params);
		//public static extern void glGetUniformfv (int program, int location, float params);
		//public static extern void glGetUniformiv (int program, int location, int params);
		//public static extern void glGetVertexAttribfv (int index, int pname, float params);
		//public static extern void glGetVertexAttribiv (int index, int pname, int params);
		//public static extern void glGetVertexAttribPointerv (int index, int pname, GLvoid** pointer);

		[DllImport(Path, EntryPoint = "glGetUniformLocation")]
		public static extern int GetUniformLocation (int program, string name);

		[DllImport(Path, EntryPoint = "glHint")]
		public static extern void Hint (int target, int mode);

		[DllImport(Path, EntryPoint = "glIsBuffer")]
		public static extern bool IsBuffer(int buffer);

		[DllImport(Path, EntryPoint = "glIsEnabled")]
		public static extern bool IsEnabled(int cap);

		[DllImport(Path, EntryPoint = "glIsFramebuffer")]
		public static extern bool IsFramebuffer(int framebuffer);

		[DllImport(Path, EntryPoint = "glIsProgram")]
		public static extern bool IsProgram(int program);

		[DllImport(Path, EntryPoint = "glIsRenderbuffer")]
		public static extern bool IsRenderbuffer(int renderbuffer);

		[DllImport(Path, EntryPoint = "glIsShader")]
		public static extern bool IsShader(int shader);

		[DllImport(Path, EntryPoint = "glIsTexture")]
		public static extern bool IsTexture(int texture);

		[DllImport(Path, EntryPoint = "glLineWidth")]
		public static extern void LineWidth(float width);

		[DllImport(Path, EntryPoint = "glLinkProgram")]
		public static extern void LinkProgram(int program);

		[DllImport(Path, EntryPoint = "glPixelStorei")]
		public static extern void PixelStorei (int pname, int param);

		[DllImport(Path, EntryPoint = "glPolygonOffset")]
		public static extern void PolygonOffset (float factor, float units);

		//public static extern void glReadPixels (int x, int y, int width, int height, int format, int type, GLvoid* pixels);

		[DllImport(Path, EntryPoint = "glReleaseShaderCompiler")]
		public static extern void ReleaseShaderCompiler();

		[DllImport(Path, EntryPoint = "glRenderbufferStorage")]
		public static extern void RenderbufferStorage (int target, int internalformat, int width, int height);

		[DllImport(Path, EntryPoint = "glSampleCoverage")]
		public static extern void SampleCoverage (float value, bool invert);
        [DllImport(Path, EntryPoint = "glScissor")]
		public static extern void Scissor (int x, int y, int width, int height);

		public static void ShaderSource(int shader, string source)
		{
			ShaderSource(shader, 1, new string[] { source }, 0);
		}

		[DllImport(Path, EntryPoint = "glShaderSource")]
		public static extern void ShaderSource(int shader, int count, string[] source, int length);

#if ORBIS

		[DllImport(Path, EntryPoint = "glShaderBinary")]
        public static extern void ShaderBinary(int n, int[] shaders, int binaryformat, IntPtr binary, int length);
#else


        [DllImport(Path, EntryPoint = "glProgramBinaryOES")]
        public static extern void ProgramBinary(int program, int binaryFormat, IntPtr Buffer, int Length);
#endif


		public unsafe static byte[] GetShaderBinary(int hShader, out int BinaryFormat)
		{
			int BuffSize = 1024 * 1024 * 5;

            IntPtr Buffer = Marshal.AllocHGlobal(BuffSize);

			BinaryFormat = PrecompiledFormat;
			GetShaderBinary(hShader, BuffSize, out int Size, ref BinaryFormat, Buffer);

			if (Size <= 0)
			{
				throw new Exception("Failed to get the compiled shader binary");
			}

			byte[] Data = new byte[Size];

			fixed (byte* pData = Data)
			{
				byte* pBuffer = (byte*)Buffer.ToPointer();

				for (int i = 0; i < Size; i++)
					pData[i] = pBuffer[i];
			}

			Marshal.FreeHGlobal(Buffer);

			return Data;
		}

#if ORBIS
		public const int PrecompiledFormat = 0;
		[DllImport(Path, EntryPoint = "glPigletGetShaderBinarySCE")]
#else
		public const int PrecompiledFormat = 0x9130;//GL_SGX_PROGRAM_BINARY_IMG
        [DllImport(Path, EntryPoint = "glGetProgramBinaryOES")]
#endif
        static extern void GetShaderBinary(int program, int bufSize, out int length, ref int binaryFormat, IntPtr binary);

        [DllImport(Path, EntryPoint = "glStencilFunc")]
		public static extern void StencilFunc (int func, int @ref, int mask);

		[DllImport(Path, EntryPoint = "glStencilFuncSeparate")]
		public static extern void StencilFuncSeparate (int face, int func, int @ref, int mask);

		[DllImport(Path, EntryPoint = "glStencilMask")]
		public static extern void StencilMask (int mask);

		[DllImport(Path, EntryPoint = "glStencilMaskSeparate")]
		public static extern void StencilMaskSeparate (int face, int mask);

		[DllImport(Path, EntryPoint = "glStencilOp")]
		public static extern void StencilOp (int fail, int zfail, int zpass);

		[DllImport(Path, EntryPoint = "glStencilOpSeparate")]
		public static extern void StencilOpSeparate (int face, int fail, int zfail, int zpass);

		[DllImport(Path, EntryPoint = "glTexImage2D")]
		public static extern void TexImage2D (int target, int level, int internalformat, int width, int height, int border, int format, int type, IntPtr pixels);

		[DllImport(Path, EntryPoint = "glTexParameterf")]
		public static extern void TexParameterf (int target, int pname, float param);
		
		//public static extern void glTexParameterfv (int target, int pname, const float params);

		[DllImport(Path, EntryPoint = "glTexParameteri")]
		public static extern void TexParameteri(int target, int pname, int param);
		
		//public static extern void glTexParameteriv (int target, int pname, const int params);

		[DllImport(Path, EntryPoint = "glTexSubImage2D")]
		public static extern void TexSubImage2D (int target, int level, int xoffset, int yoffset, int width, int height, int format, int type, IntPtr pixels);

		[DllImport(Path, EntryPoint = "glUniform1f")]
		public static extern void Uniform1f (int location, float x);

		//public static extern void glUniform1fv (int location, int count, const float v);

		[DllImport(Path, EntryPoint = "glUniform1i")]
		public static extern void Uniform1i (int location, int x);

		//public static extern void glUniform1iv (int location, int count, const int v);

		[DllImport(Path, EntryPoint = "glUniform2f")]
		public static extern void Uniform2f (int location, float x, float y);

		//public static extern void glUniform2fv (int location, int count, const float v);

		[DllImport(Path, EntryPoint = "glUniform2i")]
		public static extern void Uniform2i (int location, int x, int y);

		//public static extern void glUniform2iv (int location, int count, const int v);

		[DllImport(Path, EntryPoint = "glUniform3f")]
		public static extern void Uniform3f (int location, float x, float y, float z);

		//public static extern void glUniform3fv (int location, int count, const float v);

		[DllImport(Path, EntryPoint = "glUniform3i")]
		public static extern void Uniform3i (int location, int x, int y, int z);
		//public static extern void glUniform3iv (int location, int count, const int v);

		[DllImport(Path, EntryPoint = "glUniform4f")]
		public static extern void Uniform4f (int location, float x, float y, float z, float w);
		//public static extern void glUniform4fv (int location, int count, const float v);

		[DllImport(Path, EntryPoint = "glUniform4i")]
		public static extern void Uniform4i (int location, int x, int y, int z, int w);

		//public static extern void glUniform4iv (int location, int count, const int v);
		//public static extern void glUniformMatrix2fv (int location, int count, GLboolean transpose, const float value);

		[DllImport(Path, EntryPoint = "glUniformMatrix3fv")]
		public static extern void UniformMatrix3fv(int location, int count, bool transpose, float[] value);

		//public static extern void glUniformMatrix4fv (int location, int count, GLboolean transpose, const float value);

		[DllImport(Path, EntryPoint = "glUseProgram")]
		public static extern void UseProgram (int program);

		[DllImport(Path, EntryPoint = "glValidateProgram")]
		public static extern void ValidateProgram (int program);

		[DllImport(Path, EntryPoint = "glVertexAttrib1f")]
		public static extern void VertexAttrib1f (int indx, float x);

		//public static extern void glVertexAttrib1fv (int indx, const float values);

		[DllImport(Path, EntryPoint = "glVertexAttrib2f")]
		public static extern void VertexAttrib2f (int indx, float x, float y);

		//public static extern void glVertexAttrib2fv (int indx, const float  values);

		[DllImport(Path, EntryPoint = "glVertexAttrib3f")]
		public static extern void VertexAttrib3f (int indx, float x, float y, float z);

		//public static extern void glVertexAttrib3fv (int indx, const float  values);

		[DllImport(Path, EntryPoint = "glVertexAttrib4f")]
		public static extern void VertexAttrib4f (int indx, float x, float y, float z, float w);

		//public static extern void glVertexAttrib4fv (int indx, const float  values);

		[DllImport(Path, EntryPoint = "glVertexAttribPointer")]
		public static extern void VertexAttribPointer (int indx, int size, int type, bool normalized, int stride, IntPtr ptr);

		[DllImport(Path, EntryPoint = "glViewport")]
		public static extern void Viewport(int x, int y, int width, int height);
	}
}