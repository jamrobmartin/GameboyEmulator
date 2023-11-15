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

        public bool Inserted { get; set; } = false;

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

                FilePath = fileLocation;
                Inserted = true;
            }


            return Inserted;
        }


        public Byte Read(Word address)
        {
            if (Inserted)
            {
                return Data[address];
            }
            else
            {
                throw new Exception("Cartridge - Cartridge not Inserted");
            }

            
        }

        public void Write(Word address, Byte value)
        {
            throw new Exception("Cartridge - Tried to Write memory location: " + address.ToHexString());
        }
    }
}
