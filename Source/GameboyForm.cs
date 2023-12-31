using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace GameboyEmulator
{


    public partial class GameboyForm : Form
    {


        public GameboyForm()
        {
            InitializeComponent();

            this.Location = new Point(0, 0);

            ButtonPressed += GameboyForm_ButtonPressed;
            ButtonPressed += Emulator.Instance.GameboyForm_ButtonPressed;

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 250;
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Enabled = true;

        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            pictureBox1.Invalidate();
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

        private void implementedInstructionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImplementedInstructionsForm form = new ImplementedInstructionsForm();
            form.ShowDialog();
        }

        string TestResults = string.Empty;
        string TestFolderPath = string.Empty;

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
            {
                TestFolderPath = folderBrowserDialog.SelectedPath;

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += Worker_DoWork;
                worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
                worker.RunWorkerAsync();
            }


        }

        private void Worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            Logger.WriteLine(TestResults, Logger.LogLevel.Information);
        }

        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            Logger.WriteLine("Running all files in the folder: " + TestFolderPath, Logger.LogLevel.Information);

            string[] files = Directory.GetFiles(TestFolderPath);

            Emulator.Instance.ShowBlargg = false;

            foreach (var file in files)
            {
                FileInfo info = new FileInfo(file);

                Cartridge.Instance.InsertCartridge(file);
                Emulator.Instance.TurnPowerOn();
                Thread.Sleep(20000);
                TestResults += Environment.NewLine + "Testing File: " + info.Name + Environment.NewLine;
                TestResults += "Output: \"" + Emulator.Instance.BlarggMessage + "\"" + Environment.NewLine;
                Emulator.Instance.TurnPowerOff();
            }
        }

        private void GameboyForm_Load(object sender, EventArgs e)
        {
            DebugWindow debugWindow = new DebugWindow();
            debugWindow.Show();
            debugWindow.Location = new Point(this.Width, 0);

            DebugTileViewer debugTileViewer = new DebugTileViewer();
            debugTileViewer.Show();
            debugTileViewer.Location = new Point(this.Width + debugWindow.Width, 0);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            DrawScreen(e.Graphics);
        }

        private void DrawScreen(Graphics g)
        {
            const int YRES = 144;
            const int XRES = 160;
            const int Scale = 3;

            Rectangle rect = new Rectangle(0, 0, XRES * Scale, YRES * Scale);

            g.FillRectangle(Brushes.White, rect);



            for (int lineNumber = 0; lineNumber < YRES; lineNumber++)
            {
                for (int x = 0; x < XRES; x++)
                {
                    rect.X = x * Scale;
                    rect.Y = lineNumber * Scale;
                    rect.Width = Scale;
                    rect.Height = Scale;

                    g.FillRectangle(new SolidBrush(PPU.Instance.VideoBuffer[x + lineNumber * XRES]), rect);

                }
            }
        }

        private void printCycleCountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Counts:");
            sb.AppendLine("Normal Instructions");

            int ElementWritten = 0;
            for (int i = 0; i <= 0xFF; i++)
            {
                sb.Append(Emulator.Instance.OpCodeCycleDurations[i] + " ");
                ElementWritten++;

                if(ElementWritten == 0x10)
                {
                    sb.AppendLine();
                    ElementWritten = 0;
                }
            }

            sb.AppendLine("Conditional Instructions");

            ElementWritten = 0;
            for (int i = 0; i <= 0xFF; i++)
            {
                sb.Append(Emulator.Instance.OpCodeConditionalCycleDurations[i] + " ");
                ElementWritten++;

                if (ElementWritten == 0x10)
                {
                    sb.AppendLine();
                    ElementWritten = 0;
                }
            }

            sb.AppendLine("CB Instructions");

            ElementWritten = 0;
            for (int i = 0; i <= 0xFF; i++)
            {
                sb.Append(Emulator.Instance.OpCodeCBCycleDurations[i] + " ");
                ElementWritten++;

                if (ElementWritten == 0x10)
                {
                    sb.AppendLine();
                    ElementWritten = 0;
                }
            }

            Logger.WriteLine(sb.ToString(), Logger.LogLevel.Information);
        }
    }
}