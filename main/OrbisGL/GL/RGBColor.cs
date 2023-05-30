using SixLabors.ImageSharp.ColorSpaces;

namespace OrbisGL.GL
{
    public class RGBColor
    {
        internal float RedF;
        internal float GreenF;
        internal float BlueF;

        public RGBColor(byte Red, byte Green, byte Blue)
        {
            RedF = Red / 255f;
            GreenF = Green / 255f;
            BlueF = Blue / 255f;
        }

        public int R { get => (int)(255 * RedF); set => RedF = value / 255f; }
        public int G { get => (int)(255 * GreenF); set => GreenF = value / 255f; }
        public int B { get => (int)(255 * BlueF); set => BlueF = value / 255f; }
        
        
        #region NamedColors
        public static RGBColor AliceBlue = new RGBColor(240, 248, 255);
        public static RGBColor AntiqueWhite = new RGBColor(250, 235, 215);
        public static RGBColor Aqua = new RGBColor(0, 255, 255);
        public static RGBColor Aquamarine = new RGBColor(127, 255, 212);
        public static RGBColor Azure = new RGBColor(240, 255, 255);
        public static RGBColor Beige = new RGBColor(245, 245, 220);
        public static RGBColor Bisque = new RGBColor(255, 228, 196);
        public static RGBColor Black = new RGBColor(0, 0, 0);
        public static RGBColor BlanchedAlmond = new RGBColor(255, 235, 205);
        public static RGBColor Blue = new RGBColor(0, 0, 255);
        public static RGBColor BlueViolet = new RGBColor(138, 43, 226);
        public static RGBColor Brown = new RGBColor(165, 42, 42);
        public static RGBColor BurlyWood = new RGBColor(222, 184, 135);
        public static RGBColor CadetBlue = new RGBColor(95, 158, 160);
        public static RGBColor Chartreuse = new RGBColor(127, 255, 0);
        public static RGBColor Chocolate = new RGBColor(210, 105, 30);
        public static RGBColor Coral = new RGBColor(255, 127, 80);
        public static RGBColor CornflowerBlue = new RGBColor(100, 149, 237);
        public static RGBColor Cornsilk = new RGBColor(255, 248, 220);
        public static RGBColor Crimson = new RGBColor(220, 20, 60);
        public static RGBColor Cyan = new RGBColor(0, 255, 255);
        public static RGBColor DarkBlue = new RGBColor(0, 0, 139);
        public static RGBColor DarkCyan = new RGBColor(0, 139, 139);
        public static RGBColor DarkGoldenrod = new RGBColor(184, 134, 11);
        public static RGBColor DarkGray = new RGBColor(169, 169, 169);
        public static RGBColor DarkGreen = new RGBColor(0, 100, 0);
        public static RGBColor DarkKhaki = new RGBColor(189, 183, 107);
        public static RGBColor DarkMagenta = new RGBColor(139, 0, 139);
        public static RGBColor DarkOliveGreen = new RGBColor(85, 107, 47);
        public static RGBColor DarkOrange = new RGBColor(255, 140, 0);
        public static RGBColor DarkOrchid = new RGBColor(153, 50, 204);
        public static RGBColor DarkRed = new RGBColor(139, 0, 0);
        public static RGBColor DarkSalmon = new RGBColor(233, 150, 122);
        public static RGBColor DarkSeaGreen = new RGBColor(143, 188, 139);
        public static RGBColor DarkSlateBlue = new RGBColor(72, 61, 139);
        public static RGBColor DarkSlateGray = new RGBColor(47, 79, 79);
        public static RGBColor DarkTurquoise = new RGBColor(0, 206, 209);
        public static RGBColor DarkViolet = new RGBColor(148, 0, 211);
        public static RGBColor DeepPink = new RGBColor(255, 20, 147);
        public static RGBColor DeepSkyBlue = new RGBColor(0, 191, 255);
        public static RGBColor DimGray = new RGBColor(105, 105, 105);
        public static RGBColor DodgerBlue = new RGBColor(30, 144, 255);
        public static RGBColor Firebrick = new RGBColor(178, 34, 34);
        public static RGBColor FloralWhite = new RGBColor(255, 250, 240);
        public static RGBColor ForestGreen = new RGBColor(34, 139, 34);
        public static RGBColor Fuchsia = new RGBColor(255, 0, 255);
        public static RGBColor Gainsboro = new RGBColor(220, 220, 220);
        public static RGBColor GhostWhite = new RGBColor(248, 248, 255);
        public static RGBColor Gold = new RGBColor(255, 215, 0);
        public static RGBColor Goldenrod = new RGBColor(218, 165, 32);
        public static RGBColor Gray = new RGBColor(128, 128, 128);
        public static RGBColor GreenYellow = new RGBColor(173, 255, 47);
        public static RGBColor Honeydew = new RGBColor(240, 255, 240);
        public static RGBColor HotPink = new RGBColor(255, 105, 180);
        public static RGBColor IndianRed = new RGBColor(205, 92, 92);
        public static RGBColor Indigo = new RGBColor(75, 0, 130);
        public static RGBColor Ivory = new RGBColor(255, 255, 240);
        public static RGBColor Khaki = new RGBColor(240, 230, 140);
        public static RGBColor Lavender = new RGBColor(230, 230, 250);
        public static RGBColor LavenderBlush = new RGBColor(255, 240, 245);
        public static RGBColor LawnGreen = new RGBColor(124, 252, 0);
        public static RGBColor LemonChiffon = new RGBColor(255, 250, 205);
        public static RGBColor LightBlue = new RGBColor(173, 216, 230);
        public static RGBColor LightCoral = new RGBColor(240, 128, 128);
        public static RGBColor LightCyan = new RGBColor(224, 255, 255);
        public static RGBColor LightGoldenrodYellow = new RGBColor(250, 250, 210);
        public static RGBColor LightGray = new RGBColor(211, 211, 211);
        public static RGBColor LightGreen = new RGBColor(144, 238, 144);
        public static RGBColor LightPink = new RGBColor(255, 182, 193);
        public static RGBColor LightSalmon = new RGBColor(255, 160, 122);
        public static RGBColor LightSeaGreen = new RGBColor(32, 178, 170);
        public static RGBColor LightSkyBlue = new RGBColor(135, 206, 250);
        public static RGBColor LightSlateGray = new RGBColor(119, 136, 153);
        public static RGBColor LightSteelBlue = new RGBColor(176, 196, 222);
        public static RGBColor LightYellow = new RGBColor(255, 255, 224);
        public static RGBColor Lime = new RGBColor(0, 255, 0);
        public static RGBColor LimeGreen = new RGBColor(50, 205, 50);
        public static RGBColor Linen = new RGBColor(250, 240, 230);
        public static RGBColor Magenta = new RGBColor(255, 0, 255);
        public static RGBColor Maroon = new RGBColor(128, 0, 0);
        public static RGBColor MediumAquamarine = new RGBColor(102, 205, 170);
        public static RGBColor MediumBlue = new RGBColor(0, 0, 205);
        public static RGBColor MediumOrchid = new RGBColor(186, 85, 211);
        public static RGBColor MediumPurple = new RGBColor(147, 112, 219);
        public static RGBColor MediumSeaGreen = new RGBColor(60, 179, 113);
        public static RGBColor MediumSlateBlue = new RGBColor(123, 104, 238);
        public static RGBColor MediumSpringGreen = new RGBColor(0, 250, 154);
        public static RGBColor MediumTurquoise = new RGBColor(72, 209, 204);
        public static RGBColor MediumVioletRed = new RGBColor(199, 21, 133);
        public static RGBColor MidnightBlue = new RGBColor(25, 25, 112);
        public static RGBColor MintCream = new RGBColor(245, 255, 250);
        public static RGBColor MistyRose = new RGBColor(255, 228, 225);
        public static RGBColor Moccasin = new RGBColor(255, 228, 181);
        public static RGBColor NavajoWhite = new RGBColor(255, 222, 173);
        public static RGBColor Navy = new RGBColor(0, 0, 128);
        public static RGBColor OldLace = new RGBColor(253, 245, 230);
        public static RGBColor Olive = new RGBColor(128, 128, 0);
        public static RGBColor OliveDrab = new RGBColor(107, 142, 35);
        public static RGBColor Orange = new RGBColor(255, 165, 0);
        public static RGBColor OrangeRed = new RGBColor(255, 69, 0);
        public static RGBColor Orchid = new RGBColor(218, 112, 214);
        public static RGBColor PaleGoldenrod = new RGBColor(238, 232, 170);
        public static RGBColor PaleGreen = new RGBColor(152, 251, 152);
        public static RGBColor PaleTurquoise = new RGBColor(175, 238, 238);
        public static RGBColor PaleVioletRed = new RGBColor(219, 112, 147);
        public static RGBColor PapayaWhip = new RGBColor(255, 239, 213);
        public static RGBColor PeachPuff = new RGBColor(255, 218, 185);
        public static RGBColor Peru = new RGBColor(205, 133, 63);
        public static RGBColor Pink = new RGBColor(255, 192, 203);
        public static RGBColor Plum = new RGBColor(221, 160, 221);
        public static RGBColor PowderBlue = new RGBColor(176, 224, 230);
        public static RGBColor Purple = new RGBColor(128, 0, 128);
        public static RGBColor Red = new RGBColor(255, 0, 0);
        public static RGBColor RosyBrown = new RGBColor(188, 143, 143);
        public static RGBColor RoyalBlue = new RGBColor(65, 105, 225);
        public static RGBColor SaddleBrown = new RGBColor(139, 69, 19);
        public static RGBColor Salmon = new RGBColor(250, 128, 114);
        public static RGBColor SandyBrown = new RGBColor(244, 164, 96);
        public static RGBColor SeaGreen = new RGBColor(46, 139, 87);
        public static RGBColor Seashell = new RGBColor(255, 245, 238);
        public static RGBColor Sienna = new RGBColor(160, 82, 45);
        public static RGBColor Silver = new RGBColor(192, 192, 192);
        public static RGBColor SkyBlue = new RGBColor(135, 206, 235);
        public static RGBColor SlateBlue = new RGBColor(106, 90, 205);
        public static RGBColor SlateGray = new RGBColor(112, 128, 144);
        public static RGBColor Snow = new RGBColor(255, 250, 250);
        public static RGBColor SpringGreen = new RGBColor(0, 255, 127);
        public static RGBColor SteelBlue = new RGBColor(70, 130, 180);
        public static RGBColor Tan = new RGBColor(210, 180, 140);
        public static RGBColor Teal = new RGBColor(0, 128, 128);
        public static RGBColor Thistle = new RGBColor(216, 191, 216);
        public static RGBColor Tomato = new RGBColor(255, 99, 71);
        public static RGBColor Turquoise = new RGBColor(64, 224, 208);
        public static RGBColor Violet = new RGBColor(238, 130, 238);
        public static RGBColor Wheat = new RGBColor(245, 222, 179);
        public static RGBColor White = new RGBColor(255, 255, 255);
        public static RGBColor WhiteSmoke = new RGBColor(245, 245, 245);
        public static RGBColor Yellow = new RGBColor(255, 255, 0);
        public static RGBColor YellowGreen = new RGBColor(154, 205, 50);
        #endregion
    }
}