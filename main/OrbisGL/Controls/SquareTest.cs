using OrbisGL.GL2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Controls
{
    public class SquareTest : Control
    {
        public Rectangle2D Rect = new Rectangle2D(200, 200, true);
        public override bool Focusable => false;

        public override string Name { get; set; }

        public override void Invalidate()
        {
            Rect.RefreshVertex();
            base.Invalidate();
        }

        public override void Draw(long Tick)
        {
            Rect.Draw(Tick);
            base.Draw(Tick);
        }

        public override void Dispose()
        {
        }
    }
}
