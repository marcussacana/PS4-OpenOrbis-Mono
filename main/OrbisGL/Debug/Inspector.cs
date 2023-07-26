using OrbisGL.Controls;
using OrbisGL.GL;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Debug
{
    public class Inspector : RowView
    {
        Label lblStatus = new Label("Status: ");
        Label lblCurrent = new Label("Current: ");

        Button btnBack;

        public Inspector(Vector2 Size) : base(Size)
        {
            btnBack = new Button((int)Size.X - 20, 20, 20);
            btnBack.Text = "Back";
            btnBack.OnClicked += BtnBack_OnClicked;

            AddChild(lblCurrent);
            AddChild(lblStatus);
            AddChild(btnBack);

            BackgroundColor = RGBColor.White;
        }

        public Inspector(int Width, int Height) : this(new Vector2(Width, Height)) { }

        Control _Target;
        public Control Target
        {
            get => _Target;
            set
            {
                bool Modified = _Target != value;
                _Target = value;

                InspectorStack.Clear(); 
                InspectorStack.Push(value);

                if (Modified)
                    UpdateInspector(value);
            }

        }

        Stack<object> InspectorStack = new Stack<object>();

        private void UpdateInspector(object Target)
        {
            //Prevent Dispose
            RemoveChild(lblCurrent);
            RemoveChild(lblStatus);
            RemoveChild(btnBack);

            //Remove and Dispose all childs
            RemoveChildren(true);

            AddChild(lblCurrent);
            AddChild(lblStatus);
            AddChild(btnBack);

            lblCurrent.Text = $"Current: {Target.GetType().Name}";

            foreach (var Prop in GetProperties())
            {
                if (IsEditable(Prop.PropertyType))
                {
                    object Value = null;

                    try
                    {
                        Value = Prop.GetValue(Target);
                    }
                    catch
                    {
                        continue;
                    }

                    BuildViewer(Prop, Prop.Name, Value);
                }
                else
                {
                    if ((Prop.Name.Contains("<") && Prop.Name.Contains(">")) || Prop.Name.Contains("UniformLocation"))
                        continue;

                    var Button = new Button((int)Size.X - 30, 30, 18);
                    Button.AutoSize = false;
                    Button.Text = Prop.Name;
                    Button.Model = Prop;
                    Button.OnClicked += BtnEnterClicked;
                    Button.Refresh();

                    AddChild(Button);
                }
            }

            foreach (var Field in GetFields())
            {
                if (IsEditable(Field.FieldType))
                {
                    object Value = null;

                    try
                    {
                        Value = Field.GetValue(Target);
                    }
                    catch
                    {
                        continue;
                    }

                    BuildViewer(Field, Field.Name, Value);
                }
                else
                {
                    if ((Field.Name.Contains("<") && Field.Name.Contains(">")) || Field.Name.Contains("UniformLocation"))
                        continue;

                    var Button = new Button((int)Size.X - 30, 30, 18);
                    Button.AutoSize = false;
                    Button.Text = Field.Name;
                    Button.Model = Field;
                    Button.OnClicked += BtnEnterClicked;
                    Button.Refresh();

                    AddChild(Button);
                }
            }

            Invalidate();
        }

        void BuildViewer(object Info, string Name, object Value)
        {
            if ((Name.Contains("<") && Name.Contains(">")) || Name.Contains("UniformLocation"))
                return;

            var Panel = new Panel((int)Size.X - 30, 30);

            var TB = new TextBox(100, 18);

            TB.Text = ToString(Value);
            TB.Model = Info;
            TB.TextChanged += ValueChanged;

            var lbl = new Label($"{Name}: ");
            lbl.Refresh();

            TB.Position = new Vector2(lbl.Size.X + 5, 0);
            TB.Size = new Vector2(Size.X - TB.Position.X - 30, TB.Size.Y);

            Panel.AddChild(lbl);
            Panel.AddChild(TB);

            Panel.Size = new Vector2(Panel.Size.X, Math.Max(TB.Size.Y, lbl.Size.Y));

            AddChild(Panel);
        }

        private void ValueChanged(object sender, EventArgs e)
        {
            if (sender is TextBox TB)
            {
                var Source = TB.Model;
                if (Source is PropertyInfo Prop)
                {
                    var NewValue = FromString(Prop.PropertyType, TB.Text);
                    if (NewValue == null)
                    {
                        SetStatus($"Failed to Parse: {TB.Text}");
                        return;
                    }

                    Prop.SetValue(InspectorStack.Peek(), NewValue);
                    SetStatus($"Value Updated");
                }

                if (Source is FieldInfo Field)
                {
                    var NewValue = FromString(Field.FieldType, TB.Text);
                    if (NewValue == null)
                    {
                        SetStatus($"Failed to Parse: {TB.Text}");
                        return;
                    }

                    Field.SetValue(InspectorStack.Peek(), NewValue);
                    SetStatus($"Value Updated");
                }
            }
        }

        private void BtnBack_OnClicked(object sender, EventArgs e)
        {
            if (InspectorStack.Count <= 1)
                return;

            InspectorStack.Pop();
            UpdateInspector(InspectorStack.Peek());
        }

        private void BtnEnterClicked(object sender, EventArgs e)
        {
            object Target;
            if (sender is Button btn)
                Target = btn.Model;
            else
                return;

            if (Target == null)
                return;

            if (Target is PropertyInfo Prop)
            {
                var Value = Prop.GetValue(InspectorStack.Peek());
                if (Value == null)
                {
                    SetStatus($"{Prop.Name} is null");
                    return;
                }

                InspectorStack.Push(Value);
                UpdateInspector(Value);
                return;
            }

            if (Target is FieldInfo Field)
            {
                var Value = Field.GetValue(InspectorStack.Peek());
                if (Value == null)
                {
                    SetStatus($"{Field.Name} is null");
                    return;
                }

                InspectorStack.Push(Value);
                UpdateInspector(Value);
                return;
            }

            SetStatus("Failed to Enter");
        }

        private void SetStatus(string Msg)
        {
            lblStatus.Text = $"Status: {Msg}";
        }

        private object FromString(Type Type, string Value)
        {
            switch (Type.Name)
            {
                case nameof(String):
                    return Value;

                case nameof(RGBColor):
                    return new RGBColor(Value);

                case nameof(Int64):
                    if (long.TryParse(Value, out long Val64))
                        return Val64;
                    break;

                case nameof(Int32):
                    if (int.TryParse(Value, out int Val32))
                        return Val32;
                    break;

                case nameof(Int16):
                    if (short.TryParse(Value, out short Val16))
                        return Val16;
                    break;

                case nameof(UInt64):
                    if (ulong.TryParse(Value, out ulong uVal64))
                        return uVal64;
                    break;

                case nameof(UInt32):
                    if (uint.TryParse(Value, out uint uVal32))
                        return uVal32;
                    break;

                case nameof(UInt16):
                    if (ushort.TryParse(Value, out ushort uVal16))
                        return uVal16;
                    break;

                case nameof(Double):
                    if (double.TryParse(Value, out double DoubleVal))
                        return DoubleVal;
                    break;

                case nameof(Single):
                    if (float.TryParse(Value, out float SingleVal))
                        return SingleVal;
                    break;

                case nameof(Boolean):
                    if (bool.TryParse(Value, out bool BooleanVal))
                        return BooleanVal;
                    break;
            }

            return null;
        }

        public IEnumerable<PropertyInfo> GetProperties()
        {
            var TargetType = InspectorStack.Peek().GetType();
            return TargetType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.DeclaringType == TargetType);
        }

        public IEnumerable<FieldInfo> GetFields()
        {
            var TargetType = InspectorStack.Peek().GetType();
            return TargetType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.DeclaringType == TargetType);
        }
        public string ToString(object Data)
        {
            if (Data is RGBColor Color)
                return Color.ToString();

            if (Data is int Int16)
                return Int16.ToString();

            if (Data is int Int32)
                return Int32.ToString();

            if (Data is int Int64)
                return Int64.ToString();

            if (Data is float Single)
                return Single.ToString();

            if (Data is double Double)
                return Double.ToString();

            if (Data is string String)
                return String;

            if (Data is bool Boolean)
                return Boolean.ToString();

            return $"UNK TYPE {Data.GetType().Name}";
        }
        public bool IsEditable(Type Info)
        {
            switch (Info.Name)
            {
                case nameof(Boolean):
                case nameof(String):
                case nameof(RGBColor):
                case nameof(UInt64):
                case nameof(UInt32):
                case nameof(UInt16):
                case nameof(Int64):
                case nameof(Int32):
                case nameof(Int16):
                case nameof(Double):
                case nameof(Single):
                    return true;
            }
            return false;
        }
    }
}
