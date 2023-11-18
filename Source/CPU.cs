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

        public Word InstructionAddress { get; set; }


        #endregion

        public CPU()
        {
            A = 0; F = 0;
            B = 0; C = 0;
            D = 0; E = 0;
            H = 0; L = 0;
            SP = 0; PC = 0;
            InstructionRegister = 0;
            InstructionAddress = 0;
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
            InstructionAddress = 0;
            Instruction = new Instruction();
        }

        #region CPU Helpers
        public void SetRegister(eRegisterType register, Word value)
        {
            switch (register)
            {
                case eRegisterType.A: A = value & 0xFF; break;
                case eRegisterType.F: F = value & 0xFF; break;
                case eRegisterType.B: B = value & 0xFF; break;
                case eRegisterType.C: C = value & 0xFF; break;
                case eRegisterType.D: D = value & 0xFF; break;
                case eRegisterType.E: E = value & 0xFF; break;
                case eRegisterType.H: H = value & 0xFF; break;
                case eRegisterType.L: L = value & 0xFF; break;
                case eRegisterType.AF: AF = value; break;
                case eRegisterType.BC: BC = value; break;
                case eRegisterType.DE: DE = value; break;
                case eRegisterType.HL: HL = value; break;
                case eRegisterType.SP: SP = value; break;
                case eRegisterType.PC: PC = value; break;
                default:
                    break;
            }
        }

        public Word GetRegister(eRegisterType register)
        {
            Word value = 0;
            switch (register)
            {
                case eRegisterType.A:  value = A; break;
                case eRegisterType.F:  value = F; break;
                case eRegisterType.B:  value = B; break;
                case eRegisterType.C:  value = C; break;
                case eRegisterType.D:  value = D; break;
                case eRegisterType.E:  value = E; break;
                case eRegisterType.H:  value = H; break;
                case eRegisterType.L:  value = L; break;
                case eRegisterType.AF: value = AF; break;
                case eRegisterType.BC: value = BC; break;
                case eRegisterType.DE: value = DE; break;
                case eRegisterType.HL: value = HL; break;
                case eRegisterType.SP: value = SP; break;
                case eRegisterType.PC: value = PC; break;
                default:
                    break;
            }
            return value;
        }

        public void SetFlags(Byte z, Byte n, Byte h, Byte c)
        {
            if (z != -1)
            {
                ZFlag = z;
            }

            if (n != -1)
            {
                NFlag = n;
            }

            if (h != -1)
            {
                HFlag = h;
            }

            if (c != -1)
            {
                CFlag = c;
            }
        }

        public void Push(Byte value)
        {
            SP--;
            Bus.Write(SP, value);
        }

        public void Push16(Word value)
        {
            Push((value >> 8) & 0xFF);
            Push(value & 0xFF);
        }

        public Byte Pop() 
        {
            return Bus.Read(SP++);
        }

        public Word Pop16()
        {
            Byte lo = Pop();
            Word hi = Pop();

            return (hi << 8) | lo;
        }
        #endregion

        #region CPU Step
        public void Step()
        {
            FetchInstruction();

            FetchData();

            InterpretInstruction();

            ExecuteInstruction();
        }

        public void FetchInstruction()
        {
            InstructionAddress = PC;
            InstructionRegister = Bus.Read(PC++);
            Instruction = new Instruction(InstructionRegister);

        }

        public void FetchData()
        {
            switch (Instruction.AddressingMode)
            {
                case eAddressingMode.Register_D8:
                    Instruction.Parameter = Bus.Read(PC++);
                    // Cycle
                    break;
                case eAddressingMode.MemoryRegister_D8:
                    Instruction.Parameter = Bus.Read(PC++);
                    // Cycle
                    break;
                case eAddressingMode.Register_A16:
                    Instruction.Parameter = Bus.Read(PC++);
                    // Cycle
                    Instruction.Parameter2 = Bus.Read(PC++);
                    // Cycle
                    break;
                case eAddressingMode.A16_Register:
                    Instruction.Parameter = Bus.Read(PC++);
                    // Cycle
                    Instruction.Parameter2 = Bus.Read(PC++);
                    // Cycle
                    break;

                case eAddressingMode.A8_Register:
                    Instruction.Parameter = Bus.Read(PC++);
                    // Cycle
                    break;
                case eAddressingMode.Register_A8:
                    Instruction.Parameter = Bus.Read(PC++);
                    // Cycle
                    break;
                case eAddressingMode.Register_D16:
                    Instruction.Parameter = Bus.Read(PC++);
                    // Cycle
                    Instruction.Parameter2 = Bus.Read(PC++);
                    // Cycle
                    break;
                case eAddressingMode.HL_SPR:
                    Instruction.Parameter = Bus.Read(PC++);
                    // Cycle
                    break;

                case eAddressingMode.D16:
                    Instruction.Parameter = Bus.Read(PC++);
                    // Cycle
                    Instruction.Parameter2 = Bus.Read(PC++);
                    // Cycle
                    break;

                case eAddressingMode.D8:
                    Instruction.Parameter = Bus.Read(PC++);
                    // Cycle
                    break;

                default:
                    break;
            }
        }

        public void InterpretInstruction()
        {
            switch (Instruction.AddressingMode)
            {
                case eAddressingMode.MemoryRegister_Register:
                    Instruction.DestinationIsMemory = true;
                    Instruction.DestinationAddress = GetRegister(Instruction.Register1);

                    if(Instruction.Register1 == eRegisterType.C)
                    {
                        Instruction.DestinationAddress |= 0xFF00;
                    }

                    break;

                case eAddressingMode.Register_MemoryRegister:
                    Instruction.SourceIsMemory = true;
                    Instruction.SourceAddress = GetRegister(Instruction.Register2);

                    if (Instruction.Register2 == eRegisterType.C)
                    {
                        Instruction.SourceAddress |= 0xFF00;
                    }

                    break;

                case eAddressingMode.HLI_Register:
                    Instruction.DestinationIsMemory = true;
                    Instruction.DestinationAddress = GetRegister(Instruction.Register1);
                    break;
                case eAddressingMode.HLD_Register:
                    Instruction.DestinationIsMemory = true;
                    Instruction.DestinationAddress = GetRegister(Instruction.Register1);
                    break;

                case eAddressingMode.Register_HLI:
                    Instruction.SourceIsMemory = true;
                    Instruction.SourceAddress = GetRegister(Instruction.Register2);
                    break;
                case eAddressingMode.Register_HLD:
                    Instruction.SourceIsMemory = true;
                    Instruction.SourceAddress = GetRegister(Instruction.Register2);
                    break;

                case eAddressingMode.A8_Register:
                    Instruction.DestinationIsMemory = true;
                    Instruction.DestinationAddress = Instruction.Parameter | 0xFF00;
                    break;

                case eAddressingMode.Register_A8:
                    Instruction.SourceIsMemory = true;
                    Instruction.SourceAddress = Instruction.Parameter | 0xFF00;
                    break;

                case eAddressingMode.Register_A16:
                    Instruction.SourceIsMemory = true;
                    Instruction.SourceAddress = Instruction.Parameter | (Instruction.Parameter2 << 8);
                    break;

                case eAddressingMode.A16_Register:
                    Instruction.DestinationIsMemory = true;
                    Instruction.DestinationAddress = Instruction.Parameter | (Instruction.Parameter2 << 8);
                    break;

                case eAddressingMode.MemoryRegister_D8:
                    Instruction.DestinationIsMemory = true;
                    Instruction.DestinationAddress = GetRegister(Instruction.Register1);
                    break;
            }
        }

        public void ExecuteInstruction()
        {
            Logger.WriteLine("PC:" + InstructionAddress.ToHexString() + " " + Instruction.ToString(), Logger.LogLevel.Debug);

            switch (Instruction.InstructionType)
            {
                case eInstructionType.None:
                    break;
                case eInstructionType.LD:
                    ExecuteInstructionLD(); 
                    break;
                case eInstructionType.LDH:
                    ExecuteInstructionLDH();
                    break;
                case eInstructionType.JP:
                    ExecuteInstructionJP();
                    break;
                case eInstructionType.JR:
                    ExecuteInstructionJR();
                    break;
                case eInstructionType.CALL:
                    ExecuteInstructionCALL();
                    break;
                default:
                    break;
            }
        }

        public void ExecuteInstructionLD()
        {
            // If loading from one register to another
            if (Instruction.AddressingMode == eAddressingMode.Register_Register)
            {
                SetRegister(Instruction.Register1, GetRegister(Instruction.Register2));
            }

            // If loading a register directly with a value
            else 
            if (Instruction.AddressingMode == eAddressingMode.Register_D8)
            {
                SetRegister(Instruction.Register1, Instruction.Parameter);
            }
            else
            if (Instruction.AddressingMode == eAddressingMode.Register_D16)
            {
                SetRegister(Instruction.Register1, Instruction.Parameter | (Instruction.Parameter2 << 8));
            }

            // If loading to autoincrment or autodecrement
            else 
            if (Instruction.AddressingMode == eAddressingMode.HLI_Register)
            {
                Bus.Write(Instruction.DestinationAddress, GetRegister(Instruction.Register2) & 0xFF);
                HL++;
                // Cycle
            }
            else
            if (Instruction.AddressingMode == eAddressingMode.HLD_Register)
            {
                Bus.Write(Instruction.DestinationAddress, GetRegister(Instruction.Register2) & 0xFF);
                HL--;
                // Cycle
            }

            // If loading from autoincrement or autodecrement
            else
            if (Instruction.AddressingMode == eAddressingMode.Register_HLI)
            {
                SetRegister(Instruction.Register1, Bus.Read(Instruction.SourceAddress));
                HL++;
                // Cycle
            }
            else
            if (Instruction.AddressingMode == eAddressingMode.Register_HLD)
            {
                SetRegister(Instruction.Register1, Bus.Read(Instruction.SourceAddress));
                HL--;
                // Cycle
            }

            // If we are loading to a memory location
            else 
            if(Instruction.DestinationIsMemory)
            {
                // Figure out if our source is a register
                if(Instruction.Register2 != eRegisterType.None)
                {
                    // Check if the register is one or two bytes
                    if(Instruction.Register2 >= eRegisterType.AF)
                    {
                        Bus.Write16(Instruction.DestinationAddress, GetRegister(Instruction.Register2));
                        // Cycle
                    }
                    else
                    {
                        Bus.Write(Instruction.DestinationAddress, GetRegister(Instruction.Register2) & 0xFF);
                    }
                }

                // If not a register, is must be a value
                Bus.Write(Instruction.DestinationAddress, Instruction.Parameter);

            }

            // If we are loading from a memory location
            else 
            if(Instruction.SourceIsMemory)
            {
                // We are loading to a register
                SetRegister(Instruction.Register1, Bus.Read(Instruction.SourceAddress));
                // Cycle
            }

            // If this is the special case 0xF8, HL_SPR
            else
            if(Instruction.AddressingMode == eAddressingMode.HL_SPR)
            {
                bool hflag = (SP & 0xF) + (Instruction.Parameter & 0xF) >= 0x10;

                bool Cflag = (SP & 0xFF) + (Instruction.Parameter & 0xFF) >= 0x100;

                SetFlags(0,0,hflag, Cflag);
                SetRegister(eRegisterType.HL, GetRegister(Instruction.Register2) + (sbyte)Instruction.Parameter);
            }

            // Cycle
        }

        public void ExecuteInstructionLDH()
        {
            // Only 0xE0 and 0xF0
            if(Instruction.Register1 == eRegisterType.A)
            {
                SetRegister(Instruction.Register1, Bus.Read(Instruction.SourceAddress));
            }
            else
            {
                Bus.Write(Instruction.DestinationAddress, A);
            }

            // Cycle
        }

        private bool CheckCondition()
        {
            bool proceed = false;
            switch (Instruction.ConditionType)
            {
                case eConditionType.None:
                    proceed = true;
                    break;
                case eConditionType.NZ:
                    proceed = !ZFlag;
                    break;
                case eConditionType.Z:
                    proceed = ZFlag;
                    break;
                case eConditionType.NC:
                    proceed = !CFlag;
                    break;
                case eConditionType.C:
                    proceed = CFlag;
                    break;
                default:
                    break;
            }

            return proceed;
        }

        public void ExecuteInstructionJP()
        {
            if(CheckCondition())
            {
                if(Instruction.Register1 == eRegisterType.HL)
                {
                    SetRegister(eRegisterType.PC, GetRegister(eRegisterType.HL));
                }
                else
                {
                    SetRegister(eRegisterType.PC, Instruction.Parameter | (Instruction.Parameter2 << 8));
                }
                
                // Cycle
            }
        }

        public void ExecuteInstructionJR()
        {
            if (CheckCondition())
            {
                SetRegister(eRegisterType.PC, GetRegister(eRegisterType.PC) + (sbyte)Instruction.Parameter );

                // Cycle
            }
        }

        public void ExecuteInstructionCALL()
        {
            // Call is the same as JP, but you push the contents of the PC to the Stack first.
            if (CheckCondition())
            {
                Push16(PC);

                SetRegister(eRegisterType.PC, Instruction.Parameter | (Instruction.Parameter2 << 8));

                // Cycle
            }
        }
        #endregion

    }
}
