using System.Collections.Generic;

namespace OrbisGL.Input.Layouts
{
    internal class ASCII : Latin
    {
        public override string Name => "English (United States)";

        public override string EnglishName => "English (United States)";

        public override string LayoutCode => "ASCII";
        public override int LanguageID => 1;

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
            
            { new IMEKeyModifier(IME_KeyCode.KEYPAD_PERIOD, false, false, false), '.' },
            { new IMEKeyModifier(IME_KeyCode.KEYPAD_COMMA, false, false, false), ',' },
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
