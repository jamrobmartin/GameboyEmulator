using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyEmulator
{
    public class InternalROM
    {
        #region Singleton
        private static readonly Lazy<InternalROM> lazy = new Lazy<InternalROM>(() => new InternalROM());
        public static InternalROM Instance { get; private set; } = lazy.Value;
        #endregion

        public Byte Read(Word address)
        {
            throw new Exception("InternalROM - Tried to read memory location: " + address.ToHexString());
        }

        public void Write(Word address, Byte value)
        {

        }
    }
}
