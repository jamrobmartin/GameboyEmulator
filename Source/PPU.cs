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

        public Color[] VideoBuffer { get; set; } = new Color[YRES * XRES];

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

        #region LCDC

        public bool LCDEnabled
        {
            get
            {
                return LCDC.GetBit(7);
            }

            set
            {
                LCDC.SetBit(7, value);
            }
        }

        public bool WindowEnabled
        {
            get
            {
                return LCDC.GetBit(5);
            }

            set
            {
                LCDC.SetBit(5, value);
            }
        }

        public bool ObjEnabled
        {
            get
            {
                return LCDC.GetBit(1);
            }

            set
            {
                LCDC.SetBit(1, value);
            }
        }

        public bool BGandWindowEnabled
        {
            get
            {
                return LCDC.GetBit(0);
            }

            set
            {
                LCDC.SetBit(0, value);
            }
        }

        public Word WindowTileMapArea
        {
            get
            {
                return LCDC.GetBit(6) ? 0x9C00 : 0x9800;
            }
        }

        public Word BGandWindowTileDataArea
        {
            get
            {
                return LCDC.GetBit(4) ? 0x8000 : 0x8800;
            }
        }

        public Word BGTileMapArea
        {
            get
            {
                return LCDC.GetBit(3) ? 0x9C00 : 0x9800;
            }
        }

        public int OBJSize
        {
            get
            {
                return LCDC.GetBit(2) ? 16 : 8;
            }
        }

        #endregion

        #region Palettes
        public Color[] DefaultColors { get; set; } = { Color.White, Color.FromArgb(255,170,170,170), Color.FromArgb(255, 85, 85, 85), Color.Black };

        public Color[] BGColors { get; set; } = { Color.White, Color.LightGray, Color.DarkGray, Color.Black };
        public Color[] SP1Colors { get; set; } = { Color.White, Color.LightGray, Color.DarkGray, Color.Black };
        public Color[] SP2Colors { get; set; } = { Color.White, Color.LightGray, Color.DarkGray, Color.Black };

        public void UpdatePalette(Byte data, int palette)
        {
            Color[] colors = new Color[4];

            colors[0] = DefaultColors[(data >> 0) & 0b11];
            colors[1] = DefaultColors[(data >> 2) & 0b11];
            colors[2] = DefaultColors[(data >> 4) & 0b11];
            colors[3] = DefaultColors[(data >> 6) & 0b11];

            for (int i = 0; i < 4; i++)
            {

                switch (palette)
                {
                    case 1: BGColors[i] = colors[i]; break;
                    case 2: SP1Colors[i] = colors[i]; break;
                    case 3: SP2Colors[i] = colors[i]; break;
                    default:
                        break;
                }
            }
        }
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

            for (int i = 0; i < VideoBuffer.Length; i++)
            {
                VideoBuffer[i] = Color.White;
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
            if (address == 0xFF46) { DMA  = value; DMAStart(value); return;}
            if (address == 0xFF47) { BGP  = value; UpdatePalette(value, 1); return;}
            if (address == 0xFF48) { OBP0 = value; UpdatePalette(value & 0b11111100, 2); return;}
            if (address == 0xFF49) { OBP1 = value; UpdatePalette(value & 0b11111100, 3); return;}
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

        public enum eInterrupt
        {
            HorizontalBlank,
            VerticalBlank,
            OAMScan,
            LYCompare
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

        public bool CheckInterrupt(eInterrupt interrupt)
        {
            Byte interruptByte = 0b00001000 << (int)interrupt;

            return STAT & interruptByte > 0;
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

            if(LY == LYC)
            {
                STAT.SetBit(2, true);

                if(CheckInterrupt(eInterrupt.LYCompare))
                {
                    CPU.Instance.RequestInterupt(eInterruptType.LCD);
                }

            }
            else
            {
                STAT.SetBit(2, false);
            }
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

                    CPU.Instance.RequestInterupt(eInterruptType.VBlank);

                    if(CheckInterrupt(eInterrupt.VerticalBlank))
                    {
                        CPU.Instance.RequestInterupt(eInterruptType.LCD);
                    }
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

                FetchState = eFetchState.Tile;
                LineX = 0;
                FetchX = 0;
                PushedX = 0;
                
            }

            // If this is the first tick,
            // go ahead and pull in OAM data
            // NOT IMPLEMENTED FOR NOW

        }

        public void DoPixelDraw()
        {
            // This length of this section of rendering will be depending on several factors
            // For now, we will assume it does nothing and just move straight to the next mode

            ProcessPipeline();

            // Keep in this state until we push a whole row of pixels.

            if(PushedX >= XRES)
            {
                BackgroundPixelFIFO.Reset();
                
                PPUMode = eMode.HorizontalBlank;

                if(CheckInterrupt(eInterrupt.HorizontalBlank))
                {
                    CPU.Instance.RequestInterupt(eInterruptType.LCD);
                }
            }

            
        }

        #endregion

        #region PixelFIFO

        public class PixelFIFOEntry
        {
            public PixelFIFOEntry? Next = null;
            public Color Value = Color.Transparent;

            public PixelFIFOEntry()
            {

            }

            public PixelFIFOEntry(Color color)
            {
                Value = color;
            }
        }

        public class PixelFIFO
        {
            public PixelFIFOEntry? Head = null;
            public PixelFIFOEntry? Tail = null;
            public int Size = 0;

            public void Push(Color color)
            {
                PixelFIFOEntry next = new PixelFIFOEntry(color);


                if (Head == null)
                {
                    Head = next;
                    Tail = next;
                }
                else
                {
                    Tail.Next = next;
                    Tail = next;
                }

                Size++;
            }

            public Color Pop()
            {
                Color popped = Head.Value;
                Head = Head.Next;
                Size--;

                return popped;
            }

            public void Reset()
            {
                while (Size > 0)
                    Pop();


            }
        }

        public PixelFIFO BackgroundPixelFIFO = new PixelFIFO();

        #endregion

        #region Pixel Fetcher

        public enum eFetchState
        {
            Tile,
            Data0,
            Data1,
            Sleep,
            Push
        }

        public eFetchState FetchState { get; set; } = eFetchState.Tile;

        public Byte MapY { get; set; } = 0;
        public Byte MapX { get; set; } = 0;
        public Byte TileY { get; set; } = 0;

        public Byte FetchX { get; set; } = 0;
        public Byte LineX { get; set; } = 0;
        public Byte PushedX { get; set; } = 0;

        public Byte[] BGFetchData { get; set; } = { 0, 0, 0 };

        public void ProcessPipeline()
        {
            MapY = LY + SCY;
            MapX = FetchX + SCX;
            TileY = ((LY + SCY) % 8) * 2;

            if ((LineTicks & 0b1) == 0)
            {
                PipelineFetch();
            }

            PipelinePushPixel();
        }

        public void PipelineFetch()
        {
            switch (FetchState)
            {
                case eFetchState.Tile:  ExecutePipelineFetchTile();  break;
                case eFetchState.Data0: ExecutePipelineFetchData0(); break;
                case eFetchState.Data1: ExecutePipelineFetchData1(); break;
                case eFetchState.Sleep: ExecutePipelineFetchSleep(); break;
                case eFetchState.Push:  ExecutePipelineFetchPush();  break;
                default:
                    break;
            }
        }

        public void ExecutePipelineFetchTile()
        {
            if(BGandWindowEnabled)
            {
                BGFetchData[0] = Bus.Read(BGTileMapArea + (MapX / 8) + ((MapY / 8) * 32));

                if (BGandWindowTileDataArea == 0x8800)
                {
                    BGFetchData[0] += 128;
                }
            }

            FetchState = eFetchState.Data0;
            FetchX += 8;
        }

        public void ExecutePipelineFetchData0()
        {
            BGFetchData[1] = Bus.Read(BGandWindowTileDataArea + (BGFetchData[0] * 16) + TileY);
            FetchState = eFetchState.Data1;
        }

        public void ExecutePipelineFetchData1()
        {
            BGFetchData[2] = Bus.Read(BGandWindowTileDataArea + (BGFetchData[0] * 16) + TileY + 1);
            FetchState = eFetchState.Sleep;
        }

        public void ExecutePipelineFetchSleep()
        {
            FetchState = eFetchState.Push;
        }

        public void ExecutePipelineFetchPush()
        {
            if(PipelineAdd())
            {
                FetchState = eFetchState.Tile;
            }
        }

        public bool PipelineAdd()
        {
            // First check if FIFO is full
            if(BackgroundPixelFIFO.Size > 8)
            {
                return false;
            }

            int x = FetchX - (8 - (SCX % 8));

            if(x >= 0)
            {
                for(int i = 0; i < 8; i++)
                {
                    int bit = 7 - i;

                    bool hiSet = BGFetchData[1].GetBit(bit);
                    bool loSet = BGFetchData[2].GetBit(bit);


                    Byte hi = (Byte)hiSet;
                    Byte lo = (Byte)loSet << 1;

                    Color color = BGColors[hi | lo];

                    BackgroundPixelFIFO.Push(color);
                }
            }

            return true;
        }

        public void PipelinePushPixel()
        {
            if(BackgroundPixelFIFO.Size > 8)
            {
                Color color = BackgroundPixelFIFO.Pop();

                if(LineX >= (SCX % 8))
                {
                    VideoBuffer[PushedX + (LY * XRES)] = color;

                    PushedX++;
                }

                LineX++;
            }
        }

        #endregion

        #region DMA

        public bool Active { get; set; } = false;
        public Byte CurrentByte { get; set; } = 0;

        public Byte Value { get; set; } = 0;

        public Byte StartDelay { get; set; } = 0;

        public void DMAStart(Byte startValue)
        {
            Active = true;
            CurrentByte = 0;
            Value = startValue;
            StartDelay = 2;
        }

        public void DMACycle()
        {
            if (!Active)
            {
                return;
            }

            if (StartDelay > 0)
            {
                StartDelay--;
                return;
            }

            Word readAddress = (Value * 0x100) + CurrentByte;
            Byte writeByte = Bus.Read(readAddress);
            Word writeAddress = 0xFE00 + CurrentByte;
            Bus.Write(writeAddress, writeByte);

            CurrentByte++;

            if (CurrentByte == 0xA0)
            {
                Active = false;
            }

        }

        #endregion

    }
}
