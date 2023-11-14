using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyEmulator
{
    public class Timer
    {
        #region Singleton
        private static readonly Lazy<Timer> lazy = new Lazy<Timer>(() => new Timer());
        public static Timer Instance { get; private set; } = lazy.Value;
        #endregion
    }
}
