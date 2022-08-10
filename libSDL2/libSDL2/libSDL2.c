#include <SDL2/SDL.h> 
#include <SDL2/SDL_assert.h> 
#include <SDL2/SDL_atomic.h> 
#include <SDL2/SDL_audio.h> 
#include <SDL2/SDL_bits.h> 
#include <SDL2/SDL_blendmode.h> 
#include <SDL2/SDL_clipboard.h> 
#include <SDL2/SDL_config.h> 
#include <SDL2/SDL_config_android.h> 
#include <SDL2/SDL_config_iphoneos.h> 
//#include <SDL2/SDL_config_macosx.h> 
#include <SDL2/SDL_config_minimal.h> 
#include <SDL2/SDL_config_os2.h> 
#include <SDL2/SDL_config_pandora.h> 
#include <SDL2/SDL_config_ps4.h> 
#include <SDL2/SDL_config_psp.h> 
//#include <SDL2/SDL_config_windows.h> 
//#include <SDL2/SDL_config_winrt.h> 
#include <SDL2/SDL_config_wiz.h> 
#include <SDL2/SDL_copying.h> 
#include <SDL2/SDL_cpuinfo.h> 
#include <SDL2/SDL_egl.h> 
#include <SDL2/SDL_endian.h> 
#include <SDL2/SDL_error.h> 
#include <SDL2/SDL_events.h> 
#include <SDL2/SDL_filesystem.h> 
#include <SDL2/SDL_gamecontroller.h> 
#include <SDL2/SDL_gesture.h> 
#include <SDL2/SDL_haptic.h> 
#include <SDL2/SDL_hints.h> 
#include <SDL2/SDL_image.h> 
#include <SDL2/SDL_joystick.h> 
#include <SDL2/SDL_keyboard.h> 
#include <SDL2/SDL_keycode.h> 
#include <SDL2/SDL_loadso.h> 
#include <SDL2/SDL_log.h> 
#include <SDL2/SDL_main.h> 
#include <SDL2/SDL_messagebox.h> 
#include <SDL2/SDL_mouse.h> 
#include <SDL2/SDL_mutex.h> 
#include <SDL2/SDL_name.h> 
#include <SDL2/SDL_opengl.h> 
#include <SDL2/SDL_opengl_glext.h> 
//#include <SDL2/SDL_opengles.h> 
#include <SDL2/SDL_opengles2.h> 
#include <SDL2/SDL_opengles2_gl2.h> 
#include <SDL2/SDL_opengles2_gl2ext.h> 
#include <SDL2/SDL_opengles2_gl2platform.h> 
#include <SDL2/SDL_opengles2_khrplatform.h> 
#include <SDL2/SDL_pixels.h> 
#include <SDL2/SDL_platform.h> 
#include <SDL2/SDL_power.h> 
#include <SDL2/SDL_quit.h> 
#include <SDL2/SDL_rect.h> 
#include <SDL2/SDL_render.h> 
#include <SDL2/SDL_revision.h> 
#include <SDL2/SDL_rwops.h> 
#include <SDL2/SDL_scancode.h> 
#include <SDL2/SDL_sensor.h> 
#include <SDL2/SDL_shape.h> 
#include <SDL2/SDL_stdinc.h> 
#include <SDL2/SDL_surface.h> 
#include <SDL2/SDL_system.h> 
//#include <SDL2/SDL_syswm.h> 
#include <SDL2/SDL_test.h> 
#include <SDL2/SDL_test_assert.h> 
#include <SDL2/SDL_test_common.h> 
#include <SDL2/SDL_test_compare.h> 
#include <SDL2/SDL_test_crc32.h> 
#include <SDL2/SDL_test_font.h> 
#include <SDL2/SDL_test_fuzzer.h> 
#include <SDL2/SDL_test_harness.h> 
#include <SDL2/SDL_test_images.h> 
#include <SDL2/SDL_test_log.h> 
#include <SDL2/SDL_test_md5.h> 
#include <SDL2/SDL_test_memory.h> 
#include <SDL2/SDL_test_random.h> 
#include <SDL2/SDL_thread.h> 
#include <SDL2/SDL_timer.h> 
#include <SDL2/SDL_touch.h> 
#include <SDL2/SDL_ttf.h> 
#include <SDL2/SDL_types.h> 
#include <SDL2/SDL_version.h> 
#include <SDL2/SDL_video.h> 
#include <SDL2/SDL_vulkan.h>

void* sceSDL_GetHintBoolean = SDL_GetHintBoolean;
void* sceSDL_GetError = SDL_GetError;
void* sceSDL_SetError = SDL_SetError;
void* sceSDL_Log = SDL_Log;
void* sceSDL_LogVerbose = SDL_LogVerbose;
void* sceSDL_LogDebug = SDL_LogDebug;
void* sceSDL_LogInfo = SDL_LogInfo;
void* sceSDL_LogWarn = SDL_LogWarn;
void* sceSDL_LogError = SDL_LogError;
void* sceSDL_LogCritical = SDL_LogCritical;
void* sceSDL_LogMessage = SDL_LogMessage;
void* sceSDL_LogMessageV = SDL_LogMessageV;
void* sceSDL_ShowSimpleMessageBox = SDL_ShowSimpleMessageBox;
void* sceSDL_GetRevision = SDL_GetRevision;
void* sceSDL_CreateWindow = SDL_CreateWindow;
void* sceSDL_GetCurrentVideoDriver = SDL_GetCurrentVideoDriver;
void* sceSDL_GetDisplayName = SDL_GetDisplayName;
void* sceSDL_GetVideoDriver = SDL_GetVideoDriver;
void* sceSDL_GetWindowData = SDL_GetWindowData;
void* sceSDL_GetWindowTitle = SDL_GetWindowTitle;
void* sceSDL_GL_GetProcAddress = SDL_GL_GetProcAddress;
void* sceSDL_GL_LoadLibrary = SDL_GL_LoadLibrary;
void* sceSDL_GL_ExtensionSupported = SDL_GL_ExtensionSupported;
void* sceSDL_SetWindowData = SDL_SetWindowData;
void* sceSDL_SetWindowTitle = SDL_SetWindowTitle;
void* sceSDL_VideoInit = SDL_VideoInit;
void* sceSDL_Vulkan_LoadLibrary = SDL_Vulkan_LoadLibrary;
void* sceSDL_GetPixelFormatName = SDL_GetPixelFormatName;
void* sceSDL_UpperBlit = SDL_UpperBlit;
void* sceSDL_UpperBlitScaled = SDL_UpperBlitScaled;
void* sceSDL_LoadBMP_RW = SDL_LoadBMP_RW;
void* sceSDL_SaveBMP_RW = SDL_SaveBMP_RW;
void* sceSDL_GetClipboardText = SDL_GetClipboardText;
void* sceSDL_SetClipboardText = SDL_SetClipboardText;
void* sceSDL_GetScancodeName = SDL_GetScancodeName;
void* sceSDL_GetScancodeFromName = SDL_GetScancodeFromName;
void* sceSDL_GetKeyName = SDL_GetKeyName;
void* sceSDL_GetKeyFromName = SDL_GetKeyFromName;
void* sceSDL_JoystickName = SDL_JoystickName;
void* sceSDL_JoystickNameForIndex = SDL_JoystickNameForIndex;
void* sceSDL_JoystickGetGUIDFromString = SDL_JoystickGetGUIDFromString;
void* sceSDL_GameControllerAddMapping = SDL_GameControllerAddMapping;
void* sceSDL_GameControllerMappingForIndex = SDL_GameControllerMappingForIndex;
void* sceSDL_GameControllerAddMappingsFromRW = SDL_GameControllerAddMappingsFromRW;
void* sceSDL_GameControllerMappingForGUID = SDL_GameControllerMappingForGUID;
void* sceSDL_GameControllerMapping = SDL_GameControllerMapping;
void* sceSDL_GameControllerNameForIndex = SDL_GameControllerNameForIndex;
void* sceSDL_GameControllerMappingForDeviceIndex = SDL_GameControllerMappingForDeviceIndex;
void* sceSDL_GameControllerName = SDL_GameControllerName;
void* sceSDL_GameControllerGetAxisFromString = SDL_GameControllerGetAxisFromString;
void* sceSDL_GameControllerGetStringForAxis = SDL_GameControllerGetStringForAxis;
void* sceSDL_GameControllerGetBindForAxis = SDL_GameControllerGetBindForAxis;
void* sceSDL_GameControllerGetButtonFromString = SDL_GameControllerGetButtonFromString;
void* sceSDL_GameControllerGetStringForButton = SDL_GameControllerGetStringForButton;
void* sceSDL_GameControllerGetBindForButton = SDL_GameControllerGetBindForButton;
void* sceSDL_HapticName = SDL_HapticName;
void* sceSDL_SensorGetDeviceName = SDL_SensorGetDeviceName;
void* sceSDL_SensorGetName = SDL_SensorGetName;
void* sceSDL_AudioInit = SDL_AudioInit;
void* sceSDL_GetAudioDeviceName = SDL_GetAudioDeviceName;
void* sceSDL_GetAudioDriver = SDL_GetAudioDriver;
void* sceSDL_GetCurrentAudioDriver = SDL_GetCurrentAudioDriver;
void* sceSDL_LoadWAV_RW = SDL_LoadWAV_RW;
void* sceSDL_OpenAudioDevice = SDL_OpenAudioDevice;
void* sceSDL_GetBasePath = SDL_GetBasePath;
void* sceSDL_GetPrefPath = SDL_GetPrefPath;
void* sceSDL_malloc = SDL_malloc;
void* sceSDL_free = SDL_free;
void* sceSDL_RWFromMem = SDL_RWFromMem;
void* sceSDL_SetMainReady = SDL_SetMainReady;
void* sceSDL_Init = SDL_Init;
void* sceSDL_InitSubSystem = SDL_InitSubSystem;
void* sceSDL_Quit = SDL_Quit;
void* sceSDL_QuitSubSystem = SDL_QuitSubSystem;
void* sceSDL_WasInit = SDL_WasInit;
void* sceSDL_ClearHints = SDL_ClearHints;
void* sceSDL_ClearError = SDL_ClearError;
void* sceSDL_LogGetPriority = SDL_LogGetPriority;
void* sceSDL_LogSetPriority = SDL_LogSetPriority;
void* sceSDL_LogSetAllPriority = SDL_LogSetAllPriority;
void* sceSDL_LogResetPriorities = SDL_LogResetPriorities;
void* sceSDL_LogGetOutputFunction = SDL_LogGetOutputFunction;
void* sceSDL_LogSetOutputFunction = SDL_LogSetOutputFunction;
void* sceSDL_GetVersion = SDL_GetVersion;
void* sceSDL_GetRevisionNumber = SDL_GetRevisionNumber;
void* sceSDL_CreateWindowAndRenderer = SDL_CreateWindowAndRenderer;
void* sceSDL_CreateWindowFrom = SDL_CreateWindowFrom;
void* sceSDL_DestroyWindow = SDL_DestroyWindow;
void* sceSDL_DisableScreenSaver = SDL_DisableScreenSaver;
void* sceSDL_EnableScreenSaver = SDL_EnableScreenSaver;
void* sceSDL_GetClosestDisplayMode = SDL_GetClosestDisplayMode;
void* sceSDL_GetCurrentDisplayMode = SDL_GetCurrentDisplayMode;
void* sceSDL_GetDesktopDisplayMode = SDL_GetDesktopDisplayMode;
void* sceSDL_GetDisplayBounds = SDL_GetDisplayBounds;
void* sceSDL_GetDisplayDPI = SDL_GetDisplayDPI;
void* sceSDL_GetDisplayOrientation = SDL_GetDisplayOrientation;
void* sceSDL_GetDisplayMode = SDL_GetDisplayMode;
void* sceSDL_GetDisplayUsableBounds = SDL_GetDisplayUsableBounds;
void* sceSDL_GetNumDisplayModes = SDL_GetNumDisplayModes;
void* sceSDL_GetNumVideoDisplays = SDL_GetNumVideoDisplays;
void* sceSDL_GetNumVideoDrivers = SDL_GetNumVideoDrivers;
void* sceSDL_GetWindowBrightness = SDL_GetWindowBrightness;
void* sceSDL_SetWindowOpacity = SDL_SetWindowOpacity;
void* sceSDL_GetWindowOpacity = SDL_GetWindowOpacity;
void* sceSDL_SetWindowModalFor = SDL_SetWindowModalFor;
void* sceSDL_SetWindowInputFocus = SDL_SetWindowInputFocus;
void* sceSDL_GetWindowDisplayIndex = SDL_GetWindowDisplayIndex;
void* sceSDL_GetWindowDisplayMode = SDL_GetWindowDisplayMode;
void* sceSDL_GetWindowFlags = SDL_GetWindowFlags;
void* sceSDL_GetWindowFromID = SDL_GetWindowFromID;
void* sceSDL_GetWindowGrab = SDL_GetWindowGrab;
void* sceSDL_GetWindowID = SDL_GetWindowID;
void* sceSDL_GetWindowPixelFormat = SDL_GetWindowPixelFormat;
void* sceSDL_GetWindowMaximumSize = SDL_GetWindowMaximumSize;
void* sceSDL_GetWindowMinimumSize = SDL_GetWindowMinimumSize;
void* sceSDL_GetWindowPosition = SDL_GetWindowPosition;
void* sceSDL_GetWindowSize = SDL_GetWindowSize;
void* sceSDL_GetWindowSurface = SDL_GetWindowSurface;
void* sceSDL_GL_BindTexture = SDL_GL_BindTexture;
void* sceSDL_GL_CreateContext = SDL_GL_CreateContext;
void* sceSDL_GL_DeleteContext = SDL_GL_DeleteContext;
void* sceSDL_GL_UnloadLibrary = SDL_GL_UnloadLibrary;
void* sceSDL_GL_ResetAttributes = SDL_GL_ResetAttributes;
void* sceSDL_GL_GetAttribute = SDL_GL_GetAttribute;
void* sceSDL_GL_GetSwapInterval = SDL_GL_GetSwapInterval;
void* sceSDL_GL_MakeCurrent = SDL_GL_MakeCurrent;
void* sceSDL_GL_GetCurrentWindow = SDL_GL_GetCurrentWindow;
void* sceSDL_GL_GetCurrentContext = SDL_GL_GetCurrentContext;
void* sceSDL_GL_GetDrawableSize = SDL_GL_GetDrawableSize;
void* sceSDL_GL_SetAttribute = SDL_GL_SetAttribute;
void* sceSDL_GL_SetSwapInterval = SDL_GL_SetSwapInterval;
void* sceSDL_GL_SwapWindow = SDL_GL_SwapWindow;
void* sceSDL_GL_UnbindTexture = SDL_GL_UnbindTexture;
void* sceSDL_HideWindow = SDL_HideWindow;
void* sceSDL_IsScreenSaverEnabled = SDL_IsScreenSaverEnabled;
void* sceSDL_MaximizeWindow = SDL_MaximizeWindow;
void* sceSDL_MinimizeWindow = SDL_MinimizeWindow;
void* sceSDL_RaiseWindow = SDL_RaiseWindow;
void* sceSDL_RestoreWindow = SDL_RestoreWindow;
void* sceSDL_SetWindowBrightness = SDL_SetWindowBrightness;
void* sceSDL_SetWindowDisplayMode = SDL_SetWindowDisplayMode;
void* sceSDL_SetWindowFullscreen = SDL_SetWindowFullscreen;
void* sceSDL_SetWindowGrab = SDL_SetWindowGrab;
void* sceSDL_SetWindowIcon = SDL_SetWindowIcon;
void* sceSDL_SetWindowMaximumSize = SDL_SetWindowMaximumSize;
void* sceSDL_SetWindowMinimumSize = SDL_SetWindowMinimumSize;
void* sceSDL_SetWindowPosition = SDL_SetWindowPosition;
void* sceSDL_SetWindowSize = SDL_SetWindowSize;
void* sceSDL_SetWindowBordered = SDL_SetWindowBordered;
void* sceSDL_GetWindowBordersSize = SDL_GetWindowBordersSize;
void* sceSDL_SetWindowResizable = SDL_SetWindowResizable;
void* sceSDL_ShowWindow = SDL_ShowWindow;
void* sceSDL_UpdateWindowSurface = SDL_UpdateWindowSurface;
void* sceSDL_UpdateWindowSurfaceRects = SDL_UpdateWindowSurfaceRects;
void* sceSDL_VideoQuit = SDL_VideoQuit;
void* sceSDL_SetWindowHitTest = SDL_SetWindowHitTest;
void* sceSDL_GetGrabbedWindow = SDL_GetGrabbedWindow;
void* sceSDL_ComposeCustomBlendMode = SDL_ComposeCustomBlendMode;
void* sceSDL_Vulkan_GetVkGetInstanceProcAddr = SDL_Vulkan_GetVkGetInstanceProcAddr;
void* sceSDL_Vulkan_UnloadLibrary = SDL_Vulkan_UnloadLibrary;
void* sceSDL_Vulkan_GetInstanceExtensions = SDL_Vulkan_GetInstanceExtensions;
void* sceSDL_Vulkan_CreateSurface = SDL_Vulkan_CreateSurface;
void* sceSDL_Vulkan_GetDrawableSize = SDL_Vulkan_GetDrawableSize;
void* sceSDL_CreateRenderer = SDL_CreateRenderer;
void* sceSDL_CreateSoftwareRenderer = SDL_CreateSoftwareRenderer;
void* sceSDL_CreateTexture = SDL_CreateTexture;
void* sceSDL_CreateTextureFromSurface = SDL_CreateTextureFromSurface;
void* sceSDL_DestroyRenderer = SDL_DestroyRenderer;
void* sceSDL_DestroyTexture = SDL_DestroyTexture;
void* sceSDL_GetNumRenderDrivers = SDL_GetNumRenderDrivers;
void* sceSDL_GetRenderDrawBlendMode = SDL_GetRenderDrawBlendMode;
void* sceSDL_GetRenderDrawColor = SDL_GetRenderDrawColor;
void* sceSDL_GetRenderDriverInfo = SDL_GetRenderDriverInfo;
void* sceSDL_GetRenderer = SDL_GetRenderer;
void* sceSDL_GetRendererInfo = SDL_GetRendererInfo;
void* sceSDL_GetRendererOutputSize = SDL_GetRendererOutputSize;
void* sceSDL_GetTextureAlphaMod = SDL_GetTextureAlphaMod;
void* sceSDL_GetTextureBlendMode = SDL_GetTextureBlendMode;
void* sceSDL_GetTextureColorMod = SDL_GetTextureColorMod;
void* sceSDL_LockTexture = SDL_LockTexture;
void* sceSDL_QueryTexture = SDL_QueryTexture;
void* sceSDL_RenderClear = SDL_RenderClear;
void* sceSDL_RenderCopy = SDL_RenderCopy;
void* sceSDL_RenderCopyEx = SDL_RenderCopyEx;
void* sceSDL_RenderDrawLine = SDL_RenderDrawLine;
void* sceSDL_RenderDrawLines = SDL_RenderDrawLines;
void* sceSDL_RenderDrawPoint = SDL_RenderDrawPoint;
void* sceSDL_RenderDrawPoints = SDL_RenderDrawPoints;
void* sceSDL_RenderDrawRect = SDL_RenderDrawRect;
void* sceSDL_RenderDrawRects = SDL_RenderDrawRects;
void* sceSDL_RenderFillRect = SDL_RenderFillRect;
void* sceSDL_RenderFillRects = SDL_RenderFillRects;
void* sceSDL_RenderGetClipRect = SDL_RenderGetClipRect;
void* sceSDL_RenderGetLogicalSize = SDL_RenderGetLogicalSize;
void* sceSDL_RenderGetScale = SDL_RenderGetScale;
void* sceSDL_RenderGetViewport = SDL_RenderGetViewport;
void* sceSDL_RenderPresent = SDL_RenderPresent;
void* sceSDL_RenderReadPixels = SDL_RenderReadPixels;
void* sceSDL_RenderSetClipRect = SDL_RenderSetClipRect;
void* sceSDL_RenderSetLogicalSize = SDL_RenderSetLogicalSize;
void* sceSDL_RenderSetScale = SDL_RenderSetScale;
void* sceSDL_RenderSetIntegerScale = SDL_RenderSetIntegerScale;
void* sceSDL_RenderSetViewport = SDL_RenderSetViewport;
void* sceSDL_SetRenderDrawBlendMode = SDL_SetRenderDrawBlendMode;
void* sceSDL_SetRenderDrawColor = SDL_SetRenderDrawColor;
void* sceSDL_SetRenderTarget = SDL_SetRenderTarget;
void* sceSDL_SetTextureAlphaMod = SDL_SetTextureAlphaMod;
void* sceSDL_SetTextureBlendMode = SDL_SetTextureBlendMode;
void* sceSDL_SetTextureColorMod = SDL_SetTextureColorMod;
void* sceSDL_UnlockTexture = SDL_UnlockTexture;
void* sceSDL_UpdateTexture = SDL_UpdateTexture;
void* sceSDL_UpdateYUVTexture = SDL_UpdateYUVTexture;
void* sceSDL_RenderTargetSupported = SDL_RenderTargetSupported;
void* sceSDL_GetRenderTarget = SDL_GetRenderTarget;
void* sceSDL_RenderGetMetalLayer = SDL_RenderGetMetalLayer;
void* sceSDL_RenderGetMetalCommandEncoder = SDL_RenderGetMetalCommandEncoder;
void* sceSDL_RenderIsClipEnabled = SDL_RenderIsClipEnabled;
void* sceSDL_AllocFormat = SDL_AllocFormat;
void* sceSDL_AllocPalette = SDL_AllocPalette;
void* sceSDL_FreeFormat = SDL_FreeFormat;
void* sceSDL_FreePalette = SDL_FreePalette;
void* sceSDL_GetRGB = SDL_GetRGB;
void* sceSDL_GetRGBA = SDL_GetRGBA;
void* sceSDL_MapRGB = SDL_MapRGB;
void* sceSDL_MapRGBA = SDL_MapRGBA;
void* sceSDL_MasksToPixelFormatEnum = SDL_MasksToPixelFormatEnum;
void* sceSDL_PixelFormatEnumToMasks = SDL_PixelFormatEnumToMasks;
void* sceSDL_SetPaletteColors = SDL_SetPaletteColors;
void* sceSDL_SetPixelFormatPalette = SDL_SetPixelFormatPalette;
void* sceSDL_EnclosePoints = SDL_EnclosePoints;
void* sceSDL_HasIntersection = SDL_HasIntersection;
void* sceSDL_IntersectRect = SDL_IntersectRect;
void* sceSDL_IntersectRectAndLine = SDL_IntersectRectAndLine;
void* sceSDL_UnionRect = SDL_UnionRect;
void* sceSDL_ConvertPixels = SDL_ConvertPixels;
void* sceSDL_ConvertSurface = SDL_ConvertSurface;
void* sceSDL_ConvertSurfaceFormat = SDL_ConvertSurfaceFormat;
void* sceSDL_CreateRGBSurface = SDL_CreateRGBSurface;
void* sceSDL_CreateRGBSurfaceFrom = SDL_CreateRGBSurfaceFrom;
void* sceSDL_CreateRGBSurfaceWithFormat = SDL_CreateRGBSurfaceWithFormat;
void* sceSDL_CreateRGBSurfaceWithFormatFrom = SDL_CreateRGBSurfaceWithFormatFrom;
void* sceSDL_FillRect = SDL_FillRect;
void* sceSDL_FillRects = SDL_FillRects;
void* sceSDL_FreeSurface = SDL_FreeSurface;
void* sceSDL_GetClipRect = SDL_GetClipRect;
void* sceSDL_HasColorKey = SDL_HasColorKey;
void* sceSDL_GetColorKey = SDL_GetColorKey;
void* sceSDL_GetSurfaceAlphaMod = SDL_GetSurfaceAlphaMod;
void* sceSDL_GetSurfaceBlendMode = SDL_GetSurfaceBlendMode;
void* sceSDL_GetSurfaceColorMod = SDL_GetSurfaceColorMod;
void* sceSDL_LockSurface = SDL_LockSurface;
void* sceSDL_LowerBlit = SDL_LowerBlit;
void* sceSDL_LowerBlitScaled = SDL_LowerBlitScaled;
void* sceSDL_SetClipRect = SDL_SetClipRect;
void* sceSDL_SetColorKey = SDL_SetColorKey;
void* sceSDL_SetSurfaceAlphaMod = SDL_SetSurfaceAlphaMod;
void* sceSDL_SetSurfaceBlendMode = SDL_SetSurfaceBlendMode;
void* sceSDL_SetSurfaceColorMod = SDL_SetSurfaceColorMod;
void* sceSDL_SetSurfacePalette = SDL_SetSurfacePalette;
void* sceSDL_SetSurfaceRLE = SDL_SetSurfaceRLE;
void* sceSDL_SoftStretch = SDL_SoftStretch;
void* sceSDL_UnlockSurface = SDL_UnlockSurface;
void* sceSDL_DuplicateSurface = SDL_DuplicateSurface;
void* sceSDL_HasClipboardText = SDL_HasClipboardText;
void* sceSDL_PumpEvents = SDL_PumpEvents;
void* sceSDL_PeepEvents = SDL_PeepEvents;
void* sceSDL_HasEvent = SDL_HasEvent;
void* sceSDL_HasEvents = SDL_HasEvents;
void* sceSDL_FlushEvent = SDL_FlushEvent;
void* sceSDL_FlushEvents = SDL_FlushEvents;
void* sceSDL_PollEvent = SDL_PollEvent;
void* sceSDL_WaitEvent = SDL_WaitEvent;
void* sceSDL_WaitEventTimeout = SDL_WaitEventTimeout;
void* sceSDL_PushEvent = SDL_PushEvent;
void* sceSDL_SetEventFilter = SDL_SetEventFilter;
void* sceSDL_GetEventFilter = SDL_GetEventFilter;
void* sceSDL_AddEventWatch = SDL_AddEventWatch;
void* sceSDL_DelEventWatch = SDL_DelEventWatch;
void* sceSDL_FilterEvents = SDL_FilterEvents;
void* sceSDL_EventState = SDL_EventState;
void* sceSDL_RegisterEvents = SDL_RegisterEvents;
void* sceSDL_GetKeyboardFocus = SDL_GetKeyboardFocus;
void* sceSDL_GetKeyboardState = SDL_GetKeyboardState;
void* sceSDL_GetModState = SDL_GetModState;
void* sceSDL_SetModState = SDL_SetModState;
void* sceSDL_GetKeyFromScancode = SDL_GetKeyFromScancode;
void* sceSDL_GetScancodeFromKey = SDL_GetScancodeFromKey;
void* sceSDL_StartTextInput = SDL_StartTextInput;
void* sceSDL_IsTextInputActive = SDL_IsTextInputActive;
void* sceSDL_StopTextInput = SDL_StopTextInput;
void* sceSDL_SetTextInputRect = SDL_SetTextInputRect;
void* sceSDL_HasScreenKeyboardSupport = SDL_HasScreenKeyboardSupport;
void* sceSDL_IsScreenKeyboardShown = SDL_IsScreenKeyboardShown;
void* sceSDL_GetMouseFocus = SDL_GetMouseFocus;
void* sceSDL_GetMouseState = SDL_GetMouseState;
void* sceSDL_GetGlobalMouseState = SDL_GetGlobalMouseState;
void* sceSDL_GetRelativeMouseState = SDL_GetRelativeMouseState;
void* sceSDL_WarpMouseInWindow = SDL_WarpMouseInWindow;
void* sceSDL_WarpMouseGlobal = SDL_WarpMouseGlobal;
void* sceSDL_SetRelativeMouseMode = SDL_SetRelativeMouseMode;
void* sceSDL_CaptureMouse = SDL_CaptureMouse;
void* sceSDL_GetRelativeMouseMode = SDL_GetRelativeMouseMode;
void* sceSDL_CreateCursor = SDL_CreateCursor;
void* sceSDL_CreateColorCursor = SDL_CreateColorCursor;
void* sceSDL_CreateSystemCursor = SDL_CreateSystemCursor;
void* sceSDL_SetCursor = SDL_SetCursor;
void* sceSDL_GetCursor = SDL_GetCursor;
void* sceSDL_FreeCursor = SDL_FreeCursor;
void* sceSDL_ShowCursor = SDL_ShowCursor;
void* sceSDL_GetNumTouchDevices = SDL_GetNumTouchDevices;
void* sceSDL_GetTouchDevice = SDL_GetTouchDevice;
void* sceSDL_GetNumTouchFingers = SDL_GetNumTouchFingers;
void* sceSDL_GetTouchFinger = SDL_GetTouchFinger;
void* sceSDL_JoystickRumble = SDL_JoystickRumble;
void* sceSDL_JoystickClose = SDL_JoystickClose;
void* sceSDL_JoystickEventState = SDL_JoystickEventState;
void* sceSDL_JoystickGetAxis = SDL_JoystickGetAxis;
void* sceSDL_JoystickGetAxisInitialState = SDL_JoystickGetAxisInitialState;
void* sceSDL_JoystickGetBall = SDL_JoystickGetBall;
void* sceSDL_JoystickGetButton = SDL_JoystickGetButton;
void* sceSDL_JoystickGetHat = SDL_JoystickGetHat;
void* sceSDL_JoystickNumAxes = SDL_JoystickNumAxes;
void* sceSDL_JoystickNumBalls = SDL_JoystickNumBalls;
void* sceSDL_JoystickNumButtons = SDL_JoystickNumButtons;
void* sceSDL_JoystickNumHats = SDL_JoystickNumHats;
void* sceSDL_JoystickOpen = SDL_JoystickOpen;
void* sceSDL_JoystickUpdate = SDL_JoystickUpdate;
void* sceSDL_NumJoysticks = SDL_NumJoysticks;
void* sceSDL_JoystickGetDeviceGUID = SDL_JoystickGetDeviceGUID;
void* sceSDL_JoystickGetGUID = SDL_JoystickGetGUID;
void* sceSDL_JoystickGetGUIDString = SDL_JoystickGetGUIDString;
void* sceSDL_JoystickGetDeviceVendor = SDL_JoystickGetDeviceVendor;
void* sceSDL_JoystickGetDeviceProduct = SDL_JoystickGetDeviceProduct;
void* sceSDL_JoystickGetDeviceProductVersion = SDL_JoystickGetDeviceProductVersion;
void* sceSDL_JoystickGetDeviceType = SDL_JoystickGetDeviceType;
void* sceSDL_JoystickGetDeviceInstanceID = SDL_JoystickGetDeviceInstanceID;
void* sceSDL_JoystickGetVendor = SDL_JoystickGetVendor;
void* sceSDL_JoystickGetProduct = SDL_JoystickGetProduct;
void* sceSDL_JoystickGetProductVersion = SDL_JoystickGetProductVersion;
void* sceSDL_JoystickGetType = SDL_JoystickGetType;
void* sceSDL_JoystickGetAttached = SDL_JoystickGetAttached;
void* sceSDL_JoystickInstanceID = SDL_JoystickInstanceID;
void* sceSDL_JoystickCurrentPowerLevel = SDL_JoystickCurrentPowerLevel;
void* sceSDL_JoystickFromInstanceID = SDL_JoystickFromInstanceID;
void* sceSDL_LockJoysticks = SDL_LockJoysticks;
void* sceSDL_UnlockJoysticks = SDL_UnlockJoysticks;
void* sceSDL_GameControllerNumMappings = SDL_GameControllerNumMappings;
void* sceSDL_IsGameController = SDL_IsGameController;
void* sceSDL_GameControllerOpen = SDL_GameControllerOpen;
void* sceSDL_GameControllerGetVendor = SDL_GameControllerGetVendor;
void* sceSDL_GameControllerGetProduct = SDL_GameControllerGetProduct;
void* sceSDL_GameControllerGetProductVersion = SDL_GameControllerGetProductVersion;
void* sceSDL_GameControllerGetAttached = SDL_GameControllerGetAttached;
void* sceSDL_GameControllerGetJoystick = SDL_GameControllerGetJoystick;
void* sceSDL_GameControllerEventState = SDL_GameControllerEventState;
void* sceSDL_GameControllerUpdate = SDL_GameControllerUpdate;
void* sceSDL_GameControllerGetAxis = SDL_GameControllerGetAxis;
void* sceSDL_GameControllerGetButton = SDL_GameControllerGetButton;
void* sceSDL_GameControllerRumble = SDL_GameControllerRumble;
void* sceSDL_GameControllerClose = SDL_GameControllerClose;
void* sceSDL_GameControllerFromInstanceID = SDL_GameControllerFromInstanceID;
void* sceSDL_HapticClose = SDL_HapticClose;
void* sceSDL_HapticDestroyEffect = SDL_HapticDestroyEffect;
void* sceSDL_HapticEffectSupported = SDL_HapticEffectSupported;
void* sceSDL_HapticGetEffectStatus = SDL_HapticGetEffectStatus;
void* sceSDL_HapticIndex = SDL_HapticIndex;
void* sceSDL_HapticNewEffect = SDL_HapticNewEffect;
void* sceSDL_HapticNumAxes = SDL_HapticNumAxes;
void* sceSDL_HapticNumEffects = SDL_HapticNumEffects;
void* sceSDL_HapticNumEffectsPlaying = SDL_HapticNumEffectsPlaying;
void* sceSDL_HapticOpen = SDL_HapticOpen;
void* sceSDL_HapticOpened = SDL_HapticOpened;
void* sceSDL_HapticOpenFromJoystick = SDL_HapticOpenFromJoystick;
void* sceSDL_HapticOpenFromMouse = SDL_HapticOpenFromMouse;
void* sceSDL_HapticPause = SDL_HapticPause;
void* sceSDL_HapticQuery = SDL_HapticQuery;
void* sceSDL_HapticRumbleInit = SDL_HapticRumbleInit;
void* sceSDL_HapticRumblePlay = SDL_HapticRumblePlay;
void* sceSDL_HapticRumbleStop = SDL_HapticRumbleStop;
void* sceSDL_HapticRumbleSupported = SDL_HapticRumbleSupported;
void* sceSDL_HapticRunEffect = SDL_HapticRunEffect;
void* sceSDL_HapticSetAutocenter = SDL_HapticSetAutocenter;
void* sceSDL_HapticSetGain = SDL_HapticSetGain;
void* sceSDL_HapticStopAll = SDL_HapticStopAll;
void* sceSDL_HapticStopEffect = SDL_HapticStopEffect;
void* sceSDL_HapticUnpause = SDL_HapticUnpause;
void* sceSDL_HapticUpdateEffect = SDL_HapticUpdateEffect;
void* sceSDL_JoystickIsHaptic = SDL_JoystickIsHaptic;
void* sceSDL_MouseIsHaptic = SDL_MouseIsHaptic;
void* sceSDL_NumHaptics = SDL_NumHaptics;
void* sceSDL_NumSensors = SDL_NumSensors;
void* sceSDL_SensorGetDeviceType = SDL_SensorGetDeviceType;
void* sceSDL_SensorGetDeviceNonPortableType = SDL_SensorGetDeviceNonPortableType;
void* sceSDL_SensorGetDeviceInstanceID = SDL_SensorGetDeviceInstanceID;
void* sceSDL_SensorOpen = SDL_SensorOpen;
void* sceSDL_SensorFromInstanceID = SDL_SensorFromInstanceID;
void* sceSDL_SensorGetType = SDL_SensorGetType;
void* sceSDL_SensorGetNonPortableType = SDL_SensorGetNonPortableType;
void* sceSDL_SensorGetInstanceID = SDL_SensorGetInstanceID;
void* sceSDL_SensorGetData = SDL_SensorGetData;
void* sceSDL_SensorClose = SDL_SensorClose;
void* sceSDL_SensorUpdate = SDL_SensorUpdate;
void* sceSDL_AudioQuit = SDL_AudioQuit;
void* sceSDL_CloseAudio = SDL_CloseAudio;
void* sceSDL_CloseAudioDevice = SDL_CloseAudioDevice;
void* sceSDL_FreeWAV = SDL_FreeWAV;
void* sceSDL_GetAudioDeviceStatus = SDL_GetAudioDeviceStatus;
void* sceSDL_GetAudioStatus = SDL_GetAudioStatus;
void* sceSDL_GetNumAudioDevices = SDL_GetNumAudioDevices;
void* sceSDL_GetNumAudioDrivers = SDL_GetNumAudioDrivers;
void* sceSDL_LockAudio = SDL_LockAudio;
void* sceSDL_LockAudioDevice = SDL_LockAudioDevice;
void* sceSDL_OpenAudio = SDL_OpenAudio;
void* sceSDL_PauseAudio = SDL_PauseAudio;
void* sceSDL_PauseAudioDevice = SDL_PauseAudioDevice;
void* sceSDL_UnlockAudio = SDL_UnlockAudio;
void* sceSDL_UnlockAudioDevice = SDL_UnlockAudioDevice;
void* sceSDL_QueueAudio = SDL_QueueAudio;
void* sceSDL_DequeueAudio = SDL_DequeueAudio;
void* sceSDL_GetQueuedAudioSize = SDL_GetQueuedAudioSize;
void* sceSDL_ClearQueuedAudio = SDL_ClearQueuedAudio;
void* sceSDL_NewAudioStream = SDL_NewAudioStream;
void* sceSDL_AudioStreamPut = SDL_AudioStreamPut;
void* sceSDL_AudioStreamGet = SDL_AudioStreamGet;
void* sceSDL_AudioStreamAvailable = SDL_AudioStreamAvailable;
void* sceSDL_AudioStreamClear = SDL_AudioStreamClear;
void* sceSDL_FreeAudioStream = SDL_FreeAudioStream;
void* sceSDL_Delay = SDL_Delay;
void* sceSDL_GetTicks = SDL_GetTicks;
void* sceSDL_GetPerformanceCounter = SDL_GetPerformanceCounter;
void* sceSDL_GetPerformanceFrequency = SDL_GetPerformanceFrequency;
void* sceSDL_AddTimer = SDL_AddTimer;
void* sceSDL_RemoveTimer = SDL_RemoveTimer;
void* sceSDL_IsTablet = SDL_IsTablet;
void* sceSDL_GetPowerInfo = SDL_GetPowerInfo;
void* sceSDL_GetCPUCount = SDL_GetCPUCount;
void* sceSDL_GetSystemRAM = SDL_GetSystemRAM;
void* sceSDL_ShowMessageBox = SDL_ShowMessageBox;
void* sceSDL_GetWindowGammaRamp = SDL_GetWindowGammaRamp;
void* sceSDL_SetWindowGammaRamp = SDL_SetWindowGammaRamp;
void* sceSDL_CalculateGammaRamp = SDL_CalculateGammaRamp;
void* sceSDL_MixAudio = SDL_MixAudio;
void* sceSDL_MixAudioFormat = SDL_MixAudioFormat;
void* sceIMG_Init = IMG_Init;
void* sceIMG_Quit = IMG_Quit;
void* sceIMG_Load_RW = IMG_Load_RW;
void* sceIMG_LoadTexture_RW = IMG_LoadTexture_RW;
void* sceIMG_SavePNG_RW = IMG_SavePNG_RW;
void* sceIMG_SaveJPG_RW = IMG_SaveJPG_RW;
void* sceIMG_Linked_Version = IMG_Linked_Version;
void* sceIMG_Load = IMG_Load;
void* sceIMG_LoadTyped_RW = IMG_LoadTyped_RW;
void* sceIMG_LoadTexture = IMG_LoadTexture;
void* sceIMG_LoadTextureTyped_RW = IMG_LoadTextureTyped_RW;
void* sceIMG_SavePNG = IMG_SavePNG;
