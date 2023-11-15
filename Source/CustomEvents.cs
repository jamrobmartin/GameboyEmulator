using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyEmulator
{
    public enum eButtonPress
    {
        On,
        Off,
        Up,
        Down,
        Left,
        Right,
        A,
        B,
        Start,
        Select
    }

    public class ButtonPressedEventArgs : EventArgs
    {
        public eButtonPress buttonPressed { get; set; }

        public ButtonPressedEventArgs(eButtonPress buttonPressed)
        {
            this.buttonPressed = buttonPressed;
        }
    }

    internal class CustomEvents
    {
    }
}
