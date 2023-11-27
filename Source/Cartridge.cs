using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyEmulator
{
    public class HeaderInfo
    {
        Byte[] Entry = new Byte[4];
        Byte[] Logo = new Byte[0x30];

        char[] Title = new char[16];
        Word NewLicenseCode;
        Byte SGBFlag;
        Byte Type;
        Byte ROMSize;
        Byte RAMSize;
        Byte DestinationCode;
        Byte LicenseCode;
        Byte Version;
        Byte Checksum;
        Word GlobalChecksum;

        public HeaderInfo() { }

        public HeaderInfo(byte[] data)
        {
            int pos = 0;

            this.Entry[0] =   data[pos++];
            this.Entry[1] =   data[pos++];
            this.Entry[2] =   data[pos++];
            this.Entry[3] =   data[pos++];
            this.Logo[0x00] = data[pos++];
            this.Logo[0x01] = data[pos++];
            this.Logo[0x02] = data[pos++];
            this.Logo[0x03] = data[pos++];
            this.Logo[0x04] = data[pos++];
            this.Logo[0x05] = data[pos++];
            this.Logo[0x06] = data[pos++];
            this.Logo[0x07] = data[pos++];
            this.Logo[0x08] = data[pos++];
            this.Logo[0x09] = data[pos++];
            this.Logo[0x0A] = data[pos++];
            this.Logo[0x0B] = data[pos++];
            this.Logo[0x0C] = data[pos++];
            this.Logo[0x0D] = data[pos++];
            this.Logo[0x0E] = data[pos++];
            this.Logo[0x0F] = data[pos++];
            this.Logo[0x10] = data[pos++];
            this.Logo[0x11] = data[pos++];
            this.Logo[0x12] = data[pos++];
            this.Logo[0x13] = data[pos++];
            this.Logo[0x14] = data[pos++];
            this.Logo[0x15] = data[pos++];
            this.Logo[0x16] = data[pos++];
            this.Logo[0x17] = data[pos++];
            this.Logo[0x18] = data[pos++];
            this.Logo[0x19] = data[pos++];
            this.Logo[0x1A] = data[pos++];
            this.Logo[0x1B] = data[pos++];
            this.Logo[0x1C] = data[pos++];
            this.Logo[0x1D] = data[pos++];
            this.Logo[0x1E] = data[pos++];
            this.Logo[0x1F] = data[pos++];
            this.Logo[0x20] = data[pos++];
            this.Logo[0x21] = data[pos++];
            this.Logo[0x22] = data[pos++];
            this.Logo[0x23] = data[pos++];
            this.Logo[0x24] = data[pos++];
            this.Logo[0x25] = data[pos++];
            this.Logo[0x26] = data[pos++];
            this.Logo[0x27] = data[pos++];
            this.Logo[0x28] = data[pos++];
            this.Logo[0x29] = data[pos++];
            this.Logo[0x2A] = data[pos++];
            this.Logo[0x2B] = data[pos++];
            this.Logo[0x2C] = data[pos++];
            this.Logo[0x2D] = data[pos++];
            this.Logo[0x2E] = data[pos++];
            this.Logo[0x2F] = data[pos++];

            this.Title[0x0] = (char)data[pos++];
            this.Title[0x1] = (char)data[pos++];
            this.Title[0x2] = (char)data[pos++];
            this.Title[0x3] = (char)data[pos++];
            this.Title[0x4] = (char)data[pos++];
            this.Title[0x5] = (char)data[pos++];
            this.Title[0x6] = (char)data[pos++];
            this.Title[0x7] = (char)data[pos++];
            this.Title[0x8] = (char)data[pos++];
            this.Title[0x9] = (char)data[pos++];
            this.Title[0xA] = (char)data[pos++];
            this.Title[0xB] = (char)data[pos++];
            this.Title[0xC] = (char)data[pos++];
            this.Title[0xD] = (char)data[pos++];
            this.Title[0xE] = (char)data[pos++];
            this.Title[0xF] = (char)data[pos++];
            //this.Title[0xF] = '\0';
            //pos++;

            NewLicenseCode =  data[pos++] << 8 | data[pos++];
            SGBFlag =         data[pos++];
            Type =            data[pos++];
            ROMSize =         data[pos++];
            RAMSize =         data[pos++];
            DestinationCode = data[pos++];
            LicenseCode =     data[pos++];
            Version =         data[pos++];
            Checksum =        data[pos++];
            GlobalChecksum =  data[pos++] << 8 | data[pos++]; ;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            string TitleString = new string(this.Title);
            TitleString = TitleString.Substring(0, TitleString.IndexOf('\0'));
            sb.AppendLine("Title   : " + TitleString);
            sb.AppendLine("Type    : " + TypeString());
            sb.AppendLine("ROM Size: " + ROMSizeString());
            sb.AppendLine("RAM Size: " + RAMSizeString());
            sb.AppendLine("LIC Code: " + LicenseCode.ToHexString());
            sb.AppendLine("ROM Vers: " + Version.ToHexString());


            return sb.ToString();
        }

        private string TypeString()
        {
            switch (Type)
            {
                case 0x00: return "ROM Only";
                case 0x01: return "MBC1";
                case 0x02: return "MBC1+RAM";
                case 0x03: return "MBC1+RAM+BATTERY";
                case 0x05: return "MBC2";
                case 0x06: return "MBC2+Battery";
                case 0x08: return "ROM+RAM";
                case 0x09: return "ROM+RAM+BATTERY";
                case 0x0B: return "MMM01";
                case 0x0C: return "MMM01+RAM";
                case 0x0D: return "MMM01+RAM+BATTERY";
                case 0x0F: return "MBC3+TIMER+BATTERY";
                case 0x10: return "MBC3+TIMER+RAM+BATTERY";
                case 0x11: return "MBC3";
                case 0x12: return "MBC3+RAM";
                case 0x13: return "MBC3+RAM+BATTERY";
                case 0x19: return "MBC5";
                case 0x1A: return "MBC5+RAM";
                case 0x1B: return "MBC5+RAM+BATTERY";
                case 0x1C: return "MBC5+RUMBLE";
                case 0x1D: return "MBC5+RUMBLE+RAM";
                case 0x1E: return "MBC5+RUMBLE+RAM+BATTERY";
                case 0x20: return "MBC6";
                case 0x22: return "MBC7+SENSOR+RUMBLE+RAMA+BATTERY";
                default: return "Undefined";
            }
        }

        private string ROMSizeString()
        {
            switch (ROMSize)
            {
                case 0x00: return "32 KiB";
                case 0x01: return "64 KiB";
                case 0x02: return "128 KiB";
                case 0x03: return "256 KiB";
                case 0x04: return "512 KiB";
                case 0x05: return "1 MiB";
                case 0x06: return "2 MiB";
                case 0x07: return "4 MiB";
                case 0x08: return "8 MiB";
                case 0x52: return "1.1 KiB";
                case 0x53: return "1.2 KiB";
                case 0x54: return "1.5 KiB";
                default: return "Undefined";
            }
        }

        private string RAMSizeString()
        {
            switch (RAMSize)
            {
                case 0x00: return "0";
                case 0x01: return "-";
                case 0x02: return "8 KiB";
                case 0x03: return "32 KiB";
                case 0x04: return "128 KiB";
                case 0x05: return "64 KiB";
                default: return "Undefined";
            }
        }
    }

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

        public HeaderInfo Header = new HeaderInfo();

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

                byte[] headerData = new byte[80];
                Array.Copy(temp, 0x100, headerData, 0, headerData.Length);

                Header = new HeaderInfo(headerData);
                Logger.WriteLine(Environment.NewLine + Header.ToString(), Logger.LogLevel.Information);

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
                if (address >= 0x0000 && address <= 0x7FFF) { ROM[address] = value; return; }// Cartridge ROM
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
