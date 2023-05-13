using System;
using System.Runtime.InteropServices;

namespace SharpGLES
{
	public static class EGL
	{
		#if ORBIS
		public const string Path = "libScePigletv2VSH.sprx";
		#else
		const string Path = "libEGL.dll";
		#endif

		//v1.0
		public const int  EGL_ALPHA_SIZE = 0x3021;
		public const int  EGL_BAD_ACCESS = 0x3002;
		public const int  EGL_BAD_ALLOC = 0x3003;
		public const int  EGL_BAD_ATTRIBUTE = 0x3004;
		public const int  EGL_BAD_CONFIG = 0x3005;
		public const int  EGL_BAD_CONTEXT = 0x3006;
		public const int  EGL_BAD_CURRENT_SURFACE = 0x3007;
		public const int  EGL_BAD_DISPLAY = 0x3008;
		public const int  EGL_BAD_MATCH = 0x3009;
		public const int  EGL_BAD_NATIVE_PIXMAP = 0x300A;
		public const int  EGL_BAD_NATIVE_WINDOW = 0x300B;
		public const int  EGL_BAD_PARAMETER = 0x300C;
		public const int  EGL_BAD_SURFACE = 0x300D;
		public const int  EGL_BLUE_SIZE = 0x3022;
		public const int  EGL_BUFFER_SIZE = 0x3020;
		public const int  EGL_CONFIG_CAVEAT = 0x3027;
		public const int  EGL_CONFIG_ID = 0x3028;
		public const int  EGL_CORE_NATIVE_ENGINE = 0x305B;
		public const int  EGL_DEPTH_SIZE = 0x3025;
		public const int  EGL_DONT_CARE = -1;
		public const int  EGL_DRAW = 0x3059;
		public const int  EGL_EXTENSIONS = 0x3055;
		public const int  EGL_FALSE = 0;
		public const int  EGL_GREEN_SIZE = 0x3023;
		public const int  EGL_HEIGHT = 0x3056;
		public const int  EGL_LARGEST_PBUFFER = 0x3058;
		public const int  EGL_LEVEL = 0x3029;
		public const int  EGL_MAX_PBUFFER_HEIGHT = 0x302A;
		public const int  EGL_MAX_PBUFFER_PIXELS = 0x302B;
		public const int  EGL_MAX_PBUFFER_WIDTH = 0x302C;
		public const int  EGL_NATIVE_RENDERABLE = 0x302D;
		public const int  EGL_NATIVE_VISUAL_ID = 0x302E;
		public const int  EGL_NATIVE_VISUAL_TYPE = 0x302F;
		public const int  EGL_NONE = 0x3038;
		public const int  EGL_NON_CONFORMANT_CONFIG = 0x3051;
		public const int  EGL_NOT_INITIALIZED = 0x3001;
		public static readonly IntPtr EGL_NO_CONTEXT = IntPtr.Zero;
		public static readonly IntPtr EGL_NO_DISPLAY = IntPtr.Zero;
		public static readonly IntPtr EGL_NO_SURFACE = IntPtr.Zero;
		public const int  EGL_PBUFFER_BIT = 0x0001;
		public const int  EGL_PIXMAP_BIT = 0x0002;
		public const int  EGL_READ = 0x305A;
		public const int  EGL_RED_SIZE = 0x3024;
		public const int  EGL_SAMPLES = 0x3031;
		public const int  EGL_SAMPLE_BUFFERS = 0x3032;
		public const int  EGL_SLOW_CONFIG = 0x3050;
		public const int  EGL_STENCIL_SIZE = 0x3026;
		public const int  EGL_SUCCESS = 0x3000;
		public const int  EGL_SURFACE_TYPE = 0x3033;
		public const int  EGL_TRANSPARENT_BLUE_VALUE = 0x3035;
		public const int  EGL_TRANSPARENT_GREEN_VALUE = 0x3036;
		public const int  EGL_TRANSPARENT_RED_VALUE = 0x3037;
		public const int  EGL_TRANSPARENT_RGB = 0x3052;
		public const int  EGL_TRANSPARENT_TYPE = 0x3034;
		public const int  EGL_TRUE = 1;
		public const int  EGL_VENDOR = 0x3053;
		public const int  EGL_VERSION = 0x3054;
		public const int  EGL_WIDTH = 0x3057;
		public const int  EGL_WINDOW_BIT = 0x000;

		[DllImport(Path, EntryPoint = "eglChooseConfig")]
		public static extern bool ChooseConfig (IntPtr dpy, int[] attribList, out IntPtr configs, int configSize, out int numConfig);

		[DllImport(Path, EntryPoint = "eglCopyBuffers")]
		public static extern bool eglCopyBuffers(IntPtr dpy, IntPtr surface, IntPtr target);

		[DllImport(Path, EntryPoint = "eglCreateContext")]
		public static extern IntPtr CreateContext(IntPtr dpy, IntPtr config, IntPtr share_context, int[] attribList);

		//public static extern IntPtr eglCreatePbufferSurface (EGLDisplay dpy, EGLConfig config, const EGLint *attrib_list);
		//public static extern IntPtr eglCreatePixmapSurface (EGLDisplay dpy, EGLConfig config, EGLNativePixmapType pixmap, const EGLint *attrib_list);

		[DllImport(Path, EntryPoint = "eglCreateWindowSurface")]
		public static extern IntPtr CreateWindowSurface(IntPtr dpy, IntPtr config, IntPtr win, int[] attribList);

		[DllImport(Path, EntryPoint = "eglDestroyContext")]
		public static extern bool DestroyContext(IntPtr dpy, IntPtr ctx);

		[DllImport(Path, EntryPoint = "eglDestroySurface")]
		public static extern bool DestroySurface(IntPtr dpy, IntPtr surface);

		//EGLAPI EGLBoolean EGLAPIENTRY eglGetConfigAttrib (EGLDisplay dpy, EGLConfig config, EGLint attribute, EGLint *value);
		//EGLAPI EGLBoolean EGLAPIENTRY eglGetConfigs (EGLDisplay dpy, EGLConfig *configs, EGLint config_size, EGLint *num_config);
		//EGLAPI EGLDisplay EGLAPIENTRY eglGetCurrentDisplay (void);
		//public static extern IntPtr eglGetCurrentSurface (EGLint readdraw);

		[DllImport(Path, EntryPoint = "eglGetDisplay")]
		public static extern IntPtr GetDisplay(IntPtr display_id);

		[DllImport(Path, EntryPoint = "eglGetError")]
		public static extern int eglGetError();

		//EGLAPI __eglMustCastToProperFunctionPointerType EGLAPIENTRY eglGetProcAddress (const char *procname);

		[DllImport(Path, EntryPoint = "eglInitialize")]
		public static extern bool Initialize(IntPtr dpy, out int major, out int minor);

		[DllImport(Path, EntryPoint = "eglMakeCurrent")]
		public static extern bool MakeCurrent(IntPtr dpy, IntPtr draw, IntPtr read, IntPtr ctx);

		//EGLAPI EGLBoolean EGLAPIENTRY eglQueryContext (EGLDisplay dpy, EGLContext ctx, EGLint attribute, EGLint *value);
		//EGLAPI const char *EGLAPIENTRY eglQueryString (EGLDisplay dpy, EGLint name);
		//EGLAPI EGLBoolean EGLAPIENTRY eglQuerySurface (EGLDisplay dpy, EGLSurface surface, EGLint attribute, EGLint *value);

		[DllImport(Path, EntryPoint = "eglSwapBuffers")]
		public static extern bool SwapBuffers(IntPtr dpy, IntPtr surface);

		[DllImport(Path, EntryPoint = "eglTerminate")]
		public static extern bool Terminate(IntPtr dpy);

		//EGLAPI EGLBoolean EGLAPIENTRY eglWaitGL (void);
		//EGLAPI EGLBoolean EGLAPIENTRY eglWaitNative (EGLint engine);
		
		//v1.1
		public const int EGL_BACK_BUFFER = 0x3084;
		public const int EGL_BIND_TO_TEXTURE_RGB = 0x3039;
		public const int EGL_BIND_TO_TEXTURE_RGBA = 0x303A;
		public const int EGL_CONTEXT_LOST = 0x300E;
		public const int EGL_MIN_SWAP_INTERVAL = 0x303B;
		public const int EGL_MAX_SWAP_INTERVAL = 0x303C;
		public const int EGL_MIPMAP_TEXTURE = 0x3082;
		public const int EGL_MIPMAP_LEVEL = 0x3083;
		public const int EGL_NO_TEXTURE = 0x305C;
		public const int EGL_TEXTURE_2D = 0x305F;
		public const int EGL_TEXTURE_FORMAT = 0x3080;
		public const int EGL_TEXTURE_RGB = 0x305D;
		public const int EGL_TEXTURE_RGBA = 0x305E;
		public const int EGL_TEXTURE_TARGET = 0x308;

		//EGLAPI EGLBoolean EGLAPIENTRY eglBindTexImage (EGLDisplay dpy, EGLSurface surface, EGLint buffer);
		//EGLAPI EGLBoolean EGLAPIENTRY eglReleaseTexImage (EGLDisplay dpy, EGLSurface surface, EGLint buffer);
		//EGLAPI EGLBoolean EGLAPIENTRY eglSurfaceAttrib (EGLDisplay dpy, EGLSurface surface, EGLint attribute, EGLint value);

		[DllImport(Path, EntryPoint = "eglSwapInterval")]
		public static extern bool SwapInterval(IntPtr dpy, int interval);
		
		//v1.2
		public const int EGL_ALPHA_FORMAT = 0x3088;
		public const int EGL_ALPHA_FORMAT_NONPRE = 0x308B;
		public const int EGL_ALPHA_FORMAT_PRE = 0x308C;
		public const int EGL_ALPHA_MASK_SIZE = 0x303E;
		public const int EGL_BUFFER_PRESERVED = 0x3094;
		public const int EGL_BUFFER_DESTROYED = 0x3095;
		public const int EGL_CLIENT_APIS = 0x308D;
		public const int EGL_COLORSPACE = 0x3087;
		public const int EGL_COLORSPACE_sRGB = 0x3089;
		public const int EGL_COLORSPACE_LINEAR = 0x308A;
		public const int EGL_COLOR_BUFFER_TYPE = 0x303F;
		public const int EGL_CONTEXT_CLIENT_TYPE = 0x3097;
		public const int EGL_DISPLAY_SCALING = 10000;
		public const int EGL_HORIZONTAL_RESOLUTION = 0x3090;
		public const int EGL_LUMINANCE_BUFFER = 0x308F;
		public const int EGL_LUMINANCE_SIZE = 0x303D;
		public const int EGL_OPENGL_ES_BIT = 0x0001;
		public const int EGL_OPENVG_BIT = 0x0002;
		public const int EGL_OPENGL_ES_API = 0x30A0;
		public const int EGL_OPENVG_API = 0x30A1;
		public const int EGL_OPENVG_IMAGE = 0x3096;
		public const int EGL_PIXEL_ASPECT_RATIO = 0x3092;
		public const int EGL_RENDERABLE_TYPE = 0x3040;
		public const int EGL_RENDER_BUFFER = 0x3086;
		public const int EGL_RGB_BUFFER = 0x308E;
		public const int EGL_SINGLE_BUFFER = 0x3085;
		public const int EGL_SWAP_BEHAVIOR = 0x3093;
		public const int EGL_UNKNOWN = -1;
		public const int EGL_VERTICAL_RESOLUTION = 0x3091;

		[DllImport(Path, EntryPoint = "eglBindAPI")]
		public static extern bool BindAPI(uint api);

		//EGLAPI EGLenum EGLAPIENTRY eglQueryAPI (void);
		//public static extern IntPtr eglCreatePbufferFromClientBuffer (EGLDisplay dpy, EGLenum buftype, EGLClientBuffer buffer, EGLConfig config, const EGLint *attrib_list);
		//EGLAPI EGLBoolean EGLAPIENTRY eglReleaseThread (void);
		//EGLAPI EGLBoolean EGLAPIENTRY eglWaitClient (void);

		//v1.3
		public const int EGL_CONFORMANT = 0x3042;
		public const int EGL_CONTEXT_CLIENT_VERSION = 0x3098;
		public const int EGL_MATCH_NATIVE_PIXMAP = 0x3041;
		public const int EGL_OPENGL_ES2_BIT = 0x0004;
		public const int EGL_VG_ALPHA_FORMAT = 0x3088;
		public const int EGL_VG_ALPHA_FORMAT_NONPRE = 0x308B;
		public const int EGL_VG_ALPHA_FORMAT_PRE = 0x308C;
		public const int EGL_VG_ALPHA_FORMAT_PRE_BIT = 0x0040;
		public const int EGL_VG_COLORSPACE = 0x3087;
		public const int EGL_VG_COLORSPACE_sRGB = 0x3089;
		public const int EGL_VG_COLORSPACE_LINEAR = 0x308A;
		public const int EGL_VG_COLORSPACE_LINEAR_BIT = 0x0020;

		//v1.4
		public const int EGL_DEFAULT_DISPLAY = 0;
		public const int EGL_MULTISAMPLE_RESOLVE_BOX_BIT = 0x0200;
		public const int EGL_MULTISAMPLE_RESOLVE = 0x3099;
		public const int EGL_MULTISAMPLE_RESOLVE_DEFAULT = 0x309A;
		public const int EGL_MULTISAMPLE_RESOLVE_BOX = 0x309B;
		public const int EGL_OPENGL_API = 0x30A2;
		public const int EGL_OPENGL_BIT = 0x0008;
		public const int EGL_SWAP_BEHAVIOR_PRESERVED_BIT = 0x0400;

		//public static extern IntPtr eglGetCurrentContext (void);

		//v1.5
		public const int EGL_CONTEXT_MAJOR_VERSION = 0x3098;
		public const int EGL_CONTEXT_MINOR_VERSION = 0x30FB;
		public const int EGL_CONTEXT_OPENGL_PROFILE_MASK = 0x30FD;
		public const int EGL_CONTEXT_OPENGL_RESET_NOTIFICATION_STRATEGY = 0x31BD;
		public const int EGL_NO_RESET_NOTIFICATION = 0x31BE;
		public const int EGL_LOSE_CONTEXT_ON_RESET = 0x31BF;
		public const int EGL_CONTEXT_OPENGL_CORE_PROFILE_BIT = 0x00000001;
		public const int EGL_CONTEXT_OPENGL_COMPATIBILITY_PROFILE_BIT = 0x00000002;
		public const int EGL_CONTEXT_OPENGL_DEBUG = 0x31B0;
		public const int EGL_CONTEXT_OPENGL_FORWARD_COMPATIBLE = 0x31B1;
		public const int EGL_CONTEXT_OPENGL_ROBUST_ACCESS = 0x31B2;
		public const int EGL_OPENGL_ES3_BIT = 0x00000040;
		public const int EGL_CL_EVENT_HANDLE = 0x309C;
		public const int EGL_SYNC_CL_EVENT = 0x30FE;
		public const int EGL_SYNC_CL_EVENT_COMPLETE = 0x30FF;
		public const int EGL_SYNC_PRIOR_COMMANDS_COMPLETE = 0x30F0;
		public const int EGL_SYNC_TYPE = 0x30F7;
		public const int EGL_SYNC_STATUS = 0x30F1;
		public const int EGL_SYNC_CONDITION = 0x30F8;
		public const int EGL_SIGNALED = 0x30F2;
		public const int EGL_UNSIGNALED = 0x30F3;
		public const int EGL_SYNC_FLUSH_COMMANDS_BIT = 0x0001;
		public const int EGL_FOREVER = -1;
		public const int EGL_TIMEOUT_EXPIRED = 0x30F5;
		public const int EGL_CONDITION_SATISFIED = 0x30F6;
		public const int EGL_NO_SYNC = 0;
		public const int EGL_SYNC_FENCE = 0x30F9;
		public const int EGL_GL_COLORSPACE = 0x309D;
		public const int EGL_GL_COLORSPACE_SRGB = 0x3089;
		public const int EGL_GL_COLORSPACE_LINEAR = 0x308A;
		public const int EGL_GL_RENDERBUFFER = 0x30B9;
		public const int EGL_GL_TEXTURE_2D = 0x30B1;
		public const int EGL_GL_TEXTURE_LEVEL = 0x30BC;
		public const int EGL_GL_TEXTURE_3D = 0x30B2;
		public const int EGL_GL_TEXTURE_ZOFFSET = 0x30BD;
		public const int EGL_GL_TEXTURE_CUBE_MAP_POSITIVE_X = 0x30B3;
		public const int EGL_GL_TEXTURE_CUBE_MAP_NEGATIVE_X = 0x30B4;
		public const int EGL_GL_TEXTURE_CUBE_MAP_POSITIVE_Y = 0x30B5;
		public const int EGL_GL_TEXTURE_CUBE_MAP_NEGATIVE_Y = 0x30B6;
		public const int EGL_GL_TEXTURE_CUBE_MAP_POSITIVE_Z = 0x30B7;
		public const int EGL_GL_TEXTURE_CUBE_MAP_NEGATIVE_Z = 0x30B8;

		//EGLAPI EGLSync EGLAPIENTRY eglCreateSync (EGLDisplay dpy, EGLenum type, const EGLAttrib *attrib_list);
		//EGLAPI EGLBoolean EGLAPIENTRY eglDestroySync (EGLDisplay dpy, EGLSync sync);
		//EGLAPI EGLint EGLAPIENTRY eglClientWaitSync (EGLDisplay dpy, EGLSync sync, EGLint flags, EGLTime timeout);
		//EGLAPI EGLBoolean EGLAPIENTRY eglGetSyncAttrib (EGLDisplay dpy, EGLSync sync, EGLint attribute, EGLAttrib *value);
		//public static extern IntPtr eglGetPlatformDisplay (EGLenum platform, void *native_display, const EGLAttrib *attrib_list);
		//public static extern IntPtr eglCreatePlatformWindowSurface (EGLDisplay dpy, EGLConfig config, void *native_window, const EGLAttrib *attrib_list);
		//public static extern IntPtr eglCreatePlatformPixmapSurface (EGLDisplay dpy, EGLConfig config, void *native_pixmap, const EGLAttrib *attrib_list);
		//EGLAPI EGLBoolean EGLAPIENTRY eglWaitSync (EGLDisplay dpy, EGLSync sync, EGLint flags);
		
#if ORBIS
		//Definitions by OpenOrbis and Flatz
		public static extern bool scePigletSetConfigurationVSH(ScePglConfig Config); 
		
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct ScePglConfig
		{
			public uint size;
			public uint flags;
			public byte processOrder;
			public uint unk_0x0C;
			public uint unk_0x10;
			public uint unk_0x14;
			public ulong systemSharedMemorySize;

			public uint unk_0x20;
			public uint unk_0x24;
			public ulong videoSharedMemorySize;
			public ulong maxMappedFlexibleMemory;
			public ulong minFlexibleMemoryChunkSize;

			public uint dbgPosCmd_0x40;
			public uint dbgPosCmd_0x44;
			public uint dbgPosCmd_0x48;
			public uint dbgPosCmd_0x4C;
			public byte dbgPosCmd_0x50;

			public uint drawCommandBufferSize;
			public uint lcueResourceBufferSize;

			public uint unk_0x5C;

			public ulong unk_0x60;
			public ulong unk_0x68;
			public ulong unk_0x70;
			public ulong unk_0x78;
		}
		
		public const uint ORBIS_PGL_MAX_PROCESS_ORDER             = 2         ;
		public const uint ORBIS_PGL_FLAGS_USE_COMPOSITE_EXT       = 0x8       ;
		public const uint ORBIS_PGL_FLAGS_SKIP_APP_INITIALIZATION = 0x10      ;
		public const uint ORBIS_PGL_FLAGS_USE_TILED_TEXTURE       = 0x40      ;
		public const uint ORBIS_PGL_FLAGS_USE_FLEXIBLE_MEMORY     = 0x80      ;
		public const uint ORBIS_PGL_MAX_SYS_SHARED_MEM            = 0x20000000;
		public const uint ORBIS_PGL_MAX_VIDEO_SHARED_MEM          = 0x20000000;
		public const uint ORBIS_PGL_MAX_VIDEO_PRIV_MEM            = 0x20000000;
		
#endif
		public static IntPtr EGL_D3D11_ONLY_DISPLAY_ANGLE = (IntPtr)(-3);
	}
}
