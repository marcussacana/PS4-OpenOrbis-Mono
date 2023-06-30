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
            {'\u0027', '\u0301'},
            {'\u005E', '\u0302'},
            {'\u005F', '\u0332'},
            {'\u0060', '\u0300'},
            {'\u007E', '\u0303'},
            {'\u00A8', '\u0308'},
            {'\u00AF', '\u0304'},
            {'\u00B0', '\u030A'},
            {'\u00B8', '\u0326'},
            {'\u02B1', '\u0324'},
            {'\u02B2', '\u0321'},
            {'\u02B7', '\u032A'},
            {'\u02BB', '\u0312'},
            {'\u02BC', '\u0313'},
            {'\u02BD', '\u0314'},
            {'\u02C0', '\u0309'},
            {'\u02C7', '\u030C'},
            {'\u02C8', '\u030D'},
            {'\u02CC', '\u0329'},
            {'\u02D4', '\u031D'},
            {'\u02D5', '\u031E'},
            {'\u02D6', '\u031F'},
            {'\u02D7', '\u0320'},
            {'\u02D8', '\u0306'},
            {'\u02D9', '\u0307'},
            {'\u02DB', '\u0328'},
            {'\u02F3', '\u0325'},
            {'\u0303', '\u033F'},
            {'\u037A', '\u0343'},
            {'\u0484', '\u0311'},
            {'\u0901', '\u030F'},
            {'\u1DF8', '\u0347'},
            {'\u1DF9', '\u0334'},
            {'\u204E', '\u0359'},
            {'\u20E9', '\u0346'},
            {'\uAB6A', '\u0316'},
            {'\uAB6B', '\u0319'},
            {'\uFE20', '\u0361'},
            {'\uFE22', '\u035D'}
        };
        #endregion

        public string CurrentText { get; set; } = string.Empty;

        string InternalAccumulator = string.Empty;

        /// <summary>
        /// The accumulator should be used for languages like Japanese,
        /// that the last input character may change after the next input,
        /// when spacebar is pressed the accumulator is 'flushed' and the next
        /// input will not be able to affect the current in the accumulator anymore.
        /// This may be used too for non english latin languages, the accumulator can
        /// hold an accent to apply in the next character pressed
        /// </summary>
        public string CurrentAccumulator { get; set; } = string.Empty;

        int AccumulatorCaret;

        int CurrentCaret;
        public int CaretPosition { get => CurrentCaret + AccumulatorCaret; 
            set 
            {
                InternalAccumulator = string.Empty;
                CurrentAccumulator = string.Empty;
                AccumulatorCaret = 0;

                CurrentCaret = value;

                OnTextChanged?.Invoke(this, new EventArgs());
                OnCaretMove?.Invoke(this, new EventArgs());
            }
        }

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
                    InternalAccumulator = string.Empty;
                    CurrentAccumulator = string.Empty;
                    AccumulatorCaret = 0;

                    OnTextChanged?.Invoke(this, new EventArgs());

                    if (CurrentCaret > 0)
                    {
                        CurrentCaret--;
                        OnCaretMove?.Invoke(this, new EventArgs());
                    }
                    return;
                case IME_KeyCode.RIGHTARROW:
                    InternalAccumulator = string.Empty;
                    CurrentAccumulator = string.Empty;
                    AccumulatorCaret = 0;

                    OnTextChanged?.Invoke(this, new EventArgs());

                    if (CurrentCaret < CurrentText.Length)
                    {
                        CurrentCaret++;
                        OnCaretMove?.Invoke(this, new EventArgs());
                    }
                    return;
                case IME_KeyCode.SPACEBAR:
                    if (InternalAccumulator != string.Empty)
                    {
                        var NormalizedAccumulator = InternalAccumulator.Normalize();
                        CurrentText = CurrentText.Insert(CurrentCaret, NormalizedAccumulator);

                        InternalAccumulator = string.Empty;
                        CurrentAccumulator = string.Empty;
                        AccumulatorCaret = 0;

                        CurrentCaret += NormalizedAccumulator.Length;

                        OnTextChanged?.Invoke(this, new EventArgs());
                        OnCaretMove?.Invoke(this, new EventArgs());
                        return;
                    }
                    break;
                case IME_KeyCode.BACKSPACE:
                    if (InternalAccumulator != string.Empty)
                    {
                        InternalAccumulator = RemoveAt(InternalAccumulator, AccumulatorCaret);
                        CurrentAccumulator = RemoveAt(CurrentAccumulator, AccumulatorCaret);

                        AccumulatorCaret--;

                        OnTextChanged?.Invoke(this, new EventArgs());
                        OnCaretMove?.Invoke(this, new EventArgs());
                        return;
                    }
                    if (CurrentText != string.Empty)
                    {
                        CurrentText = RemoveAt(CurrentText, CurrentCaret);
                        CurrentCaret--;

                        OnTextChanged?.Invoke(this, new EventArgs());
                        OnCaretMove?.Invoke(this, new EventArgs());
                        return;
                    }
                    break;
            }

            if (Args.KeyChar == null)
                return;

            if (Args.KeyChar == '\n' && !Multiline)
            {
                OnComplete?.Invoke(this, new EventArgs());
                return;
            }

            if (CurrentAccumulator.Length == 1 && CombiningMap.ContainsKey(CurrentAccumulator[0]))
            {
                CurrentAccumulator = $"{CurrentAccumulator}{Args.KeyChar}";
                InternalAccumulator = $"{Args.KeyChar}{InternalAccumulator}";

                var Normalized = InternalAccumulator.Normalize();

                if (Normalized.Length == 1)
                {
                    CurrentText = CurrentText.Insert(CurrentCaret, Normalized);
                    CurrentCaret++;
                }
                else
                {
                    CurrentText = CurrentText.Insert(CurrentCaret, CurrentAccumulator);
                    CurrentCaret += 2;
                }

                CurrentAccumulator = string.Empty;
                InternalAccumulator = string.Empty; 
                AccumulatorCaret = 0;

                OnTextChanged?.Invoke(this, new EventArgs());
                OnCaretMove?.Invoke(this, new EventArgs());
                return;
            }

            if (CombiningMap.ContainsKey((char)Args.KeyChar))
            {
                CurrentAccumulator += (char)Args.KeyChar;
                InternalAccumulator += CombiningMap[(char)Args.KeyChar].ToString();
                AccumulatorCaret++;

                OnTextChanged?.Invoke(this, new EventArgs());
                OnCaretMove?.Invoke(this, new EventArgs());
                return;
            }

            if (InternalAccumulator != string.Empty)
            {
                CurrentAccumulator += Args.KeyChar;
                InternalAccumulator += Args.KeyChar;
                AccumulatorCaret++;

                OnTextChanged?.Invoke(this, new EventArgs());
                OnCaretMove?.Invoke(this, new EventArgs());
                return;
            }

            CurrentText = CurrentText.Insert(CurrentCaret, Args.KeyChar.ToString());
            CurrentCaret++;

            OnTextChanged?.Invoke(this, new EventArgs());
            OnCaretMove?.Invoke(this, new EventArgs());

            //[WIP] ensure normalized string length == 1 otherwise convert combining character for normalized form
        }

        private string RemoveAt(string String, int Index)
        {
            if (Index <= 0 || Index > String.Length)
                return String;

            return String.Substring(0, Index - 1) + String.Substring(Index);
        }
    }
}
