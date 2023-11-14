using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyEmulator
{
    public class IO
    {
        #region Singleton
        private static readonly Lazy<IO> lazy = new Lazy<IO>(() => new IO());
        public static IO Instance { get; private set; } = lazy.Value;
        #endregion
    }
}
