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
                CurrentCaret = value;

                ClearAccumulator();

                OnCaretMove?.Invoke(this, new EventArgs());
            }
        }

        int SelectionDirection;
        public int SelectionLength { get; set; }


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

            bool CtrlPressed = PressedKeys.Contains(IME_KeyCode.LEFTCONTROL) || PressedKeys.Contains(IME_KeyCode.RIGHTCONTROL);
            bool ShiftPressed = PressedKeys.Contains(IME_KeyCode.LEFTSHIFT) || PressedKeys.Contains(IME_KeyCode.RIGHTSHIFT);

            switch (Args.Keycode)
            {
                case IME_KeyCode.LEFTARROW:
                    ClearAccumulator();

                    if (ShiftPressed)
                    {
                        LeftMoveSelection(CtrlPressed);
                        OnCaretMove?.Invoke(this, new EventArgs());
                        return;
                    }

                    SelectionLength = 0;

                    if (CtrlPressed)
                    {
                        SkipBehind();
                        return;
                    }

                    if (CurrentCaret > 0)
                    {
                        CurrentCaret--;
                        OnCaretMove?.Invoke(this, new EventArgs());
                    }
                    return;
                case IME_KeyCode.RIGHTARROW:
                    ClearAccumulator();

                    if (ShiftPressed)
                    {
                        RightMoveSelection(CtrlPressed);
                        OnCaretMove?.Invoke(this, new EventArgs());
                        return;
                    }

                    SelectionLength = 0;

                    if (CtrlPressed)
                    {
                        SkipAhead();
                        return;
                    }

                    if (CurrentCaret < CurrentText.Length)
                    {
                        CurrentCaret++;
                        OnCaretMove?.Invoke(this, new EventArgs());
                    }
                    return;
                case IME_KeyCode.UPARROW:
                    ClearAccumulator();

                    SelectionLength = 0;

                    if (CurrentCaret != 0)
                    {
                        CurrentCaret = 0;
                        OnCaretMove?.Invoke(this, new EventArgs());
                    }
                    return;
                case IME_KeyCode.DOWNARROW:
                    ClearAccumulator();

                    SelectionLength = 0;

                    if (CurrentCaret != CurrentText.Length)
                    {
                        CurrentCaret = CurrentText.Length;
                        OnCaretMove?.Invoke(this, new EventArgs());
                    }
                    return;
                case IME_KeyCode.SPACEBAR:
                    if (InternalAccumulator != string.Empty)
                    {
                        var NormalizedAccumulator = InternalAccumulator.Normalize();
                        CurrentText = CurrentText.Insert(CurrentCaret, NormalizedAccumulator);

                        ClearAccumulator();

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

                ReplaceSelection();
                NormalizeAccumulatorAndFlush();
                ClearAccumulator();

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

            ReplaceSelection();

            CurrentText = CurrentText.Insert(CurrentCaret, Args.KeyChar.ToString());
            CurrentCaret++;

            OnTextChanged?.Invoke(this, new EventArgs());
            OnCaretMove?.Invoke(this, new EventArgs());
        }

        private void RightMoveSelection(bool CtrlPressed)
        {
            if (CurrentCaret + SelectionLength + 1 <= CurrentText.Length && SelectionDirection >= 0)
            {
                SelectionDirection = 1;
                int NewSelectionEnd = CtrlPressed ? FindAhead(SelectionLength) : CurrentCaret + SelectionLength + 1;
                int DeltaSelection = NewSelectionEnd - (CurrentCaret + SelectionLength);
                SelectionLength += DeltaSelection;
                OnCaretMove?.Invoke(this, new EventArgs());
            }
            if (SelectionLength > 0 && SelectionDirection < 0)
            {
                int NewCaretOffset = CtrlPressed ? FindAhead() : CurrentCaret + 1;
                int DeltaCaret = NewCaretOffset - CurrentCaret;
                CurrentCaret += DeltaCaret;
                SelectionLength -= DeltaCaret;
                OnCaretMove?.Invoke(this, new EventArgs());
            }
            if (SelectionLength <= 0)
            {
                SelectionLength = 0;
                SelectionDirection = 0;
            }

            if (CurrentCaret < 0)
                CurrentCaret = 0;

            if (CurrentCaret > CurrentText.Length)
                CurrentCaret = CurrentText.Length;

            if (CurrentCaret + SelectionLength > CurrentText.Length)
            {
                SelectionLength = CurrentText.Length - CurrentCaret;
            }
        }

        private void LeftMoveSelection(bool CtrlPressed)
        {
            if (CurrentCaret - 1 >= 0 && SelectionDirection <= 0)
            {
                SelectionDirection = -1;
                int NewCaret = CtrlPressed ? FindBehind() : CurrentCaret - 1;
                int DeltaCaret = CurrentCaret - NewCaret;
                CurrentCaret = NewCaret;
                SelectionLength += DeltaCaret;
            }

            if (SelectionLength >= 0 && SelectionDirection > 0)
            {
                int NewSelectionEnd = CtrlPressed ? FindBehind(SelectionLength) : CurrentCaret + SelectionLength - 1;
                int DeltaSelectionEnd = (CurrentCaret + SelectionLength) - NewSelectionEnd;

                SelectionLength -= DeltaSelectionEnd;
            }

            if (SelectionLength <= 0)
            {
                SelectionLength = 0;
                SelectionDirection = 0;
            }

            if (CurrentCaret < 0)
                CurrentCaret = 0;

            if (CurrentCaret > CurrentText.Length)
                CurrentCaret = CurrentText.Length;

            if (CurrentCaret + SelectionLength > CurrentText.Length)
            {
                SelectionLength = CurrentText.Length - CurrentCaret;
            }
        }

        private void ReplaceSelection()
        {
            if (SelectionLength > 0)
            {
                CurrentText = CurrentText.Remove(CurrentCaret, SelectionLength);
                SelectionLength = 0;
            }
        }

        private void SkipBehind()
        {
            int i = FindBehind();

            if (i >= 0)
            {
                CurrentCaret = i;
                OnCaretMove?.Invoke(this, new EventArgs());
            }
        }

        private int FindBehind(int Offset = 0)
        {
            int i = CurrentCaret - 1 + Offset;
            for (bool Found = false; i > 0; i--)
            {
                char CurrentChar = CurrentText[i];
                if (char.IsSeparator(CurrentChar))
                {
                    if (Found)
                    {
                        i++;
                        break;
                    }
                }
                else
                {
                    Found = true;
                }
            }

            return i;
        }

        private void SkipAhead()
        {
            int i = FindAhead();

            if (i <= CurrentText.Length)
            {
                CurrentCaret = i;
                OnCaretMove?.Invoke(this, new EventArgs());
            }
        }

        private int FindAhead(int Offset = 0)
        {
            int i = CurrentCaret + 1 + Offset;
            for (bool Found = false; i < CurrentText.Length; i++)
            {
                char CurrentChar = CurrentText[i];
                if (char.IsSeparator(CurrentChar))
                {
                    if (Found)
                    {
                        break;
                    }
                }
                else
                {
                    Found = true;
                }
            }

            return i;
        }

        private void NormalizeAccumulatorAndFlush()
        {
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
        }

        private void ClearAccumulator()
        {
            bool Changed = InternalAccumulator != string.Empty;
            InternalAccumulator = string.Empty;
            CurrentAccumulator = string.Empty;
            AccumulatorCaret = 0;

            if (Changed)
                OnTextChanged?.Invoke(this, new EventArgs());
        }

        private string RemoveAt(string String, int Index)
        {
            if (Index <= 0 || Index > String.Length)
                return String;

            return String.Substring(0, Index - 1) + String.Substring(Index);
        }
    }
}
