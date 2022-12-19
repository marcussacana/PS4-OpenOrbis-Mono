namespace SDL2.Types.FreeType
{
    public enum FT_Glyph_Format
    {
        None = 0,
        Composite = 0x636f6d70, // 'comp'
        Bitmap = 0x62697473, // 'bits'
        Outline = 0x6f75746c, // 'outl'
        Plotter = 0x706c6f74, // 'plot'
        SVG = 0x53564720 // 'SVG '
    }
}