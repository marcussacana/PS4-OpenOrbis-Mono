using OrbisGL;
using OrbisGL.Controls;
using OrbisGL.GL;
using OrbisGL.GL2D;
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Permissions;
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

            GLControl.GLApplication.Objects.Add(Obj);
#endif
       }

        private void button2_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var Objs = GLControl.GLApplication.Objects.ToArray();
            GLControl.GLApplication.Objects.Clear();

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

            GLControl.GLApplication.Objects.Add(Obj);
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

            GLControl.GLApplication.Objects.Add(Obj);
#endif
        }

        private void button5_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var Rect = new Rectangle2D(1280, 720, true, ResLoader.GetResource("ThemeFrag"));
            //Rect.Offset = new Vector3(XOffset * Rand.Next(0, GLControl.Width), YOffset * Rand.Next(GLControl.Height), 1);

            Rect.Program.SetUniform("Resolution", new Vector2(1280f, 720f));

            GLControl.GLApplication.Objects.Add(Rect);
#endif
        }

        private void button6_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var Rect = new RoundedRectangle2D(250, 100, Rand.Next(0, 2) == 1);
            Rect.Position = new Vector2(Rand.Next(0, GLControl.Width - 250), Rand.Next(GLControl.Height - 100));
            Rect.Color = new RGBColor((byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255));
            //Rect.Transparecy = (byte)Rand.Next(0, 255);

            Rect.RoundLevel = Rand.Next(0, 100) / 100f;
            GLControl.GLApplication.Objects.Add(Rect);
#endif
        }

        private void button7_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var Rect = new Elipse2D(200, 200, Rand.Next(0, 2) == 1);
            Rect.Position = new Vector2(Rand.Next(0, GLControl.Width - 200), Rand.Next(GLControl.Height - 200));
            Rect.Color = new RGBColor((byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255));
            Rect.Opacity = (byte)Rand.Next(0, 255);

            GLControl.GLApplication.Objects.Add(Rect);
#endif
        }

        private void button8_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var Rect = new PartialElipse2D(200, 200, Rand.Next(0, 2) == 1);
            Rect.Position = new Vector2(Rand.Next(0, GLControl.Width - 200), Rand.Next(GLControl.Height - 200));
            Rect.Color = new RGBColor((byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255));
            Rect.StartAngle = Rand.Next(-314, 314) / 100;
            Rect.EndAngle = Rand.Next(-314, 314) / 100; 
            Rect.Thickness = Rand.Next(0, 100) / 100;


            //Rect.Transparency = (byte)Rand.Next(0, 255);

            GLControl.GLApplication.Objects.Add(Rect);
#endif
        }

        private void button9_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var Font = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "*.ttf").First();
            var Text = new Text2D(Font, 24);
            Text.SetText("Hello World");
            Text.Position = new Vector2(Rand.Next(0, GLControl.Width - 200), Rand.Next(GLControl.Height - 200));
            Text.Color = new RGBColor((byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255));
            GLControl.GLApplication.Objects.Add(Text);
#endif
        }

        private void button10_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var Rect = new Elipse2D(200, 200, true);
            Rect.Position = new Vector2(Rand.Next(0, GLControl.Width - 200), Rand.Next(GLControl.Height - 200));
            Rect.Color = new RGBColor((byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255));


            var Rect2 = new Rectangle2D(20, 300, true);
            Rect2.Position = new Vector2(0, 0);
            Rect2.Color = new RGBColor((byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255));

            Rect.AddChild(Rect2);

            Rect.SetVisibleRectangle(0, 0, 200, 100);

            GLControl.GLApplication.Objects.Add(Rect);
#endif
        }

        private void button11_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var BG = new OrbisGL.Controls.Panel(GLControl.Size.Width, GLControl.Size.Height);


            var Button = new OrbisGL.Controls.Button(50, 25, 18);
            Button.Text = "Hello World";
            Button.Primary = Rand.Next(0, 2) == 1;

            Button.Position = new Vector2(Rand.Next(0, GLControl.Width - 200), Rand.Next(GLControl.Height - 200));

            BG.AddChild(Button);

            GLControl.GLApplication.Objects.Add(BG);
#endif
        }

        private void button12_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var BG = new OrbisGL.Controls.Panel(GLControl.Size.Width, GLControl.Size.Height);

            var RT2D = new RichText2D(28, RGBColor.Black, null);

            RT2D.SetRichText("<align=horizontal>Hello <backcolor=f00><color=0f0>World</color></backcolor></align>\n<align=vertical>Testing<size=48> simple </size><font=Inkfree.ttf>rich</font> <color=F00>text</color></align>");

            foreach (var Glyph in RT2D.GlyphsSpace)
            {
                var Box = new Rectangle2D(Glyph.Area, false);
                Box.Color = RGBColor.Red;
                RT2D.AddChild(Box);
            }

            GLControl.MouseUp += (This, Args) =>
            {
                foreach (var Glyph in RT2D.GlyphsSpace)
                {
                    if (Glyph.Area.IsInBounds(Args.X, Args.Y))
                    {
                        MessageBox.Show($"{Glyph.Char} Clicked, Index: {Glyph.Index}, RichIndex: {RT2D.RichText[Glyph.Index]}");
                    }
                }
            };

            GLControl.GLApplication.Objects.Add(BG);
            GLControl.GLApplication.Objects.Add(RT2D);
#endif
        }

        private void button13_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var Line = new Line2D(new Line[]
            {
                new Line()
                {
                    Begin = new Vector2(Rand.Next(0, GLControl.Width), Rand.Next(0, GLControl.Height)),
                    End = new Vector2(Rand.Next(0, GLControl.Width), Rand.Next(0, GLControl.Height)),
                },
                new Line()
                {
                    Begin = new Vector2(Rand.Next(0, GLControl.Width), Rand.Next(0, GLControl.Height)),
                    End = new Vector2(Rand.Next(0, GLControl.Width), Rand.Next(0, GLControl.Height)),
                },
                new Line()
                {
                    Begin = new Vector2(Rand.Next(0, GLControl.Width), Rand.Next(0, GLControl.Height)),
                    End = new Vector2(Rand.Next(0, GLControl.Width), Rand.Next(0, GLControl.Height)),
                }
            }, false);

            Line.SetVisibleRectangle(300, 100, 800, 500);

            GLControl.GLApplication.Objects.Add(Line);
#endif
        }

        private void button14_Click(object sender, EventArgs e)
        {

#if !ORBIS

            var BG = new OrbisGL.Controls.Panel(GLControl.Size.Width, GLControl.Size.Height);
            BG.BackgroundColor = RGBColor.ReallyLightBlue;

            var TB = new OrbisGL.Controls.TextBox(200, 18);
            TB.Position = new Vector2(10, 10);

            TB.Text = "Debug texbox test";
            TB.SelectionStart = Rand.Next(0, TB.Text.Length);

            BG.AddChild(TB);

            GLControl.GLApplication.Objects.Add(BG);
            GLControl.Focus();
#endif
        }

        private void button15_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var BG = new OrbisGL.Controls.Panel(GLControl.Size.Width, GLControl.Size.Height);
            BG.BackgroundColor = RGBColor.Red;

            var BG2 = new OrbisGL.Controls.Panel(GLControl.Size.Width / 2, GLControl.Size.Height / 2);
            BG2.BackgroundColor = RGBColor.ReallyLightBlue;
            BG2.Position = new Vector2(100, 100);
            BG2.AllowScroll = true;

            var TB = new OrbisGL.Controls.TextBox(200, 18);
            TB.Position = new Vector2(10, 10);

            var TB2 = new OrbisGL.Controls.TextBox(200, 18);
            TB2.Position = new Vector2(10, 400);
            TB2.Text = "Debug texbox test 2";

            TB.Text = "Debug texbox test";

            BG2.AddChild(TB);
            BG2.AddChild(TB2);

            BG.AddChild(BG2);

            GLControl.GLApplication.Objects.Add(BG);
            GLControl.Focus();
#endif
        }

        private void button16_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var Rect = new Triangle2D(200, 200);
            Rect.Position = new Vector2(Rand.Next(0, GLControl.Width - 200), Rand.Next(GLControl.Height - 200));
            Rect.Color = new RGBColor((byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255));
            Rect.RoundLevel = (float)Rand.NextDouble() * 0.3f;

            switch (Rand.Next(0, 8))
            {
                case 0:
                    Rect.Rotation = Triangle2D.Degrees.Degree0;
                    break;
                case 1:
                    Rect.Rotation = Triangle2D.Degrees.Degree45; 
                    break;
                case 2:
                    Rect.Rotation = Triangle2D.Degrees.Degree90;
                    break;
                case 3:
                    Rect.Rotation = Triangle2D.Degrees.Degree135;
                    break;
                case 4:
                    Rect.Rotation = Triangle2D.Degrees.Degree180;
                    break;
                case 5:
                    Rect.Rotation = Triangle2D.Degrees.Degree225;
                    break;
                case 6:
                    Rect.Rotation = Triangle2D.Degrees.Degree270;
                    break;
                case 7:
                    Rect.Rotation = Triangle2D.Degrees.Degree315;
                    break;
            }


            //Rect.Transparency = (byte)Rand.Next(10, 255);

            GLControl.GLApplication.Objects.Add(Rect);
#endif
        }

        private void button17_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var BG = new OrbisGL.Controls.Panel(GLControl.Size.Width, GLControl.Size.Height);
            BG.BackgroundColor = RGBColor.White;


            var CK = new Checkbox(20);
            CK.Text = "Hello World";
            CK.Position = new Vector2(10, 10);

            BG.AddChild(CK);

            GLControl.GLApplication.Objects.Add(BG);
#endif

        }

        private void button18_Click(object sender, EventArgs e)
        {

#if !ORBIS
            var BG = new OrbisGL.Controls.Panel(GLControl.Size.Width, GLControl.Size.Height);
            BG.BackgroundColor = RGBColor.White;


            var RB1 = new Radiobutton(20);
            RB1.Text = "Hello World 1";
            RB1.Position = new Vector2(10, 10);

            var RB2 = new Radiobutton(20);
            RB2.Text = "Hello World 2";
            RB2.Position = new Vector2(10, 50);

            var RB3 = new Radiobutton(20);
            RB3.Text = "Hello World 3";
            RB3.Position = new Vector2(10, 90);

            BG.AddChild(RB1);
            BG.AddChild(RB2);
            BG.AddChild(RB3);

            GLControl.GLApplication.Objects.Add(BG);
#endif
        }
    }
}