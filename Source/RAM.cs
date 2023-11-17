using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyEmulator
{
    public class RAM
    {
        #region Singleton
        private static readonly Lazy<RAM> lazy = new Lazy<RAM>(() => new RAM());
        public static RAM Instance { get; private set; } = lazy.Value;
        #endregion

        // 0x8000 - 0x9FFF
        private Byte[] VRAM = new Byte[0x2000];

        // 0xC000 - 0xDFFF
        private Byte[] WRAM = new Byte[0x2000];

        // 0xFF80 - 0xFFFE
        private Byte[] HRAM = new Byte[0x80];

        public RAM()
        {
            for (int i = 0; i < VRAM.Length; i++)
            {
                VRAM[i] = 0;
            }

            for (int i = 0; i < WRAM.Length; i++)
            {
                WRAM[i] = 0;
            }

            for (int i = 0; i < HRAM.Length; i++)
            {
                HRAM[i] = 0;
            }
        }

        public Byte Read(Word address)
        {
            if (address >= 0x8000 && address <= 0x9FFF) { return VRAM[address - 0x8000]; }
            if (address >= 0xC000 && address <= 0xDFFF) { return WRAM[address - 0xC000]; }
            if (address >= 0xFF80 && address <= 0xFFFE) { return HRAM[address - 0xFF80]; }

            throw new Exception("RAM - Tried to read memory location: " + address.ToHexString());
        }

        public void Write(Word address, Byte value)
        {
            if (address >= 0x8000 && address <= 0x9FFF) { VRAM[address - 0x8000] = value; return; }
            if (address >= 0xC000 && address <= 0xDFFF) { WRAM[address - 0xC000] = value; return; }
            if (address >= 0xFF80 && address <= 0xFFFE) { HRAM[address - 0xFF80] = value; return; }

            throw new Exception("RAM - Tried to Write memory location: " + address.ToHexString());
        }
    }
}
