using SharpGLES;

namespace OrbisGL
{
    public static class State
    {
        public static int GetLastGLError()
        {
            int error;
            int lastError = GLES20.GL_NO_ERROR;

            while ((error = GLES20.GetError()) != GLES20.GL_NO_ERROR)
            {
                lastError = error;
            }

            return lastError;
        }
    }
}