using OrbisGL.Controls.Events;
using OrbisGL.GL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Input
{
    public class KeyboardTypewriter : ITypewriter
    {
        //` -> \u300
        #region CombiningMap
        static Dictionary<char, char> CombiningMap = new Dictionary<char, char>()
        {
            {'\u0060', '\u0300'},
            {'\u0027', '\u0301'},
            {'\u005E', '\u0302'},
            {'\u007E', '\u0303'},
            {'\u00AF', '\u0304'},
            {'\u00AF', '\u0305'},
            {'\u02D8', '\u0306'},
            {'\u02D9', '\u0307'},
            {'\u00A8', '\u0308'},
            {'\u02C0', '\u0309'},
            {'\u00B0', '\u030A'},
            {'\u0022', '\u030B'},
            {'\u02C7', '\u030C'},
            {'\u02C8', '\u030D'},
            {'\u0022', '\u030E'},
            {'\u0901', '\u030F'},
            {'\u0484', '\u0311'},
            {'\u02BB', '\u0312'},
            {'\u02BC', '\u0313'},
            {'\u02BD', '\u0314'},
            {'\u02BC', '\u0315'},
            {'\uAB6A', '\u0316'},
            {'\uAB6B', '\u0319'},
            {'\u02D5', '\u031A'},
            {'\u02D4', '\u031D'},
            {'\u02D5', '\u031E'},
            {'\u02D6', '\u031F'},
            {'\u02D7', '\u0320'},
            {'\u02B2', '\u0321'},
            {'\u02D4', '\u0322'},
            {'\u02B1', '\u0324'},
            {'\u02F3', '\u0325'},
            {'\u00B8', '\u0326'},
            {'\u02DB', '\u0328'},
            {'\u02CC', '\u0329'},
            {'\u02B7', '\u032A'},
            {'\u005F', '\u0330'},
            {'\u005F', '\u0332'},
            {'\u0347', '\u0333'},
            {'\u1DF9', '\u0334'},
            {'\u0484', '\u033B'},
            {'\u0303', '\u033F'},
            {'\u037A', '\u0343'},
            {'\u20E9', '\u0346'},
            {'\u1DF8', '\u0347'},
            {'\u204E', '\u0359'},
            {'\u035D', '\u035A'},
            {'\uFE22', '\u035D'},
            {'\uFE20', '\u0361'}
        };
        #endregion

        public string CurrentText { get; set; }

        string InternalAccumulator;

        /// <summary>
        /// The accumulator should be used for languages like Japanese,
        /// that the last input character may change after the next input,
        /// when spacebar is pressed the accumulator is 'flushed' and the next
        /// input will not be able to affect the current in the accumulator anymore.
        /// This may be used too for non english latin languages, the accumulator can
        /// hold an accent to apply in the next character pressed
        /// </summary>
        public string CurrentAccumulator { get; set; }

        public int CaretPosition { get; set; }

        public event EventHandler OnComplete;
        public event EventHandler OnTextChanged;
        public event EventHandler OnCaretMove;

        public bool Multiline;

        public void Enter(Rectangle TextArea)
        {
           
        }

        List<IME_KeyCode> PressedKeys = new List<IME_KeyCode>();

        public void OnKeyDown(KeyboardEventArgs Args)
        {
            if (!PressedKeys.Contains(Args.Keycode))
            {
                PressedKeys.Add(Args.Keycode);
            }
        }
        public void OnKeyUp(KeyboardEventArgs Args)
        {
            if (!PressedKeys.Contains(Args.Keycode))
            {
                return;
            }

            PressedKeys.Remove(Args.Keycode);
            
            switch (Args.Keycode)
            {
                case IME_KeyCode.LEFTARROW:
                    CurrentAccumulator = string.Empty;
                    InternalAccumulator = string.Empty;
                    OnTextChanged?.Invoke(this, new EventArgs());

                    if (CaretPosition > 0)
                    {
                        CaretPosition--;
                        OnCaretMove?.Invoke(this, new EventArgs());
                    }
                    return;
                case IME_KeyCode.RIGHTARROW:
                    CurrentAccumulator = string.Empty;
                    InternalAccumulator = string.Empty;
                    OnTextChanged?.Invoke(this, new EventArgs());

                    if (CaretPosition < CurrentText.Length)
                    {
                        CaretPosition++;
                        OnCaretMove?.Invoke(this, new EventArgs());
                    }
                    return;
                case IME_KeyCode.SPACEBAR:
                    if (CurrentAccumulator != string.Empty)
                    {
                        CurrentText += CurrentAccumulator.Normalize();
                        CurrentAccumulator = string.Empty;
                        InternalAccumulator = string.Empty;
                        
                        CaretPosition++;

                        OnTextChanged?.Invoke(this, new EventArgs());
                        OnCaretMove?.Invoke(this, new EventArgs());
                        return;
                    }
                    break;
            }

            if (Args.KeyChar == null)
                return;

            if (CombiningMap.ContainsKey((char)Args.KeyChar))
            {
                InternalAccumulator += CombiningMap;
                CurrentAccumulator = CombiningMap[(char)Args.KeyChar].ToString();
                OnTextChanged?.Invoke(this, new EventArgs());
                return;
            }

            if (Args.KeyChar == '\n' && !Multiline)
            {
                OnComplete?.Invoke(this, new EventArgs());
                return;
            }

            if (CurrentAccumulator.Length == 1 && CombiningMap.ContainsKey(CurrentAccumulator[0]))
            {
                InternalAccumulator = $"{Args.KeyChar}{InternalAccumulator}";

                var Normalized = InternalAccumulator.Normalize();

                CurrentText = CurrentText.Substring(0, CaretPosition) + Normalized + CurrentText.Substring(CaretPosition);
                CaretPosition++;

                InternalAccumulator = string.Empty;
                CurrentAccumulator = string.Empty;

                OnTextChanged?.Invoke(this, new EventArgs());
                OnCaretMove?.Invoke(this, new EventArgs());
                return;
            }

            if (CurrentAccumulator != string.Empty)
            {
                InternalAccumulator += Args.KeyChar;
                CurrentAccumulator = InternalAccumulator;
                OnTextChanged?.Invoke(this, new EventArgs());
                OnCaretMove?.Invoke(this, new EventArgs());
                return;
            }

            CurrentText = CurrentText.Substring(0, CaretPosition) + Args.KeyChar.ToString() + CurrentText.Substring(CaretPosition);
            CaretPosition++;

            OnTextChanged?.Invoke(this, new EventArgs());
            OnCaretMove?.Invoke(this, new EventArgs());

            //[WIP] ensure normalized string length == 1 otherwise convert combining character for normalized form
        }
    }
}
