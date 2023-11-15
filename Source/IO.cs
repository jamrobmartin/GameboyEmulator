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

        public Byte Read(Word address)
        {
            if (address >= 0xFF01 && address <= 0xFF01) { return serialData[0]; }
            if (address >= 0xFF02 && address <= 0xFF02) { return serialData[1]; }

            throw new Exception("IO - Tried to read memory location: " + address.ToHexString());
        }

        public void Write(Word address, Byte value)
        {
            if (address >= 0xFF01 && address <= 0xFF01) { serialData[0] = value; return; }
            if (address >= 0xFF02 && address <= 0xFF02) { serialData[1] = value; return; }

            throw new Exception("IO - Tried to Write memory location: " + address.ToHexString());
        }
    }
}
