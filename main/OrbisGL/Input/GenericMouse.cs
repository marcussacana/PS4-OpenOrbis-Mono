using System;
using System.Numerics;

namespace OrbisGL.Input
{
    public class GenericMouse : IMouse
    {
        Func<Vector2> GetCoordinates;
        public GenericMouse(Func<Vector2> GetCoordinates)
        {
            this.GetCoordinates = GetCoordinates;
        }
        public bool Available()
        {
            return true;
        }

        public void Dispose() { }

        public Vector2 GetPosition()
        {
            return GetCoordinates();
        }

        public bool Initialize(int UserID = -1)
        {
            if (GetCoordinates == null)
                return false;

            return true;
        }
    }
}
