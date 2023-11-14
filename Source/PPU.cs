﻿using System;
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
    }
}