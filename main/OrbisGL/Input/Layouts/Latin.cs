using System;
using System.Collections.Generic;

namespace OrbisGL.Input.Layouts
{
    abstract class Latin : ILayout
    {

        Dictionary<IMEKeyModifier, char> Mapper = new Dictionary<IMEKeyModifier, char>() {
            #region A-Z
            { new IMEKeyModifier(IME_KeyCode.A, true, false, false), 'A' },
            { new IMEKeyModifier(IME_KeyCode.B, true, false, false), 'B' },
            { new IMEKeyModifier(IME_KeyCode.C, true, false, false), 'C' },
            { new IMEKeyModifier(IME_KeyCode.D, true, false, false), 'D' },
            { new IMEKeyModifier(IME_KeyCode.E, true, false, false), 'E' },
            { new IMEKeyModifier(IME_KeyCode.F, true, false, false), 'F' },
            { new IMEKeyModifier(IME_KeyCode.G, true, false, false), 'G' },
            { new IMEKeyModifier(IME_KeyCode.H, true, false, false), 'H' },
            { new IMEKeyModifier(IME_KeyCode.I, true, false, false), 'I' },
            { new IMEKeyModifier(IME_KeyCode.J, true, false, false), 'J' },
            { new IMEKeyModifier(IME_KeyCode.K, true, false, false), 'K' },
            { new IMEKeyModifier(IME_KeyCode.L, true, false, false), 'L' },
            { new IMEKeyModifier(IME_KeyCode.M, true, false, false), 'M' },
            { new IMEKeyModifier(IME_KeyCode.N, true, false, false), 'N' },
            { new IMEKeyModifier(IME_KeyCode.O, true, false, false), 'O' },
            { new IMEKeyModifier(IME_KeyCode.P, true, false, false), 'P' },
            { new IMEKeyModifier(IME_KeyCode.Q, true, false, false), 'Q' },
            { new IMEKeyModifier(IME_KeyCode.R, true, false, false), 'R' },
            { new IMEKeyModifier(IME_KeyCode.S, true, false, false), 'S' },
            { new IMEKeyModifier(IME_KeyCode.T, true, false, false), 'T' },
            { new IMEKeyModifier(IME_KeyCode.U, true, false, false), 'U' },
            { new IMEKeyModifier(IME_KeyCode.V, true, false, false), 'V' },
            { new IMEKeyModifier(IME_KeyCode.X, true, false, false), 'X' },
            { new IMEKeyModifier(IME_KeyCode.W, true, false, false), 'W' },
            { new IMEKeyModifier(IME_KeyCode.Y, true, false, false), 'Y' },
            { new IMEKeyModifier(IME_KeyCode.Z, true, false, false), 'Z' },
            #endregion
            #region a-z
            { new IMEKeyModifier(IME_KeyCode.A, false, false, false), 'a' },
            { new IMEKeyModifier(IME_KeyCode.B, false, false, false), 'b' },
            { new IMEKeyModifier(IME_KeyCode.C, false, false, false), 'c' },
            { new IMEKeyModifier(IME_KeyCode.D, false, false, false), 'd' },
            { new IMEKeyModifier(IME_KeyCode.E, false, false, false), 'e' },
            { new IMEKeyModifier(IME_KeyCode.F, false, false, false), 'f' },
            { new IMEKeyModifier(IME_KeyCode.G, false, false, false), 'g' },
            { new IMEKeyModifier(IME_KeyCode.H, false, false, false), 'h' },
            { new IMEKeyModifier(IME_KeyCode.I, false, false, false), 'i' },
            { new IMEKeyModifier(IME_KeyCode.J, false, false, false), 'j' },
            { new IMEKeyModifier(IME_KeyCode.K, false, false, false), 'k' },
            { new IMEKeyModifier(IME_KeyCode.L, false, false, false), 'l' },
            { new IMEKeyModifier(IME_KeyCode.M, false, false, false), 'm' },
            { new IMEKeyModifier(IME_KeyCode.N, false, false, false), 'n' },
            { new IMEKeyModifier(IME_KeyCode.O, false, false, false), 'o' },
            { new IMEKeyModifier(IME_KeyCode.P, false, false, false), 'p' },
            { new IMEKeyModifier(IME_KeyCode.Q, false, false, false), 'q' },
            { new IMEKeyModifier(IME_KeyCode.R, false, false, false), 'r' },
            { new IMEKeyModifier(IME_KeyCode.S, false, false, false), 's' },
            { new IMEKeyModifier(IME_KeyCode.T, false, false, false), 't' },
            { new IMEKeyModifier(IME_KeyCode.U, false, false, false), 'u' },
            { new IMEKeyModifier(IME_KeyCode.V, false, false, false), 'v' },
            { new IMEKeyModifier(IME_KeyCode.X, false, false, false), 'x' },
            { new IMEKeyModifier(IME_KeyCode.W, false, false, false), 'w' },
            { new IMEKeyModifier(IME_KeyCode.Y, false, false, false), 'y' },
            { new IMEKeyModifier(IME_KeyCode.Z, false, false, false), 'z' },
            #endregion
            #region 0-9
            { new IMEKeyModifier(IME_KeyCode.N0, false, false, false), '0' },
            { new IMEKeyModifier(IME_KeyCode.N1, false, false, false), '1' },
            { new IMEKeyModifier(IME_KeyCode.N2, false, false, false), '2' },
            { new IMEKeyModifier(IME_KeyCode.N3, false, false, false), '3' },
            { new IMEKeyModifier(IME_KeyCode.N4, false, false, false), '4' },
            { new IMEKeyModifier(IME_KeyCode.N5, false, false, false), '5' },
            { new IMEKeyModifier(IME_KeyCode.N6, false, false, false), '6' },
            { new IMEKeyModifier(IME_KeyCode.N7, false, false, false), '7' },
            { new IMEKeyModifier(IME_KeyCode.N8, false, false, false), '8' },
            { new IMEKeyModifier(IME_KeyCode.N9, false, false, false), '9' },
            #endregion
            #region NumPad_0-9
            { new IMEKeyModifier(IME_KeyCode.KEYPAD_0, false, false, true), '0' },
            { new IMEKeyModifier(IME_KeyCode.KEYPAD_1, false, false, true), '1' },
            { new IMEKeyModifier(IME_KeyCode.KEYPAD_2, false, false, true), '2' },
            { new IMEKeyModifier(IME_KeyCode.KEYPAD_3, false, false, true), '3' },
            { new IMEKeyModifier(IME_KeyCode.KEYPAD_4, false, false, true), '4' },
            { new IMEKeyModifier(IME_KeyCode.KEYPAD_5, false, false, true), '5' },
            { new IMEKeyModifier(IME_KeyCode.KEYPAD_6, false, false, true), '6' },
            { new IMEKeyModifier(IME_KeyCode.KEYPAD_7, false, false, true), '7' },
            { new IMEKeyModifier(IME_KeyCode.KEYPAD_8, false, false, true), '8' },
            { new IMEKeyModifier(IME_KeyCode.KEYPAD_9, false, false, true), '9' },
            #endregion
            #region Special_Chars
            { new IMEKeyModifier(IME_KeyCode.RETURN,       false, false, false), '\n' },
            { new IMEKeyModifier(IME_KeyCode.SPACEBAR,     false, false, false), ' ' },
            { new IMEKeyModifier(IME_KeyCode.MINUS,        false, false, false), '-' },
            { new IMEKeyModifier(IME_KeyCode.EQUAL,        false, false, false), '=' },
            { new IMEKeyModifier(IME_KeyCode.LEFTBRACKET,  false, false, false), '[' },
            { new IMEKeyModifier(IME_KeyCode.RIGHTBRACKET, false, false, false), ']' },
            { new IMEKeyModifier(IME_KeyCode.BACKSLASH,    false, false, false), '\\' },
            { new IMEKeyModifier(IME_KeyCode.SEMICOLON,    false, false, false), ';' },
            { new IMEKeyModifier(IME_KeyCode.SINGLEQUOTE,  false, false, false), '\'' },
            { new IMEKeyModifier(IME_KeyCode.BACKQUOTE,    false, false, false), '`' },
            { new IMEKeyModifier(IME_KeyCode.COMMA,        false, false, false), ',' },
            { new IMEKeyModifier(IME_KeyCode.PERIOD,       false, false, false), '.' },
            { new IMEKeyModifier(IME_KeyCode.SLASH,        false, false, false), '/' },
            { new IMEKeyModifier(IME_KeyCode.MINUS,        true, false, false), '_' },
            { new IMEKeyModifier(IME_KeyCode.EQUAL,        true, false, false), '+' },
            { new IMEKeyModifier(IME_KeyCode.LEFTBRACKET,  true, false, false), '{' },
            { new IMEKeyModifier(IME_KeyCode.RIGHTBRACKET, true, false, false), '}' },
            { new IMEKeyModifier(IME_KeyCode.BACKSLASH,    true, false, false), '|' },
            { new IMEKeyModifier(IME_KeyCode.SEMICOLON,    true, false, false), ':' },
            { new IMEKeyModifier(IME_KeyCode.SINGLEQUOTE,  true, false, false), '"' },
            { new IMEKeyModifier(IME_KeyCode.BACKQUOTE,    true, false, false), '~' },
            { new IMEKeyModifier(IME_KeyCode.COMMA,        true, false, false), '<' },
            { new IMEKeyModifier(IME_KeyCode.PERIOD,       true, false, false), '>' },
            { new IMEKeyModifier(IME_KeyCode.SLASH,        true, false, false), '?' },
            #endregion
        };

        public abstract string Name { get; }

        public abstract string EnglishName { get; }

        public abstract string LayoutCode { get; }

        public abstract int LanguageID { get; }

        public virtual char? GetKeyChar(IMEKeyModifier Key)
        {
            throw new NotImplementedException();
        }
    }
}
