using System;
using System.IO;
using OrbisGL.GL;
using System.Numerics;
using Orbis.Internals;
using OrbisGL.Controls;
using OrbisGL;
using OrbisGL.Audio;
using OrbisGL.Debug;

namespace Orbis
{
    internal class Entrypoint : Application
    {
        public Entrypoint() : base(1920, 1080, 60)
        {
#if ORBIS
            EnableKeyboard();

            EnableDualshock(new DualshockSettings() 
            {
                LeftAnalogAsPad = true, 
                PadAsSelector = true,
                Mouse = VirtualMouse.Touchpad
            });
#endif
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            var BG = new Panel(1920, 1080);
            BG.Size = new Vector2(Width, Height);

            var Inspect = new Inspector(600, 600);
            Inspect.Position = new Vector2(400, 0);

            var List = new RowView(300, 600);
            List.Position = new Vector2(0, 0);
            List.BackgroundColor = RGBColor.ReallyLightBlue;

            List.Links.Right = Inspect;
            Inspect.Links.Left = List;

            var RB = new Radiobutton(28);
            RB.Text = "Hello World";
            RB.OnMouseClick += (s, a) => { Inspect.Target = (Control)s; };
            
            var CB = new Checkbox(28);
            CB.Text = "Hello World";
            CB.OnMouseClick += (s, a) => { Inspect.Target = (Control)s; };

            var BTN = new Button(200, 20, 28);
            BTN.Text = "Hello World";
            BTN.OnMouseClick += (s, a) => { Inspect.Target = (Control)s; };

            var TB = new TextBox(200, 28);
            TB.Text = "Hello World";
            TB.OnMouseClick += (s, a) => { Inspect.Target = (Control)s; };

            var Play = new Button(200, 20, 28);
            Play.Text = "Play Audio";
            Play.Position = new Vector2(Inspect.AbsoluteRectangle.Right + 20, Inspect.AbsolutePosition.Y);
            Play.OnClicked += PlayOnClicked;

            Inspect.Links.Right = Play;
            Play.Links.Left = Inspect;

            List.AddChild(RB);
            List.AddChild(CB);
            List.AddChild(BTN);
            List.AddChild(TB);

            BG.AddChild(List);
            BG.AddChild(Inspect);
            BG.AddChild(Play);

            AddObject(BG); 
        }

        private IAudioPlayer Player;
        private void PlayOnClicked(object sender, EventArgs e)
        {
            var AudioOut = new OrbisAudioOut();
            if (Player == null)
            {
                Player = new WavePlayer();
                
                var Stream = File.OpenRead(Path.Combine(IO.GetAppBaseDirectory(), "assets", "audio", "Test.wav"));
                
                Player.Open(Stream);
                Player.SetAudioDriver(AudioOut);
            }
            
            Player.Resume();
            AudioOut.SetVolume(100);
        }
    }
}
