using OrbisGL.Controls;
using OrbisGL.GL;
using OrbisGL.GL2D;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Input
{
    internal class Selector : IRenderable
    {
        Control TargetControl = null;
        Control SelectedControl = null;
        Rectangle2D Rectangle = new Rectangle2D(1, 1, false)
        {
            Color = RGBColor.Black,
            ContourWidth = 2f
        };


        public void Select(Control Controller)
        {
            TargetControl = Controller;
        }
        public void Dispose()
        {
            Rectangle?.Dispose();
        }

        const int AnimDuration = Constants.SCE_SECOND;

        public void Draw(long Tick)
        {
            var Percentage = ((float)(Tick % AnimDuration) * 2) / AnimDuration;
            var Opacity = Math.Abs(Percentage - 1) * 255;

            if (Opacity < 10 && TargetControl != SelectedControl)
            {
                Refresh();
            }

            if (Invalidated)
                Refresh();

            Rectangle.Visible = TargetControl?.Visible ?? false;
            Rectangle.Opacity = (byte)Opacity;
            Rectangle.Draw(Tick);
        }

        private void Refresh()
        {
            if (SelectedControl != TargetControl)
            {
                if (SelectedControl != null)
                    SelectedControl.OnControlInvalidated -= TargetInvalidated;

                TargetControl.OnControlInvalidated += TargetInvalidated;
                TargetControl.Focus();
            }

            SelectedControl = TargetControl;

            var Rect = TargetControl.AbsoluteRectangle;

            Rect.Top -= 3;
            Rect.Left -= 3;
            Rect.Right += 8;
            Rect.Bottom += 8;

            if (Rectangle.Rectangle != Rect)
            {
                Rectangle.Rectangle = Rect;
                Rectangle.RefreshVertex();
            }

            Invalidated = false;
        }

        bool Invalidated = false;
        private void TargetInvalidated(object sender, EventArgs e)
        {
            Invalidated = true;
        }
    }
}
