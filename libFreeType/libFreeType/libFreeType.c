#include <proto-include.h>
#include <freetype/freetype.h>
#include <freetype/ftcache.h>
#include <freetype/ftsizes.h>
#include <freetype/ftmodapi.h>
#include <freetype/fttrigon.h>
#include <freetype/ftmm.h>
#include <freetype/ftsnames.h>
#include <freetype/ftrender.h>
#include <freetype/tttables.h>
#include <freetype/ftlist.h>
#include <freetype/ftbbox.h>

void* sceFT_Render_Glyph = FT_Render_Glyph;
//void* sceFTC_CMapCache_Lookup = FTC_CMapCache_Lookup;
//void* sceFTC_CMapCache_New = FTC_CMapCache_New;
//void* sceFTC_ImageCache_Lookup = FTC_ImageCache_Lookup;
//void* sceFTC_ImageCache_New = FTC_ImageCache_New;
//void* sceFTC_Image_Cache_Lookup = FTC_Image_Cache_Lookup;
//void* sceFTC_Image_Cache_New = FTC_Image_Cache_New;
//void* sceFTC_Manager_Done = FTC_Manager_Done;
//void* sceFTC_Manager_Lookup_Face = FTC_Manager_Lookup_Face;
//void* sceFTC_Manager_Lookup_Size = FTC_Manager_Lookup_Size;
//void* sceFTC_Manager_New = FTC_Manager_New;
//void* sceFTC_Manager_Reset = FTC_Manager_Reset;
//void* sceFTC_Node_Unref = FTC_Node_Unref;
//void* sceFTC_SBitCache_Lookup = FTC_SBitCache_Lookup;
//void* sceFTC_SBitCache_New = FTC_SBitCache_New;
//void* sceFTC_SBit_Cache_Lookup = FTC_SBit_Cache_Lookup;
//void* sceFTC_SBit_Cache_New = FTC_SBit_Cache_New;
void* sceFT_Activate_Size = FT_Activate_Size;
void* sceFT_Add_Default_Modules = FT_Add_Default_Modules;
void* sceFT_Add_Module = FT_Add_Module;
void* sceFT_Atan2 = FT_Atan2;
void* sceFT_Attach_File = FT_Attach_File;
void* sceFT_Attach_Stream = FT_Attach_Stream;
void* sceFT_CeilFix = FT_CeilFix;
void* sceFT_Cos = FT_Cos;
void* sceFT_DivFix = FT_DivFix;
void* sceFT_Done_Face = FT_Done_Face;
void* sceFT_Done_FreeType = FT_Done_FreeType;
void* sceFT_Done_Glyph = FT_Done_Glyph;
void* sceFT_Done_Library = FT_Done_Library;
void* sceFT_Done_Size = FT_Done_Size;
void* sceFT_FloorFix = FT_FloorFix;
void* sceFT_Get_Char_Index = FT_Get_Char_Index;
void* sceFT_Get_First_Char = FT_Get_First_Char;
void* sceFT_Get_Glyph = FT_Get_Glyph;
void* sceFT_Get_Glyph_Name = FT_Get_Glyph_Name;
void* sceFT_Get_Kerning = FT_Get_Kerning;
void* sceFT_Get_Module = FT_Get_Module;
void* sceFT_Get_Multi_Master = FT_Get_Multi_Master;
void* sceFT_Get_Name_Index = FT_Get_Name_Index;
void* sceFT_Get_Next_Char = FT_Get_Next_Char;
void* sceFT_Get_Postscript_Name = FT_Get_Postscript_Name;
void* sceFT_Get_Renderer = FT_Get_Renderer;
void* sceFT_Get_Sfnt_Name = FT_Get_Sfnt_Name;
void* sceFT_Get_Sfnt_Name_Count = FT_Get_Sfnt_Name_Count;
void* sceFT_Get_Sfnt_Table = FT_Get_Sfnt_Table;
void* sceFT_Glyph_Copy = FT_Glyph_Copy;
void* sceFT_Glyph_Get_CBox = FT_Glyph_Get_CBox;
void* sceFT_Glyph_To_Bitmap = FT_Glyph_To_Bitmap;
void* sceFT_Glyph_Transform = FT_Glyph_Transform;
void* sceFT_Init_FreeType = FT_Init_FreeType;
void* sceFT_Library_Version = FT_Library_Version;
void* sceFT_List_Add = FT_List_Add;
void* sceFT_List_Finalize = FT_List_Finalize;
void* sceFT_List_Find = FT_List_Find;
void* sceFT_List_Insert = FT_List_Insert;
void* sceFT_List_Iterate = FT_List_Iterate;
void* sceFT_List_Remove = FT_List_Remove;
void* sceFT_List_Up = FT_List_Up;
void* sceFT_Load_Char = FT_Load_Char;
void* sceFT_Load_Glyph = FT_Load_Glyph;
void* sceFT_Matrix_Invert = FT_Matrix_Invert;
void* sceFT_Matrix_Multiply = FT_Matrix_Multiply;
void* sceFT_MulDiv = FT_MulDiv;
void* sceFT_New_Face = FT_New_Face;
void* sceFT_New_Library = FT_New_Library;
void* sceFT_New_Memory_Face = FT_New_Memory_Face;
void* sceFT_New_Size = FT_New_Size;
void* sceFT_Open_Face = FT_Open_Face;
void* sceFT_Outline_Check = FT_Outline_Check;
void* sceFT_Outline_Copy = FT_Outline_Copy;
void* sceFT_Outline_Decompose = FT_Outline_Decompose;
void* sceFT_Outline_Done = FT_Outline_Done;
void* sceFT_Outline_Done_Internal = FT_Outline_Done_Internal;
void* sceFT_Outline_Get_BBox = FT_Outline_Get_BBox;
void* sceFT_Outline_Get_Bitmap = FT_Outline_Get_Bitmap;
void* sceFT_Outline_Get_CBox = FT_Outline_Get_CBox;
void* sceFT_Outline_New = FT_Outline_New;
void* sceFT_Outline_New_Internal = FT_Outline_New_Internal;
void* sceFT_Outline_Render = FT_Outline_Render;
void* sceFT_Outline_Reverse = FT_Outline_Reverse;
void* sceFT_Outline_Transform = FT_Outline_Transform;
void* sceFT_Outline_Translate = FT_Outline_Translate;
void* sceFT_Remove_Module = FT_Remove_Module;
void* sceFT_RoundFix = FT_RoundFix;
void* sceFT_Select_Charmap = FT_Select_Charmap;
void* sceFT_Set_Char_Size = FT_Set_Char_Size;
void* sceFT_Set_Charmap = FT_Set_Charmap;
void* sceFT_Set_Debug_Hook = FT_Set_Debug_Hook;
void* sceFT_Set_MM_Blend_Coordinates = FT_Set_MM_Blend_Coordinates;
void* sceFT_Set_MM_Design_Coordinates = FT_Set_MM_Design_Coordinates;
void* sceFT_Set_Pixel_Sizes = FT_Set_Pixel_Sizes;
void* sceFT_Set_Renderer = FT_Set_Renderer;
void* sceFT_Sin = FT_Sin;
void* sceFT_Tan = FT_Tan;
void* sceFT_Vector_Length = FT_Vector_Length;
void* sceFT_Vector_Polarize = FT_Vector_Polarize;
void* sceFT_Vector_Rotate = FT_Vector_Rotate;
void* sceFT_Vector_Transform = FT_Vector_Transform;
void* sceFT_Vector_Unit = FT_Vector_Unit;

