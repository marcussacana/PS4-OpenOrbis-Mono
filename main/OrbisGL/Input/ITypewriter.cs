using OrbisGL.GL;
using System;

namespace OrbisGL.Input
{
    public interface ITypewriter
    {
        event EventHandler OnComplete;
        event EventHandler OnTextChanged;
        event EventHandler OnCaretMove;
        string CurrentText { get; set; }
        string CurrentAccumulator { get; set; }
        int CaretPosition { get; set; }
        int SelectionLength { get; set; }
        void Enter(Rectangle TextArea);
    }
}
