using System;
using System.Numerics;

namespace OrbisGL.Input
{
    public interface IMouse : IDisposable
    {

        bool Available();

        bool Initialize(int UserID = -1);

        Vector2 GetPosition();

        MouseButtons GetMouseButtons();
    }
}
