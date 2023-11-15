using System.Text;

namespace GameboyEmulator
{


    public partial class GameboyForm : Form
    {


        public GameboyForm()
        {
            InitializeComponent();

            this.Location = new Point(0, 0);

            DebugWindow debugWindow = new DebugWindow();
            debugWindow.Show();
            debugWindow.Location = new Point(this.Width, 0);

            ButtonPressed += GameboyForm_ButtonPressed;
            ButtonPressed += Emulator.Instance.GameboyForm_ButtonPressed;

        }

        private void GameboyForm_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            Logger.WriteLine("Button Pressed: " + e.buttonPressed.ToString(), Logger.LogLevel.Debug);
        }


        // Button Pressed Event
        public delegate void ButtomPressedEventHandler(object sender, ButtonPressedEventArgs e);
        public event ButtomPressedEventHandler ButtonPressed = delegate { };

        public void FireButtonPressedEvent(eButtonPress e)
        {
            ButtonPressed?.Invoke(this, new ButtonPressedEventArgs(e));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // On
            FireButtonPressedEvent(eButtonPress.On);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Off
            FireButtonPressedEvent(eButtonPress.Off);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Up
            FireButtonPressedEvent(eButtonPress.Up);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Right
            FireButtonPressedEvent(eButtonPress.Right);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Down
            FireButtonPressedEvent(eButtonPress.Down);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Left
            FireButtonPressedEvent(eButtonPress.Left);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // A
            FireButtonPressedEvent(eButtonPress.A);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // B
            FireButtonPressedEvent(eButtonPress.B);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            // Start
            FireButtonPressedEvent(eButtonPress.Start);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // Select
            FireButtonPressedEvent(eButtonPress.Select);
        }

        private void GameboyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Emulator.Instance.TurnPowerOff();
        }

        private void insertROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                bool inserted = Cartridge.Instance.InsertCartridge(openFileDialog.FileName);
                StringBuilder sb = new StringBuilder();
                sb.Append("File Location: " + openFileDialog.FileName);
                sb.Append(" - Inserted: " + inserted.ToString());
                Logger.WriteLine("Cartridge Insert Status - " + sb.ToString(), Logger.LogLevel.Information);
            }
        }

        private void errorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logger.Level = Logger.LogLevel.Error;
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logger.Level = Logger.LogLevel.Information;
        }

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logger.Level = Logger.LogLevel.Debug;
        }
    }
}