namespace OrbisGL.Controls.Events
{
    public delegate void ButtonEventHandler(object Sender, ButtonEventArgs Args);

    public class ButtonEventArgs : PropagableEventArgs
    {
        public OrbisPadButton Button { get; }
        public ButtonEventArgs(OrbisPadButton Button) {
            this.Button = Button;
        }
    }
}
