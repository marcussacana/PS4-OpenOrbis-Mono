using OrbisGL;
using OrbisGL.GL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OrbisGL.GL2D;
using static OrbisGL.GL2D.Coordinates2D;

namespace Orbis
{
    internal class MainDisplay : Display
    {
        public MainDisplay() : base(1920, 1080, 60)
        {
            InitializeComponents();
        }

        GLProgram Program;

        private void InitializeComponents()
        {
            Rectangle2D BG = new Rectangle2D(1920, 1080, true, ResLoader.GetResource("ThemeFrag"));
            BG.Program.SetUniform("Resolution", new Vector2(1920f, 1080f));
            Objects.Add(BG);
        }


    }
}
