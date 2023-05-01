using System;
using System.Runtime.InteropServices;

namespace SharpGLES
{
	public static class EGLX
	{
		public const int EGL_KHR_cl_event = 1;
		public const int EGL_CL_EVENT_HANDLE_KHR = 0x309C;
		public const int EGL_SYNC_CL_EVENT_KHR = 0x30FE;
		public const int EGL_SYNC_CL_EVENT_COMPLETE_KHR = 0x30FF;
		public const int EGL_KHR_cl_event2 = 1;
		
		//typedef EGLSyncKHR (EGLAPIENTRYP PFNEGLCREATESYNC64KHRPROC) (EGLDisplay dpy, EGLenum type, const EGLAttribKHR *attrib_list);
		//EGLAPI EGLSyncKHR EGLAPIENTRY eglCreateSync64KHR (EGLDisplay dpy, EGLenum type, const EGLAttribKHR *attrib_list);
		
		public const int EGL_KHR_client_get_all_proc_addresses = 1;
		public const int EGL_KHR_config_attribs = 1;
		public const int EGL_CONFORMANT_KHR = 0x3042;
		public const int EGL_VG_COLORSPACE_LINEAR_BIT_KHR = 0x0020;
		public const int EGL_VG_ALPHA_FORMAT_PRE_BIT_KHR = 0x0040;
		public const int EGL_KHR_create_context = 1;
		public const int EGL_CONTEXT_MAJOR_VERSION_KHR = 0x3098;
		public const int EGL_CONTEXT_MINOR_VERSION_KHR = 0x30FB;
		public const int EGL_CONTEXT_FLAGS_KHR = 0x30FC;
		public const int EGL_CONTEXT_OPENGL_PROFILE_MASK_KHR = 0x30FD;
		public const int EGL_CONTEXT_OPENGL_RESET_NOTIFICATION_STRATEGY_KHR = 0x31BD;
		public const int EGL_NO_RESET_NOTIFICATION_KHR = 0x31BE;
		public const int EGL_LOSE_CONTEXT_ON_RESET_KHR = 0x31BF;
		public const int EGL_CONTEXT_OPENGL_DEBUG_BIT_KHR = 0x00000001;
		public const int EGL_CONTEXT_OPENGL_FORWARD_COMPATIBLE_BIT_KHR = 0x00000002;
		public const int EGL_CONTEXT_OPENGL_ROBUST_ACCESS_BIT_KHR = 0x00000004;
		public const int EGL_CONTEXT_OPENGL_CORE_PROFILE_BIT_KHR = 0x00000001;
		public const int EGL_CONTEXT_OPENGL_COMPATIBILITY_PROFILE_BIT_KHR = 0x00000002;
		public const int EGL_OPENGL_ES3_BIT_KHR = 0x00000040;
		public const int EGL_KHR_fence_sync = 1;
		public const int EGL_SYNC_PRIOR_COMMANDS_COMPLETE_KHR = 0x30F0;
		public const int EGL_SYNC_CONDITION_KHR = 0x30F8;
		public const int EGL_SYNC_FENCE_KHR = 0x30F9;
		public const int EGL_KHR_get_all_proc_addresses = 1;
		public const int EGL_KHR_gl_colorspace = 1;
		public const int EGL_GL_COLORSPACE_KHR = 0x309D;
		public const int EGL_GL_COLORSPACE_SRGB_KHR = 0x3089;
		public const int EGL_GL_COLORSPACE_LINEAR_KHR = 0x308A;
		public const int EGL_KHR_gl_renderbuffer_image = 1;
		public const int EGL_GL_RENDERBUFFER_KHR = 0x30B9;
		public const int EGL_KHR_gl_texture_2D_image = 1;
		public const int EGL_GL_TEXTURE_2D_KHR = 0x30B1;
		public const int EGL_GL_TEXTURE_LEVEL_KHR = 0x30BC;
		public const int EGL_KHR_gl_texture_3D_image = 1;
		public const int EGL_GL_TEXTURE_3D_KHR = 0x30B2;
		public const int EGL_GL_TEXTURE_ZOFFSET_KHR = 0x30BD;
		public const int EGL_KHR_gl_texture_cubemap_image = 1;
		public const int EGL_GL_TEXTURE_CUBE_MAP_POSITIVE_X_KHR = 0x30B3;
		public const int EGL_GL_TEXTURE_CUBE_MAP_NEGATIVE_X_KHR = 0x30B4;
		public const int EGL_GL_TEXTURE_CUBE_MAP_POSITIVE_Y_KHR = 0x30B5;
		public const int EGL_GL_TEXTURE_CUBE_MAP_NEGATIVE_Y_KHR = 0x30B6;
		public const int EGL_GL_TEXTURE_CUBE_MAP_POSITIVE_Z_KHR = 0x30B7;
		public const int EGL_GL_TEXTURE_CUBE_MAP_NEGATIVE_Z_KHR = 0x30B8;
		public const int EGL_KHR_image = 1;
		public const int EGL_NATIVE_PIXMAP_KHR = 0x30B0;
		public static readonly IntPtr EGL_NO_IMAGE_KHR = IntPtr.Zero;

		//typedef EGLImageKHR (EGLAPIENTRYP PFNEGLCREATEIMAGEKHRPROC) (EGLDisplay dpy, EGLContext ctx, EGLenum target, EGLClientBuffer buffer, const EGLint *attrib_list);
		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLDESTROYIMAGEKHRPROC) (EGLDisplay dpy, EGLImageKHR image);
		//EGLAPI EGLImageKHR EGLAPIENTRY eglCreateImageKHR (EGLDisplay dpy, EGLContext ctx, EGLenum target, EGLClientBuffer buffer, const EGLint *attrib_list);
		//EGLAPI EGLBoolean EGLAPIENTRY eglDestroyImageKHR (EGLDisplay dpy, EGLImageKHR image);

		public const int EGL_KHR_image_base = 1;
		public const int EGL_IMAGE_PRESERVED_KHR = 0x30D2;
		public const int EGL_KHR_image_pixmap = 1;
		public const int EGL_KHR_lock_surface = 1;
		public const int EGL_READ_SURFACE_BIT_KHR = 0x0001;
		public const int EGL_WRITE_SURFACE_BIT_KHR = 0x0002;
		public const int EGL_LOCK_SURFACE_BIT_KHR = 0x0080;
		public const int EGL_OPTIMAL_FORMAT_BIT_KHR = 0x0100;
		public const int EGL_MATCH_FORMAT_KHR = 0x3043;
		public const int EGL_FORMAT_RGB_565_EXACT_KHR = 0x30C0;
		public const int EGL_FORMAT_RGB_565_KHR = 0x30C1;
		public const int EGL_FORMAT_RGBA_8888_EXACT_KHR = 0x30C2;
		public const int EGL_FORMAT_RGBA_8888_KHR = 0x30C3;
		public const int EGL_MAP_PRESERVE_PIXELS_KHR = 0x30C4;
		public const int EGL_LOCK_USAGE_HINT_KHR = 0x30C5;
		public const int EGL_BITMAP_POINTER_KHR = 0x30C6;
		public const int EGL_BITMAP_PITCH_KHR = 0x30C7;
		public const int EGL_BITMAP_ORIGIN_KHR = 0x30C8;
		public const int EGL_BITMAP_PIXEL_RED_OFFSET_KHR = 0x30C9;
		public const int EGL_BITMAP_PIXEL_GREEN_OFFSET_KHR = 0x30CA;
		public const int EGL_BITMAP_PIXEL_BLUE_OFFSET_KHR = 0x30CB;
		public const int EGL_BITMAP_PIXEL_ALPHA_OFFSET_KHR = 0x30CC;
		public const int EGL_BITMAP_PIXEL_LUMINANCE_OFFSET_KHR = 0x30CD;
		public const int EGL_LOWER_LEFT_KHR = 0x30CE;
		public const int EGL_UPPER_LEFT_KHR = 0x30CF;
		
		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLLOCKSURFACEKHRPROC) (EGLDisplay dpy, EGLSurface surface, const EGLint *attrib_list);
		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLUNLOCKSURFACEKHRPROC) (EGLDisplay dpy, EGLSurface surface);
		//EGLAPI EGLBoolean EGLAPIENTRY eglLockSurfaceKHR (EGLDisplay dpy, EGLSurface surface, const EGLint *attrib_list);
		//EGLAPI EGLBoolean EGLAPIENTRY eglUnlockSurfaceKHR (EGLDisplay dpy, EGLSurface surface);

		public const int EGL_KHR_lock_surface2 = 1;
		public const int EGL_BITMAP_PIXEL_SIZE_KHR = 0x3110;
		public const int EGL_KHR_lock_surface3 = 1;
		
		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLQUERYSURFACE64KHRPROC) (EGLDisplay dpy, EGLSurface surface, EGLint attribute, EGLAttribKHR *value);
		//EGLAPI EGLBoolean EGLAPIENTRY eglQuerySurface64KHR (EGLDisplay dpy, EGLSurface surface, EGLint attribute, EGLAttribKHR *value);

		public const int EGL_KHR_platform_android = 1;
		public const int EGL_PLATFORM_ANDROID_KHR = 0x3141;
		public const int EGL_KHR_platform_gbm = 1;
		public const int EGL_PLATFORM_GBM_KHR = 0x31D7;
		public const int EGL_KHR_platform_wayland = 1;
		public const int EGL_PLATFORM_WAYLAND_KHR = 0x31D8;
		public const int EGL_KHR_platform_x11 = 1;
		public const int EGL_PLATFORM_X11_KHR = 0x31D5;
		public const int EGL_PLATFORM_X11_SCREEN_KHR = 0x31D6;
		public const int EGL_KHR_reusable_sync = 1;
		
		//typedef khronos_utime_nanoseconds_t EGLTimeKHR;
		
		public const int EGL_SYNC_STATUS_KHR = 0x30F1;
		public const int EGL_SIGNALED_KHR = 0x30F2;
		public const int EGL_UNSIGNALED_KHR = 0x30F3;
		public const int EGL_TIMEOUT_EXPIRED_KHR = 0x30F5;
		public const int EGL_CONDITION_SATISFIED_KHR = 0x30F6;
		public const int EGL_SYNC_TYPE_KHR = 0x30F7;
		public const int EGL_SYNC_REUSABLE_KHR = 0x30FA;
		public const int EGL_SYNC_FLUSH_COMMANDS_BIT_KHR = 0x0001;
		public const int EGL_FOREVER_KHR = -1;
		public static readonly IntPtr EGL_NO_SYNC_KHR = IntPtr.Zero;
		
		//typedef EGLSyncKHR (EGLAPIENTRYP PFNEGLCREATESYNCKHRPROC) (EGLDisplay dpy, EGLenum type, const EGLint *attrib_list);
		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLDESTROYSYNCKHRPROC) (EGLDisplay dpy, EGLSyncKHR sync);
		//typedef EGLint (EGLAPIENTRYP PFNEGLCLIENTWAITSYNCKHRPROC) (EGLDisplay dpy, EGLSyncKHR sync, EGLint flags, EGLTimeKHR timeout);
		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLSIGNALSYNCKHRPROC) (EGLDisplay dpy, EGLSyncKHR sync, EGLenum mode);
		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLGETSYNCATTRIBKHRPROC) (EGLDisplay dpy, EGLSyncKHR sync, EGLint attribute, EGLint *value);
		//EGLAPI EGLSyncKHR EGLAPIENTRY eglCreateSyncKHR (EGLDisplay dpy, EGLenum type, const EGLint *attrib_list);
		//EGLAPI EGLBoolean EGLAPIENTRY eglDestroySyncKHR (EGLDisplay dpy, EGLSyncKHR sync);
		//EGLAPI EGLint EGLAPIENTRY eglClientWaitSyncKHR (EGLDisplay dpy, EGLSyncKHR sync, EGLint flags, EGLTimeKHR timeout);
		//EGLAPI EGLBoolean EGLAPIENTRY eglSignalSyncKHR (EGLDisplay dpy, EGLSyncKHR sync, EGLenum mode);
		//EGLAPI EGLBoolean EGLAPIENTRY eglGetSyncAttribKHR (EGLDisplay dpy, EGLSyncKHR sync, EGLint attribute, EGLint *value);
		
		public const int EGL_KHR_stream = 1;
		public static readonly IntPtr EGL_NO_STREAM_KHR = IntPtr.Zero;
		public const int EGL_CONSUMER_LATENCY_USEC_KHR = 0x3210;
		public const int EGL_PRODUCER_FRAME_KHR = 0x3212;
		public const int EGL_CONSUMER_FRAME_KHR = 0x3213;
		public const int EGL_STREAM_STATE_KHR = 0x3214;
		public const int EGL_STREAM_STATE_CREATED_KHR = 0x3215;
		public const int EGL_STREAM_STATE_CONNECTING_KHR = 0x3216;
		public const int EGL_STREAM_STATE_EMPTY_KHR = 0x3217;
		public const int EGL_STREAM_STATE_NEW_FRAME_AVAILABLE_KHR = 0x3218;
		public const int EGL_STREAM_STATE_OLD_FRAME_AVAILABLE_KHR = 0x3219;
		public const int EGL_STREAM_STATE_DISCONNECTED_KHR = 0x321A;
		public const int EGL_BAD_STREAM_KHR = 0x321B;
		public const int EGL_BAD_STATE_KHR = 0x321C;

		//typedef EGLStreamKHR (EGLAPIENTRYP PFNEGLCREATESTREAMKHRPROC) (EGLDisplay dpy, const EGLint *attrib_list);
		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLDESTROYSTREAMKHRPROC) (EGLDisplay dpy, EGLStreamKHR stream);
		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLSTREAMATTRIBKHRPROC) (EGLDisplay dpy, EGLStreamKHR stream, EGLenum attribute, EGLint value);
		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLQUERYSTREAMKHRPROC) (EGLDisplay dpy, EGLStreamKHR stream, EGLenum attribute, EGLint *value);
		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLQUERYSTREAMU64KHRPROC) (EGLDisplay dpy, EGLStreamKHR stream, EGLenum attribute, EGLuint64KHR *value);
		//EGLAPI EGLStreamKHR EGLAPIENTRY eglCreateStreamKHR (EGLDisplay dpy, const EGLint *attrib_list);
		//EGLAPI EGLBoolean EGLAPIENTRY eglDestroyStreamKHR (EGLDisplay dpy, EGLStreamKHR stream);
		//EGLAPI EGLBoolean EGLAPIENTRY eglStreamAttribKHR (EGLDisplay dpy, EGLStreamKHR stream, EGLenum attribute, EGLint value);
		//EGLAPI EGLBoolean EGLAPIENTRY eglQueryStreamKHR (EGLDisplay dpy, EGLStreamKHR stream, EGLenum attribute, EGLint *value);
		//EGLAPI EGLBoolean EGLAPIENTRY eglQueryStreamu64KHR (EGLDisplay dpy, EGLStreamKHR stream, EGLenum attribute, EGLuint64KHR *value);

		public const int EGL_KHR_stream_consumer_gltexture = 1;
		public const int EGL_CONSUMER_ACQUIRE_TIMEOUT_USEC_KHR = 0x321E;

		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLSTREAMCONSUMERGLTEXTUREEXTERNALKHRPROC) (EGLDisplay dpy, EGLStreamKHR stream);
		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLSTREAMCONSUMERACQUIREKHRPROC) (EGLDisplay dpy, EGLStreamKHR stream);
		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLSTREAMCONSUMERRELEASEKHRPROC) (EGLDisplay dpy, EGLStreamKHR stream);
		//EGLAPI EGLBoolean EGLAPIENTRY eglStreamConsumerGLTextureExternalKHR (EGLDisplay dpy, EGLStreamKHR stream);
		//EGLAPI EGLBoolean EGLAPIENTRY eglStreamConsumerAcquireKHR (EGLDisplay dpy, EGLStreamKHR stream);
		//EGLAPI EGLBoolean EGLAPIENTRY eglStreamConsumerReleaseKHR (EGLDisplay dpy, EGLStreamKHR stream);
		
		public const int EGL_KHR_stream_cross_process_fd = 1;

		//typedef int EGLNativeFileDescriptorKHR;

		public const int EGL_NO_FILE_DESCRIPTOR_KHR = -1;

		//typedef EGLNativeFileDescriptorKHR (EGLAPIENTRYP PFNEGLGETSTREAMFILEDESCRIPTORKHRPROC) (EGLDisplay dpy, EGLStreamKHR stream);
		//typedef EGLStreamKHR (EGLAPIENTRYP PFNEGLCREATESTREAMFROMFILEDESCRIPTORKHRPROC) (EGLDisplay dpy, EGLNativeFileDescriptorKHR file_descriptor);
		//EGLAPI EGLNativeFileDescriptorKHR EGLAPIENTRY eglGetStreamFileDescriptorKHR (EGLDisplay dpy, EGLStreamKHR stream);
		//EGLAPI EGLStreamKHR EGLAPIENTRY eglCreateStreamFromFileDescriptorKHR (EGLDisplay dpy, EGLNativeFileDescriptorKHR file_descriptor);
		
		public const int EGL_KHR_stream_fifo = 1;
		public const int EGL_STREAM_FIFO_LENGTH_KHR = 0x31FC;
		public const int EGL_STREAM_TIME_NOW_KHR = 0x31FD;
		public const int EGL_STREAM_TIME_CONSUMER_KHR = 0x31FE;
		public const int EGL_STREAM_TIME_PRODUCER_KHR = 0x31FF;

		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLQUERYSTREAMTIMEKHRPROC) (EGLDisplay dpy, EGLStreamKHR stream, EGLenum attribute, EGLTimeKHR *value);
		//EGLAPI EGLBoolean EGLAPIENTRY eglQueryStreamTimeKHR (EGLDisplay dpy, EGLStreamKHR stream, EGLenum attribute, EGLTimeKHR *value);
		
		public const int EGL_KHR_stream_producer_aldatalocator = 1;
		public const int EGL_KHR_stream_producer_eglsurface = 1;
		public const int EGL_STREAM_BIT_KHR = 0x0800;

		//typedef EGLSurface (EGLAPIENTRYP PFNEGLCREATESTREAMPRODUCERSURFACEKHRPROC) (EGLDisplay dpy, EGLConfig config, EGLStreamKHR stream, const EGLint *attrib_list);
		//EGLAPI EGLSurface EGLAPIENTRY eglCreateStreamProducerSurfaceKHR (EGLDisplay dpy, EGLConfig config, EGLStreamKHR stream, const EGLint *attrib_list);
	
		public const int EGL_KHR_surfaceless_context = 1;
		public const int EGL_KHR_vg_parent_image = 1;
		public const int EGL_VG_PARENT_IMAGE_KHR = 0x30BA;
		public const int EGL_KHR_wait_sync = 1;

		//typedef EGLint (EGLAPIENTRYP PFNEGLWAITSYNCKHRPROC) (EGLDisplay dpy, EGLSyncKHR sync, EGLint flags);
		//EGLAPI EGLint EGLAPIENTRY eglWaitSyncKHR (EGLDisplay dpy, EGLSyncKHR sync, EGLint flags);
		
		public const int EGL_ANDROID_blob_cache = 1;

		//typedef khronos_ssize_t EintANDROID;
		//typedef void (*EGLSetBlobFuncANDROID) (const void *key, EintANDROID keySize, const void *value, EintANDROID valueSize);
		//typedef EintANDROID (*EGLGetBlobFuncANDROID) (const void *key, EintANDROID keySize, void *value, EintANDROID valueSize);
		//typedef void (EGLAPIENTRYP PFNEGLSETBLOBCACHEFUNCSANDROIDPROC) (EGLDisplay dpy, EGLSetBlobFuncANDROID set, EGLGetBlobFuncANDROID get);
		//EGLAPI void EGLAPIENTRY eglSetBlobCacheFuncsANDROID (EGLDisplay dpy, EGLSetBlobFuncANDROID set, EGLGetBlobFuncANDROID get);
		
		public const int EGL_ANDROID_framebuffer_target = 1;
		public const int EGL_FRAMEBUFFER_TARGET_ANDROID = 0x3147;
		public const int EGL_ANDROID_image_native_buffer = 1;
		public const int EGL_NATIVE_BUFFER_ANDROID = 0x3140;
		public const int EGL_ANDROID_native_fence_sync = 1;
		public const int EGL_SYNC_NATIVE_FENCE_ANDROID = 0x3144;
		public const int EGL_SYNC_NATIVE_FENCE_FD_ANDROID = 0x3145;
		public const int EGL_SYNC_NATIVE_FENCE_SIGNALED_ANDROID = 0x3146;
		public const int EGL_NO_NATIVE_FENCE_FD_ANDROID = -1;

		//typedef EGLint (EGLAPIENTRYP PFNEGLDUPNATIVEFENCEFDANDROIDPROC) (EGLDisplay dpy, EGLSyncKHR sync);
		//EGLAPI EGLint EGLAPIENTRY eglDupNativeFenceFDANDROID (EGLDisplay dpy, EGLSyncKHR sync);
		
		public const int EGL_ANDROID_recordable = 1;
		public const int EGL_RECORDABLE_ANDROID = 0x3142;
		public const int EGL_ANGLE_d3d_share_handle_client_buffer = 1;
		public const int EGL_D3D_TEXTURE_2D_SHARE_HANDLE_ANGLE = 0x3200;
		public const int EGL_ANGLE_window_fixed_size = 1;
		public const int EGL_FIXED_SIZE_ANGLE = 0x3201;
		public const int EGL_ANGLE_query_surface_pointer = 1;

		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLQUERYSURFACEPOINTERANGLEPROC) (EGLDisplay dpy, EGLSurface surface, EGLint attribute, void **value);
		//EGLAPI EGLBoolean EGLAPIENTRY eglQuerySurfacePointerANGLE (EGLDisplay dpy, EGLSurface surface, EGLint attribute, void **value);
		
		public const int EGL_ANGLE_software_display = 1;
		public const int EGL_SOFTWARE_DISPLAY_ANGLE = -1;
		public const int EGL_ANGLE_direct3d_display = 1;
		public const int EGL_D3D11_ELSE_D3D9_DISPLAY_ANGLE = -2;
		public const int EGL_D3D11_ONLY_DISPLAY_ANGLE = -3;
		public const int EGL_ANGLE_surface_d3d_texture_2d_share_handle = 1;
		public const int EGL_ARM_pixmap_multisample_discard = 1;
		public const int EGL_DISCARD_SAMPLES_ARM = 0x3286;
		public const int EGL_EXT_buffer_age = 1;
		public const int EGL_BUFFER_AGE_EXT = 0x313D;
		public const int EGL_EXT_client_extensions = 1;
		public const int EGL_EXT_create_context_robustness = 1;
		public const int EGL_CONTEXT_OPENGL_ROBUST_ACCESS_EXT = 0x30BF;
		public const int EGL_CONTEXT_OPENGL_RESET_NOTIFICATION_STRATEGY_EXT = 0x3138;
		public const int EGL_NO_RESET_NOTIFICATION_EXT = 0x31BE;
		public const int EGL_LOSE_CONTEXT_ON_RESET_EXT = 0x31BF;
		public const int EGL_EXT_device_base = 1;
		public const int EGL_NO_DEVICE_EXT = 0;
		public const int EGL_BAD_DEVICE_EXT = 0x322B;
		public const int EGL_DEVICE_EXT = 0x322C;

		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLQUERYDEVICEATTRIBEXTPROC) (EGLDeviceEXT device, EGLint attribute, EGLAttrib *value);
		//typedef const char *(EGLAPIENTRYP PFNEGLQUERYDEVICESTRINGEXTPROC) (EGLDeviceEXT device, EGLint name);
		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLQUERYDEVICESEXTPROC) (EGLint max_devices, EGLDeviceEXT *devices, EGLint *num_devices);
		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLQUERYDISPLAYATTRIBEXTPROC) (EGLDisplay dpy, EGLint attribute, EGLAttrib *value);
		//EGLAPI EGLBoolean EGLAPIENTRY eglQueryDeviceAttribEXT (EGLDeviceEXT device, EGLint attribute, EGLAttrib *value);
		//EGLAPI const char *EGLAPIENTRY eglQueryDeviceStringEXT (EGLDeviceEXT device, EGLint name);
		//EGLAPI EGLBoolean EGLAPIENTRY eglQueryDevicesEXT (EGLint max_devices, EGLDeviceEXT *devices, EGLint *num_devices);
		//EGLAPI EGLBoolean EGLAPIENTRY eglQueryDisplayAttribEXT (EGLDisplay dpy, EGLint attribute, EGLAttrib *value);
	
		public const int EGL_EXT_image_dma_buf_import = 1;
		public const int EGL_LINUX_DMA_BUF_EXT = 0x3270;
		public const int EGL_LINUX_DRM_FOURCC_EXT = 0x3271;
		public const int EGL_DMA_BUF_PLANE0_FD_EXT = 0x3272;
		public const int EGL_DMA_BUF_PLANE0_OFFSET_EXT = 0x3273;
		public const int EGL_DMA_BUF_PLANE0_PITCH_EXT = 0x3274;
		public const int EGL_DMA_BUF_PLANE1_FD_EXT = 0x3275;
		public const int EGL_DMA_BUF_PLANE1_OFFSET_EXT = 0x3276;
		public const int EGL_DMA_BUF_PLANE1_PITCH_EXT = 0x3277;
		public const int EGL_DMA_BUF_PLANE2_FD_EXT = 0x3278;
		public const int EGL_DMA_BUF_PLANE2_OFFSET_EXT = 0x3279;
		public const int EGL_DMA_BUF_PLANE2_PITCH_EXT = 0x327A;
		public const int EGL_YUV_COLOR_SPACE_HINT_EXT = 0x327B;
		public const int EGL_SAMPLE_RANGE_HINT_EXT = 0x327C;
		public const int EGL_YUV_CHROMA_HORIZONTAL_SITING_HINT_EXT = 0x327D;
		public const int EGL_YUV_CHROMA_VERTICAL_SITING_HINT_EXT = 0x327E;
		public const int EGL_ITU_REC601_EXT = 0x327F;
		public const int EGL_ITU_REC709_EXT = 0x3280;
		public const int EGL_ITU_REC2020_EXT = 0x3281;
		public const int EGL_YUV_FULL_RANGE_EXT = 0x3282;
		public const int EGL_YUV_NARROW_RANGE_EXT = 0x3283;
		public const int EGL_YUV_CHROMA_SITING_0_EXT = 0x3284;
		public const int EGL_YUV_CHROMA_SITING_0_5_EXT = 0x3285;
		public const int EGL_EXT_multiview_window = 1;
		public const int EGL_MULTIVIEW_VIEW_COUNT_EXT = 0x3134;
		public const int EGL_EXT_platform_base = 1;

		//typedef EGLDisplay (EGLAPIENTRYP PFNEGLGETPLATFORMDISPLAYEXTPROC) (EGLenum platform, void *native_display, const EGLint *attrib_list);
		//typedef EGLSurface (EGLAPIENTRYP PFNEGLCREATEPLATFORMWINDOWSURFACEEXTPROC) (EGLDisplay dpy, EGLConfig config, void *native_window, const EGLint *attrib_list);
		//typedef EGLSurface (EGLAPIENTRYP PFNEGLCREATEPLATFORMPIXMAPSURFACEEXTPROC) (EGLDisplay dpy, EGLConfig config, void *native_pixmap, const EGLint *attrib_list);
		//EGLAPI EGLDisplay EGLAPIENTRY eglGetPlatformDisplayEXT (EGLenum platform, void *native_display, const EGLint *attrib_list);
		//EGLAPI EGLSurface EGLAPIENTRY eglCreatePlatformWindowSurfaceEXT (EGLDisplay dpy, EGLConfig config, void *native_window, const EGLint *attrib_list);
		//EGLAPI EGLSurface EGLAPIENTRY eglCreatePlatformPixmapSurfaceEXT (EGLDisplay dpy, EGLConfig config, void *native_pixmap, const EGLint *attrib_list);
	
		public const int EGL_EXT_platform_device = 1;
		public const int EGL_PLATFORM_DEVICE_EXT = 0x313F;
		public const int EGL_EXT_platform_wayland = 1;
		public const int EGL_PLATFORM_WAYLAND_EXT = 0x31D8;
		public const int EGL_EXT_platform_x11 = 1;
		public const int EGL_PLATFORM_X11_EXT = 0x31D5;
		public const int EGL_PLATFORM_X11_SCREEN_EXT = 0x31D6;
		public const int EGL_EXT_protected_surface = 1;
		public const int EGL_PROTECTED_CONTENT_EXT = 0x32C0;
		public const int EGL_EXT_swap_buffers_with_damage = 1;

		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLSWAPBUFFERSWITHDAMAGEEXTPROC) (EGLDisplay dpy, EGLSurface surface, EGLint *rects, EGLint n_rects);
		//EGLAPI EGLBoolean EGLAPIENTRY eglSwapBuffersWithDamageEXT (EGLDisplay dpy, EGLSurface surface, EGLint *rects, EGLint n_rects);
	
		public const int EGL_HI_clientpixmap = 1;

		[StructLayout(LayoutKind.Sequential)]
		public struct EGLClientPixmapHI
		{
			public IntPtr Data;
			public int Width;
			public int Height;
			public int Stride;
		}

		public const int EGL_CLIENT_PIXMAP_POINTER_HI = 0x8F74;
		//typedef EGLSurface (EGLAPIENTRYP PFNEGLCREATEPIXMAPSURFACEHIPROC) (EGLDisplay dpy, EGLConfig config, struct EGLClientPixmapHI *pixmap);
		//EGL_EGLEXT_PROTOTYPES
		//EGLAPI EGLSurface EGLAPIENTRY eglCreatePixmapSurfaceHI (EGLDisplay dpy, EGLConfig config, struct EGLClientPixmapHI *pixmap);

		public const int EGL_HI_colorformats = 1;
		public const int EGL_COLOR_FORMAT_HI = 0x8F70;
		public const int EGL_COLOR_RGB_HI = 0x8F71;
		public const int EGL_COLOR_RGBA_HI = 0x8F72;
		public const int EGL_COLOR_ARGB_HI = 0x8F73;
		public const int EGL_IMG_context_priority = 1;
		public const int EGL_CONTEXT_PRIORITY_LEVEL_IMG = 0x3100;
		public const int EGL_CONTEXT_PRIORITY_HIGH_IMG = 0x3101;
		public const int EGL_CONTEXT_PRIORITY_MEDIUM_IMG = 0x3102;
		public const int EGL_CONTEXT_PRIORITY_LOW_IMG = 0x3103;
		public const int EGL_MESA_drm_image = 1;
		public const int EGL_DRM_BUFFER_FORMAT_MESA = 0x31D0;
		public const int EGL_DRM_BUFFER_USE_MESA = 0x31D1;
		public const int EGL_DRM_BUFFER_FORMAT_ARGB32_MESA = 0x31D2;
		public const int EGL_DRM_BUFFER_MESA = 0x31D3;
		public const int EGL_DRM_BUFFER_STRIDE_MESA = 0x31D4;
		public const int EGL_DRM_BUFFER_USE_SCANOUT_MESA = 0x00000001;
		public const int EGL_DRM_BUFFER_USE_SHARE_MESA = 0x00000002;
		
		//typedef EGLImageKHR (EGLAPIENTRYP PFNEGLCREATEDRMIMAGEMESAPROC) (EGLDisplay dpy, const EGLint *attrib_list);
		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLEXPORTDRMIMAGEMESAPROC) (EGLDisplay dpy, EGLImageKHR image, EGLint *name, EGLint *handle, EGLint *stride);
		//EGLAPI EGLImageKHR EGLAPIENTRY eglCreateDRMImageMESA (EGLDisplay dpy, const EGLint *attrib_list);
		//EGLAPI EGLBoolean EGLAPIENTRY eglExportDRMImageMESA (EGLDisplay dpy, EGLImageKHR image, EGLint *name, EGLint *handle, EGLint *stride);
	
		public const int EGL_MESA_platform_gbm = 1;
		public const int EGL_PLATFORM_GBM_MESA = 0x31D7;
		public const int EGL_NOK_swap_region = 1;

		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLSWAPBUFFERSREGIONNOKPROC) (EGLDisplay dpy, EGLSurface surface, EGLint numRects, const EGLint *rects);
		//EGLAPI EGLBoolean EGLAPIENTRY eglSwapBuffersRegionNOK (EGLDisplay dpy, EGLSurface surface, EGLint numRects, const EGLint *rects);
	
		public const int EGL_NOK_swap_region2 = 1;

		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLSWAPBUFFERSREGION2NOKPROC) (EGLDisplay dpy, EGLSurface surface, EGLint numRects, const EGLint *rects);
		//EGLAPI EGLBoolean EGLAPIENTRY eglSwapBuffersRegion2NOK (EGLDisplay dpy, EGLSurface surface, EGLint numRects, const EGLint *rects);
	
		public const int EGL_NOK_texture_from_pixmap = 1;
		public const int EGL_Y_INVERTED_NOK = 0x307F;
		public const int EGL_NV_3dvision_surface = 1;
		public const int EGL_AUTO_STEREO_NV = 0x3136;
		public const int EGL_NV_coverage_sample = 1;
		public const int EGL_COVERAGE_BUFFERS_NV = 0x30E0;
		public const int EGL_COVERAGE_SAMPLES_NV = 0x30E1;
		public const int EGL_NV_coverage_sample_resolve = 1;
		public const int EGL_COVERAGE_SAMPLE_RESOLVE_NV = 0x3131;
		public const int EGL_COVERAGE_SAMPLE_RESOLVE_DEFAULT_NV = 0x3132;
		public const int EGL_COVERAGE_SAMPLE_RESOLVE_NONE_NV = 0x3133;
		public const int EGL_NV_depth_nonlinear = 1;
		public const int EGL_DEPTH_ENCODING_NV = 0x30E2;
		public const int EGL_DEPTH_ENCODING_NONE_NV = 0;
		public const int EGL_DEPTH_ENCODING_NONLINEAR_NV = 0x30E3;
		public const int EGL_NV_native_query = 1;

		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLQUERYNATIVEDISPLAYNVPROC) (EGLDisplay dpy, EGLNativeDisplayType *display_id);
		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLQUERYNATIVEWINDOWNVPROC) (EGLDisplay dpy, EGLSurface surf, EGLNativeWindowType *window);
		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLQUERYNATIVEPIXMAPNVPROC) (EGLDisplay dpy, EGLSurface surf, EGLNativePixmapType *pixmap);
		//EGLAPI EGLBoolean EGLAPIENTRY eglQueryNativeDisplayNV (EGLDisplay dpy, EGLNativeDisplayType *display_id);
		//EGLAPI EGLBoolean EGLAPIENTRY eglQueryNativeWindowNV (EGLDisplay dpy, EGLSurface surf, EGLNativeWindowType *window);
		//EGLAPI EGLBoolean EGLAPIENTRY eglQueryNativePixmapNV (EGLDisplay dpy, EGLSurface surf, EGLNativePixmapType *pixmap);
		
		public const int EGL_NV_post_convert_rounding = 1;
		public const int EGL_NV_post_sub_buffer = 1;
		public const int EGL_POST_SUB_BUFFER_SUPPORTED_NV = 0x30BE;

		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLPOSTSUBBUFFERNVPROC) (EGLDisplay dpy, EGLSurface surface, EGLint x, EGLint y, EGLint width, EGLint height);
		//EGLAPI EGLBoolean EGLAPIENTRY eglPostSubBufferNV (EGLDisplay dpy, EGLSurface surface, EGLint x, EGLint y, EGLint width, EGLint height);
	
		public const int EGL_NV_stream_sync = 1;
		public const int EGL_SYNC_NEW_FRAME_NV = 0x321F;

		//typedef EGLSyncKHR (EGLAPIENTRYP PFNEGLCREATESTREAMSYNCNVPROC) (EGLDisplay dpy, EGLStreamKHR stream, EGLenum type, const EGLint *attrib_list);
		//EGLAPI EGLSyncKHR EGLAPIENTRY eglCreateStreamSyncNV (EGLDisplay dpy, EGLStreamKHR stream, EGLenum type, const EGLint *attrib_list);
	
		public const int EGL_NV_sync = 1;

		//typedef void *EGLSyncNV;
		//typedef khronos_utime_nanoseconds_t EGLTimeNV;
	
		public const int EGL_SYNC_PRIOR_COMMANDS_COMPLETE_NV = 0x30E6;
		public const int EGL_SYNC_STATUS_NV = 0x30E7;
		public const int EGL_SIGNALED_NV = 0x30E8;
		public const int EGL_UNSIGNALED_NV = 0x30E9;
		public const int EGL_SYNC_FLUSH_COMMANDS_BIT_NV = 0x0001;
		public const int EGL_FOREVER_NV = -1;
		public const int EGL_ALREADY_SIGNALED_NV = 0x30EA;
		public const int EGL_TIMEOUT_EXPIRED_NV = 0x30EB;
		public const int EGL_CONDITION_SATISFIED_NV = 0x30EC;
		public const int EGL_SYNC_TYPE_NV = 0x30ED;
		public const int EGL_SYNC_CONDITION_NV = 0x30EE;
		public const int EGL_SYNC_FENCE_NV = 0x30EF;
		public static readonly IntPtr EGL_NO_SYNC_NV = IntPtr.Zero;

		//typedef EGLSyncNV (EGLAPIENTRYP PFNEGLCREATEFENCESYNCNVPROC) (EGLDisplay dpy, EGLenum condition, const EGLint *attrib_list);
		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLDESTROYSYNCNVPROC) (EGLSyncNV sync);
		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLFENCENVPROC) (EGLSyncNV sync);
		//typedef EGLint (EGLAPIENTRYP PFNEGLCLIENTWAITSYNCNVPROC) (EGLSyncNV sync, EGLint flags, EGLTimeNV timeout);
		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLSIGNALSYNCNVPROC) (EGLSyncNV sync, EGLenum mode);
		//typedef EGLBoolean (EGLAPIENTRYP PFNEGLGETSYNCATTRIBNVPROC) (EGLSyncNV sync, EGLint attribute, EGLint *value);
		//EGLAPI EGLSyncNV EGLAPIENTRY eglCreateFenceSyncNV (EGLDisplay dpy, EGLenum condition, const EGLint *attrib_list);
		//EGLAPI EGLBoolean EGLAPIENTRY eglDestroySyncNV (EGLSyncNV sync);
		//EGLAPI EGLBoolean EGLAPIENTRY eglFenceNV (EGLSyncNV sync);
		//EGLAPI EGLint EGLAPIENTRY eglClientWaitSyncNV (EGLSyncNV sync, EGLint flags, EGLTimeNV timeout);
		//EGLAPI EGLBoolean EGLAPIENTRY eglSignalSyncNV (EGLSyncNV sync, EGLenum mode);
		//EGLAPI EGLBoolean EGLAPIENTRY eglGetSyncAttribNV (EGLSyncNV sync, EGLint attribute, EGLint *value);
		
		public const int EGL_NV_system_time = 1;

		//typedef khronos_utime_nanoseconds_t EGLuint64NV;
		//typedef EGLuint64NV (EGLAPIENTRYP PFNEGLGETSYSTEMTIMEFREQUENCYNVPROC) (void);
		//typedef EGLuint64NV (EGLAPIENTRYP PFNEGLGETSYSTEMTIMENVPROC) (void);
		//EGLAPI EGLuint64NV EGLAPIENTRY eglGetSystemTimeFrequencyNV (void);
		//EGLAPI EGLuint64NV EGLAPIENTRY eglGetSystemTimeNV (void);
	}
}
