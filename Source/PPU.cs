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

        #region Registers
        public Byte LCDC { get; set; } = 0; // 0xFF40
        public Byte STAT { get; set; } = 0; // 0xFF41
        public Byte SCY { get; set; } = 0;  // 0xFF42
        public Byte SCX { get; set; } = 0;  // 0xFF43
        public Byte LY { get; set; } = 0;   // 0xFF44
        public Byte LYC { get; set; } = 0;  // 0xFF45
        public Byte DMA { get; set; } = 0;  // 0xFF46
        public Byte BGP { get; set; } = 0;  // 0xFF47
        public Byte OBP0 { get; set; } = 0; // 0xFF48
        public Byte OBP1 { get; set; } = 0; // 0xFF49
        public Byte WY { get; set; } = 0;   // 0xFFA
        public Byte WX { get; set; } = 0;   // 0xFFB

        #endregion

        public PPU()
        {
            for (int i = 0; i < OAM.Length; i++)
            {
                OAM[i] = 0;
            }
        }

        public void Init()
        {
            LCDC = 0x91;
            STAT = 0;
            SCY = 0;
            SCX = 0;
            LY = 0;
            LYC = 0;
            BGP = 0xFC;
            OBP0 = 0xFF;
            OBP1 = 0xFF;
            WY = 0;
            WX = 0;

            LineTicks = 0;

            PPUMode = eMode.OAMScan;

            for (int i = 0; i < OAM.Length; i++)
            {
                OAM[i] = 0;
            }
        }

        #region R/W
        public Byte Read(Word address)
        {
            if (address >= 0xFE00 && address <= 0xFE9F) { return OAM[address - 0xFE00]; }

            if (address == 0xFF40) { return LCDC; }
            if (address == 0xFF41) { return STAT; }
            if (address == 0xFF42) { return SCY;  }
            if (address == 0xFF43) { return SCX;  }
            if (address == 0xFF44) { return LY;   }
            if (address == 0xFF45) { return LYC;  }
            if (address == 0xFF46) { return DMA;  }
            if (address == 0xFF47) { return BGP;  }
            if (address == 0xFF48) { return OBP0; }
            if (address == 0xFF49) { return OBP1; }
            if (address == 0xFF4A) { return WY;   }
            if (address == 0xFF4B) { return WX;   }

            throw new Exception("PPU - Tried to read memory location: " + address.ToHexString());
        }

        public void Write(Word address, Byte value)
        {
            if (address >= 0xFE00 && address <= 0xFE9F) { OAM[address - 0xFE00] = value; return; }

            if (address == 0xFF40) { LCDC = value; return;}
            if (address == 0xFF41) { STAT = value; return;}
            if (address == 0xFF42) { SCY  = value; return;}
            if (address == 0xFF43) { SCX  = value; return;}
            if (address == 0xFF44) { LY   = value; return;}
            if (address == 0xFF45) { LYC  = value; return;}
            if (address == 0xFF46) { DMA  = value; return;}
            if (address == 0xFF47) { BGP  = value; return;}
            if (address == 0xFF48) { OBP0 = value; return;}
            if (address == 0xFF49) { OBP1 = value; return;}
            if (address == 0xFF4A) { WY   = value; return;}
            if (address == 0xFF4B) { WX   = value; return;}

            throw new Exception("PPU - Tried to Write memory location: " + address.ToHexString());
        }
        #endregion

        #region StateMachine

        public const int LINES_PER_FRAME = 154;
        public const int TICKS_PER_LINE = 456;
        public const int YRES = 144;
        public const int XRES = 160;

        public UInt32 LineTicks = 0;

        public enum eMode
        {
            HorizontalBlank = 0,
            VerticalBlank = 1,
            OAMScan = 2,
            PixelDraw = 3
        }

        public eMode PPUMode 
        { 
            get
            {
                return (eMode)(STAT & 0b11);
            }

            set
            {
                STAT &= 0b11111100;
                STAT |= (byte)value;
            }
        }

        public void Tick()
        {
            LineTicks++;

            switch (PPUMode)
            {
                case eMode.HorizontalBlank: DoHorizontalBlank(); break;
                case eMode.VerticalBlank:   DoVerticalBlank(); break;
                case eMode.OAMScan:         DoOAMScan(); break;
                case eMode.PixelDraw:       DoPixelDraw(); break;
                default:
                    break;
            }
        }

        public void IncrementLY()
        {
            LY++;
        }

        public void DoHorizontalBlank()
        {
            // If you have reached the end of the line
            if(LineTicks >= TICKS_PER_LINE)
            {
                // Move to the next line
                IncrementLY();
                LineTicks = 0;

                // If you have reached the bottom of the screen
                if (LY >= YRES)
                {
                    // Change to Vertical Blank mode
                    PPUMode = eMode.VerticalBlank;
                }
                else
                {
                    // We have more lines to render, 
                    // Change to OAM Scan
                    PPUMode = eMode.OAMScan;
                }
            }

        }

        public void DoVerticalBlank()
        {
            // If you have reached the end of the line
            if (LineTicks >= TICKS_PER_LINE)
            {
                IncrementLY();
                LineTicks = 0;

                //If we have reached the bottom of the frame
                if (LY >= LINES_PER_FRAME)
                {
                    // Go back to the top.
                    LY = 0;
                    PPUMode = eMode.OAMScan;
                }
            }
        }

        public void DoOAMScan()
        {
            // If you are done reading OAM
            if (LineTicks >= 80)
            {
                // Move to Pixel Draw
                PPUMode = eMode.PixelDraw;
            }

        }

        public void DoPixelDraw()
        {
            // This length of this section of rendering will be depending on several factors
            // For now, we will assume it does nothing and just move straight to the next mode

            PPUMode = eMode.HorizontalBlank;
        }

        #endregion

    }
}
