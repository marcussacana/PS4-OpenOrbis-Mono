using OrbisGL;
using OrbisGL.GL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
            var hPorg =  Shader.GetProgram(ShaderSource.Vertex, ShaderSource.FragmentColor);
            Program = new GLProgram(hPorg);
            Program.AddBufferAttribute("Position", AttributeType.Float, AttributeSize.Vector3);

            GLObject Test = new GLObject(Program, RenderMode.Triangle);


            Test.AddArray(XToPoint(400), YToPoint(100), 0);
            Test.AddArray(XToPoint(600), YToPoint(100), 0);
            Test.AddArray(XToPoint(600), YToPoint(300), 0);
            Test.AddArray(XToPoint(400), YToPoint(300), 0);

            Test.AddIndex(0, 1, 2, 0, 2, 3);

            Program.SetUniform("Color", RGBColor.Red, 255);

            Objects.Add(Test);
        }


    }
}
