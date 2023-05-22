using System;

namespace OrbisGL.GL
{
    public interface IRenderable : IDisposable
    {
        void Draw(long Tick);
    }
}