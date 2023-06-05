using OrbisGL;
using OrbisGL.GL;
using OrbisGL.GL2D;
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using static OrbisGL.GL2D.Coordinates2D;

namespace GLTest
{
    public partial class GLTest : Form
    {
#if !ORBIS
        GLControl GLControl;
        public GLTest()
        {
            InitializeComponent();

                GLControl = new GLControl(1280, 720);
                this.Controls.Add(GLControl);
        }
#endif
const string Vertex =
@"
attribute vec3 Position;

void main(void) {
    gl_Position = vec4(Position, 1.0);
}
";
const string VertexMat4 =
@"
attribute vec3 Position;
uniform mat4 Transformation;

void main(void) {
    gl_Position = Transformation * vec4(Position, 1.0);
}
";
const string VertexOffset =
@"
attribute vec3 Position;
uniform vec2 Offset;

void main(void) {
    gl_Position = vec4(Position + Offset, 1.0);
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
#if !ORBIS
            var hProg = Shader.GetProgram(Vertex, FragmentColor);
            
            var Prog = new GLProgram(hProg);

            Prog.AddBufferAttribute("Position", AttributeType.Float, AttributeSize.Vector3);

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
#endif
       }

        private void button2_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var Objs = GLControl.GLDisplay.Objects.ToArray();
            GLControl.GLDisplay.Objects.Clear();

            foreach (var Obj in Objs)
            {
                Obj.Dispose();
            }
#endif
        }

        private void button3_Click(object sender, EventArgs e)
        {
#if !ORBIS
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

            Prog.AddBufferAttribute("Position", AttributeType.Float, AttributeSize.Vector3);

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
#endif
        }

        private void button4_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var hProg = Shader.GetProgram(VertexMat4, FragmentColor);

            var GLProg = new GLProgram(hProg);
            GLProg.AddBufferAttribute("Position", AttributeType.Float, AttributeSize.Vector3);

            var Translation = new Matrix4x4(0.755f, 0, 0, 0,
                                            0, 1, 0, 0,
                                            0, 0, 1, 0,
                                            0, 0, 0, 1);

            GLProg.SetUniform("Color", new RGBColor((byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255)), 255);


            var Obj = new GLObject(GLProg, RenderMode.Triangle);

            Obj.AddArray(-0.3f, -0.6f, 1);
            Obj.AddArray(0.6f, -0.6f, 1);
            Obj.AddArray(-0.3f, 0.6f, 1);
            Obj.AddArray(0.6f, 0.6f, 1);

            Obj.AddIndex(0, 1, 2, 1, 2, 3);

           // var Scale = Matrix4x4.CreateScale(1, 1, 1, Vector3.One);


            GLProg.SetUniform("Transformation", Translation);

            GLControl.GLDisplay.Objects.Add(Obj);
#endif
        }

        private void button5_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var Rect = new Rectangle2D(1280, 720, true, ResLoader.GetResource("ThemeFrag"));
            //Rect.Offset = new Vector3(XOffset * Rand.Next(0, GLControl.Width), YOffset * Rand.Next(GLControl.Height), 1);

            Rect.Program.SetUniform("Resolution", new Vector2(1280f, 720f));

            GLControl.GLDisplay.Objects.Add(Rect);
#endif
        }

        private void button6_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var Rect = new RoundedRectangle2D(250, 100, Rand.Next(0, 2) == 1);
            Rect.Position = new Vector3(Rand.Next(0, GLControl.Width - 250), Rand.Next(GLControl.Height - 100), 1);
            Rect.Color = new RGBColor((byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255));
            //Rect.Transparecy = (byte)Rand.Next(0, 255);

            Rect.RoundLevel = Rand.Next(0, 100) / 100f;
            GLControl.GLDisplay.Objects.Add(Rect);
#endif
        }

        private void button7_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var Rect = new Elipse2D(200, 200, Rand.Next(0, 2) == 1);
            Rect.Position = new Vector3(Rand.Next(0, GLControl.Width - 200), Rand.Next(GLControl.Height - 200), 1);
            Rect.Color = new RGBColor((byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255));
            Rect.Transparency = (byte)Rand.Next(0, 255);

            GLControl.GLDisplay.Objects.Add(Rect);
#endif
        }

        private void button8_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var Rect = new PartialElipse2D(200, 200, Rand.Next(0, 2) == 1);
            Rect.Position = new Vector3(Rand.Next(0, GLControl.Width - 200), Rand.Next(GLControl.Height - 200), 1);
            Rect.Color = new RGBColor((byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255));
            Rect.StartAngle = Rand.Next(-314, 314) / 100;
            Rect.EndAngle = Rand.Next(-314, 314) / 100; 
            Rect.Thickness = Rand.Next(0, 100) / 100;


            //Rect.Transparency = (byte)Rand.Next(0, 255);

            GLControl.GLDisplay.Objects.Add(Rect);
#endif
        }

        private void button9_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var Font = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "*.ttf").First();
            var Text = new Text2D(Font, 24);
            Text.SetText("Hello World");
            Text.Position = new Vector3(Rand.Next(0, GLControl.Width - 200), Rand.Next(GLControl.Height - 200), 1);
            Text.Color = new RGBColor((byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255));
            GLControl.GLDisplay.Objects.Add(Text);
#endif
        }

        private void button10_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var Rect = new Elipse2D(200, 200, true);
            Rect.Position = new Vector3(Rand.Next(0, GLControl.Width - 200), Rand.Next(GLControl.Height - 200), 1);
            Rect.Color = new RGBColor((byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255));


            var Rect2 = new Rectangle2D(20, 300, true);
            Rect2.Position = new Vector3(0, 0, 1);
            Rect2.Color = new RGBColor((byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255));

            Rect.AddChild(Rect2);

            Rect.SetVisibleRectangle(0, 0, 200, 100);

            GLControl.GLDisplay.Objects.Add(Rect);
#endif
        }
    }
}