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

        public Byte Read(Word address)
        {
            throw new Exception("PPU - Tried to read memory location: " + address.ToHexString());
        }

        public void Write(Word address, Byte value)
        {
            throw new Exception("PPU - Tried to Write memory location: " + address.ToHexString());
        }
    }
}
