using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyEmulator
{
    public class IO
    {
        #region Singleton
        private static readonly Lazy<IO> lazy = new Lazy<IO>(() => new IO());
        public static IO Instance { get; private set; } = lazy.Value;
        #endregion

        private Byte[] serialData = { 0, 0 };
        private Byte LY = 0;

        public Byte Read(Word address)
        {
            if (address >= 0xFF01 && address <= 0xFF01) { return serialData[0]; }
            if (address >= 0xFF02 && address <= 0xFF02) { return serialData[1]; }

            if (address >= 0xFF04 && address <= 0xFF07) { return Timer.Instance.Read(address); }

            if (address >= 0xFF0F && address <= 0xFF0F) { return CPU.Instance.IF; }

            if (address >= 0xFF40 && address <= 0xFF4B) { return PPU.Instance.Read(address); }

            //throw new Exception("IO - Tried to read memory location: " + address.ToHexString());
            return 0x00;
        }

        public void Write(Word address, Byte value)
        {
            if (address >= 0xFF01 && address <= 0xFF01) { serialData[0] = value; return; }
            if (address >= 0xFF02 && address <= 0xFF02) { serialData[1] = value; return; }

            if (address >= 0xFF04 && address <= 0xFF07) { Timer.Instance.Write(address, value); return; }

            if (address >= 0xFF0F && address <= 0xFF0F) { CPU.Instance.IF = value; return; }

            if (address >= 0xFF40 && address <= 0xFF4B) { PPU.Instance.Write(address, value); return; }

            //throw new Exception("IO - Tried to Write memory location: " + address.ToHexString());
        }
    }
}
