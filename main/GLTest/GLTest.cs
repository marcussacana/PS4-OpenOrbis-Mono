using OrbisGL;
using OrbisGL.GL;
using SharpGLES;
using System;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Windows.Forms;
using static OrbisGL.GL2D.Coordinates2D;

namespace GLTest
{
    public partial class GLTest : Form
    {
        GLControl GLControl;
        public GLTest()
        {
            InitializeComponent();

                GLControl = new GLControl(500, 500);
                this.Controls.Add(GLControl);
        }

const string Vertex =
@"
attribute vec3 Position;

void main(void) {
    gl_Position = vec4(Position, 1.0);
}
";
const string UVVertex =
@"
attribute vec3 Position;
attribute vec2 uv;
 
varying lowp vec2 UV;

void main(void) {
    gl_Position = vec4(Position, 1.0);
    UV = uv;
}
";
const string FragmentColor =
@"
uniform lowp vec4 Color;
 
void main(void) {
    gl_FragColor = Color;
}
";
const string FragmentTexture =
@"
varying lowp vec2 UV;

uniform sampler2D Texture;
 
void main(void) {
    gl_FragColor = texture2D(Texture, UV);
}
";


        Random Rand = new Random();

        private void button1_Click(object sender, EventArgs e)
        {
            var hProg = Shader.GetProgram(Vertex, FragmentColor);
            
            var Prog = new GLProgram(hProg);

            Prog.AddBufferAttribute(new BufferAttribute("Position", AttributeType.Float, AttributeSize.Vector3));

            var Obj = new GLObject(Prog, RenderMode.Triangle);

            var XPos = Rand.Next(0, 450);
            var YPos = Rand.Next(0, 450);

            Obj.AddArray(XToPoint(10 + XPos), YToPoint(10 + YPos), 1);
            Obj.AddArray(XToPoint(60 + XPos), YToPoint(10 + YPos), 1);
            Obj.AddArray(XToPoint(10 + XPos), YToPoint(60 + YPos), 1);
            Obj.AddArray(XToPoint(60 + XPos), YToPoint(60 + YPos), 1);

            Obj.AddIndex(0, 1, 2, 2, 1, 3);


            Prog.SetUniform("Color", new RGBColor((byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255)), 255);

            GLControl.GLDisplay.Objects.Add(Obj);
       }

        private void button2_Click(object sender, EventArgs e)
        {
            var Objs = GLControl.GLDisplay.Objects.ToArray();
            GLControl.GLDisplay.Objects.Clear();

            foreach (var Obj in Objs)
            {
                Obj.Dispose();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var ProgFile = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "Program-*.bin");

            if (!ProgFile.Any())
            {
                var Data = Shader.GetProgramBinary(Vertex, FragmentColor, out int Format);
                File.WriteAllBytes($"Program-{Format}.bin", Data);

                MessageBox.Show("Program Shader Compiled");
            }

            ProgFile = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "Program-*.bin");

            int ProgFormat = int.Parse(ProgFile.Single().Split('-').Last().Split('.').First());

            var Program = File.ReadAllBytes(ProgFile.Single());

            var hProg = Shader.GetProgram(Program, ProgFormat);

            var Prog = new GLProgram(hProg);

            Prog.AddBufferAttribute(new BufferAttribute("Position", AttributeType.Float, AttributeSize.Vector3));

            var Obj = new GLObject(Prog, RenderMode.ClosedLine);

            var XPos = Rand.Next(0, 450);
            var YPos = Rand.Next(0, 450);

            Obj.AddArray(XToPoint(10 + XPos), YToPoint(10 + YPos), 1);
            Obj.AddArray(XToPoint(60 + XPos), YToPoint(10 + YPos), 1);
            Obj.AddArray(XToPoint(10 + XPos), YToPoint(60 + YPos), 1);
            Obj.AddArray(XToPoint(60 + XPos), YToPoint(60 + YPos), 1);

            Obj.AddIndex(0, 1, 2, 2, 1, 3);

            Prog.SetUniform("Color", new RGBColor((byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255)), 255);

            GLControl.GLDisplay.Objects.Add(Obj);
        }
    }
}