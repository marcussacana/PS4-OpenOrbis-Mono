using System.Collections.Generic;

namespace OrbisGL.Input.Layouts
{
    internal class ABNT2 : Latin
    {
        public override string Name => "Português (Brasil)";

        public override string EnglishName => "Portuguese (Brazil)";

        public override string LayoutCode => "ABNT2";

        public override int LanguageID => 17;

        Dictionary<IMEKeyModifier, char> Mapper = new Dictionary<IMEKeyModifier, char>() {
            { new IMEKeyModifier(IME_KeyCode.N1, true, false, false), '!' },
            { new IMEKeyModifier(IME_KeyCode.N2, true, false, false), '@' },
            { new IMEKeyModifier(IME_KeyCode.N3, true, false, false), '#' },
            { new IMEKeyModifier(IME_KeyCode.N4, true, false, false), '$' },
            { new IMEKeyModifier(IME_KeyCode.N5, true, false, false), '%' },
            { new IMEKeyModifier(IME_KeyCode.N6, true, false, false), '¨' },
            { new IMEKeyModifier(IME_KeyCode.N7, true, false, false), '&' },
            { new IMEKeyModifier(IME_KeyCode.N8, true, false, false), '*' },
            { new IMEKeyModifier(IME_KeyCode.N9, true, false, false), '(' },
            { new IMEKeyModifier(IME_KeyCode.N0, true, false, false), ')' },
            { new IMEKeyModifier(IME_KeyCode.N1, false, true, false), '¹' },
            { new IMEKeyModifier(IME_KeyCode.N2, false, true, false), '²' },
            { new IMEKeyModifier(IME_KeyCode.N3, false, true, false), '³' },
            { new IMEKeyModifier(IME_KeyCode.N4, false, true, false), '£' },
            { new IMEKeyModifier(IME_KeyCode.N5, false, true, false), '¢' },
            { new IMEKeyModifier(IME_KeyCode.N6, false, true, false), '¬' },

            { new IMEKeyModifier(IME_KeyCode.EQUAL, false, true, false), '§' },
            { new IMEKeyModifier(IME_KeyCode.SINGLEQUOTE, false, false, false), '~' },
            { new IMEKeyModifier(IME_KeyCode.SINGLEQUOTE, true, false, false), '^' },
            { new IMEKeyModifier(IME_KeyCode.BACKQUOTE, false, false, false), '\'' },
            { new IMEKeyModifier(IME_KeyCode.BACKQUOTE, true, false, false), '"' },
            
            { new IMEKeyModifier(IME_KeyCode.SEMICOLON, false, false, false), 'ç' },
            { new IMEKeyModifier(IME_KeyCode.SEMICOLON, true, false, false), 'Ç' },
            { new IMEKeyModifier(IME_KeyCode.INTERNATIONAL1, false, false, false), '/' },
            { new IMEKeyModifier(IME_KeyCode.INTERNATIONAL1, true, false, false), '?' },
            { new IMEKeyModifier(IME_KeyCode.INTERNATIONAL1, false, true, false), '°' },
            
            { new IMEKeyModifier(IME_KeyCode.LEFTBRACKET, false, false, false), '´' },
            { new IMEKeyModifier(IME_KeyCode.LEFTBRACKET, true, false, false), '`' },
            
            
            { new IMEKeyModifier(IME_KeyCode.RIGHTBRACKET, false, false, false), '[' },
            { new IMEKeyModifier(IME_KeyCode.RIGHTBRACKET, true, false, false), '{' },
            { new IMEKeyModifier(IME_KeyCode.RIGHTBRACKET, false, true, false), 'ª' },
            
            { new IMEKeyModifier(IME_KeyCode.BACKSLASH, false, false, false), ']' },
            { new IMEKeyModifier(IME_KeyCode.BACKSLASH, true, false, false), '}' },
            { new IMEKeyModifier(IME_KeyCode.BACKSLASH, false, true, false), 'º' },
            
            { new IMEKeyModifier(IME_KeyCode.SLASH, false, false, false), ';' },
            { new IMEKeyModifier(IME_KeyCode.SLASH, true, false, false), ':' },
            
            { new IMEKeyModifier(IME_KeyCode.COMMA, false, false, false), ',' },
            { new IMEKeyModifier(IME_KeyCode.COMMA, true, false, false), '<' },
            
            { new IMEKeyModifier(IME_KeyCode.KEYPAD_PERIOD, false, false, false), ',' },
            { new IMEKeyModifier(IME_KeyCode.KEYPAD_COMMA, false, false, false), '.' },
        };

        public override char? GetKeyChar(IMEKeyModifier Key)
        {
            if (Mapper.TryGetValue(Key, out var Char))
                return Char;

            if (Key.NumLock)
            {
                Key.NumLock = false;
                if (Mapper.TryGetValue(Key, out Char))
                    return Char;
                Key.NumLock = true;
            }

            return base.GetKeyChar(Key);
        }
    }
}
