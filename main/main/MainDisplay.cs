﻿using OrbisGL.GL;
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

            var View = new RowView(300, 600);

            for (int i = 0; i < 30; i++)
            {
                View.AddChild(new Checkbox(28)
                {
                    Text = $"Checkbox {i}"
                });
            }

            var ButtonA = new Button(1, 1, 28);
            ButtonA.Text = "Button A";

            var ButtonB = new Button(1, 1, 28);
            ButtonB.Text = "Button B";

            ButtonA.Position = new Vector2(10, 10);
            View.Position = new Vector2(10, 80);
            ButtonB.Position = new Vector2(10, 80 + View.Size.Y + 60);

            ButtonA.OnClicked += (sender, args) =>
            {
                User.Notify(User.PlaystationButtons, "Button A Clicked");
            };
            
            ButtonB.OnClicked += (sender, args) =>
            {
                User.Notify(User.PlaystationButtons, "Button B Clicked");
            };

            ButtonA.Links.Down = View;
            ButtonB.Links.Up = View;

            View.Links.Up = ButtonA;
            View.Links.Down = ButtonB;

            BG.AddChild(ButtonA);
            BG.AddChild(View);
            BG.AddChild(ButtonB);

            Objects.Add(BG);
        }


    }
}
