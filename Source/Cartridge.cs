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

        public byte Read(Word address)
        {
            throw new Exception("Cartridge - Tried to read memory location: " + address.ToHexString());
        }

        public void Write(Word address, byte value)
        {

        }
    }
}
