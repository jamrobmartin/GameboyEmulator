using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyEmulator
{
    public class PPU
    {
        #region Singleton
        private static readonly Lazy<PPU> lazy = new Lazy<PPU>(() => new PPU());
        public static PPU Instance { get; private set; } = lazy.Value;
        #endregion

        private Byte[] OAM = new Byte[160];

        public PPU()
        {
            for (int i = 0; i < OAM.Length; i++)
            {
                OAM[i] = 0;
            }
        }

        #region R/W
        public Byte Read(Word address)
        {
            if (address >= 0xFE00 && address <= 0xFE9F) { return OAM[address - 0xFE00]; }

            throw new Exception("PPU - Tried to read memory location: " + address.ToHexString());
        }

        public void Write(Word address, Byte value)
        {
            if (address >= 0xFE00 && address <= 0xFE9F) { OAM[address - 0xFE00] = value; return; }

            throw new Exception("PPU - Tried to Write memory location: " + address.ToHexString());
        }
        #endregion

    }
}
