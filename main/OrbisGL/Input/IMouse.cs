using System;
using System.Numerics;

namespace OrbisGL.Input
{
    public interface IMouse : IDisposable
    {
        bool Initialize(int UserID = -1);

        void RefreshData();

        Vector2 GetPosition();

        MouseButtons GetMouseButtons();
    }
}
