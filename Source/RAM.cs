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

        public Byte Read(Word address)
        {
            throw new Exception("RAM - Tried to read memory location: " + address.ToHexString());
        }

        public void Write(Word address, Byte value)
        {

        }
    }
}
