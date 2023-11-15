using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyEmulator
{
    public class Bus
    {
        #region Read/Write

        //  Start   End     Description
        //  0000	3FFF	16 KiB ROM bank 00
        //  4000    7FFF    16 KiB ROM Bank 01~NN
        //  8000    9FFF    8 KiB Video RAM(VRAM)
        //  A000    BFFF    8 KiB External RAM
        //  C000    CFFF    4 KiB Work RAM(WRAM)
        //  D000    DFFF    4 KiB Work RAM(WRAM)   In CGB mode, switchable bank 1~7
        //  E000    FDFF    Mirror of C000~DDFF(ECHO RAM)
        //  FE00    FE9F    Object attribute memory(OAM)
        //  FEA0    FEFF    Not Usable
        //  FF00    FF7F    I / O Registers
        //  FF80    FFFE    High RAM(HRAM)
        //  FFFF    FFFF    Interrupt Enable register(IE)

        public static byte Read(Word address)
        {
            if (address >= 0x0000 && address <= 0x7FFF) { return Cartridge.Instance.Read(address); }
            if (address >= 0x8000 && address <= 0x9FFF) { return RAM.Instance.Read(address); }
            if (address >= 0xA000 && address <= 0xBFFF) { return Cartridge.Instance.Read(address); }
            if (address >= 0xC000 && address <= 0xDFFF) { return RAM.Instance.Read(address); }
            if (address >= 0xE000 && address <= 0xFDFF) { return RAM.Instance.Read((Word)(address - 0x2000)); }// Echo RAM
            if (address >= 0xFE00 && address <= 0xFE9F) { return PPU.Instance.Read(address); }
            if (address >= 0xFEA0 && address <= 0xFEFF) { }// Not Used
            if (address >= 0xFF00 && address <= 0xFF7F) { return IO.Instance.Read(address); }
            if (address >= 0xFF80 && address <= 0xFFFE) { return RAM.Instance.Read(address); }
            if (address >= 0xFFFF && address <= 0xFFFF) { return IO.Instance.Read(address); }

            throw new Exception("BUS - Tried to read memory location: " + address.ToHexString());

        }

        public static void Write(Word address, byte value) 
        {

        }

        #endregion
    }
}
