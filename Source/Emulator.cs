using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyEmulator
{
    public class Emulator
    {
        #region Singleton
        private static readonly Lazy<Emulator> lazy = new Lazy<Emulator>(() => new Emulator());
        public static Emulator Instance { get; private set; } = lazy.Value;
        #endregion

        public Emulator() { }

        public bool PoweredOn { get; set; } = false;

        #region ThreadLoop

        Thread? Thread { get; set; } = null;

        public void StartThread()
        {
            Logger.WriteLine("Starting Emulator ThreadLoop.", Logger.LogLevel.Information);
            Thread = new Thread(ThreadLoop);
            Thread.Start();
            Logger.WriteLine("Emulator ThreadLoop Started.", Logger.LogLevel.Information);
        }

        public void StopThread()
        {
            Logger.WriteLine("Stopping Emulator ThreadLoop.", Logger.LogLevel.Information);
            Logger.WriteLine("Emulator ThreadLoop Stopped.", Logger.LogLevel.Information);
        }

        public void ThreadLoop()
        {
            // Init() Calls
            CPU.Instance.Init();

            InstructionCount = 0;

            BlarggMessage = string.Empty;

            while (PoweredOn)
            {
                try
                {
                    Logger.WriteLine("CPU Step", Logger.LogLevel.Debug);

                    CPU.Instance.Step();

                    InstructionCount++;
                    BlarggUpdate();



                    //Thread.Sleep(10);
                }
                catch ( Exception e)
                {
                    PoweredOn = false;
                    Logger.WriteLine("Exception - " + e.Message, Logger.LogLevel.Error);
                    Logger.WriteLine("PC:" + CPU.Instance.InstructionAddress.ToHexString() + " " + CPU.Instance.Instruction.ToString(), Logger.LogLevel.Error);
                    CPU.Instance.PrintState(Logger.LogLevel.Error);
                }
            }
        }

        #endregion


        #region BLARGG Tests
        private int InstructionCount = 0;

        private string BlarggMessage = string.Empty;

        public void BlarggUpdate()
        {
            if (Bus.Read(0xFF02) == 0x81)
            {
                
                Byte val = Bus.Read(0xFF01);
                //MessageBox.Show("DBG_Update() - " + val.ToHexString());

                if (val == 10 || val == 13)
                {
                    val = 32;
                } 

                if(val >= 32 && val <= 126)
                {
                    char c = (char)val;
                    BlarggMessage = BlarggMessage + c.ToString();
                }

                Bus.Write(0xFF02, 0);

                BlarggDisplay();
            }
        }

        public void BlarggDisplay()
        {
            if (BlarggMessage != string.Empty)
            {
                Logger.WriteLine("Blarrg - \"" + BlarggMessage + "\"", Logger.LogLevel.Information);
            }
        }


        #endregion

        #region Button Presses
        public void GameboyForm_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            switch (e.buttonPressed)
            {
                case eButtonPress.On:
                    if (!PoweredOn)
                    {
                        // If not already powered on, turn power on
                        TurnPowerOn();
                    }
                    break;
                case eButtonPress.Off:
                    if (PoweredOn)
                    {
                        // If powered on, turn power off
                        TurnPowerOff();
                    }
                    break;
                case eButtonPress.Up:
                    break;
                case eButtonPress.Down:
                    break;
                case eButtonPress.Left:
                    break;
                case eButtonPress.Right:
                    break;
                case eButtonPress.A:
                    break;
                case eButtonPress.B:
                    break;
                case eButtonPress.Start:
                    break;
                case eButtonPress.Select:
                    break;
                default:
                    break;
            }
        }

        public void TurnPowerOn()
        {
            PoweredOn = true;
            StartThread();
        }

        public void TurnPowerOff()
        {
            PoweredOn = false;
            StopThread();
        }

        #endregion

    }
}
