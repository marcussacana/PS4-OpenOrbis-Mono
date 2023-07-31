using OrbisGL;
using OrbisGL.Controls;
using OrbisGL.Debug;
using OrbisGL.GL;
using OrbisGL.GL2D;
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Permissions;
using System.Windows.Forms;
using static OrbisGL.GL2D.Coordinates2D;
using Button = OrbisGL.Controls.Button;
using Panel = OrbisGL.Controls.Panel;
using TextBox = OrbisGL.Controls.TextBox;

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
            throw new NotImplementedException();
       }

        private void button2_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var Objs = GLControl.GLApplication.Objects.ToArray();
            GLControl.GLApplication.RemoveObjects();

            foreach (var Obj in Objs)
            {
                Obj.Dispose();
            }
#endif
        }

        private void button3_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void button5_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var Rect = new Rectangle2D(1280, 720, true, ResLoader.GetResource("ThemeFrag"));
            //Rect.Offset = new Vector3(XOffset * Rand.Next(0, GLControl.Width), YOffset * Rand.Next(GLControl.Height), 1);

            Rect.Program.SetUniform("Resolution", new Vector2(1280f, 720f));

            GLControl.GLApplication.AddObject(Rect);
#endif
        }

        private void button6_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var Rect = new RoundedRectangle2D(250, 100, Rand.Next(0, 2) == 1);
            Rect.Position = new Vector2(Rand.Next(0, GLControl.Width - 250), Rand.Next(GLControl.Height - 100));
            Rect.Color = new RGBColor((byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255));
            //Rect.Transparecy = (byte)Rand.Next(0, 255);

            Rect.Rotate = Rand.Next(0, 3600) / 10f;

            Rect.RoundLevel = Rand.Next(0, 100) / 100f;
            GLControl.GLApplication.AddObject(Rect);
#endif
        }

        private void button7_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var Rect = new Elipse2D(200, 200, Rand.Next(0, 2) == 1);
            Rect.Position = new Vector2(Rand.Next(0, GLControl.Width - 200), Rand.Next(GLControl.Height - 200));
            Rect.Color = new RGBColor((byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255));
            Rect.Opacity = (byte)Rand.Next(0, 255);

            GLControl.GLApplication.AddObject(Rect);
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

            GLControl.GLApplication.AddObject(Rect);
#endif
        }

        private void button9_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var Font = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "*.ttf").First();
            var Text = new Text2D(24, Font);
            Text.SetText("Hello World");
            Text.Position = new Vector2(Rand.Next(0, GLControl.Width - 200), Rand.Next(GLControl.Height - 200));
            Text.Color = new RGBColor((byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255));
            GLControl.GLApplication.AddObject(Text);
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

            GLControl.GLApplication.AddObject(Rect);
#endif
        }

        private void button11_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var BG = new OrbisGL.Controls.RowView(GLControl.Size.Width, GLControl.Size.Height);


            var Button = new OrbisGL.Controls.Button(50, 25, 18);
            Button.Text = "Hello World";
            Button.Primary = Rand.Next(0, 2) == 1;

            Button.Position = new Vector2(Rand.Next(0, GLControl.Width - 200), Rand.Next(GLControl.Height - 200));

            BG.AddChild(Button);

            GLControl.GLApplication.AddObject(BG);
#endif
        }

        private void button12_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var BG = new OrbisGL.Controls.RowView(GLControl.Size.Width, GLControl.Size.Height);

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

            GLControl.GLApplication.AddObject(BG);
            GLControl.GLApplication.AddObject(RT2D);
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

            GLControl.GLApplication.AddObject(Line);
#endif
        }

        private void button14_Click(object sender, EventArgs e)
        {

#if !ORBIS

            var BG = new OrbisGL.Controls.RowView(GLControl.Size.Width, GLControl.Size.Height);
            BG.BackgroundColor = RGBColor.ReallyLightBlue;

            var TB = new OrbisGL.Controls.TextBox(200, 18);
            TB.Position = new Vector2(10, 10);

            TB.Text = "Debug texbox test";
            TB.SelectionStart = Rand.Next(0, TB.Text.Length);

            BG.AddChild(TB);

            GLControl.GLApplication.AddObject(BG);
            GLControl.Focus();
#endif
        }

        private void button15_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var BG = new OrbisGL.Controls.RowView(GLControl.Size.Width, GLControl.Size.Height);
            BG.BackgroundColor = RGBColor.Red;

            var BG2 = new OrbisGL.Controls.RowView(GLControl.Size.Width / 2, GLControl.Size.Height / 2);
            BG2.BackgroundColor = RGBColor.ReallyLightBlue;
            BG2.Position = new Vector2(0, 10);
            BG2.AllowScroll = true;

            var BG3 = new OrbisGL.Controls.RowView(GLControl.Size.Width - 100, GLControl.Size.Height / 3); BG2.BackgroundColor = RGBColor.ReallyLightBlue;
            BG3.BackgroundColor = RGBColor.Yellowish;
            BG3.Position = new Vector2(50, 100);
            BG3.AllowScroll = true;


            var TB = new OrbisGL.Controls.TextBox(200, 18);
            TB.Position = new Vector2(10, 10);

            var TB2 = new OrbisGL.Controls.TextBox(200, 18);
            TB2.Position = new Vector2(10, 400);
            TB2.Text = "Debug texbox test 2";

            TB.Text = "Debug texbox test";

            BG2.AddChild(TB);
            BG2.AddChild(TB2);

            BG3.AddChild(BG2);

            BG.AddChild(BG3);

            GLControl.GLApplication.AddObject(BG);
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

            GLControl.GLApplication.AddObject(Rect);
#endif
        }

        private void button17_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var BG = new OrbisGL.Controls.RowView(GLControl.Size.Width, GLControl.Size.Height);
            BG.BackgroundColor = RGBColor.White;


            var CK = new Checkbox(20);
            CK.Text = "Hello World";
            CK.Position = new Vector2(10, 10);

            BG.AddChild(CK);

            GLControl.GLApplication.AddObject(BG);
#endif

        }

        private void button18_Click(object sender, EventArgs e)
        {

#if !ORBIS
            var BG = new OrbisGL.Controls.RowView(GLControl.Size.Width, GLControl.Size.Height);
            BG.BackgroundColor = RGBColor.White;

            var List = new RowView(300, 500);
            List.Position = new Vector2(100, 100);
            List.BackgroundColor = RGBColor.ReallyLightBlue;

            for (int i = 0; i < 15; i++)
            {
                var RB = new Radiobutton(20);

                RB.Text = "Hello World " + i;
                List.AddChild(RB);
            }

            BG.AddChild(List);

            GLControl.GLApplication.AddObject(BG);
#endif
        }

        private void button19_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var BG = new Panel(GLControl.Size.Width, GLControl.Size.Height);
            BG.BackgroundColor = RGBColor.White;

            var Inspect = new Inspector(600, 600);
            Inspect.Position = new Vector2(400, 0);

            var List = new RowView(300, 600);
            List.Position = new Vector2(0, 0);
            List.BackgroundColor = RGBColor.ReallyLightBlue;

            var RB = new Radiobutton(28);
            RB.Text = "Hello World";
            RB.OnMouseClick += (s, a) => { Inspect.Target = (OrbisGL.Controls.Control)s; };

            var BTN = new Button(200, 20, 28);
            BTN.Text = "Hello World";
            BTN.OnMouseClick += (s, a) => { Inspect.Target = (OrbisGL.Controls.Control)s; };

            var TB = new TextBox(200, 28);
            TB.Text = "Hello World";
            TB.OnMouseClick += (s, a) => { Inspect.Target = (OrbisGL.Controls.Control)s; };

            List.AddChild(RB);
            List.AddChild(BTN);
            List.AddChild(TB);

            BG.AddChild(List);
            BG.AddChild(Inspect);

            GLControl.GLApplication.AddObject(BG);
#endif
        }

        private void button20_Click(object sender, EventArgs e)
        {
#if !ORBIS
            var BG = new Panel(GLControl.Size.Width, GLControl.Size.Height);
            BG.BackgroundColor = RGBColor.White;

            var DropButton = new DropDownButton(200);

            DropButton.Items.Add("Option A");
            DropButton.Items.Add("Option B");
            DropButton.Items.Add("Option C");
            DropButton.Items.Add("Option D");
            DropButton.Items.Add("Option E");
            DropButton.Items.Add("Option F");
            DropButton.Items.Add("Option G");
            DropButton.Items.Add("Option H");
            DropButton.Items.Add("Option I");
            DropButton.Items.Add("Option J");
            DropButton.Items.Add("Option K");
            DropButton.Items.Add("Option L");
            DropButton.Items.Add("Option M");
            DropButton.Items.Add("Option N");
            DropButton.Items.Add("Option O");

            DropButton.Text = "Select an Option";

            DropButton.Position = new Vector2(100, 20);

            BG.AddChild(DropButton);

            GLControl.GLApplication.AddObject(BG);
#endif

        }
    }
}