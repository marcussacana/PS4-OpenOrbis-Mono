using System;
using System.Numerics;

namespace OrbisGL.Input
{
    public class GenericMouse : IMouse
    {
        Func<Vector2> GetCoordinates;
        Func<MouseButtons> GetButtonState;
        public GenericMouse(Func<Vector2> GetCoordinates, Func<MouseButtons> GetButtonState)
        {
            this.GetCoordinates = GetCoordinates;
            this.GetButtonState = GetButtonState;
        }

        public void Dispose() { }

        public Vector2 GetPosition()
        {
            return GetCoordinates();
        }

        public MouseButtons GetMouseButtons()
        {
            return GetButtonState();
        }

        public bool Initialize(int UserID = -1)
        {
            if (GetCoordinates == null)
                return false;

            return true;
        }
    }
}
