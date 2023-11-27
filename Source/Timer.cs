using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyEmulator
{
    public class Timer
    {
        #region Singleton
        private static readonly Lazy<Timer> lazy = new Lazy<Timer>(() => new Timer());
        public static Timer Instance { get; private set; } = lazy.Value;
        #endregion

        // 0xFF04
        public Word DIV { get; set; } = 0;

        // 0xFF05
        public Byte TIMA { get; set; } = 0;

        // 0xFF06
        public Byte TMA { get; set; } = 0;

        // 0xFF07
        public Byte TAC { get; set; } = 0;

        public void Init()
        {
            DIV = 0xAC00;
            TIMA = 0;
            TMA = 0;
            TAC = 0;
        }

        public void DoCycles(int T_Cycles)
        {
            Word prev_div = DIV;

            DIV++;

            bool timer_update = false;

            switch (TAC & 0b11)
            {
                case 0b00: timer_update = ((prev_div & (1 << 9)) > 0) && ((DIV & (1 << 9)) == 0); break;
                case 0b01: timer_update = ((prev_div & (1 << 3)) > 0) && ((DIV & (1 << 3)) == 0); break;
                case 0b10: timer_update = ((prev_div & (1 << 5)) > 0) && ((DIV & (1 << 5)) == 0); break;
                case 0b11: timer_update = ((prev_div & (1 << 7)) > 0) && ((DIV & (1 << 7)) == 0); break;
            }

            if (timer_update && ((TAC & (1 << 2)) > 0))
            {
                TIMA++;

                if (TIMA == 0xFF)
                {
                    TIMA = TMA;

                    CPU.Instance.RequestInterupt(eInterruptType.Timer);
                }
            }

            if (T_Cycles >= 2)
                DoCycles(T_Cycles - 1);
        }

        public Byte Read(Word address)
        {
            if (address == 0xFF04) { return (DIV >> 8) & 0x00FF; }
            if (address == 0xFF05) { return TIMA; }
            if (address == 0xFF06) { return TMA; }
            if (address == 0xFF07) { return TAC; }

            throw new Exception("Timer - Tried to read memory location: " + address.ToHexString());
        }

        public void Write(Word address, Byte value)
        {
            if (address == 0xFF04) { DIV = 0; return; }
            if (address == 0xFF05) { TIMA = value; return; }
            if (address == 0xFF06) { TMA = value; return; }
            if (address == 0xFF07) { TAC = value; return; }

            throw new Exception("Timer - Tried to Write memory location: " + address.ToHexString());
        }
    }
}
