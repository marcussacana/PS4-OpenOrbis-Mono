using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using OrbisGL.FreeTypeLib;
using OrbisGL.GL;

namespace OrbisGL.GL2D
{
    public class RichText2D : GLObject2D
    {
        public RGBColor FontColor { get; set; } = RGBColor.White;
        public int FontSize { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }


        public FontFaceHandler Font { get; set; }

        public RichText2D(int FontSize, RGBColor FontColor, string FontPath) : this(Text2D.GetFont(FontPath, FontSize, out _), FontSize, FontColor) {

        }

        public RichText2D(FontFaceHandler FontFace, int FontSize, RGBColor FontColor)
        {
            this.FontColor = FontColor;
            this.Font = FontFace;
            this.FontSize = FontSize;
        }

        public string RichText { get; private set; }

        public string Text { get; private set; }

        public Vector4[] GlyphsSpace { get; private set; }

        //<font=Font file name.ttf></font>  - Search the Font in directories defined in FreeType.FontSearchDirectories
        //<color=FFFFFF></color>            - Set the font color as RGB
        //<size=28></size>                  - Set the font size

        public bool SetRichText(string String)
        {
            RemoveChildren(true);
            RichText = String;

            if (RichText == null)
            {
                this.GlyphsSpace = null;
                Text = null;
                return true;
            }

            Text = string.Empty;

            sColors.Clear();
            sFontSize.Clear();
            sFont.Clear();

            sColors.Push(FontColor);
            sFontSize.Push(FontSize);
            sFont.Push(Font);

            int XOffset = 0;
            int YOffset = 0;
            int AdvanceY = 0;
            Dictionary<int, Vector4> GlyphsSpace = new Dictionary<int, Vector4>();
            ProcessTextBlock(String, 0, ref XOffset, ref YOffset, ref AdvanceY, ref GlyphsSpace);

            return true;
        }

        Stack<RGBColor> sColors = new Stack<RGBColor>();
        Stack<int> sFontSize = new Stack<int>();
        Stack<FontFaceHandler> sFont = new Stack<FontFaceHandler>();

        private void ProcessTextBlock(string String, int Index, ref int XOffset, ref int YOffset, ref int AdvanceY, ref Dictionary<int, Vector4> GlyphsSpace)
        {
            string Accumulator = string.Empty;

            for (int i = 0; i < String.Length; i++)
            {
                char Current = String[i];
                char? Next = i + 1 < String.Length ? (char?)String[i + 1] : null;

                if (Current == '\n')
                {
                    FlushString(Index, ref XOffset, YOffset, ref AdvanceY, GlyphsSpace, ref Accumulator);

                    Text += '\n';

                    GlyphsSpace.Add(Index + i, new Vector4(XOffset, YOffset, 1, AdvanceY));

                    XOffset = 0;
                    YOffset += AdvanceY;
                    AdvanceY = 0;
                    continue;
                }

                if (Current == '<' && Next != '<')
                {
                    FlushString(Index, ref XOffset, YOffset, ref AdvanceY, GlyphsSpace, ref Accumulator);

                    if (ProcessTextBlockTag(String, Index, ref XOffset, ref YOffset, ref AdvanceY, ref GlyphsSpace, ref i))
                        continue;
                }
                else if (Current == Next && Current == '<')
                    i++;

                Accumulator += Current;
            }

            FlushString(Index, ref XOffset, YOffset, ref AdvanceY, GlyphsSpace, ref Accumulator);
        }

        private void FlushString(int Index, ref int XOffset, int YOffset, ref int AdvanceY, Dictionary<int, Vector4> GlyphsSpace, ref string Accumulator)
        {
            if (Accumulator == string.Empty)
                return;

            var CurrentColor = sColors.Peek();
            var CurrentFace = sFont.Peek();
            var CurrentSize = sFontSize.Peek();

            var TextFragment = new Text2D(CurrentFace, CurrentSize);
            TextFragment.Color = CurrentColor;
            TextFragment.Position = new Vector2(XOffset, YOffset);
            TextFragment.SetText(Accumulator);

            AddChild(TextFragment);

            for (int x = 0; x < TextFragment.Text.Length; x++)
            {
                var AbsIndex = x + Index;
                var CurrentGlyphSpace = TextFragment.GlyphsSpace[x];
                GlyphsSpace[AbsIndex] = new Vector4(CurrentGlyphSpace.X + XOffset, CurrentGlyphSpace.Y + YOffset, CurrentGlyphSpace.Z, CurrentGlyphSpace.W);
            }

            AdvanceY = Math.Max(AdvanceY, TextFragment.Height);
            XOffset += TextFragment.Width;

            Text += Accumulator;
            Accumulator = string.Empty;
        }

        private bool ProcessTextBlockTag(string String, int Index, ref int XOffset, ref int YOffset, ref int AdvanceY, ref Dictionary<int, Vector4> GlyphsSpace, ref int i)
        {
            //Find Tag Begin and End Position
            var TagOpen = i;
            var TagClose = FindTagClose(String, i);

            //Get the tag open block <hi>sample</hi> => <hi>
            var Tag = String.Substring(TagOpen + 1, String.IndexOf('>', TagOpen) - TagOpen - 1);
            var TagName = Tag.Split(' ', '=').First().ToLowerInvariant();

            if (TagName.StartsWith("/"))
                return false;

            //Get anything after the tag name
            var TagAttribute = Tag.Substring(TagName.Length);

            string AttributeValue = null;
            if (TagAttribute.StartsWith("="))
                AttributeValue = TagAttribute.Substring(1).Trim();

            //Get the inner content of the tag
            var InnerContent = String.Substring(TagOpen, TagClose - TagOpen);
            InnerContent = InnerContent.Substring(InnerContent.IndexOf('>') + 1);
            InnerContent = InnerContent.Substring(0, InnerContent.LastIndexOf('<'));

            EnableTag(TagName, AttributeValue);

            ProcessTextBlock(InnerContent, Index + i, ref XOffset, ref YOffset, ref AdvanceY, ref GlyphsSpace);

            sColors.Pop();
            sFont.Pop();
            sFontSize.Pop();

            i = TagClose - 1;
            return true;
        }

        private void EnableTag(string Name, string Value)
        {
            int.TryParse(Value, out int ValueAsInt);

            var lValue = Value.ToLowerInvariant();
            
            switch (Name.ToLowerInvariant())
            {
                case "color":
                    sFont.Push(sFont.Peek());
                    sFontSize.Push(sFontSize.Peek());
                    sColors.Push(new RGBColor(Value));
                    break;
                case "size":
                    sFont.Push(sFont.Peek());
                    sColors.Push(sColors.Peek());
                    sFontSize.Push(ValueAsInt);
                    break;
                case "font":
                    var NewFont = Text2D.GetFont(Value, sFontSize.Peek(), out bool Success);
                    if (Success)
                    {
                        sFontSize.Push(sFontSize.Peek());
                        sColors.Push(sColors.Peek());
                        sFont.Push(NewFont);
                    }
                    else
                    {
                        sFontSize.Push(sFontSize.Peek());
                        sColors.Push(sColors.Peek());
                        sFont.Push(sFont.Peek());
                    }
                    break;
            }
        }

        private static int FindTagClose(string String, int TagOpen)
        {
            int Level = 1;

            if (String[TagOpen] != '<')
                return TagOpen;

            //Skip '<' from the tag begin
            TagOpen++;

            //Check if is an escaped character or tag end
            if (String[TagOpen] == '<' || String[TagOpen] == '/')
                return TagOpen - 1;

            //Get the index of the tag inner content
            int i = String.IndexOf('>', TagOpen);

            //Get tag name ignoring attribute values
            var TagName = String.Substring(TagOpen, i - TagOpen).Split(' ', '=').First();

            for (; i < String.Length && Level > 0; i++)
            {
                char Current = String[i];
                char? Next = i + 1 < String.Length ? (char?)String[i + 1] : null;


                if (Current == '<' && Next != '<')
                {
                    //Find current tag end
                    var TagEnd = String.IndexOf('>', i);
                    //Get the tag content ignoring the attribute values
                    var TagContent = String.Substring(i + 1, TagEnd - i - 1).Split(' ', '=').First();

                    //Check if is a closing tag
                    bool Closing = Next == '/';
                    if (Closing)
                        TagContent = TagContent.Substring(1);

                    //if the tag is the same of the openning, process the tag level
                    if (TagContent.ToLowerInvariant() == TagName.ToLowerInvariant())
                    {
                        Level += Closing ? -1 : 1;
                    }

                    //Update current index
                    i = TagEnd;
                    continue;
                }
            }

            return i;
        }
    }
}
