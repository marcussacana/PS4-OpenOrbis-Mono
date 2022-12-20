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

#define EXPORT __attribute__((used))

EXPORT void* sceSDL_GetHintBoolean = SDL_GetHintBoolean;
EXPORT void* sceSDL_GetError = SDL_GetError;
EXPORT void* sceSDL_SetError = SDL_SetError;
EXPORT void* sceSDL_Log = SDL_Log;
EXPORT void* sceSDL_LogVerbose = SDL_LogVerbose;
EXPORT void* sceSDL_LogDebug = SDL_LogDebug;
EXPORT void* sceSDL_LogInfo = SDL_LogInfo;
EXPORT void* sceSDL_LogWarn = SDL_LogWarn;
EXPORT void* sceSDL_LogError = SDL_LogError;
EXPORT void* sceSDL_LogCritical = SDL_LogCritical;
EXPORT void* sceSDL_LogMessage = SDL_LogMessage;
EXPORT void* sceSDL_LogMessageV = SDL_LogMessageV;
EXPORT void* sceSDL_ShowSimpleMessageBox = SDL_ShowSimpleMessageBox;
EXPORT void* sceSDL_GetRevision = SDL_GetRevision;
EXPORT void* sceSDL_CreateWindow = SDL_CreateWindow;
EXPORT void* sceSDL_GetCurrentVideoDriver = SDL_GetCurrentVideoDriver;
EXPORT void* sceSDL_GetDisplayName = SDL_GetDisplayName;
EXPORT void* sceSDL_GetVideoDriver = SDL_GetVideoDriver;
EXPORT void* sceSDL_GetWindowData = SDL_GetWindowData;
EXPORT void* sceSDL_GetWindowTitle = SDL_GetWindowTitle;
EXPORT void* sceSDL_GL_GetProcAddress = SDL_GL_GetProcAddress;
EXPORT void* sceSDL_GL_LoadLibrary = SDL_GL_LoadLibrary;
EXPORT void* sceSDL_GL_ExtensionSupported = SDL_GL_ExtensionSupported;
EXPORT void* sceSDL_SetWindowData = SDL_SetWindowData;
EXPORT void* sceSDL_SetWindowTitle = SDL_SetWindowTitle;
EXPORT void* sceSDL_VideoInit = SDL_VideoInit;
EXPORT void* sceSDL_Vulkan_LoadLibrary = SDL_Vulkan_LoadLibrary;
EXPORT void* sceSDL_GetPixelFormatName = SDL_GetPixelFormatName;
EXPORT void* sceSDL_UpperBlit = SDL_UpperBlit;
EXPORT void* sceSDL_UpperBlitScaled = SDL_UpperBlitScaled;
EXPORT void* sceSDL_LoadBMP_RW = SDL_LoadBMP_RW;
EXPORT void* sceSDL_SaveBMP_RW = SDL_SaveBMP_RW;
EXPORT void* sceSDL_GetClipboardText = SDL_GetClipboardText;
EXPORT void* sceSDL_SetClipboardText = SDL_SetClipboardText;
EXPORT void* sceSDL_GetScancodeName = SDL_GetScancodeName;
EXPORT void* sceSDL_GetScancodeFromName = SDL_GetScancodeFromName;
EXPORT void* sceSDL_GetKeyName = SDL_GetKeyName;
EXPORT void* sceSDL_GetKeyFromName = SDL_GetKeyFromName;
EXPORT void* sceSDL_JoystickName = SDL_JoystickName;
EXPORT void* sceSDL_JoystickNameForIndex = SDL_JoystickNameForIndex;
EXPORT void* sceSDL_JoystickGetGUIDFromString = SDL_JoystickGetGUIDFromString;
EXPORT void* sceSDL_GameControllerAddMapping = SDL_GameControllerAddMapping;
EXPORT void* sceSDL_GameControllerMappingForIndex = SDL_GameControllerMappingForIndex;
EXPORT void* sceSDL_GameControllerAddMappingsFromRW = SDL_GameControllerAddMappingsFromRW;
EXPORT void* sceSDL_GameControllerMappingForGUID = SDL_GameControllerMappingForGUID;
EXPORT void* sceSDL_GameControllerMapping = SDL_GameControllerMapping;
EXPORT void* sceSDL_GameControllerNameForIndex = SDL_GameControllerNameForIndex;
EXPORT void* sceSDL_GameControllerMappingForDeviceIndex = SDL_GameControllerMappingForDeviceIndex;
EXPORT void* sceSDL_GameControllerName = SDL_GameControllerName;
EXPORT void* sceSDL_GameControllerGetAxisFromString = SDL_GameControllerGetAxisFromString;
EXPORT void* sceSDL_GameControllerGetStringForAxis = SDL_GameControllerGetStringForAxis;
EXPORT void* sceSDL_GameControllerGetBindForAxis = SDL_GameControllerGetBindForAxis;
EXPORT void* sceSDL_GameControllerGetButtonFromString = SDL_GameControllerGetButtonFromString;
EXPORT void* sceSDL_GameControllerGetStringForButton = SDL_GameControllerGetStringForButton;
EXPORT void* sceSDL_GameControllerGetBindForButton = SDL_GameControllerGetBindForButton;
EXPORT void* sceSDL_HapticName = SDL_HapticName;
EXPORT void* sceSDL_SensorGetDeviceName = SDL_SensorGetDeviceName;
EXPORT void* sceSDL_SensorGetName = SDL_SensorGetName;
EXPORT void* sceSDL_AudioInit = SDL_AudioInit;
EXPORT void* sceSDL_GetAudioDeviceName = SDL_GetAudioDeviceName;
EXPORT void* sceSDL_GetAudioDriver = SDL_GetAudioDriver;
EXPORT void* sceSDL_GetCurrentAudioDriver = SDL_GetCurrentAudioDriver;
EXPORT void* sceSDL_LoadWAV_RW = SDL_LoadWAV_RW;
EXPORT void* sceSDL_OpenAudioDevice = SDL_OpenAudioDevice;
EXPORT void* sceSDL_GetBasePath = SDL_GetBasePath;
EXPORT void* sceSDL_GetPrefPath = SDL_GetPrefPath;
EXPORT void* sceSDL_malloc = SDL_malloc;
EXPORT void* sceSDL_free = SDL_free;
EXPORT void* sceSDL_RWFromMem = SDL_RWFromMem;
EXPORT void* sceSDL_SetMainReady = SDL_SetMainReady;
EXPORT void* sceSDL_Init = SDL_Init;
EXPORT void* sceSDL_InitSubSystem = SDL_InitSubSystem;
EXPORT void* sceSDL_Quit = SDL_Quit;
EXPORT void* sceSDL_QuitSubSystem = SDL_QuitSubSystem;
EXPORT void* sceSDL_WasInit = SDL_WasInit;
EXPORT void* sceSDL_ClearHints = SDL_ClearHints;
EXPORT void* sceSDL_ClearError = SDL_ClearError;
EXPORT void* sceSDL_LogGetPriority = SDL_LogGetPriority;
EXPORT void* sceSDL_LogSetPriority = SDL_LogSetPriority;
EXPORT void* sceSDL_LogSetAllPriority = SDL_LogSetAllPriority;
EXPORT void* sceSDL_LogResetPriorities = SDL_LogResetPriorities;
EXPORT void* sceSDL_LogGetOutputFunction = SDL_LogGetOutputFunction;
EXPORT void* sceSDL_LogSetOutputFunction = SDL_LogSetOutputFunction;
EXPORT void* sceSDL_GetVersion = SDL_GetVersion;
EXPORT void* sceSDL_GetRevisionNumber = SDL_GetRevisionNumber;
EXPORT void* sceSDL_CreateWindowAndRenderer = SDL_CreateWindowAndRenderer;
EXPORT void* sceSDL_CreateWindowFrom = SDL_CreateWindowFrom;
EXPORT void* sceSDL_DestroyWindow = SDL_DestroyWindow;
EXPORT void* sceSDL_DisableScreenSaver = SDL_DisableScreenSaver;
EXPORT void* sceSDL_EnableScreenSaver = SDL_EnableScreenSaver;
EXPORT void* sceSDL_GetClosestDisplayMode = SDL_GetClosestDisplayMode;
EXPORT void* sceSDL_GetCurrentDisplayMode = SDL_GetCurrentDisplayMode;
EXPORT void* sceSDL_GetDesktopDisplayMode = SDL_GetDesktopDisplayMode;
EXPORT void* sceSDL_GetDisplayBounds = SDL_GetDisplayBounds;
EXPORT void* sceSDL_GetDisplayDPI = SDL_GetDisplayDPI;
EXPORT void* sceSDL_GetDisplayOrientation = SDL_GetDisplayOrientation;
EXPORT void* sceSDL_GetDisplayMode = SDL_GetDisplayMode;
EXPORT void* sceSDL_GetDisplayUsableBounds = SDL_GetDisplayUsableBounds;
EXPORT void* sceSDL_GetNumDisplayModes = SDL_GetNumDisplayModes;
EXPORT void* sceSDL_GetNumVideoDisplays = SDL_GetNumVideoDisplays;
EXPORT void* sceSDL_GetNumVideoDrivers = SDL_GetNumVideoDrivers;
EXPORT void* sceSDL_GetWindowBrightness = SDL_GetWindowBrightness;
EXPORT void* sceSDL_SetWindowOpacity = SDL_SetWindowOpacity;
EXPORT void* sceSDL_GetWindowOpacity = SDL_GetWindowOpacity;
EXPORT void* sceSDL_SetWindowModalFor = SDL_SetWindowModalFor;
EXPORT void* sceSDL_SetWindowInputFocus = SDL_SetWindowInputFocus;
EXPORT void* sceSDL_GetWindowDisplayIndex = SDL_GetWindowDisplayIndex;
EXPORT void* sceSDL_GetWindowDisplayMode = SDL_GetWindowDisplayMode;
EXPORT void* sceSDL_GetWindowFlags = SDL_GetWindowFlags;
EXPORT void* sceSDL_GetWindowFromID = SDL_GetWindowFromID;
EXPORT void* sceSDL_GetWindowGrab = SDL_GetWindowGrab;
EXPORT void* sceSDL_GetWindowID = SDL_GetWindowID;
EXPORT void* sceSDL_GetWindowPixelFormat = SDL_GetWindowPixelFormat;
EXPORT void* sceSDL_GetWindowMaximumSize = SDL_GetWindowMaximumSize;
EXPORT void* sceSDL_GetWindowMinimumSize = SDL_GetWindowMinimumSize;
EXPORT void* sceSDL_GetWindowPosition = SDL_GetWindowPosition;
EXPORT void* sceSDL_GetWindowSize = SDL_GetWindowSize;
EXPORT void* sceSDL_GetWindowSurface = SDL_GetWindowSurface;
EXPORT void* sceSDL_GL_BindTexture = SDL_GL_BindTexture;
EXPORT void* sceSDL_GL_CreateContext = SDL_GL_CreateContext;
EXPORT void* sceSDL_GL_DeleteContext = SDL_GL_DeleteContext;
EXPORT void* sceSDL_GL_UnloadLibrary = SDL_GL_UnloadLibrary;
EXPORT void* sceSDL_GL_ResetAttributes = SDL_GL_ResetAttributes;
EXPORT void* sceSDL_GL_GetAttribute = SDL_GL_GetAttribute;
EXPORT void* sceSDL_GL_GetSwapInterval = SDL_GL_GetSwapInterval;
EXPORT void* sceSDL_GL_MakeCurrent = SDL_GL_MakeCurrent;
EXPORT void* sceSDL_GL_GetCurrentWindow = SDL_GL_GetCurrentWindow;
EXPORT void* sceSDL_GL_GetCurrentContext = SDL_GL_GetCurrentContext;
EXPORT void* sceSDL_GL_GetDrawableSize = SDL_GL_GetDrawableSize;
EXPORT void* sceSDL_GL_SetAttribute = SDL_GL_SetAttribute;
EXPORT void* sceSDL_GL_SetSwapInterval = SDL_GL_SetSwapInterval;
EXPORT void* sceSDL_GL_SwapWindow = SDL_GL_SwapWindow;
EXPORT void* sceSDL_GL_UnbindTexture = SDL_GL_UnbindTexture;
EXPORT void* sceSDL_HideWindow = SDL_HideWindow;
EXPORT void* sceSDL_IsScreenSaverEnabled = SDL_IsScreenSaverEnabled;
EXPORT void* sceSDL_MaximizeWindow = SDL_MaximizeWindow;
EXPORT void* sceSDL_MinimizeWindow = SDL_MinimizeWindow;
EXPORT void* sceSDL_RaiseWindow = SDL_RaiseWindow;
EXPORT void* sceSDL_RestoreWindow = SDL_RestoreWindow;
EXPORT void* sceSDL_SetWindowBrightness = SDL_SetWindowBrightness;
EXPORT void* sceSDL_SetWindowDisplayMode = SDL_SetWindowDisplayMode;
EXPORT void* sceSDL_SetWindowFullscreen = SDL_SetWindowFullscreen;
EXPORT void* sceSDL_SetWindowGrab = SDL_SetWindowGrab;
EXPORT void* sceSDL_SetWindowIcon = SDL_SetWindowIcon;
EXPORT void* sceSDL_SetWindowMaximumSize = SDL_SetWindowMaximumSize;
EXPORT void* sceSDL_SetWindowMinimumSize = SDL_SetWindowMinimumSize;
EXPORT void* sceSDL_SetWindowPosition = SDL_SetWindowPosition;
EXPORT void* sceSDL_SetWindowSize = SDL_SetWindowSize;
EXPORT void* sceSDL_SetWindowBordered = SDL_SetWindowBordered;
EXPORT void* sceSDL_GetWindowBordersSize = SDL_GetWindowBordersSize;
EXPORT void* sceSDL_SetWindowResizable = SDL_SetWindowResizable;
EXPORT void* sceSDL_ShowWindow = SDL_ShowWindow;
EXPORT void* sceSDL_UpdateWindowSurface = SDL_UpdateWindowSurface;
EXPORT void* sceSDL_UpdateWindowSurfaceRects = SDL_UpdateWindowSurfaceRects;
EXPORT void* sceSDL_VideoQuit = SDL_VideoQuit;
EXPORT void* sceSDL_SetWindowHitTest = SDL_SetWindowHitTest;
EXPORT void* sceSDL_GetGrabbedWindow = SDL_GetGrabbedWindow;
EXPORT void* sceSDL_ComposeCustomBlendMode = SDL_ComposeCustomBlendMode;
EXPORT void* sceSDL_Vulkan_GetVkGetInstanceProcAddr = SDL_Vulkan_GetVkGetInstanceProcAddr;
EXPORT void* sceSDL_Vulkan_UnloadLibrary = SDL_Vulkan_UnloadLibrary;
EXPORT void* sceSDL_Vulkan_GetInstanceExtensions = SDL_Vulkan_GetInstanceExtensions;
EXPORT void* sceSDL_Vulkan_CreateSurface = SDL_Vulkan_CreateSurface;
EXPORT void* sceSDL_Vulkan_GetDrawableSize = SDL_Vulkan_GetDrawableSize;
EXPORT void* sceSDL_CreateRenderer = SDL_CreateRenderer;
EXPORT void* sceSDL_CreateSoftwareRenderer = SDL_CreateSoftwareRenderer;
EXPORT void* sceSDL_CreateTexture = SDL_CreateTexture;
EXPORT void* sceSDL_CreateTextureFromSurface = SDL_CreateTextureFromSurface;
EXPORT void* sceSDL_DestroyRenderer = SDL_DestroyRenderer;
EXPORT void* sceSDL_DestroyTexture = SDL_DestroyTexture;
EXPORT void* sceSDL_GetNumRenderDrivers = SDL_GetNumRenderDrivers;
EXPORT void* sceSDL_GetRenderDrawBlendMode = SDL_GetRenderDrawBlendMode;
EXPORT void* sceSDL_GetRenderDrawColor = SDL_GetRenderDrawColor;
EXPORT void* sceSDL_GetRenderDriverInfo = SDL_GetRenderDriverInfo;
EXPORT void* sceSDL_GetRenderer = SDL_GetRenderer;
EXPORT void* sceSDL_GetRendererInfo = SDL_GetRendererInfo;
EXPORT void* sceSDL_GetRendererOutputSize = SDL_GetRendererOutputSize;
EXPORT void* sceSDL_GetTextureAlphaMod = SDL_GetTextureAlphaMod;
EXPORT void* sceSDL_GetTextureBlendMode = SDL_GetTextureBlendMode;
EXPORT void* sceSDL_GetTextureColorMod = SDL_GetTextureColorMod;
EXPORT void* sceSDL_LockTexture = SDL_LockTexture;
EXPORT void* sceSDL_QueryTexture = SDL_QueryTexture;
EXPORT void* sceSDL_RenderClear = SDL_RenderClear;
EXPORT void* sceSDL_RenderCopy = SDL_RenderCopy;
EXPORT void* sceSDL_RenderCopyEx = SDL_RenderCopyEx;
EXPORT void* sceSDL_RenderDrawLine = SDL_RenderDrawLine;
EXPORT void* sceSDL_RenderDrawLines = SDL_RenderDrawLines;
EXPORT void* sceSDL_RenderDrawPoint = SDL_RenderDrawPoint;
EXPORT void* sceSDL_RenderDrawPoints = SDL_RenderDrawPoints;
EXPORT void* sceSDL_RenderDrawRect = SDL_RenderDrawRect;
EXPORT void* sceSDL_RenderDrawRects = SDL_RenderDrawRects;
EXPORT void* sceSDL_RenderFillRect = SDL_RenderFillRect;
EXPORT void* sceSDL_RenderFillRects = SDL_RenderFillRects;
EXPORT void* sceSDL_RenderGetClipRect = SDL_RenderGetClipRect;
EXPORT void* sceSDL_RenderGetLogicalSize = SDL_RenderGetLogicalSize;
EXPORT void* sceSDL_RenderGetScale = SDL_RenderGetScale;
EXPORT void* sceSDL_RenderGetViewport = SDL_RenderGetViewport;
EXPORT void* sceSDL_RenderPresent = SDL_RenderPresent;
EXPORT void* sceSDL_RenderReadPixels = SDL_RenderReadPixels;
EXPORT void* sceSDL_RenderSetClipRect = SDL_RenderSetClipRect;
EXPORT void* sceSDL_RenderSetLogicalSize = SDL_RenderSetLogicalSize;
EXPORT void* sceSDL_RenderSetScale = SDL_RenderSetScale;
EXPORT void* sceSDL_RenderSetIntegerScale = SDL_RenderSetIntegerScale;
EXPORT void* sceSDL_RenderSetViewport = SDL_RenderSetViewport;
EXPORT void* sceSDL_SetRenderDrawBlendMode = SDL_SetRenderDrawBlendMode;
EXPORT void* sceSDL_SetRenderDrawColor = SDL_SetRenderDrawColor;
EXPORT void* sceSDL_SetRenderTarget = SDL_SetRenderTarget;
EXPORT void* sceSDL_SetTextureAlphaMod = SDL_SetTextureAlphaMod;
EXPORT void* sceSDL_SetTextureBlendMode = SDL_SetTextureBlendMode;
EXPORT void* sceSDL_SetTextureColorMod = SDL_SetTextureColorMod;
EXPORT void* sceSDL_UnlockTexture = SDL_UnlockTexture;
EXPORT void* sceSDL_UpdateTexture = SDL_UpdateTexture;
EXPORT void* sceSDL_UpdateYUVTexture = SDL_UpdateYUVTexture;
EXPORT void* sceSDL_RenderTargetSupported = SDL_RenderTargetSupported;
EXPORT void* sceSDL_GetRenderTarget = SDL_GetRenderTarget;
EXPORT void* sceSDL_RenderGetMetalLayer = SDL_RenderGetMetalLayer;
EXPORT void* sceSDL_RenderGetMetalCommandEncoder = SDL_RenderGetMetalCommandEncoder;
EXPORT void* sceSDL_RenderIsClipEnabled = SDL_RenderIsClipEnabled;
EXPORT void* sceSDL_AllocFormat = SDL_AllocFormat;
EXPORT void* sceSDL_AllocPalette = SDL_AllocPalette;
EXPORT void* sceSDL_FreeFormat = SDL_FreeFormat;
EXPORT void* sceSDL_FreePalette = SDL_FreePalette;
EXPORT void* sceSDL_GetRGB = SDL_GetRGB;
EXPORT void* sceSDL_GetRGBA = SDL_GetRGBA;
EXPORT void* sceSDL_MapRGB = SDL_MapRGB;
EXPORT void* sceSDL_MapRGBA = SDL_MapRGBA;
EXPORT void* sceSDL_MasksToPixelFormatEnum = SDL_MasksToPixelFormatEnum;
EXPORT void* sceSDL_PixelFormatEnumToMasks = SDL_PixelFormatEnumToMasks;
EXPORT void* sceSDL_SetPaletteColors = SDL_SetPaletteColors;
EXPORT void* sceSDL_SetPixelFormatPalette = SDL_SetPixelFormatPalette;
EXPORT void* sceSDL_EnclosePoints = SDL_EnclosePoints;
EXPORT void* sceSDL_HasIntersection = SDL_HasIntersection;
EXPORT void* sceSDL_IntersectRect = SDL_IntersectRect;
EXPORT void* sceSDL_IntersectRectAndLine = SDL_IntersectRectAndLine;
EXPORT void* sceSDL_UnionRect = SDL_UnionRect;
EXPORT void* sceSDL_ConvertPixels = SDL_ConvertPixels;
EXPORT void* sceSDL_ConvertSurface = SDL_ConvertSurface;
EXPORT void* sceSDL_ConvertSurfaceFormat = SDL_ConvertSurfaceFormat;
EXPORT void* sceSDL_CreateRGBSurface = SDL_CreateRGBSurface;
EXPORT void* sceSDL_CreateRGBSurfaceFrom = SDL_CreateRGBSurfaceFrom;
EXPORT void* sceSDL_CreateRGBSurfaceWithFormat = SDL_CreateRGBSurfaceWithFormat;
EXPORT void* sceSDL_CreateRGBSurfaceWithFormatFrom = SDL_CreateRGBSurfaceWithFormatFrom;
EXPORT void* sceSDL_FillRect = SDL_FillRect;
EXPORT void* sceSDL_FillRects = SDL_FillRects;
EXPORT void* sceSDL_FreeSurface = SDL_FreeSurface;
EXPORT void* sceSDL_GetClipRect = SDL_GetClipRect;
EXPORT void* sceSDL_HasColorKey = SDL_HasColorKey;
EXPORT void* sceSDL_GetColorKey = SDL_GetColorKey;
EXPORT void* sceSDL_GetSurfaceAlphaMod = SDL_GetSurfaceAlphaMod;
EXPORT void* sceSDL_GetSurfaceBlendMode = SDL_GetSurfaceBlendMode;
EXPORT void* sceSDL_GetSurfaceColorMod = SDL_GetSurfaceColorMod;
EXPORT void* sceSDL_LockSurface = SDL_LockSurface;
EXPORT void* sceSDL_LowerBlit = SDL_LowerBlit;
EXPORT void* sceSDL_LowerBlitScaled = SDL_LowerBlitScaled;
EXPORT void* sceSDL_SetClipRect = SDL_SetClipRect;
EXPORT void* sceSDL_SetColorKey = SDL_SetColorKey;
EXPORT void* sceSDL_SetSurfaceAlphaMod = SDL_SetSurfaceAlphaMod;
EXPORT void* sceSDL_SetSurfaceBlendMode = SDL_SetSurfaceBlendMode;
EXPORT void* sceSDL_SetSurfaceColorMod = SDL_SetSurfaceColorMod;
EXPORT void* sceSDL_SetSurfacePalette = SDL_SetSurfacePalette;
EXPORT void* sceSDL_SetSurfaceRLE = SDL_SetSurfaceRLE;
EXPORT void* sceSDL_SoftStretch = SDL_SoftStretch;
EXPORT void* sceSDL_UnlockSurface = SDL_UnlockSurface;
EXPORT void* sceSDL_DuplicateSurface = SDL_DuplicateSurface;
EXPORT void* sceSDL_HasClipboardText = SDL_HasClipboardText;
EXPORT void* sceSDL_PumpEvents = SDL_PumpEvents;
EXPORT void* sceSDL_PeepEvents = SDL_PeepEvents;
EXPORT void* sceSDL_HasEvent = SDL_HasEvent;
EXPORT void* sceSDL_HasEvents = SDL_HasEvents;
EXPORT void* sceSDL_FlushEvent = SDL_FlushEvent;
EXPORT void* sceSDL_FlushEvents = SDL_FlushEvents;
EXPORT void* sceSDL_PollEvent = SDL_PollEvent;
EXPORT void* sceSDL_WaitEvent = SDL_WaitEvent;
EXPORT void* sceSDL_WaitEventTimeout = SDL_WaitEventTimeout;
EXPORT void* sceSDL_PushEvent = SDL_PushEvent;
EXPORT void* sceSDL_SetEventFilter = SDL_SetEventFilter;
EXPORT void* sceSDL_GetEventFilter = SDL_GetEventFilter;
EXPORT void* sceSDL_AddEventWatch = SDL_AddEventWatch;
EXPORT void* sceSDL_DelEventWatch = SDL_DelEventWatch;
EXPORT void* sceSDL_FilterEvents = SDL_FilterEvents;
EXPORT void* sceSDL_EventState = SDL_EventState;
EXPORT void* sceSDL_RegisterEvents = SDL_RegisterEvents;
EXPORT void* sceSDL_GetKeyboardFocus = SDL_GetKeyboardFocus;
EXPORT void* sceSDL_GetKeyboardState = SDL_GetKeyboardState;
EXPORT void* sceSDL_GetModState = SDL_GetModState;
EXPORT void* sceSDL_SetModState = SDL_SetModState;
EXPORT void* sceSDL_GetKeyFromScancode = SDL_GetKeyFromScancode;
EXPORT void* sceSDL_GetScancodeFromKey = SDL_GetScancodeFromKey;
EXPORT void* sceSDL_StartTextInput = SDL_StartTextInput;
EXPORT void* sceSDL_IsTextInputActive = SDL_IsTextInputActive;
EXPORT void* sceSDL_StopTextInput = SDL_StopTextInput;
EXPORT void* sceSDL_SetTextInputRect = SDL_SetTextInputRect;
EXPORT void* sceSDL_HasScreenKeyboardSupport = SDL_HasScreenKeyboardSupport;
EXPORT void* sceSDL_IsScreenKeyboardShown = SDL_IsScreenKeyboardShown;
EXPORT void* sceSDL_GetMouseFocus = SDL_GetMouseFocus;
EXPORT void* sceSDL_GetMouseState = SDL_GetMouseState;
EXPORT void* sceSDL_GetGlobalMouseState = SDL_GetGlobalMouseState;
EXPORT void* sceSDL_GetRelativeMouseState = SDL_GetRelativeMouseState;
EXPORT void* sceSDL_WarpMouseInWindow = SDL_WarpMouseInWindow;
EXPORT void* sceSDL_WarpMouseGlobal = SDL_WarpMouseGlobal;
EXPORT void* sceSDL_SetRelativeMouseMode = SDL_SetRelativeMouseMode;
EXPORT void* sceSDL_CaptureMouse = SDL_CaptureMouse;
EXPORT void* sceSDL_GetRelativeMouseMode = SDL_GetRelativeMouseMode;
EXPORT void* sceSDL_CreateCursor = SDL_CreateCursor;
EXPORT void* sceSDL_CreateColorCursor = SDL_CreateColorCursor;
EXPORT void* sceSDL_CreateSystemCursor = SDL_CreateSystemCursor;
EXPORT void* sceSDL_SetCursor = SDL_SetCursor;
EXPORT void* sceSDL_GetCursor = SDL_GetCursor;
EXPORT void* sceSDL_FreeCursor = SDL_FreeCursor;
EXPORT void* sceSDL_ShowCursor = SDL_ShowCursor;
EXPORT void* sceSDL_GetNumTouchDevices = SDL_GetNumTouchDevices;
EXPORT void* sceSDL_GetTouchDevice = SDL_GetTouchDevice;
EXPORT void* sceSDL_GetNumTouchFingers = SDL_GetNumTouchFingers;
EXPORT void* sceSDL_GetTouchFinger = SDL_GetTouchFinger;
EXPORT void* sceSDL_JoystickRumble = SDL_JoystickRumble;
EXPORT void* sceSDL_JoystickClose = SDL_JoystickClose;
EXPORT void* sceSDL_JoystickEventState = SDL_JoystickEventState;
EXPORT void* sceSDL_JoystickGetAxis = SDL_JoystickGetAxis;
EXPORT void* sceSDL_JoystickGetAxisInitialState = SDL_JoystickGetAxisInitialState;
EXPORT void* sceSDL_JoystickGetBall = SDL_JoystickGetBall;
EXPORT void* sceSDL_JoystickGetButton = SDL_JoystickGetButton;
EXPORT void* sceSDL_JoystickGetHat = SDL_JoystickGetHat;
EXPORT void* sceSDL_JoystickNumAxes = SDL_JoystickNumAxes;
EXPORT void* sceSDL_JoystickNumBalls = SDL_JoystickNumBalls;
EXPORT void* sceSDL_JoystickNumButtons = SDL_JoystickNumButtons;
EXPORT void* sceSDL_JoystickNumHats = SDL_JoystickNumHats;
EXPORT void* sceSDL_JoystickOpen = SDL_JoystickOpen;
EXPORT void* sceSDL_JoystickUpdate = SDL_JoystickUpdate;
EXPORT void* sceSDL_NumJoysticks = SDL_NumJoysticks;
EXPORT void* sceSDL_JoystickGetDeviceGUID = SDL_JoystickGetDeviceGUID;
EXPORT void* sceSDL_JoystickGetGUID = SDL_JoystickGetGUID;
EXPORT void* sceSDL_JoystickGetGUIDString = SDL_JoystickGetGUIDString;
EXPORT void* sceSDL_JoystickGetDeviceVendor = SDL_JoystickGetDeviceVendor;
EXPORT void* sceSDL_JoystickGetDeviceProduct = SDL_JoystickGetDeviceProduct;
EXPORT void* sceSDL_JoystickGetDeviceProductVersion = SDL_JoystickGetDeviceProductVersion;
EXPORT void* sceSDL_JoystickGetDeviceType = SDL_JoystickGetDeviceType;
EXPORT void* sceSDL_JoystickGetDeviceInstanceID = SDL_JoystickGetDeviceInstanceID;
EXPORT void* sceSDL_JoystickGetVendor = SDL_JoystickGetVendor;
EXPORT void* sceSDL_JoystickGetProduct = SDL_JoystickGetProduct;
EXPORT void* sceSDL_JoystickGetProductVersion = SDL_JoystickGetProductVersion;
EXPORT void* sceSDL_JoystickGetType = SDL_JoystickGetType;
EXPORT void* sceSDL_JoystickGetAttached = SDL_JoystickGetAttached;
EXPORT void* sceSDL_JoystickInstanceID = SDL_JoystickInstanceID;
EXPORT void* sceSDL_JoystickCurrentPowerLevel = SDL_JoystickCurrentPowerLevel;
EXPORT void* sceSDL_JoystickFromInstanceID = SDL_JoystickFromInstanceID;
EXPORT void* sceSDL_LockJoysticks = SDL_LockJoysticks;
EXPORT void* sceSDL_UnlockJoysticks = SDL_UnlockJoysticks;
EXPORT void* sceSDL_GameControllerNumMappings = SDL_GameControllerNumMappings;
EXPORT void* sceSDL_IsGameController = SDL_IsGameController;
EXPORT void* sceSDL_GameControllerOpen = SDL_GameControllerOpen;
EXPORT void* sceSDL_GameControllerGetVendor = SDL_GameControllerGetVendor;
EXPORT void* sceSDL_GameControllerGetProduct = SDL_GameControllerGetProduct;
EXPORT void* sceSDL_GameControllerGetProductVersion = SDL_GameControllerGetProductVersion;
EXPORT void* sceSDL_GameControllerGetAttached = SDL_GameControllerGetAttached;
EXPORT void* sceSDL_GameControllerGetJoystick = SDL_GameControllerGetJoystick;
EXPORT void* sceSDL_GameControllerEventState = SDL_GameControllerEventState;
EXPORT void* sceSDL_GameControllerUpdate = SDL_GameControllerUpdate;
EXPORT void* sceSDL_GameControllerGetAxis = SDL_GameControllerGetAxis;
EXPORT void* sceSDL_GameControllerGetButton = SDL_GameControllerGetButton;
EXPORT void* sceSDL_GameControllerRumble = SDL_GameControllerRumble;
EXPORT void* sceSDL_GameControllerClose = SDL_GameControllerClose;
EXPORT void* sceSDL_GameControllerFromInstanceID = SDL_GameControllerFromInstanceID;
EXPORT void* sceSDL_HapticClose = SDL_HapticClose;
EXPORT void* sceSDL_HapticDestroyEffect = SDL_HapticDestroyEffect;
EXPORT void* sceSDL_HapticEffectSupported = SDL_HapticEffectSupported;
EXPORT void* sceSDL_HapticGetEffectStatus = SDL_HapticGetEffectStatus;
EXPORT void* sceSDL_HapticIndex = SDL_HapticIndex;
EXPORT void* sceSDL_HapticNewEffect = SDL_HapticNewEffect;
EXPORT void* sceSDL_HapticNumAxes = SDL_HapticNumAxes;
EXPORT void* sceSDL_HapticNumEffects = SDL_HapticNumEffects;
EXPORT void* sceSDL_HapticNumEffectsPlaying = SDL_HapticNumEffectsPlaying;
EXPORT void* sceSDL_HapticOpen = SDL_HapticOpen;
EXPORT void* sceSDL_HapticOpened = SDL_HapticOpened;
EXPORT void* sceSDL_HapticOpenFromJoystick = SDL_HapticOpenFromJoystick;
EXPORT void* sceSDL_HapticOpenFromMouse = SDL_HapticOpenFromMouse;
EXPORT void* sceSDL_HapticPause = SDL_HapticPause;
EXPORT void* sceSDL_HapticQuery = SDL_HapticQuery;
EXPORT void* sceSDL_HapticRumbleInit = SDL_HapticRumbleInit;
EXPORT void* sceSDL_HapticRumblePlay = SDL_HapticRumblePlay;
EXPORT void* sceSDL_HapticRumbleStop = SDL_HapticRumbleStop;
EXPORT void* sceSDL_HapticRumbleSupported = SDL_HapticRumbleSupported;
EXPORT void* sceSDL_HapticRunEffect = SDL_HapticRunEffect;
EXPORT void* sceSDL_HapticSetAutocenter = SDL_HapticSetAutocenter;
EXPORT void* sceSDL_HapticSetGain = SDL_HapticSetGain;
EXPORT void* sceSDL_HapticStopAll = SDL_HapticStopAll;
EXPORT void* sceSDL_HapticStopEffect = SDL_HapticStopEffect;
EXPORT void* sceSDL_HapticUnpause = SDL_HapticUnpause;
EXPORT void* sceSDL_HapticUpdateEffect = SDL_HapticUpdateEffect;
EXPORT void* sceSDL_JoystickIsHaptic = SDL_JoystickIsHaptic;
EXPORT void* sceSDL_MouseIsHaptic = SDL_MouseIsHaptic;
EXPORT void* sceSDL_NumHaptics = SDL_NumHaptics;
EXPORT void* sceSDL_NumSensors = SDL_NumSensors;
EXPORT void* sceSDL_SensorGetDeviceType = SDL_SensorGetDeviceType;
EXPORT void* sceSDL_SensorGetDeviceNonPortableType = SDL_SensorGetDeviceNonPortableType;
EXPORT void* sceSDL_SensorGetDeviceInstanceID = SDL_SensorGetDeviceInstanceID;
EXPORT void* sceSDL_SensorOpen = SDL_SensorOpen;
EXPORT void* sceSDL_SensorFromInstanceID = SDL_SensorFromInstanceID;
EXPORT void* sceSDL_SensorGetType = SDL_SensorGetType;
EXPORT void* sceSDL_SensorGetNonPortableType = SDL_SensorGetNonPortableType;
EXPORT void* sceSDL_SensorGetInstanceID = SDL_SensorGetInstanceID;
EXPORT void* sceSDL_SensorGetData = SDL_SensorGetData;
EXPORT void* sceSDL_SensorClose = SDL_SensorClose;
EXPORT void* sceSDL_SensorUpdate = SDL_SensorUpdate;
EXPORT void* sceSDL_AudioQuit = SDL_AudioQuit;
EXPORT void* sceSDL_CloseAudio = SDL_CloseAudio;
EXPORT void* sceSDL_CloseAudioDevice = SDL_CloseAudioDevice;
EXPORT void* sceSDL_FreeWAV = SDL_FreeWAV;
EXPORT void* sceSDL_GetAudioDeviceStatus = SDL_GetAudioDeviceStatus;
EXPORT void* sceSDL_GetAudioStatus = SDL_GetAudioStatus;
EXPORT void* sceSDL_GetNumAudioDevices = SDL_GetNumAudioDevices;
EXPORT void* sceSDL_GetNumAudioDrivers = SDL_GetNumAudioDrivers;
EXPORT void* sceSDL_LockAudio = SDL_LockAudio;
EXPORT void* sceSDL_LockAudioDevice = SDL_LockAudioDevice;
EXPORT void* sceSDL_OpenAudio = SDL_OpenAudio;
EXPORT void* sceSDL_PauseAudio = SDL_PauseAudio;
EXPORT void* sceSDL_PauseAudioDevice = SDL_PauseAudioDevice;
EXPORT void* sceSDL_UnlockAudio = SDL_UnlockAudio;
EXPORT void* sceSDL_UnlockAudioDevice = SDL_UnlockAudioDevice;
EXPORT void* sceSDL_QueueAudio = SDL_QueueAudio;
EXPORT void* sceSDL_DequeueAudio = SDL_DequeueAudio;
EXPORT void* sceSDL_GetQueuedAudioSize = SDL_GetQueuedAudioSize;
EXPORT void* sceSDL_ClearQueuedAudio = SDL_ClearQueuedAudio;
EXPORT void* sceSDL_NewAudioStream = SDL_NewAudioStream;
EXPORT void* sceSDL_AudioStreamPut = SDL_AudioStreamPut;
EXPORT void* sceSDL_AudioStreamGet = SDL_AudioStreamGet;
EXPORT void* sceSDL_AudioStreamAvailable = SDL_AudioStreamAvailable;
EXPORT void* sceSDL_AudioStreamClear = SDL_AudioStreamClear;
EXPORT void* sceSDL_FreeAudioStream = SDL_FreeAudioStream;
EXPORT void* sceSDL_Delay = SDL_Delay;
EXPORT void* sceSDL_GetTicks = SDL_GetTicks;
EXPORT void* sceSDL_GetPerformanceCounter = SDL_GetPerformanceCounter;
EXPORT void* sceSDL_GetPerformanceFrequency = SDL_GetPerformanceFrequency;
EXPORT void* sceSDL_AddTimer = SDL_AddTimer;
EXPORT void* sceSDL_RemoveTimer = SDL_RemoveTimer;
EXPORT void* sceSDL_IsTablet = SDL_IsTablet;
EXPORT void* sceSDL_GetPowerInfo = SDL_GetPowerInfo;
EXPORT void* sceSDL_GetCPUCount = SDL_GetCPUCount;
EXPORT void* sceSDL_GetSystemRAM = SDL_GetSystemRAM;
EXPORT void* sceSDL_ShowMessageBox = SDL_ShowMessageBox;
EXPORT void* sceSDL_GetWindowGammaRamp = SDL_GetWindowGammaRamp;
EXPORT void* sceSDL_SetWindowGammaRamp = SDL_SetWindowGammaRamp;
EXPORT void* sceSDL_CalculateGammaRamp = SDL_CalculateGammaRamp;
EXPORT void* sceSDL_MixAudio = SDL_MixAudio;
EXPORT void* sceSDL_MixAudioFormat = SDL_MixAudioFormat;
EXPORT void* sceIMG_Init = IMG_Init;
EXPORT void* sceIMG_Quit = IMG_Quit;
EXPORT void* sceIMG_Load_RW = IMG_Load_RW;
EXPORT void* sceIMG_LoadTexture_RW = IMG_LoadTexture_RW;
EXPORT void* sceIMG_SavePNG_RW = IMG_SavePNG_RW;
EXPORT void* sceIMG_SaveJPG_RW = IMG_SaveJPG_RW;
EXPORT void* sceIMG_Linked_Version = IMG_Linked_Version;
EXPORT void* sceIMG_Load = IMG_Load;
EXPORT void* sceIMG_LoadTyped_RW = IMG_LoadTyped_RW;
EXPORT void* sceIMG_LoadTexture = IMG_LoadTexture;
EXPORT void* sceIMG_LoadTextureTyped_RW = IMG_LoadTextureTyped_RW;
EXPORT void* sceIMG_SavePNG = IMG_SavePNG;
