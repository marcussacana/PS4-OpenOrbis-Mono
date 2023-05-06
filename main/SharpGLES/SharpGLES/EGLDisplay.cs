using System;
using System.Runtime.InteropServices;
using Orbis.Internals;

namespace SharpGLES
{
	public class EGLDisplay : IDisposable
	{
		private uint Width, Height;
		
		private IntPtr _nativeDisplay = IntPtr.Zero;
		private IntPtr _display;
		private IntPtr _surface;
		private IntPtr _context;
		private IntPtr _handle;
		
		
		/// <summary>
		/// Initialize the EGL and Piglet if running in PS4
		/// </summary>
		/// <param name="handle">Used only In PC</param>
		/// <param name="Width">Used only In PS4</param>
		/// <param name="Height">Used only In PS4</param>
		public EGLDisplay(IntPtr handle, uint Width, uint Height)
		{
			this.Width = Width;
			this.Height = Height;
			
#if ORBIS
			InitializePiglet();
			
#else
			_handle = handle;
#endif
			
			InitializeWindow();
			InitializeEL();
		}

		public void Dispose()
		{
			EGL.Terminate(_display);

			EGL.DestroySurface(_display, _surface);

			EGL.DestroyContext(_display, _context);

			EGLDC.ReleaseDC(_handle, _nativeDisplay);
		}

		public void SwapBuffers()
		{
			EGL.SwapBuffers(_display, _surface);
		}

		private unsafe void InitializeShaderCompiler()
		{
			if (GLES20.HasShaderCompiler)
				return;
			
			var ShaderModule  = Kernel.LoadStartModule("libSceShaccVSH.prx", out _);

			if ((ShaderModule & 0x80000000) != 0)
			{
				Kernel.Log("OpenGL Shader Compiler Unavailable");
				return;
			}
			
			
			Kernel.Log("OpenGL Shader Compiler Found - Applying Patches...");
			if (!Kernel.GetModuleBase(EGL.Path, out long BaseAddress, out long Size))
			{
				Kernel.Log("OpenGL Shader Compiler Unavailable - Failed to find the Piglet Base Address");
				return;
			}

			bool Jailbroken = Kernel.IsJailbroken();

			if (!Jailbroken)
				Kernel.Jailbreak();
			
			//THX FLATZ I'M YOUR FAN
			/* XXX: patches below are given for Piglet module from 4.74 Devkit PUP */
			byte* pTarget = (byte*)BaseAddress+0x5451F;
			byte[] SetEAXTo1 = new byte[] { 0x31, 0xC0, 0xFF, 0xC0, 0x90 };

			for (int i = 0; i < SetEAXTo1.Length; i++)
			{
				pTarget[i] = SetEAXTo1[i];
			}
			
			/* Tell that runtime compiler exists */
			*((byte*)BaseAddress + 0xB2DEC) = 0;
			*((byte*)BaseAddress + 0xB2DED) = 0;
			*((byte*)BaseAddress + 0xB2DEE) = 1;
			*((byte*)BaseAddress + 0xB2E21) = 1;
			
			/* Inform Piglet that we have shader compiler module loaded */
			*((int*)BaseAddress +0xB2E24) = ShaderModule;

			GLES20.HasShaderCompiler = true;
			Kernel.Log("OpenGL Shader Compiler Enabled");
		}

		private const uint KB = 1024;
		private const uint MB = KB * 1024;
		private const uint GB = MB * 1024;
		private void InitializePiglet()
		{
			var Module  = Kernel.LoadStartModule(EGL.Path, out _);

			if ((Module & 0x80000000) != 0)
			{
				throw new DllNotFoundException($"LoadStartModule({EGL.Path}) result 0x{Module:X8}");
			}
			
			EGL.ScePglConfig Config = new EGL.ScePglConfig();
			
			Config.size = (uint)Marshal.SizeOf(typeof(EGL.ScePglConfig));
			Config.flags = EGL.ORBIS_PGL_FLAGS_USE_COMPOSITE_EXT | EGL.ORBIS_PGL_FLAGS_USE_FLEXIBLE_MEMORY | 0x60;
			Config.processOrder = 1;
			Config.systemSharedMemorySize = 250 * MB;
			Config.maxMappedFlexibleMemory = 170 * MB;
			Config.drawCommandBufferSize = 1 * MB;
			Config.lcueResourceBufferSize = 1 * MB;
			Config.dbgPosCmd_0x40 = Width;
			Config.dbgPosCmd_0x44 = Height;
			Config.dbgPosCmd_0x48 = 0;
			Config.dbgPosCmd_0x4C = 0;
			Config.unk_0x5C = 2;

			if (!EGL.scePigletSetConfigurationVSH(Config))
			{
				throw new Exception("Set Piglet configuration Failed");
			}
			
			
		}
		
		private void InitializeWindow()
		{
#if !ORBIS
			_nativeDisplay = EGLDC.GetDC(_handle);
#endif
			IntPtr requestedRenderer = _nativeDisplay;

			/*if (requestedRenderer == RENDERER_D3D11)
			{
				requestedRenderer = Hook.EGL_D3D11_ONLY_DISPLAY_ANGLE;
			}*/

			_display = EGL.GetDisplay(requestedRenderer);

			int minor;
			int major;

			if (!EGL.Initialize(_display, out major, out minor))
			{
				throw new EGLException("Initialize failed.");
			}

			if (!EGL.BindAPI(EGL.EGL_OPENGL_ES_API))
			{
				throw new EGLException("BindAPI failed.");
			}
		}

		private void InitializeEL()
		{
			
#if ORBIS
			int[] configAttributes =
			{
				EGL.EGL_RED_SIZE, 8,
				EGL.EGL_GREEN_SIZE, 8,
				EGL.EGL_BLUE_SIZE, 8,
				EGL.EGL_ALPHA_SIZE, 8,
				EGL.EGL_DEPTH_SIZE, 0,
				EGL.EGL_STENCIL_SIZE, 0,
				EGL.EGL_SAMPLE_BUFFERS, 0,
				EGL.EGL_SAMPLES, 4,  // This is for 4x MSAA.
				EGL.EGL_RENDERABLE_TYPE, EGL.EGL_OPENGL_ES2_BIT,
				EGL.EGL_SURFACE_TYPE, EGL.EGL_WINDOW_BIT,
				EGL.EGL_NONE
			};
#else
			int[] configAttributes =
			{
				EGL.EGL_RED_SIZE, 8,
				EGL.EGL_GREEN_SIZE, 8,
				EGL.EGL_BLUE_SIZE, 8,
				EGL.EGL_ALPHA_SIZE, 8,
				EGL.EGL_DEPTH_SIZE, 24,
				EGL.EGL_STENCIL_SIZE, 8,
				EGL.EGL_SAMPLE_BUFFERS, EGL.EGL_DONT_CARE,
				EGL.EGL_SAMPLES, 4,  // This is for 4x MSAA.
				EGL.EGL_NONE
			};
#endif

			int configCount;
			IntPtr configs;

			if (!EGL.ChooseConfig(_display, configAttributes, out configs, 1, out configCount))
			{
				throw new EGLException("ChooseConfig failed.");
			}

			int[] surfaceAttributes =
			{
				EGLX.EGL_POST_SUB_BUFFER_SUPPORTED_NV, EGL.EGL_TRUE, EGL.EGL_NONE, EGL.EGL_NONE
			};

			_surface = EGL.CreateWindowSurface(_display, configs, _handle, surfaceAttributes);

			int[] contextAttibutes =
			{
				EGL.EGL_CONTEXT_CLIENT_VERSION, 2, EGL.EGL_NONE
			};

			_context = EGL.CreateContext(_display, configs, IntPtr.Zero, contextAttibutes);

			EGL.MakeCurrent(_display, _surface, _surface, _context);

			EGL.SwapInterval(_display, 0);
		}
	}
}
