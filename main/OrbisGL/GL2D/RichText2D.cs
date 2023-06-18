using OrbisGL.GL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace OrbisGL.GL2D
{
    internal class RichText2D : GLObject2D
    {
        public RGBColor DefaultTextColor { get; set; } = RGBColor.White;
        public int DefaultFontSize { get; set; }
        public bool DefaultBold { get; set; }
        public bool DefaultItalic { get; set; }

        public string DefaultFontPath { get; set; }

        public string RichText { get; private set; }

        public string Text { get; private set; }

        public Vector4[] GlyphsSpace { get; private set; }

        //<i></i>                   - Swap the italic effect (if enabled, disable it, otherwise enable it)
        //<i=false></i>             - Disable the italic effect
        //<i=true></i>              - Enable the italic effect
        //<color=FFFFFF></color>    - Set the font color as RGB
        //<b></b>                   - Swap the bold effect (if enabled, disable it, otherwise enable it)
        //<b=false></b>             - Disable the italic effect
        //<b=true></b>              - Enable the italic effect
        //<size=28></size>          - Set the font size

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

            Colors.Clear();
            FontSize.Clear();
            Bold.Clear();
            Italic.Clear();

            int XOffset = 0;
            int YOffset = 0;
            int AdvanceY = 0;
            Dictionary<int, Vector4> GlyphsSpace = new Dictionary<int, Vector4>();
            ProcessTextBlock(String, 0, ref XOffset, ref YOffset, ref AdvanceY, ref GlyphsSpace);

            return true;
        }

        Stack<RGBColor> Colors = new Stack<RGBColor>();
        Stack<int> FontSize = new Stack<int>();
        Stack<bool> Bold = new Stack<bool>();
        Stack<bool> Italic = new Stack<bool>();

        private void ProcessTextBlock(string String, int Index, ref int XOffset, ref int YOffset, ref int AdvanceY, ref Dictionary<int, Vector4> GlyphsSpace)
        {
            string Accumulator = string.Empty;

            for (int i = 0; i < String.Length; i++)
            {
                char Current = String[i];
                char? Next = i + 1 < String.Length ? (char?)String[i + 1] : null;

                //handle line breaks

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
        }

        private void FlushString(int Index, ref int XOffset, int YOffset, ref int AdvanceY, Dictionary<int, Vector4> GlyphsSpace, ref string Accumulator)
        {
            var CurrentColor = Colors.Peek();
            var CurrentBold = Bold.Peek();
            var CurrentItalic = Bold.Peek();
            var CurrentSize = FontSize.Peek();

            var TextFragment = new Text2D(DefaultFontPath, CurrentSize);
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
            InnerContent.Substring(0, InnerContent.LastIndexOf('<'));

            EnableTag(TagName, AttributeValue);

            ProcessTextBlock(InnerContent, Index + i, ref XOffset, ref YOffset, ref AdvanceY, ref GlyphsSpace);

            Colors.Pop();
            Bold.Pop();
            Italic.Pop();
            FontSize.Pop();

            i = TagClose;
            return true;
        }

        private void EnableTag(string Name, string Value)
        {
            int.TryParse(Value, out int ValueAsInt);

            var lValue = Value.ToLowerInvariant();
            var ValueAsBoolean = lValue == "true" || lValue == "1" || lValue == "enable" || lValue == "enabled" || lValue == "yes";

            switch (Name.ToLowerInvariant())
            {
                case "color":
                    Bold.Push(Bold.Peek());
                    Italic.Push(Italic.Peek());
                    FontSize.Push(FontSize.Peek());
                    Colors.Push(new RGBColor(Value));
                    break;
                case "size":
                    Bold.Push(Bold.Peek());
                    Italic.Push(Italic.Peek());
                    Colors.Push(Colors.Peek());
                    FontSize.Push(ValueAsInt);
                    break;
                case "b":
                    Italic.Push(Italic.Peek());
                    FontSize.Push(FontSize.Peek());
                    Colors.Push(Colors.Peek());

                    if (Value == null)
                        Bold.Push(!Bold.Peek());
                    else
                        Bold.Push(ValueAsBoolean);

                    break;
                case "i":
                    FontSize.Push(FontSize.Peek());
                    Colors.Push(Colors.Peek());
                    Bold.Push(Bold.Peek());

                    if (Value == null)
                        Italic.Push(!Italic.Peek());
                    else
                        Italic.Push(ValueAsBoolean);
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
