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

        public byte Read(Word address)
        {
            throw new Exception("PPU - Tried to read memory location: " + address.ToHexString());
        }

        public void Write(Word address, byte value)
        {

        }
    }
}
