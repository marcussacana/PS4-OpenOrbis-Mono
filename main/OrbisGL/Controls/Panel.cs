using OrbisGL.GL;
using OrbisGL.GL2D;
using SharpGLES;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Controls
{
    public class Panel : Control
    {
        Texture2D Rectangle;
        int FrameBuffer;
        public Panel(string Name, Vector2 Position, Vector2 Size)
        {
            this.Name = Name;
            this.Position = Position;
            this.Size = Size;

            int[] buffer = new int[1];
            GLES20.GenFramebuffers(1, buffer);
            FrameBuffer = buffer.Single();

            Rectangle = new Texture2D();
            Rectangle.Texture = Cache = new Texture(true);
        }

        public override Vector2 RenderPosition => Vector2.Zero;

        Texture Cache;
        bool Invalidated = false;

        public override void Invalidate()
        {
            Invalidated = true;
            base.Invalidate();
        }
        public override bool Focusable => false;

        public override string Name { get; set; }

        public override void Draw(long Tick)
        {
            int OriWidth = Coordinates2D.Width;
            int OriHeight = Coordinates2D.Height;
            int OriBuffer = GLES20.CurrentFrameBuffer;

            if (Invalidated)
            {
                try
                {

                    GLES20.Viewport(0, 0, (int)Size.X, (int)Size.Y);
                    Coordinates2D.SetSize((int)Size.X, (int)Size.Y);
                    GLES20.BindFrameBuffer(GLES20.GL_FRAMEBUFFER, FrameBuffer);


                    Rectangle.Width = (int)Size.X;
                    Rectangle.Height = (int)Size.Y;
                    Rectangle.RefreshVertex();

                    Cache.Bind();

                    GLES20.TexImage2D(GLES20.GL_TEXTURE_2D, 0, GLES20.GL_RGBA, (int)Size.X, (int)Size.Y, 0, GLES20.GL_RGBA, GLES20.GL_UNSIGNED_BYTE, IntPtr.Zero);

                    GLES20.TexParameteri(GLES20.GL_TEXTURE_2D, GLES20.GL_TEXTURE_MAG_FILTER, GLES20.GL_NEAREST);
                    GLES20.TexParameteri(GLES20.GL_TEXTURE_2D, GLES20.GL_TEXTURE_MIN_FILTER, GLES20.GL_NEAREST);

                    GLES20.FramebufferTexture2D(GLES20.GL_FRAMEBUFFER, GLES20.GL_COLOR_ATTACHMENT0, GLES20.GL_TEXTURE_2D, Cache.TextureID, 0);

                    if (GLES20.CheckFramebufferStatus(GLES20.GL_FRAMEBUFFER) != GLES20.GL_FRAMEBUFFER_COMPLETE)
                    {
                        throw new Exception("Failed to Render to the texture");
                    }

                    base.Draw(Tick);
                    GLES20.Flush();

                    Invalidated = false;
                }
                finally
                {
                    GLES20.BindFrameBuffer(GLES20.GL_FRAMEBUFFER, OriBuffer);

                    GLES20.Viewport(0, 0, OriWidth, OriHeight);
                    Coordinates2D.SetSize(OriWidth, OriHeight);


                    //Rectangle.RefreshVertex();
                }
            }

            Rectangle.Position = new Vector3(Position, -1);
            Rectangle.Draw(Tick);
        }

        public override void Dispose()
        {
            foreach (var Child in Childs)
                Child.Dispose();

            Rectangle.Dispose();
        }
    }
}
