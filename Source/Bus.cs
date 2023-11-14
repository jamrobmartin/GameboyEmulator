using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyEmulator
{
    public class Bus
    {
        #region Singleton
        private static readonly Lazy<Bus> lazy = new Lazy<Bus>(() => new Bus());
        public static Bus Instance { get; private set; } = lazy.Value;
        #endregion
    }
}
