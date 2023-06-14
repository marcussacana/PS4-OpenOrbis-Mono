﻿using System.Collections.Generic;

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
        };

        public override char? GetKeyChar(IMEKeyModifier Key)
        {
            if (!Key.Shift && !Key.Alt)
                return null;

            if (Mapper.ContainsKey(Key))
                return Mapper[Key];

            return base.GetKeyChar(Key);
        }
    }
}
