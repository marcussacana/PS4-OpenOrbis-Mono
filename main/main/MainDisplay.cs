using OrbisGL.GL;
using System;
using System.Numerics;
using OrbisGL.Input;
using OrbisGL.Controls;

namespace Orbis
{
    internal class MainDisplay : Application
    {
        public MainDisplay() : base(1920, 1080, 60)
        {
            InitializeComponents();
#if ORBIS
            EnabledKeyboard();
#endif
        }

        Random Rand = new Random();

        private void InitializeComponents()
        {
            var BG = new OrbisGL.Controls.Panel();
            BG.Size = new Vector2(Width, Height);

            MouseDriver = new OrbisMouse();

            var Button = new OrbisGL.Controls.Button(50, 25, 18);
            Button.Text = "Hello World";
            Button.Primary = Rand.Next(0, 2) == 1;

            Button.OnKeyDown += (sender, args) =>
            {
                Button.Text = $"{args.KeyChar?.ToString() ?? args.Keycode.ToString()} Pressed";
            };

            Button.OnKeyUp += (sender, args) =>
            {
                Button.Text = $"{args.KeyChar?.ToString() ?? args.Keycode.ToString()} Released";
            };
            
            Button.OnMouseClick += (sender, args) =>
            {
                Button.Text = "Clicked!";
            };

            Button.Position = new Vector2(Rand.Next(0, Width - 200), Rand.Next(Height - 200));

            var TB = new TextBox(200, 24);
            TB.Text = "test";
            TB.Position = new Vector2(Rand.Next(0, Width - 200), Rand.Next(Height - 200));

            BG.AddChild(Button);
            BG.AddChild(TB);

            Objects.Add(BG);
        }


    }
}
