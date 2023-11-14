using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyEmulator
{
    public class APU
    {
        #region Singleton
        private static readonly Lazy<APU> lazy = new Lazy<APU>(() => new APU());
        public static APU Instance { get; private set; } = lazy.Value;
        #endregion
    }
}
