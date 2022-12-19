using System;
using System.IO;
using Orbis.Internals;
using SDL2.Object;
using SDL2.Types.FreeType;

namespace Orbis
{
    public class SampleWindow : Window
    {
        private const int WINDOW_WIDTH = 1920;
        private const int WINDOW_HEIGHT = 1080;
        
        private static Random Rand = new Random();
        
        private int XAccel = 1;
        private int YAccel = 1;
        
        private int LogoSpeed = 8;
        private ImageElement Logo;

        private TextElement Label;
        
        public SampleWindow() : base(WINDOW_WIDTH, WINDOW_HEIGHT)
        {
            
            string BaseDir = IO.GetAppBaseDirectory();
            string TGAPath = Path.Combine(BaseDir, "assets", "images", "dvd-logo.png");

            Logo = new ImageElement(this, TGAPath);
            Logo.ChangeColor((byte)Rand.Next(256), (byte)Rand.Next(256), (byte)Rand.Next(256));

            string TTFPath = Path.Combine(BaseDir, "assets", "fonts", "Gontserrat-Regular.ttf");
            
            Label = new TextElement(this, "labelTest");
            TextElement.InitFont(Label, TTFPath, 64);
            Label.ParentLocation.Set(10, 10);

            Label.Text = "Hello World from C#";


            if (Joystick.Online > 0)
                Joystick.Open(0);
        }

        public override void OnCycleBegin(uint FrameTime, uint NextFrameTime)
        {
            if (FrameTime < NextFrameTime)
                return;

            int X = XAccel * LogoSpeed / 2;
            int Y = YAccel * LogoSpeed / 2;

            Logo.ParentLocation.Sum(X, Y);
            Collision();
        }

        private void Collision()
        {
            if (Logo.ParentLocation.X + Logo.Size.Width >= WINDOW_WIDTH) {
                XAccel = -1;
                Logo.ChangeColor((byte)Rand.Next(256), (byte)Rand.Next(256), (byte)Rand.Next(256));
            }
            if (Logo.ParentLocation.X <= 0) {
                XAccel = 1;
                Logo.ChangeColor((byte)Rand.Next(256), (byte)Rand.Next(256), (byte)Rand.Next(256));
            }
            if (Logo.ParentLocation.Y <= 0) {
                YAccel = 1;
                Logo.ChangeColor((byte)Rand.Next(256), (byte)Rand.Next(256), (byte)Rand.Next(256));
            }
            if (Logo.ParentLocation.Y + Logo.Size.Height >= WINDOW_HEIGHT) {
                YAccel = -1;
                Logo.ChangeColor((byte)Rand.Next(256), (byte)Rand.Next(256), (byte)Rand.Next(256));
            }
        }
    }
}