using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyEmulator
{
    public class CPU
    {
        #region Singleton
        private static readonly Lazy<CPU> lazy = new Lazy<CPU>(() => new CPU());
        public static CPU Instance { get; private set; } = lazy.Value;
        #endregion

    }
}
