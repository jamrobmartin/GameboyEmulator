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
    }
}
