using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyEmulator
{
    public class Cartridge
    {
        #region Singleton
        private static readonly Lazy<Cartridge> lazy = new Lazy<Cartridge>(() => new Cartridge());
        public static Cartridge Instance { get; private set; } = lazy.Value;
        #endregion

        public String FilePath { get; set; } = string.Empty;

        public Byte[] Data = new Byte[1024];
        public Byte[] ROM = new Byte[0x8000];
        public Byte[] RAM = new Byte[0x2000];

        public bool Inserted { get; set; } = false;

        public Cartridge()
        {
            for (int i = 0; i < ROM.Length; i++)
            {
                ROM[i] = 0;
            }

            for (int i = 0; i < RAM.Length; i++)
            {
                RAM[i] = 0;
            }
        }

        public bool InsertCartridge(string fileLocation)
        {
            // First check if the file even exists
            if (File.Exists(fileLocation))
            {
                // If it does exist, try to load it.
                FileInfo cartfile = new FileInfo(fileLocation);
                int size = (int)cartfile.Length;
                Data = new Byte[size];
                byte[] temp = new byte[size];

                FileStream stream = cartfile.OpenRead();
                
                stream.Read(temp, 0, size);

                for (int i = 0; i < size; i++)
                {
                    Data[i] = temp[i];
                }

                MapInitialROM();

                FilePath = fileLocation;
                Inserted = true;
            }


            return Inserted;
        }

        public void MapInitialROM()
        {
            // Write up to the first 0x8000 bytes to ROM
            int size = 0x8000;
            if(Data.Length < size)
            {
                size = Data.Length;
            }

            for (int i = 0; i < size; i++)
            {
                ROM[i] = Data[i];
            }
        }


        public Byte Read(Word address)
        {
            if (Inserted)
            {
                if (address >= 0x0000 && address <= 0x7FFF) { return ROM[address]; }
                if (address >= 0xA000 && address <= 0xBFFF) { return RAM[address - 0xA000]; }

                throw new Exception("Cartridge - Tried to Read memory location: " + address.ToHexString());
            }
            else
            {
                throw new Exception("Cartridge - Cartridge not Inserted");
            }

        }

        public void Write(Word address, Byte value)
        {
            if (Inserted)
            {
                if (address >= 0xA000 && address <= 0xBFFF) { RAM[address - 0xA000] = value; return; }

                throw new Exception("Cartridge - Tried to Write memory location: " + address.ToHexString());
            }
            else
            {
                throw new Exception("Cartridge - Cartridge not Inserted");
            }
        }
    }
}
