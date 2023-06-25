﻿using OrbisGL.GL;
using OrbisGL.GL2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Controls
{
    public class Panel : Control
    {
        public Panel(bool Transparent = false)
        {
            if (Transparent)
                return;

            Background = new Rectangle2D((int)Size.X, (int)Size.Y, true);
            Background.Color = BackgroundColor;
        }

        public override bool Focusable => false;

        public override string Name { get; }

        public override string Text { get; set; }

        Rectangle2D Background;

        public override void Draw(long Tick)
        {
            if (Invalidated)
            {
                if (Background != null && Size.X != Background.Width || Size.Y != Background.Height)
                {
                    Background = new Rectangle2D((int)Size.X, (int)Size.Y, true);
                }

                Background.Color = BackgroundColor;
                Invalidated = true;
            }

            Background.Draw(Tick);
            base.Draw(Tick);
        }
    }
}
