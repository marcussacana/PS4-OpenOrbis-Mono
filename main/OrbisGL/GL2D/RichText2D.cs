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

        public List<GlyphInfo> GlyphsSpace { get; private set; } = new List<GlyphInfo>();

        //<font=Font file name.ttf></font>  - Search the Font in directories defined in FreeType.FontSearchDirectories
        //<color=FFFFFF></color>            - Set the font color as RGB
        //<size=28></size>                  - Set the font size
        //<align=center></align>            - Set the line alignment, valid values: top|bottom|left|right|center|verticalcenter|horizontalcenter

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

            LinesInfoList.Clear();

            sColors.Push(FontColor);
            sFontSize.Push(FontSize);
            sFont.Push(Font);

            CurrentAlign = LineAlignMode.None;

            int XOffset = 0;
            int YOffset = 0;
            int AdvanceY = 0;

            CurrentLineGlyphIndex = 0;

            GlyphsSpace.Clear();

            List<Text2D> LineBuffer = new List<Text2D>();

            ProcessTextBlock(String, 0, ref XOffset, ref YOffset, ref AdvanceY, ref LineBuffer);

            //Flush Last Line Info
            LinesInfoList.Add(new LineInfo(LineBuffer.ToList(), CurrentAlign, CurrentLineGlyphIndex));


            ProcessAlignment();

            return true;
        }

        private void ProcessAlignment()
        {
            float MaxLineWidth = LinesInfoList.Max(x => x.LineWidth);
            float TextHCenter = MaxLineWidth / 2;


            for (int i = 0; i < LinesInfoList.Count; i++)
            {
                var LineInfo = LinesInfoList[i];
                int LineGlyphIndex = 0;

                float LineWidth = LineInfo.LineWidth;
                float LineHeight = LineInfo.LineHeight;

                float LineXOffset = 0;

                float LineHCenter = (LineInfo.LineWidth / 2f);
                float LineVCenter = (LineInfo.LineHeight / 2f);
                foreach (var Item in LineInfo.Items)
                {
                    float NewX = 0, NewY = 0;

                    float VCenterDistance = LineVCenter - Item.Position.X;

                    switch (LineInfo.AlignMode)
                    {
                        //Default Aligns
                        case LineAlignMode.Left:
                        case LineAlignMode.Top:
                        case LineAlignMode.Left | LineAlignMode.Top:
                        case LineAlignMode.None:
                            NewX = float.NaN;
                            NewY = float.NaN;
                            break;

                        case LineAlignMode.VerticalCenter:
                            NewX = float.NaN;
                            NewY = LineVCenter - (Item.Height / 2f);
                            break;

                        case LineAlignMode.HorizontalCenter:
                            NewX = TextHCenter - LineHCenter + LineXOffset;
                            NewY = float.NaN;
                            break;

                        case LineAlignMode.Center:
                            NewY = LineVCenter - (Item.Height / 2f);
                            NewX = TextHCenter - LineHCenter + LineXOffset;
                            break;

                        case LineAlignMode.Right:
                            NewY = float.NaN;
                            NewX = (MaxLineWidth - LineWidth) + LineXOffset;
                            break;

                        case LineAlignMode.Bottom:
                            NewX = float.NaN;
                            NewY = LineHeight - Item.Height;
                            break;
                    }

                    if (float.IsNaN(NewX))
                        NewX = Item.Position.X;

                    if (float.IsNaN(NewY))
                        NewY = Item.Position.Y;

                    Item.Position = new Vector2(NewX, NewY + LineInfo.LineY);

                    //Update glyph space offsets
                    UpdateGlyphOffset(LineInfo.GlyphIndex + LineGlyphIndex, (int)NewX, (int)(NewY + LineInfo.LineY), Item);

                    LineXOffset += Item.Width;
                    LineGlyphIndex += Item.Text.Length;
                }
            }
        }

        struct LineInfo
        {
            public LineInfo(List<Text2D> Items, LineAlignMode AlignMode, int GlyphIndex)
            {
                this.Items = Items;
                this.AlignMode = AlignMode;
                this.GlyphIndex = GlyphIndex;
            }

            public List<Text2D> Items;

            public LineAlignMode AlignMode;

            public int GlyphIndex;

            public float LineWidth => Items.Max(x => x.Position.X + x.Width);
            public float LineHeight => Items.Max(x => x.Height);

            public float LineX => Items.Min(x => x.Position.X);
            public float LineY => Items.Min(x => x.Position.Y);
        }

        [Flags]
        enum LineAlignMode
        {
            None = 0, 
            Left = 1 << 0, 
            Right = 1 << 1, 
            Top = 1 << 2,
            Bottom = 1 << 3,
            VerticalCenter = Top | Bottom,
            HorizontalCenter = Left | Right,
            Center = VerticalCenter | HorizontalCenter
        }

        List<LineInfo> LinesInfoList = new List<LineInfo>();

        Stack<RGBColor> sColors = new Stack<RGBColor>();
        Stack<int> sFontSize = new Stack<int>();
        Stack<FontFaceHandler> sFont = new Stack<FontFaceHandler>();

        LineAlignMode CurrentAlign = LineAlignMode.None;
        int CurrentLineGlyphIndex = 0;

        private void ProcessTextBlock(string String, int Index, ref int XOffset, ref int YOffset, ref int AdvanceY, ref List<Text2D> LineBuffer)
        {
            //The Accumulator holds the max sequential string with no style changes
            //and it is flushed when a new tag or a new line appears in the string
            string Accumulator = string.Empty;

            for (int i = 0; i < String.Length; i++)
            {
                //Get the current string char and the next one
                //The next character is used to determine if the current character is escaped
                char Current = String[i];
                char? Next = i + 1 < String.Length ? (char?)String[i + 1] : null;


                //End of the current Line
                if (Current == '\n')
                {
                    //Flush the Accumulator and store the text information
                    var TextItem = FlushString(Index, ref XOffset, YOffset, ref AdvanceY, ref Accumulator);

                    if (TextItem != null)
                        LineBuffer.Add(TextItem);

                    //Update the plain text output
                    Text += '\n';

                    //Add empty glyph to keep the glyph space index equivalent with the input string
                    GlyphsSpace.Add(new GlyphInfo(XOffset, YOffset, 1, AdvanceY, '\n', Text.Length - 1));

                    //Add Current Line Info to the list
                    LinesInfoList.Add(new LineInfo(LineBuffer.ToList(), CurrentAlign, CurrentLineGlyphIndex));

                    //Clear the line buffer
                    LineBuffer.Clear();

                    //Update the line draw offsets
                    XOffset = 0;
                    YOffset += AdvanceY;
                    AdvanceY = 0;

                    //Update Line Params
                    CurrentAlign = LineAlignMode.None;
                    CurrentLineGlyphIndex = GlyphsSpace.Count;
                    continue;
                }

                //Non escaped tag, start the processing
                if (Current == '<' && Next != '<')
                {
                    //Flush the Accumulator and store the text information
                    var TextItem = FlushString(Index, ref XOffset, YOffset, ref AdvanceY, ref Accumulator);

                    if (TextItem != null)
                        LineBuffer.Add(TextItem);

                    //Process the style tag and his contents and continue to the string after the tag end
                    if (ProcessTextBlockTag(String, Index, ref XOffset, ref YOffset, ref AdvanceY, ref i, ref LineBuffer))
                        continue;
                }
                else if (Current == Next && Current == '<') //Escaped '<', just push to accumulator
                    i++;

                Accumulator += Current;
            }

            var NewTextItem = FlushString(Index, ref XOffset, YOffset, ref AdvanceY, ref Accumulator);

            if (NewTextItem != null)
                LineBuffer.Add(NewTextItem);
        }

        //Flush the string accumulator with the current text style
        private Text2D FlushString(int Index, ref int XOffset, int YOffset, ref int AdvanceY, ref string Accumulator)
        {
            //Empty Accumulator, just return
            if (Accumulator == string.Empty)
                return null;

            //Peek the current Font style, the alignment is a post-process
            var CurrentColor = sColors.Peek();
            var CurrentFace = sFont.Peek();
            var CurrentSize = sFontSize.Peek();

            //Create the text fragment in the current style
            var TextFragment = new Text2D(CurrentFace, CurrentSize);
            TextFragment.Color = CurrentColor;
            TextFragment.Position = new Vector2(XOffset, YOffset);
            TextFragment.SetText(Accumulator);

            //Add fragment as object child
            AddChild(TextFragment);

            int TextIndex = Text.Length;

            //Update the plain text string
            Text += Accumulator;
            Accumulator = string.Empty;

            //Update the relative fragment glyph space to a relative one with the RichText2D position
            UpdateGlyphOffset(TextIndex, XOffset, YOffset, TextFragment);

            //Update the Line X/Y Advance
            AdvanceY = Math.Max(AdvanceY, TextFragment.Height);
            XOffset += TextFragment.Width;

            return TextFragment;
        }

        private void UpdateGlyphOffset(int Index, int XOffset, int YOffset, Text2D TextFragment)
        {
            for (int x = 0; x < TextFragment.Text.Length; x++)
            {
                var AbsIndex = x + Index;
                var CurrentGlyph = TextFragment.GlyphsInfo[x];
                var Area = CurrentGlyph.Area;
                
                if (AbsIndex == GlyphsSpace.Count)
                    GlyphsSpace.Add(default);

                GlyphsSpace[AbsIndex] = new GlyphInfo(Area.X + XOffset, Area.Y + YOffset, Area.Width, Area.Height, CurrentGlyph.Char, PlainIndexToRichIndex(AbsIndex));
            }

            var NextIndex = Index + TextFragment.Text.Length;
            if (NextIndex < GlyphsSpace.Count)
            {
                var Glyph = GlyphsSpace[NextIndex];
                if (Glyph.Char == '\n')
                {
                    Glyph.Area.X = XOffset + TextFragment.Width;
                    Glyph.Area.Y = YOffset;

                    Glyph.Index = PlainIndexToRichIndex(NextIndex);

                    GlyphsSpace[Index + TextFragment.Text.Length] = Glyph;
                }
            }
        }

        /// <summary>
        /// Convert an <see cref="RichText"/> index to <see cref="Text"/> index
        /// </summary>
        public int RichIndexToPlainIndex(int RichIndex)
        {
            bool InTag = false;
            for (int i = 0, x = 0; i < RichText.Length; i++)
            {
                char Char = RichText[i];
                char? NChar = i + 1 < RichText.Length ? (char?)RichText[i + 1] : null;

                if (Char == '<' && NChar != '<')
                    InTag = true;

                if (Char == '>')
                {
                    InTag = false;
                    continue;
                }

                if (i >= RichIndex)
                    return x;

                if (!InTag)
                    x++;
            }

            return -1;
        }

        /// <summary>
        /// Convert an <see cref="Text"/> index to <see cref="RichText"/> index
        /// </summary>
        public int PlainIndexToRichIndex(int PlainIndex)
        {
            bool InTag = false;
            for (int i = 0, x = 0; i < RichText.Length; i++)
            {
                char Char = RichText[i];
                char? NChar = i + 1 < RichText.Length ? (char?)RichText[i + 1] : null;

                if (Char == '<' && NChar != '<')
                    InTag = true;

                if (Char == '>')
                {
                    InTag = false;
                    continue;
                }

                if (x >= PlainIndex && !InTag)
                    return i;

                if (!InTag)
                    x++;
            }

            return -1;
        }

        private bool ProcessTextBlockTag(string String, int Index, ref int XOffset, ref int YOffset, ref int AdvanceY, ref int i, ref List<Text2D> LineBuffer)
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
            int InnerIndex = InnerContent.IndexOf('>') + 1;
            InnerContent = InnerContent.Substring(InnerIndex);
            InnerContent = InnerContent.Substring(0, InnerContent.LastIndexOf('<'));

            //Apply Tag Styles
            EnableTag(TagName, AttributeValue);

            //Render the inner tag text content
            ProcessTextBlock(InnerContent, Index + i, ref XOffset, ref YOffset, ref AdvanceY, ref LineBuffer);

            //Undo Style changes
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

                    sFontSize.Push(sFontSize.Peek());
                    sColors.Push(sColors.Peek());
                    sFont.Push(Success ? NewFont : sFont.Peek());                    
                    break;
                case "align":
                    CurrentAlign = LineAlignMode.None;
                    foreach (var Frag in lValue.Split(';', ',', '|'))
                    {
                        switch (Frag.Trim())
                        {
                            case "top":
                                CurrentAlign |= LineAlignMode.Top;
                                break;
                            case "bottom":
                                CurrentAlign |= LineAlignMode.Bottom;
                                break;
                            case "right":
                                CurrentAlign |= LineAlignMode.Right;
                                break;
                            case "left":
                                CurrentAlign |= LineAlignMode.Left;
                                break;
                            case "center":
                                CurrentAlign |= LineAlignMode.Center;
                                break;
                            case "horizontal":
                                CurrentAlign |= LineAlignMode.HorizontalCenter;
                                break;
                            case "vertical":
                                CurrentAlign |= LineAlignMode.VerticalCenter;
                                break;
                        }
                    }

                    sFont.Push(sFont.Peek());
                    sColors.Push(sColors.Peek());
                    sFontSize.Push(sFontSize.Peek());
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

                    //if the tag is the same of the opened one, process the tag level
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
