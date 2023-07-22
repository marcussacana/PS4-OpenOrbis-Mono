using OrbisGL.GL;
using System;
using System.Numerics;
using OrbisGL.Input;
using OrbisGL.Controls;
using OrbisGL;

namespace Orbis
{
    internal class MainDisplay : Application
    {
        public MainDisplay() : base(1920, 1080, 60)
        {
#if ORBIS
            EnableKeyboard();

            EnableDualshock(new DualshockSettings() 
            {
                LeftAnalogAsPad = true, 
                Mouse = VirtualMouse.Touchpad
            });
#endif
            InitializeComponents();
        }

        Random Rand = new Random();

        private void InitializeComponents()
        {
            var BG = new Panel(1920, 1080);
            BG.Size = new Vector2(Width, Height);

            var Button = new Button(150, 25, 24);
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

            Dualshock.OnButtonDown += (sender, args) =>
            {
                Button.Text = $"{args.Button} Pressed";
            };
            Dualshock.OnButtonUp += (sender, args) =>
            {
                Button.Text = $"{args.Button} Released";
            };
            
            Button.Position = new Vector2(Rand.Next(0, Width - 200), Rand.Next(Height - 200));

            var TB = new TextBox(300, 24);
            TB.Text = "test";
            TB.Position = new Vector2(Rand.Next(0, Width - 200), Rand.Next(Height - 200));

            BG.AddChild(Button);
            BG.AddChild(TB);

            Objects.Add(BG);
        }


    }
}
