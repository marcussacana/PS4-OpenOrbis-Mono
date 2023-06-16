using OrbisGL.GL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.GL2D
{
    internal class RichText2D : GLObject2D
    {
        public RGBColor DefaultTextColor { get; set; } = RGBColor.White;
        public int DefaultFontSize { get; set; }
        public bool DefaultBold { get; set; }
        public bool DefaultItalic { get; set; }

        public string RichText { get; private set; }

        public string Text { get; private set; }

        public Vector4[] GlyphsSpace { get; private set; }

        //<i></i>
        //<color=FFFFFFFF></color>
        //<b></b>
        //<size=28></size>
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
            var CurrentColor = Colors.Peek();
            var CurrentBold = Bold.Peek();
            var CurrentItalic = Bold.Peek();
            var CurrentSize = FontSize.Peek();

            for (int i = 0; i < String.Length; i++)
            {
                char Current = String[i];
                char? Next = i + 1 < String.Length ? (char?)String[i + 1] : null;

                if (Current == '<' && Next != '<')
                {
                    if (ProcessTextBlockTag(String, Index, ref XOffset, ref YOffset, ref AdvanceY, ref GlyphsSpace, ref i))
                        continue;
                }
                else if (Current == Next && Current == '<')
                    i++;


            }
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

            ProcessTextBlock(String, Index + i, ref XOffset, ref YOffset, ref AdvanceY, ref GlyphsSpace);

            i = TagClose;
            return true;
        }

        private void EnableTag(string Name, string Value)
        {
            int.TryParse(Value, out int ValueAsInt);

            switch (Name.ToLowerInvariant())
            {
                case "color":
                    Colors.Push(new RGBColor(Value));
                    break;
                case "size":
                    FontSize.Push(ValueAsInt);
                    break;
                case "bold":
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
