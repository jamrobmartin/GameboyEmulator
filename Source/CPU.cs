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

        #region Registers
        // Registers
        public Byte A { get; set; }
        public Byte F { get; set; }
        public Word AF
        {
            get
            {
                return ((Word)A << 8) | (Word)F;
            }
            set
            {
                A = (value >> 8) & 0x00ff;
                F = value & 0x00ff;
            }
        }
        public bool ZFlag
        {
            get
            {
                return F & (Byte)0b10000000;
            }
            set
            {
                if (value)
                {
                    F = F | 0b10000000;
                }
                else
                {
                    F = F & ~0b10000000;
                }
            }
        }

        public bool NFlag
        {
            get
            {
                return F & (Byte)0b01000000;
            }
            set
            {
                if (value)
                {
                    F = F | 0b01000000;
                }
                else
                {
                    F = F & ~0b01000000;
                }
            }
        }

        public bool HFlag
        {
            get
            {
                return F & (Byte)0b00100000;
            }
            set
            {
                if (value)
                {
                    F = F | 0b00100000;
                }
                else
                {
                    F = F & ~0b00100000;
                }
            }
        }

        public bool CFlag
        {
            get
            {
                return F & (Byte)0b00010000;
            }
            set
            {
                if (value)
                {
                    F = F | 0b00010000;
                }
                else
                {
                    F = F & ~0b00010000;
                }
            }
        }

        public Byte B { get; set; }
        public Byte C { get; set; }
        public Word BC
        {
            get
            {
                return ((Word)B << 8) | (Word)C;
            }
            set
            {
                B = (value >> 8) & 0x00ff;
                C = value & 0x00ff;
            }
        }
        public Byte D { get; set; }
        public Byte E { get; set; }
        public Word DE
        {
            get
            {
                return ((Word)D << 8) | (Word)E;
            }
            set
            {
                D = (value >> 8) & 0x00ff;
                E = value & 0x00ff;
            }
        }
        public Byte H { get; set; }
        public Byte L { get; set; }
        public Word HL
        {
            get
            {
                return ((Word)H << 8) | (Word)L;
            }
            set
            {
                H = (value >> 8) & 0x00ff;
                L = value & 0x00ff;
            }
        }

        public Word SP { get; set; }
        public Word PC { get; set; }

        public Byte InstructionRegister { get; set; }

        public Instruction Instruction { get; set; }


        #endregion

        public CPU()
        {
            A = 0; F = 0;
            B = 0; C = 0;
            D = 0; E = 0;
            H = 0; L = 0;
            SP = 0; PC = 0;
            InstructionRegister = 0;
            Instruction = new Instruction();
        }


        public void Init()
        {
            PC = 0x0000;
            SP = 0xFFFE;
            AF = 0xB001;
            BC = 0x1300;
            DE = 0xD800;
            HL = 0x4D01;

            InstructionRegister = 0;
            Instruction = new Instruction();
        }

        public void Step()
        {
            FetchInstruction();

            FetchData();

            ExecuteInstruction();
        }

        public void FetchInstruction()
        {
            InstructionRegister = Bus.Read(PC++);
            Instruction = new Instruction(InstructionRegister);

        }

        public void FetchData()
        {

        }

        public void ExecuteInstruction()
        {

        }


    }
}
