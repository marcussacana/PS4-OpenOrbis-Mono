using OrbisGL.GL;
using System;
using System.Numerics;
using OrbisGL.Input;

namespace Orbis
{
    internal class MainDisplay : Application
    {
        public MainDisplay() : base(1920, 1080, 60)
        {
            InitializeComponents();
            EnabledKeyboard();
        }

        Random Rand = new Random();

        private void InitializeComponents()
        {
            var BG = new OrbisGL.Controls.Panel();
            BG.Size = new Vector2(Width, Height);

            MouseDriver = new OrbisMouse();

            var Button = new OrbisGL.Controls.Button(50, 25, 18);
            Button.Name = "Hello World";
            Button.Primary = Rand.Next(0, 2) == 1;

            Button.Position = new Vector2(Rand.Next(0, Width - 200), Rand.Next(Height - 200));

            BG.AddChild(Button);

            Objects.Add(BG);
        }


    }
}
