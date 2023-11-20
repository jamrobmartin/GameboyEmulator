using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyEmulator
{
    public enum eInterruptType
    {
        VBlank = 1,
        LCD = 2,
        Timer = 4,
        Serial = 8,
        Joypad = 16
    }

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

        #region Interupts
        public bool InterruptMasterEnabled { get; set; } = false;
        public bool EnablingIME { get; set; } = false;

        public bool Halted { get; set; } = false;

        public Byte IE { get; set; } = 0x00;
        public Byte IF { get; set; } = 0x00;

        private void HandleInterupt(Word address)
        {
            Push16(PC);
            PC = address;
        }

        private bool CheckInterrupt(Word address, eInterruptType type)
        {
            Byte it = (byte)type;

            bool enabled = IE & it;
            bool flagged = IF & it;

            if (flagged && enabled)
            {
                HandleInterupt(address);
                IF &= ~it;
                Halted = false;
                InterruptMasterEnabled = false;

                return true;
            }

            return false;
        }

        public void HandleInterupts()
        {
            if (CheckInterrupt(0x40, eInterruptType.VBlank)) { }
            else if (CheckInterrupt(0x48, eInterruptType.LCD)) { }
            else if (CheckInterrupt(0x50, eInterruptType.Timer)) { }
            else if (CheckInterrupt(0x58, eInterruptType.Serial)) { }
            else if (CheckInterrupt(0x60, eInterruptType.Joypad)) { }
        }

        public void RequestInterupt(eInterruptType type) 
        {
            Byte it = (byte)type;

            IF |= it;
        }
        #endregion

        public CPU()
        {
            A = 0; F = 0;
            B = 0; C = 0;
            D = 0; E = 0;
            H = 0; L = 0;
            SP = 0; PC = 0;

            InterruptMasterEnabled = false;
            EnablingIME = false;
            Halted = false;
            IE = 0;
            IF = 0;

            InstructionRegister = 0;
            InstructionAddress = 0;
            Instruction = new Instruction();
        }


        public void Init()
        {
            PC = 0x0100;
            SP = 0xFFFE;
            AF = 0xB001;
            BC = 0x1300;
            DE = 0xD800;
            HL = 0x4D01;

            InterruptMasterEnabled = false;
            EnablingIME = false;
            Halted = false;
            IE = 0;
            IF = 0;

            InstructionRegister = 0;
            InstructionAddress = 0;
            Instruction = new Instruction();
        }

        public void PrintState(Logger.LogLevel level = Logger.LogLevel.Debug)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("AF:" + AF.ToHexString());
            sb.Append(' ');

            sb.Append("BC:" + BC.ToHexString());
            sb.Append(' ');

            sb.Append("DE:" + DE.ToHexString());
            sb.Append(' ');

            sb.Append("HL:" + HL.ToHexString());
            sb.Append(' ');

            sb.Append("SP:" + SP.ToHexString());
            sb.Append(' ');

            sb.Append("PC:" + PC.ToHexString());
            sb.Append(' ');

            Logger.WriteLine("[Status] " + sb.ToString(), level);


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

        public void SetRegister8(eRegisterType register, Byte value)
        {
            switch (register)
            {
                case eRegisterType.A: A = value; break;
                case eRegisterType.F: F = value; break;
                case eRegisterType.B: B = value; break;
                case eRegisterType.C: C = value; break;
                case eRegisterType.D: D = value; break;
                case eRegisterType.E: E = value; break;
                case eRegisterType.H: H = value; break;
                case eRegisterType.L: L = value; break;

                case eRegisterType.HL: Bus.Write(GetRegister(eRegisterType.HL), value); break;

                default:
                    break;
            }
        }

        public Byte GetRegister8(eRegisterType register)
        {
            Byte value = 0;
            switch (register)
            {
                case eRegisterType.A: value = A; break;
                case eRegisterType.F: value = F; break;
                case eRegisterType.B: value = B; break;
                case eRegisterType.C: value = C; break;
                case eRegisterType.D: value = D; break;
                case eRegisterType.E: value = E; break;
                case eRegisterType.H: value = H; break;
                case eRegisterType.L: value = L; break;

                case eRegisterType.HL: value = Bus.Read(GetRegister(eRegisterType.HL)); break;

                default:
                    break;
            }
            return value;
        }

        public void SetFlags(Byte z, Byte n, Byte h, Byte c)
        {
            Byte DontUpdate = (Byte)(-1);

            if (z != DontUpdate)
            {
                ZFlag = z;
            }

            if (n != DontUpdate)
            {
                NFlag = n;
            }

            if (h != DontUpdate)
            {
                HFlag = h;
            }

            if (c != DontUpdate)
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
            if(!Halted)
            {
                FetchInstruction();

                FetchData();

                InterpretInstruction();

                ExecuteInstruction();
            }
            else
            {
                Emulator.Instance.DoCycles(1);
                if(IF)
                {
                    Halted = false;
                }
                
            }
            

            if(InterruptMasterEnabled)
            {
                EnablingIME = false;
                HandleInterupts();
            }

            if(EnablingIME)
            {
                InterruptMasterEnabled = true;
            }
        }

        public void FetchInstruction()
        {
            InstructionAddress = PC;
            InstructionRegister = Bus.Read(PC++);
            Instruction = new Instruction(InstructionRegister);
            Emulator.Instance.DoCycles(1);

        }

        public void FetchData()
        {
            switch (Instruction.AddressingMode)
            {
                case eAddressingMode.Register_D8:
                    Instruction.Parameter = Bus.Read(PC++);
                    Emulator.Instance.DoCycles(1);
                    break;
                case eAddressingMode.MemoryRegister_D8:
                    Instruction.Parameter = Bus.Read(PC++);
                    Emulator.Instance.DoCycles(1);
                    break;
                case eAddressingMode.Register_A16:
                    Instruction.Parameter = Bus.Read(PC++);
                    Emulator.Instance.DoCycles(1);
                    Instruction.Parameter2 = Bus.Read(PC++);
                    Emulator.Instance.DoCycles(1);
                    break;
                case eAddressingMode.A16_Register:
                    Instruction.Parameter = Bus.Read(PC++);
                    Emulator.Instance.DoCycles(1);
                    Instruction.Parameter2 = Bus.Read(PC++);
                    Emulator.Instance.DoCycles(1);
                    break;

                case eAddressingMode.A8_Register:
                    Instruction.Parameter = Bus.Read(PC++);
                    Emulator.Instance.DoCycles(1);
                    break;
                case eAddressingMode.Register_A8:
                    Instruction.Parameter = Bus.Read(PC++);
                    Emulator.Instance.DoCycles(1);
                    break;
                case eAddressingMode.Register_D16:
                    Instruction.Parameter = Bus.Read(PC++);
                    Emulator.Instance.DoCycles(1);
                    Instruction.Parameter2 = Bus.Read(PC++);
                    Emulator.Instance.DoCycles(1);
                    break;
                case eAddressingMode.Register_R8:
                    Instruction.Parameter = Bus.Read(PC++);
                    Emulator.Instance.DoCycles(1);
                    break;
                case eAddressingMode.HL_SPR:
                    Instruction.Parameter = Bus.Read(PC++);
                    Emulator.Instance.DoCycles(1);
                    break;

                case eAddressingMode.D16:
                    Instruction.Parameter = Bus.Read(PC++);
                    Emulator.Instance.DoCycles(1);
                    Instruction.Parameter2 = Bus.Read(PC++);
                    Emulator.Instance.DoCycles(1);
                    break;

                case eAddressingMode.D8:
                    Instruction.Parameter = Bus.Read(PC++);
                    Emulator.Instance.DoCycles(1);
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

            // If an instruction hasn't been implemented yet, alert us
            if(Instruction.InstructionType == eInstructionType.None)
            {
                Logger.WriteLine("Instruction not implemented yet...", Logger.LogLevel.Error);
                Logger.WriteLine("PC:" + InstructionAddress.ToHexString() + " " + Instruction.ToString(), Logger.LogLevel.Error);
            }

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
                case eInstructionType.RET:
                    ExecuteInstructionRET();
                    break;
                case eInstructionType.RETI:
                    ExecuteInstructionRETI();
                    break;
                case eInstructionType.ADD:
                    ExecuteInstructionADD();
                    break;
                case eInstructionType.ADC:
                    ExecuteInstructionADC();
                    break;
                case eInstructionType.SUB:
                    ExecuteInstructionSUB();
                    break;
                case eInstructionType.SBC:
                    ExecuteInstructionSBC();
                    break;
                case eInstructionType.AND:
                    ExecuteInstructionAND();
                    break;
                case eInstructionType.XOR:
                    ExecuteInstructionXOR();
                    break;
                case eInstructionType.OR:
                    ExecuteInstructionOR();
                    break;
                case eInstructionType.CP:
                    ExecuteInstructionCP();
                    break;
                case eInstructionType.CB:
                    ExecuteInstructionCB();
                    break;
                case eInstructionType.INC:
                    ExecuteInstructionINC();
                    break;
                case eInstructionType.DEC:
                    ExecuteInstructionDEC();
                    break;
                case eInstructionType.DI:
                    ExecuteInstructionDI();
                    break;
                case eInstructionType.EI:
                    ExecuteInstructionEI();
                    break;
                case eInstructionType.POP:
                    ExecuteInstructionPOP();
                    break;
                case eInstructionType.PUSH:
                    ExecuteInstructionPUSH();
                    break;
                case eInstructionType.DAA:
                    ExecuteInstructionDAA();
                    break;
                case eInstructionType.CPL:
                    ExecuteInstructionCPL();
                    break;
                case eInstructionType.SCF:
                    ExecuteInstructionSCF();
                    break;
                case eInstructionType.CCF:
                    ExecuteInstructionCCF();
                    break;
                case eInstructionType.RLCA:
                    ExecuteInstructionRLCA();
                    break;
                case eInstructionType.RRCA:
                    ExecuteInstructionRRCA();
                    break;
                case eInstructionType.RLA:
                    ExecuteInstructionRLA();
                    break;
                case eInstructionType.RRA:
                    ExecuteInstructionRRA();
                    break;
                case eInstructionType.RST:
                    ExecuteInstructionRST();
                    break;
                case eInstructionType.HALT:
                    ExecuteInstructionHALT();
                    break;
                default:
                    break;
            }

            PrintState();
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
                Emulator.Instance.DoCycles(1);
            }
            else
            if (Instruction.AddressingMode == eAddressingMode.HLD_Register)
            {
                Bus.Write(Instruction.DestinationAddress, GetRegister(Instruction.Register2) & 0xFF);
                HL--;
                Emulator.Instance.DoCycles(1);
            }

            // If loading from autoincrement or autodecrement
            else
            if (Instruction.AddressingMode == eAddressingMode.Register_HLI)
            {
                SetRegister(Instruction.Register1, Bus.Read(Instruction.SourceAddress));
                HL++;
                Emulator.Instance.DoCycles(1);
            }
            else
            if (Instruction.AddressingMode == eAddressingMode.Register_HLD)
            {
                SetRegister(Instruction.Register1, Bus.Read(Instruction.SourceAddress));
                HL--;
                Emulator.Instance.DoCycles(1);
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
                        Emulator.Instance.DoCycles(1);
                    }
                    else
                    {
                        Bus.Write(Instruction.DestinationAddress, GetRegister(Instruction.Register2) & 0xFF);
                    }
                }
                else
                {
                    // If not a register, is must be a value
                    Bus.Write(Instruction.DestinationAddress, Instruction.Parameter);
                }

                Emulator.Instance.DoCycles(1);

            }

            // If we are loading from a memory location
            else 
            if(Instruction.SourceIsMemory)
            {
                // We are loading to a register
                SetRegister(Instruction.Register1, Bus.Read(Instruction.SourceAddress));
                Emulator.Instance.DoCycles(1);
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

            Emulator.Instance.DoCycles(1);
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
                
                Emulator.Instance.DoCycles(1);
            }
        }

        public void ExecuteInstructionJR()
        {
            if (CheckCondition())
            {
                SetRegister(eRegisterType.PC, GetRegister(eRegisterType.PC) + (sbyte)Instruction.Parameter );

                Emulator.Instance.DoCycles(1);
            }
        }

        public void ExecuteInstructionCALL()
        {
            // Call is the same as JP, but you push the contents of the PC to the Stack first.
            if (CheckCondition())
            {
                Push16(PC);
                Emulator.Instance.DoCycles(2);

                SetRegister(eRegisterType.PC, Instruction.Parameter | (Instruction.Parameter2 << 8));

                Emulator.Instance.DoCycles(1);
            }
        }

        public void ExecuteInstructionRET()
        {
            if (CheckCondition())
            {
                Word value = Pop16();
                Emulator.Instance.DoCycles(2);

                SetRegister(eRegisterType.PC, value);

                Emulator.Instance.DoCycles(1);
            }
        }

        public void ExecuteInstructionRETI()
        {
            InterruptMasterEnabled = true;
            ExecuteInstructionRET();
        }

        public void ExecuteInstructionADD()
        {
            if(Instruction.AddressingMode == eAddressingMode.Register_D8)
            {
                Byte param = Instruction.Parameter;
                Byte a = A;
                Word result = a + param;
                bool zFlag = (result & 0xFF) == 0;
                bool hFlag = (a & 0xF) + (param & 0xF) >= 0x10;
                bool cFlag = (a & 0xFF) + (param & 0xFF) >= 0x100;

                SetRegister(eRegisterType.A, result);
                SetFlags(zFlag, 0, hFlag, cFlag);
            }
            else
            if (Instruction.AddressingMode == eAddressingMode.Register_R8)
            {
                Byte param = Instruction.Parameter;
                Word sp = SP;
                Word result = sp + (sbyte)param;
                bool zFlag = false;
                bool hFlag = (sp & 0xF) + (param & 0xF) >= 0x10;
                bool cFlag = (sp & 0xFF) + (param & 0xFF) >= 0x100;

                SetRegister(eRegisterType.SP, result);
                SetFlags(zFlag, 0, hFlag, cFlag);
            }
            else
            if(Instruction.AddressingMode == eAddressingMode.Register_Register && Instruction.Register1 >= eRegisterType.AF)
            {
                Word param = GetRegister(Instruction.Register2);
                Word reg = GetRegister(Instruction.Register1);

                Word result = reg + param;

                bool hFlag = (reg & 0xFFF) + (param & 0xFFF) >= 0x1000;
                UInt32 n = (UInt32)reg + (UInt32)param;
                bool cFlag = n >= 0x10000;

                SetRegister(Instruction.Register1, result & 0xFFFF);
                SetFlags(-1, 0, hFlag, cFlag);
            }
            else
            {
                Byte param = GetRegister8(Instruction.Register2);
                Byte a = A;
                Word result = a + param;
                bool zFlag = (result & 0xFF) == 0;
                bool hFlag = (a & 0xF) + (param & 0xF) >= 0x10;
                bool cFlag = (a & 0xFF) + (param & 0xFF) >= 0x100;

                SetRegister(eRegisterType.A, result);
                SetFlags(zFlag, 0, hFlag, cFlag);

                if(Instruction.Register2 == eRegisterType.HL)
                {
                    Emulator.Instance.DoCycles(1);
                }
            }
        }

        public void ExecuteInstructionADC()
        {
            if (Instruction.AddressingMode == eAddressingMode.Register_D8)
            {
                Byte param = Instruction.Parameter;
                Byte a = A;
                Byte c = CFlag;

                Word result = (a + param + c) & 0xFF;
                bool zFlag = (result & 0xFF) == 0;
                bool hFlag = (a & 0xF) + (param & 0xF) + c >= 0x10;
                bool cFlag = (a & 0xFF) + (param & 0xFF) + c >= 0x100;

                SetRegister(eRegisterType.A, result);
                SetFlags(zFlag, 0, hFlag, cFlag);
            }
            else
            {
                Byte param = GetRegister8(Instruction.Register2);
                Byte a = A;
                Byte c = CFlag;

                Word result = (a + param + c) & 0xFF;
                bool zFlag = (result & 0xFF) == 0;
                bool hFlag = (a & 0xF) + (param & 0xF) + c >= 0x10;
                bool cFlag = (a & 0xFF) + (param & 0xFF) + c >= 0x100;

                SetRegister(eRegisterType.A, result);
                SetFlags(zFlag, 0, hFlag, cFlag);

                if (Instruction.Register2 == eRegisterType.HL)
                {
                    Emulator.Instance.DoCycles(1);
                }
            }
        }

        public void ExecuteInstructionSUB()
        {
            if (Instruction.AddressingMode == eAddressingMode.Register_D8)
            {
                Byte param = Instruction.Parameter;
                Byte a = A;
                Word result = a - param;
                bool zFlag = (result & 0xFF) == 0;
                bool hFlag = ((int)a & 0xF) - ((int)param & 0xF) < 0;
                bool cFlag = ((int)a & 0xFF) - ((int)param & 0xFF) < 0;

                SetRegister(eRegisterType.A, result);
                SetFlags(zFlag, 1, hFlag, cFlag);
            }
            else
            {
                Byte param = GetRegister8(Instruction.Register2);
                Byte a = A;
                Word result = a - param;
                bool zFlag = (result & 0xFF) == 0;
                bool hFlag = ((int)a & 0xF) - ((int)param & 0xF) < 0;
                bool cFlag = ((int)a & 0xFF) - ((int)param & 0xFF) < 0;

                SetRegister(eRegisterType.A, result);
                SetFlags(zFlag, 1, hFlag, cFlag);

                if (Instruction.Register2 == eRegisterType.HL)
                {
                    Emulator.Instance.DoCycles(1);
                }
            }
        }

        public void ExecuteInstructionSBC()
        {
            if (Instruction.AddressingMode == eAddressingMode.Register_D8)
            {
                Byte param = Instruction.Parameter;
                Byte a = A;
                Byte c = CFlag;

                Word result = (a - param - c) & 0xFF;
                bool zFlag = (result & 0xFF) == 0;
                bool hFlag = ((int)a & 0xF) - ((int)param & 0xF) - (int)c < 0;
                bool cFlag = ((int)a & 0xFF) - ((int)param & 0xFF) - (int)c < 0;

                SetRegister(eRegisterType.A, result);
                SetFlags(zFlag, 1, hFlag, cFlag);
            }
            else
            {
                Byte param = GetRegister8(Instruction.Register2);
                Byte a = A;
                Byte c = CFlag;

                Word result = (a - param - c) & 0xFF;
                bool zFlag = (result & 0xFF) == 0;
                bool hFlag = ((int)a & 0xF) - ((int)param & 0xF) - (int)c < 0;
                bool cFlag = ((int)a & 0xFF) - ((int)param & 0xFF) - (int)c < 0;

                SetRegister(eRegisterType.A, result);
                SetFlags(zFlag, 1, hFlag, cFlag);

                if (Instruction.Register2 == eRegisterType.HL)
                {
                    Emulator.Instance.DoCycles(1);
                }
            }
        }

        public void ExecuteInstructionAND()
        {
            if (Instruction.AddressingMode == eAddressingMode.Register_D8)
            {
                Byte param = Instruction.Parameter;
                Byte a = A;

                Byte result = a & param;

                SetRegister(eRegisterType.A, result);
                SetFlags(result == 0, 0, 1, 0);
            }
            else
            {
                Byte param = GetRegister8(Instruction.Register2);
                Byte a = A;

                Byte result = a & param;

                SetRegister(eRegisterType.A, result);
                SetFlags(result == 0, 0, 1, 0);

                if (Instruction.Register2 == eRegisterType.HL)
                {
                    Emulator.Instance.DoCycles(1);
                }
            }
        }

        public void ExecuteInstructionXOR()
        {
            if (Instruction.AddressingMode == eAddressingMode.Register_D8)
            {
                Byte param = Instruction.Parameter;
                Byte a = A;

                Byte result = a ^ param;

                SetRegister(eRegisterType.A, result);
                SetFlags(result == 0, 0, 0, 0);
            }
            else
            {
                Byte param = GetRegister8(Instruction.Register2);
                Byte a = A;

                Byte result = a ^ param;

                SetRegister(eRegisterType.A, result);
                SetFlags(result == 0, 0, 0, 0);

                if (Instruction.Register2 == eRegisterType.HL)
                {
                    Emulator.Instance.DoCycles(1);
                }
            }
        }

        public void ExecuteInstructionOR()
        {
            if (Instruction.AddressingMode == eAddressingMode.Register_D8)
            {
                Byte param = Instruction.Parameter;
                Byte a = A;

                Byte result = a | param;

                SetRegister(eRegisterType.A, result);
                SetFlags(result == 0, 0, 0, 0);
            }
            else
            {
                Byte param = GetRegister8(Instruction.Register2);
                Byte a = A;

                Byte result = a | param;

                SetRegister(eRegisterType.A, result);
                SetFlags(result == 0, 0, 0, 0);

                if (Instruction.Register2 == eRegisterType.HL)
                {
                    Emulator.Instance.DoCycles(1);
                }
            }
        }

        public void ExecuteInstructionCP() 
        {
            if (Instruction.AddressingMode == eAddressingMode.Register_D8)
            {
                Byte param = Instruction.Parameter;
                Byte a = A;

                int result = (int)a - (int)param;

                bool hFlag = ((int)a & 0xF) - ((int)param & 0xF) < 0;
                bool cFlag = result < 0;

                SetFlags(result == 0, 1, hFlag, cFlag);
            }
            else
            {
                Byte param = GetRegister8(Instruction.Register2);
                Byte a = A;

                int result = (int)a - (int)param;

                bool hFlag = ((int)a & 0xF) - ((int)param & 0xF) < 0;
                bool cFlag = result < 0;

                SetFlags(result == 0, 1, hFlag, cFlag);

                if (Instruction.Register2 == eRegisterType.HL)
                {
                    Emulator.Instance.DoCycles(1);
                }
            }
        }

        public eRegisterType DecodeReg(Byte reg)
        {
            if (reg == 0) return eRegisterType.B;
            if (reg == 1) return eRegisterType.C;
            if (reg == 2) return eRegisterType.D;
            if (reg == 3) return eRegisterType.E;
            if (reg == 4) return eRegisterType.H;
            if (reg == 5) return eRegisterType.L;
            if (reg == 6) return eRegisterType.HL;
            if (reg == 7) return eRegisterType.A;

            return eRegisterType.None;
        }

        public void ExecuteInstructionCB()
        {
            // CB Instructions perform BIT operations on register values.
            Byte op = Instruction.Parameter;

            eRegisterType reg = DecodeReg(op & 0b111);
            Byte bit = (op >> 3) & 0b111;
            Byte bitOp = (op >> 6) & 0b11;

            Byte regVal = GetRegister8(reg);

            // Always do one cycle
            Emulator.Instance.DoCycles(1);

            // If you had to read HL from Memory, do two cycles
            if(reg == eRegisterType.HL)
            {
                Emulator.Instance.DoCycles(2);
            }

            // Now execute the command
            switch (bitOp)
            {
                case 1:
                    // BIT
                    SetFlags((regVal & (1 << bit)) == 0, 0, 1, -1);
                    return;

                case 2:
                    // RST
                    regVal = regVal & ~(1 << bit);
                    SetRegister8(reg, regVal);
                    return;

                case 3:
                    // SET
                    regVal = regVal | (1 << bit);
                    SetRegister8(reg, regVal);
                    return;
            }

            Byte cFlag = CFlag;

            switch (bit)
            {
                case 0:
                    {
                        // RLC
                        Byte old = regVal;
                        regVal <<= 1;
                        regVal |= (old & 0x80) >> 7;

                        SetRegister8(reg, regVal);
                        SetFlags(!regVal, 0, 0, !!(Byte)(old & 0x80));
                    }
                    return;

                case 1:
                    {
                        // RRC
                        Byte old = regVal;
                        regVal >>= 1;
                        regVal |= (old << 7);

                        SetRegister8(reg, regVal);
                        SetFlags(!regVal, 0, 0, old & 1);
                    }
                    return;

                case 2:
                    {
                        // RL
                        Byte old = regVal;
                        regVal <<= 1;
                        regVal |= cFlag;

                        SetRegister8(reg, regVal);
                        SetFlags(!regVal, 0, 0, !!(Byte)(old & 0x80));
                    }
                    return;

                case 3:
                    {
                        // RR
                        Byte old = regVal;
                        regVal >>= 1;
                        regVal |= (cFlag << 7);

                        SetRegister8(reg, regVal);
                        SetFlags(!regVal, 0, 0, old & 1);
                    }
                    return;

                case 4:
                    {
                        // SLA
                        Byte old = regVal;
                        regVal <<= 1;

                        SetRegister8(reg, regVal);
                        SetFlags(!regVal, 0, 0, !!(Byte)(old & 0x80));
                    }
                    return;

                case 5:
                    {
                        // SRA
                        Byte old = regVal;
                        regVal  = (sbyte)regVal >> 1;

                        SetRegister8(reg, regVal);
                        SetFlags(!regVal, 0, 0, old & 1);
                    }
                    return;

                case 6:
                    {
                        // SWAP
                        regVal = ((regVal & 0xF0) >> 4) | ((regVal & 0xF) << 4);

                        SetRegister8(reg, regVal);
                        SetFlags(!regVal, 0, 0, 0);
                    }
                    return;

                case 7:
                    {
                        // SRL
                        Byte old = regVal;
                        regVal = regVal >> 1;

                        SetRegister8(reg, regVal);
                        SetFlags(!regVal, 0, 0, old & 1);
                    }
                    return;

            }

        }

        public void ExecuteInstructionINC()
        {
            // If incrementing a single Byte register
            if(Instruction.Register1 < eRegisterType.AF)
            {
                Byte value = (GetRegister(Instruction.Register1) + 1) & 0xFF;
                SetRegister(Instruction.Register1, value);
                SetFlags(value == 0, 0, (value & 0x0F) == 0, -1);
                return;
            }

            // If incrementing (HL)
            if(Instruction.AddressingMode == eAddressingMode.MemoryRegister)
            {
                Byte value = (Bus.Read(GetRegister(eRegisterType.HL)) + 1) & 0xFF;
                Bus.Write(GetRegister(eRegisterType.HL), value);
                SetFlags(value == 0, 0, (value & 0x0F) == 0, -1);
                Emulator.Instance.DoCycles(2);
                return;
            }

            // If incrementing a register pair
            if(Instruction.Register1 >= eRegisterType.AF)
            {
                Word value = GetRegister(Instruction.Register1) + 1;
                SetRegister(Instruction.Register1, value);
                Emulator.Instance.DoCycles(1);
                return;
            }
        }

        public void ExecuteInstructionDEC()
        {
            // If decrementing a single Byte register
            if (Instruction.Register1 < eRegisterType.AF)
            {
                Byte value = (GetRegister(Instruction.Register1) - 1) & 0xFF;
                SetRegister(Instruction.Register1, value);
                value = GetRegister8(Instruction.Register1);
                SetFlags(value == 0, 1, (value & 0x0F) == 0x0F, -1);
                return;
            }

            // If decrementing (HL)
            if (Instruction.AddressingMode == eAddressingMode.MemoryRegister)
            {
                Byte value = (Bus.Read(GetRegister(eRegisterType.HL)) - 1) & 0xFF;
                Bus.Write(GetRegister(eRegisterType.HL), value);
                SetFlags(value == 0, 1, (value & 0x0F) == 0x0F, -1);
                Emulator.Instance.DoCycles(2);
                return;
            }

            // If decrementing a register pair
            if (Instruction.Register1 >= eRegisterType.AF)
            {
                Word value = GetRegister(Instruction.Register1) - 1;
                SetRegister(Instruction.Register1, value);
                Emulator.Instance.DoCycles(1);
                return;
            }
        }

        public void ExecuteInstructionDI()
        {
            InterruptMasterEnabled = false;
        }

        public void ExecuteInstructionEI()
        {
            // You need to go through a CPU step before you can start handling interupts
            EnablingIME = true;
        }

        public void ExecuteInstructionPOP()
        {
            Word value = Pop16();

            if(Instruction.Register1 == eRegisterType.AF)
            {
                value = value & 0xFFF0;
            }

            SetRegister(Instruction.Register1, value);
            
            Emulator.Instance.DoCycles(2);
        }

        public void ExecuteInstructionPUSH()
        {
            Word value = GetRegister(Instruction.Register1);

            Push16(value);
            Emulator.Instance.DoCycles(3);

        }

        public void ExecuteInstructionDAA()
        {
            Byte u = 0;
            Byte fc = 0;

            if(HFlag || (!NFlag && (A & 0xF) > 9))
            {
                u = 6;
            }

            if (CFlag || (!NFlag && A  > 0x99))
            {
                u |= 0x60;
                fc = 1;
            }

            A += NFlag ? 0-u : u;

            SetFlags(A == 0, -1, 0, fc);

        }

        public void ExecuteInstructionCPL()
        {
            A = ~A;
            SetFlags(-1, 1, 1, -1);
        }

        public void ExecuteInstructionSCF()
        {
            SetFlags(-1, 0, 0, 1);
        }

        public void ExecuteInstructionCCF() 
        {
            SetFlags(-1, 0, 0, (Byte)CFlag ^ 1);
        }

        public void ExecuteInstructionRLCA()
        {
            Byte u = A;
            Byte c = (u >> 7) & 1;
            u = (u << 1) | c;
            A = u;
            SetFlags(0, 0, 0, c);
        }

        public void ExecuteInstructionRRCA()
        {
            Byte b = A & 1;
            A >>= 1;
            A |= (b << 7);

            SetFlags(0, 0, 0, b);
        }

        public void ExecuteInstructionRLA()
        {
            Byte u = A;
            Byte cf = CFlag;
            Byte c = (u >> 7) & 1;

            A = (u << 1) | cf;
            SetFlags(0, 0, 0, c);
        }

        public void ExecuteInstructionRRA()
        {
            Byte c = CFlag;
            Byte newC = A & 1;

            A >>= 1;
            A |= (c << 7);

            SetFlags(0, 0, 0, newC);
        }

        public void ExecuteInstructionRST()
        {
            Push16(PC);
            Emulator.Instance.DoCycles(2);

            SetRegister(eRegisterType.PC, Instruction.Parameter);

            Emulator.Instance.DoCycles(1);
        }

        public void ExecuteInstructionHALT()
        {
            Halted = true;
        }
        #endregion

    }
}
