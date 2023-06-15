using OrbisGL.Controls.Events;
using OrbisGL.Input.Layouts;

namespace OrbisGL.Input
{
    public interface IKeyboard
    {
        event KeyboardEventDelegate OnKeyDown;
        event KeyboardEventDelegate OnKeyUp;
        bool Initialize(int UserID = -1);
        void RefreshData();
    }
}