using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyEmulator
{
    public enum eInstructionType
    {
        None
    }

    public enum eAddressingMode
    {
        None
    }

    public enum eRegisterType
    {
        None
    }

    public enum eConditionType
    {
        None
    }

    public class Instruction
    {
        Byte OpCode = 0xDD;
        eInstructionType InstructionType = eInstructionType.None;
        eAddressingMode AddressingMode = eAddressingMode.None;
        eRegisterType Register1 = eRegisterType.None;
        eRegisterType Register2 = eRegisterType.None;
        eConditionType ConditionType = eConditionType.None;
        Byte Parameter = 0;

        public Instruction() { }

        public Instruction(Byte opcode) 
        {
            OpCode = opcode;
            SetUpInstruction(opcode);
        }

        public void SetUpInstruction(Byte opcode)
        {
            switch (opcode)
            {
                case 0x00: SetUpInstruction(eInstructionType.None); break;
                case 0x01: SetUpInstruction(eInstructionType.None); break;
                case 0x02: SetUpInstruction(eInstructionType.None); break;
                case 0x03: SetUpInstruction(eInstructionType.None); break;
                case 0x04: SetUpInstruction(eInstructionType.None); break;
                case 0x05: SetUpInstruction(eInstructionType.None); break;
                case 0x06: SetUpInstruction(eInstructionType.None); break;
                case 0x07: SetUpInstruction(eInstructionType.None); break;
                case 0x08: SetUpInstruction(eInstructionType.None); break;
                case 0x09: SetUpInstruction(eInstructionType.None); break;
                case 0x0A: SetUpInstruction(eInstructionType.None); break;
                case 0x0B: SetUpInstruction(eInstructionType.None); break;
                case 0x0C: SetUpInstruction(eInstructionType.None); break;
                case 0x0D: SetUpInstruction(eInstructionType.None); break;
                case 0x0E: SetUpInstruction(eInstructionType.None); break;
                case 0x0F: SetUpInstruction(eInstructionType.None); break;

                case 0x10: SetUpInstruction(eInstructionType.None); break;
                case 0x11: SetUpInstruction(eInstructionType.None); break;
                case 0x12: SetUpInstruction(eInstructionType.None); break;
                case 0x13: SetUpInstruction(eInstructionType.None); break;
                case 0x14: SetUpInstruction(eInstructionType.None); break;
                case 0x15: SetUpInstruction(eInstructionType.None); break;
                case 0x16: SetUpInstruction(eInstructionType.None); break;
                case 0x17: SetUpInstruction(eInstructionType.None); break;
                case 0x18: SetUpInstruction(eInstructionType.None); break;
                case 0x19: SetUpInstruction(eInstructionType.None); break;
                case 0x1A: SetUpInstruction(eInstructionType.None); break;
                case 0x1B: SetUpInstruction(eInstructionType.None); break;
                case 0x1C: SetUpInstruction(eInstructionType.None); break;
                case 0x1D: SetUpInstruction(eInstructionType.None); break;
                case 0x1E: SetUpInstruction(eInstructionType.None); break;
                case 0x1F: SetUpInstruction(eInstructionType.None); break;

                case 0x20: SetUpInstruction(eInstructionType.None); break;
                case 0x21: SetUpInstruction(eInstructionType.None); break;
                case 0x22: SetUpInstruction(eInstructionType.None); break;
                case 0x23: SetUpInstruction(eInstructionType.None); break;
                case 0x24: SetUpInstruction(eInstructionType.None); break;
                case 0x25: SetUpInstruction(eInstructionType.None); break;
                case 0x26: SetUpInstruction(eInstructionType.None); break;
                case 0x27: SetUpInstruction(eInstructionType.None); break;
                case 0x28: SetUpInstruction(eInstructionType.None); break;
                case 0x29: SetUpInstruction(eInstructionType.None); break;
                case 0x2A: SetUpInstruction(eInstructionType.None); break;
                case 0x2B: SetUpInstruction(eInstructionType.None); break;
                case 0x2C: SetUpInstruction(eInstructionType.None); break;
                case 0x2D: SetUpInstruction(eInstructionType.None); break;
                case 0x2E: SetUpInstruction(eInstructionType.None); break;
                case 0x2F: SetUpInstruction(eInstructionType.None); break;

                case 0x30: SetUpInstruction(eInstructionType.None); break;
                case 0x31: SetUpInstruction(eInstructionType.None); break;
                case 0x32: SetUpInstruction(eInstructionType.None); break;
                case 0x33: SetUpInstruction(eInstructionType.None); break;
                case 0x34: SetUpInstruction(eInstructionType.None); break;
                case 0x35: SetUpInstruction(eInstructionType.None); break;
                case 0x36: SetUpInstruction(eInstructionType.None); break;
                case 0x37: SetUpInstruction(eInstructionType.None); break;
                case 0x38: SetUpInstruction(eInstructionType.None); break;
                case 0x39: SetUpInstruction(eInstructionType.None); break;
                case 0x3A: SetUpInstruction(eInstructionType.None); break;
                case 0x3B: SetUpInstruction(eInstructionType.None); break;
                case 0x3C: SetUpInstruction(eInstructionType.None); break;
                case 0x3D: SetUpInstruction(eInstructionType.None); break;
                case 0x3E: SetUpInstruction(eInstructionType.None); break;
                case 0x3F: SetUpInstruction(eInstructionType.None); break;

                case 0x40: SetUpInstruction(eInstructionType.None); break;
                case 0x41: SetUpInstruction(eInstructionType.None); break;
                case 0x42: SetUpInstruction(eInstructionType.None); break;
                case 0x43: SetUpInstruction(eInstructionType.None); break;
                case 0x44: SetUpInstruction(eInstructionType.None); break;
                case 0x45: SetUpInstruction(eInstructionType.None); break;
                case 0x46: SetUpInstruction(eInstructionType.None); break;
                case 0x47: SetUpInstruction(eInstructionType.None); break;
                case 0x48: SetUpInstruction(eInstructionType.None); break;
                case 0x49: SetUpInstruction(eInstructionType.None); break;
                case 0x4A: SetUpInstruction(eInstructionType.None); break;
                case 0x4B: SetUpInstruction(eInstructionType.None); break;
                case 0x4C: SetUpInstruction(eInstructionType.None); break;
                case 0x4D: SetUpInstruction(eInstructionType.None); break;
                case 0x4E: SetUpInstruction(eInstructionType.None); break;
                case 0x4F: SetUpInstruction(eInstructionType.None); break;

                case 0x50: SetUpInstruction(eInstructionType.None); break;
                case 0x51: SetUpInstruction(eInstructionType.None); break;
                case 0x52: SetUpInstruction(eInstructionType.None); break;
                case 0x53: SetUpInstruction(eInstructionType.None); break;
                case 0x54: SetUpInstruction(eInstructionType.None); break;
                case 0x55: SetUpInstruction(eInstructionType.None); break;
                case 0x56: SetUpInstruction(eInstructionType.None); break;
                case 0x57: SetUpInstruction(eInstructionType.None); break;
                case 0x58: SetUpInstruction(eInstructionType.None); break;
                case 0x59: SetUpInstruction(eInstructionType.None); break;
                case 0x5A: SetUpInstruction(eInstructionType.None); break;
                case 0x5B: SetUpInstruction(eInstructionType.None); break;
                case 0x5C: SetUpInstruction(eInstructionType.None); break;
                case 0x5D: SetUpInstruction(eInstructionType.None); break;
                case 0x5E: SetUpInstruction(eInstructionType.None); break;
                case 0x5F: SetUpInstruction(eInstructionType.None); break;

                case 0x60: SetUpInstruction(eInstructionType.None); break;
                case 0x61: SetUpInstruction(eInstructionType.None); break;
                case 0x62: SetUpInstruction(eInstructionType.None); break;
                case 0x63: SetUpInstruction(eInstructionType.None); break;
                case 0x64: SetUpInstruction(eInstructionType.None); break;
                case 0x65: SetUpInstruction(eInstructionType.None); break;
                case 0x66: SetUpInstruction(eInstructionType.None); break;
                case 0x67: SetUpInstruction(eInstructionType.None); break;
                case 0x68: SetUpInstruction(eInstructionType.None); break;
                case 0x69: SetUpInstruction(eInstructionType.None); break;
                case 0x6A: SetUpInstruction(eInstructionType.None); break;
                case 0x6B: SetUpInstruction(eInstructionType.None); break;
                case 0x6C: SetUpInstruction(eInstructionType.None); break;
                case 0x6D: SetUpInstruction(eInstructionType.None); break;
                case 0x6E: SetUpInstruction(eInstructionType.None); break;
                case 0x6F: SetUpInstruction(eInstructionType.None); break;

                case 0x70: SetUpInstruction(eInstructionType.None); break;
                case 0x71: SetUpInstruction(eInstructionType.None); break;
                case 0x72: SetUpInstruction(eInstructionType.None); break;
                case 0x73: SetUpInstruction(eInstructionType.None); break;
                case 0x74: SetUpInstruction(eInstructionType.None); break;
                case 0x75: SetUpInstruction(eInstructionType.None); break;
                case 0x76: SetUpInstruction(eInstructionType.None); break;
                case 0x77: SetUpInstruction(eInstructionType.None); break;
                case 0x78: SetUpInstruction(eInstructionType.None); break;
                case 0x79: SetUpInstruction(eInstructionType.None); break;
                case 0x7A: SetUpInstruction(eInstructionType.None); break;
                case 0x7B: SetUpInstruction(eInstructionType.None); break;
                case 0x7C: SetUpInstruction(eInstructionType.None); break;
                case 0x7D: SetUpInstruction(eInstructionType.None); break;
                case 0x7E: SetUpInstruction(eInstructionType.None); break;
                case 0x7F: SetUpInstruction(eInstructionType.None); break;

                case 0x80: SetUpInstruction(eInstructionType.None); break;
                case 0x81: SetUpInstruction(eInstructionType.None); break;
                case 0x82: SetUpInstruction(eInstructionType.None); break;
                case 0x83: SetUpInstruction(eInstructionType.None); break;
                case 0x84: SetUpInstruction(eInstructionType.None); break;
                case 0x85: SetUpInstruction(eInstructionType.None); break;
                case 0x86: SetUpInstruction(eInstructionType.None); break;
                case 0x87: SetUpInstruction(eInstructionType.None); break;
                case 0x88: SetUpInstruction(eInstructionType.None); break;
                case 0x89: SetUpInstruction(eInstructionType.None); break;
                case 0x8A: SetUpInstruction(eInstructionType.None); break;
                case 0x8B: SetUpInstruction(eInstructionType.None); break;
                case 0x8C: SetUpInstruction(eInstructionType.None); break;
                case 0x8D: SetUpInstruction(eInstructionType.None); break;
                case 0x8E: SetUpInstruction(eInstructionType.None); break;
                case 0x8F: SetUpInstruction(eInstructionType.None); break;

                case 0x90: SetUpInstruction(eInstructionType.None); break;
                case 0x91: SetUpInstruction(eInstructionType.None); break;
                case 0x92: SetUpInstruction(eInstructionType.None); break;
                case 0x93: SetUpInstruction(eInstructionType.None); break;
                case 0x94: SetUpInstruction(eInstructionType.None); break;
                case 0x95: SetUpInstruction(eInstructionType.None); break;
                case 0x96: SetUpInstruction(eInstructionType.None); break;
                case 0x97: SetUpInstruction(eInstructionType.None); break;
                case 0x98: SetUpInstruction(eInstructionType.None); break;
                case 0x99: SetUpInstruction(eInstructionType.None); break;
                case 0x9A: SetUpInstruction(eInstructionType.None); break;
                case 0x9B: SetUpInstruction(eInstructionType.None); break;
                case 0x9C: SetUpInstruction(eInstructionType.None); break;
                case 0x9D: SetUpInstruction(eInstructionType.None); break;
                case 0x9E: SetUpInstruction(eInstructionType.None); break;
                case 0x9F: SetUpInstruction(eInstructionType.None); break;

                case 0xA0: SetUpInstruction(eInstructionType.None); break;
                case 0xA1: SetUpInstruction(eInstructionType.None); break;
                case 0xA2: SetUpInstruction(eInstructionType.None); break;
                case 0xA3: SetUpInstruction(eInstructionType.None); break;
                case 0xA4: SetUpInstruction(eInstructionType.None); break;
                case 0xA5: SetUpInstruction(eInstructionType.None); break;
                case 0xA6: SetUpInstruction(eInstructionType.None); break;
                case 0xA7: SetUpInstruction(eInstructionType.None); break;
                case 0xA8: SetUpInstruction(eInstructionType.None); break;
                case 0xA9: SetUpInstruction(eInstructionType.None); break;
                case 0xAA: SetUpInstruction(eInstructionType.None); break;
                case 0xAB: SetUpInstruction(eInstructionType.None); break;
                case 0xAC: SetUpInstruction(eInstructionType.None); break;
                case 0xAD: SetUpInstruction(eInstructionType.None); break;
                case 0xAE: SetUpInstruction(eInstructionType.None); break;
                case 0xAF: SetUpInstruction(eInstructionType.None); break;

                case 0xB0: SetUpInstruction(eInstructionType.None); break;
                case 0xB1: SetUpInstruction(eInstructionType.None); break;
                case 0xB2: SetUpInstruction(eInstructionType.None); break;
                case 0xB3: SetUpInstruction(eInstructionType.None); break;
                case 0xB4: SetUpInstruction(eInstructionType.None); break;
                case 0xB5: SetUpInstruction(eInstructionType.None); break;
                case 0xB6: SetUpInstruction(eInstructionType.None); break;
                case 0xB7: SetUpInstruction(eInstructionType.None); break;
                case 0xB8: SetUpInstruction(eInstructionType.None); break;
                case 0xB9: SetUpInstruction(eInstructionType.None); break;
                case 0xBA: SetUpInstruction(eInstructionType.None); break;
                case 0xBB: SetUpInstruction(eInstructionType.None); break;
                case 0xBC: SetUpInstruction(eInstructionType.None); break;
                case 0xBD: SetUpInstruction(eInstructionType.None); break;
                case 0xBE: SetUpInstruction(eInstructionType.None); break;
                case 0xBF: SetUpInstruction(eInstructionType.None); break;

                case 0xC0: SetUpInstruction(eInstructionType.None); break;
                case 0xC1: SetUpInstruction(eInstructionType.None); break;
                case 0xC2: SetUpInstruction(eInstructionType.None); break;
                case 0xC3: SetUpInstruction(eInstructionType.None); break;
                case 0xC4: SetUpInstruction(eInstructionType.None); break;
                case 0xC5: SetUpInstruction(eInstructionType.None); break;
                case 0xC6: SetUpInstruction(eInstructionType.None); break;
                case 0xC7: SetUpInstruction(eInstructionType.None); break;
                case 0xC8: SetUpInstruction(eInstructionType.None); break;
                case 0xC9: SetUpInstruction(eInstructionType.None); break;
                case 0xCA: SetUpInstruction(eInstructionType.None); break;
                case 0xCB: SetUpInstruction(eInstructionType.None); break;
                case 0xCC: SetUpInstruction(eInstructionType.None); break;
                case 0xCD: SetUpInstruction(eInstructionType.None); break;
                case 0xCE: SetUpInstruction(eInstructionType.None); break;
                case 0xCF: SetUpInstruction(eInstructionType.None); break;

                case 0xD0: SetUpInstruction(eInstructionType.None); break;
                case 0xD1: SetUpInstruction(eInstructionType.None); break;
                case 0xD2: SetUpInstruction(eInstructionType.None); break;
                case 0xD3: SetUpInstruction(eInstructionType.None); break;
                case 0xD4: SetUpInstruction(eInstructionType.None); break;
                case 0xD5: SetUpInstruction(eInstructionType.None); break;
                case 0xD6: SetUpInstruction(eInstructionType.None); break;
                case 0xD7: SetUpInstruction(eInstructionType.None); break;
                case 0xD8: SetUpInstruction(eInstructionType.None); break;
                case 0xD9: SetUpInstruction(eInstructionType.None); break;
                case 0xDA: SetUpInstruction(eInstructionType.None); break;
                case 0xDB: SetUpInstruction(eInstructionType.None); break;
                case 0xDC: SetUpInstruction(eInstructionType.None); break;
                case 0xDD: SetUpInstruction(eInstructionType.None); break;
                case 0xDE: SetUpInstruction(eInstructionType.None); break;
                case 0xDF: SetUpInstruction(eInstructionType.None); break;

                case 0xE0: SetUpInstruction(eInstructionType.None); break;
                case 0xE1: SetUpInstruction(eInstructionType.None); break;
                case 0xE2: SetUpInstruction(eInstructionType.None); break;
                case 0xE3: SetUpInstruction(eInstructionType.None); break;
                case 0xE4: SetUpInstruction(eInstructionType.None); break;
                case 0xE5: SetUpInstruction(eInstructionType.None); break;
                case 0xE6: SetUpInstruction(eInstructionType.None); break;
                case 0xE7: SetUpInstruction(eInstructionType.None); break;
                case 0xE8: SetUpInstruction(eInstructionType.None); break;
                case 0xE9: SetUpInstruction(eInstructionType.None); break;
                case 0xEA: SetUpInstruction(eInstructionType.None); break;
                case 0xEB: SetUpInstruction(eInstructionType.None); break;
                case 0xEC: SetUpInstruction(eInstructionType.None); break;
                case 0xED: SetUpInstruction(eInstructionType.None); break;
                case 0xEE: SetUpInstruction(eInstructionType.None); break;
                case 0xEF: SetUpInstruction(eInstructionType.None); break;

                case 0xF0: SetUpInstruction(eInstructionType.None); break;
                case 0xF1: SetUpInstruction(eInstructionType.None); break;
                case 0xF2: SetUpInstruction(eInstructionType.None); break;
                case 0xF3: SetUpInstruction(eInstructionType.None); break;
                case 0xF4: SetUpInstruction(eInstructionType.None); break;
                case 0xF5: SetUpInstruction(eInstructionType.None); break;
                case 0xF6: SetUpInstruction(eInstructionType.None); break;
                case 0xF7: SetUpInstruction(eInstructionType.None); break;
                case 0xF8: SetUpInstruction(eInstructionType.None); break;
                case 0xF9: SetUpInstruction(eInstructionType.None); break;
                case 0xFA: SetUpInstruction(eInstructionType.None); break;
                case 0xFB: SetUpInstruction(eInstructionType.None); break;
                case 0xFC: SetUpInstruction(eInstructionType.None); break;
                case 0xFD: SetUpInstruction(eInstructionType.None); break;
                case 0xFE: SetUpInstruction(eInstructionType.None); break;
                case 0xFF: SetUpInstruction(eInstructionType.None); break;


            }
        }

        public void SetUpInstruction(eInstructionType instructionType)
        {
            InstructionType = instructionType;
        }

        public void SetUpInstruction(eInstructionType instructionType, eAddressingMode addressingMode)
        {
            InstructionType = instructionType;
            AddressingMode = addressingMode;
        }

        public void SetUpInstruction(eInstructionType instructionType, eAddressingMode addressingMode, eRegisterType register1)
        {
            InstructionType = instructionType;
            AddressingMode = addressingMode;
            Register1 = register1;
        }

        public void SetUpInstruction(eInstructionType instructionType, eAddressingMode addressingMode, eRegisterType register1, eRegisterType register2)
        {
            InstructionType = instructionType;
            AddressingMode = addressingMode;
            Register1 = register1;
            Register2 = register2;
        }
        public void SetUpInstruction(eInstructionType instructionType, eAddressingMode addressingMode, eRegisterType register1, eRegisterType register2, eConditionType condition)
        {
            InstructionType = instructionType;
            AddressingMode = addressingMode;
            Register1 = register1;
            Register2 = register2;
            ConditionType = condition;
        }

        public void SetUpInstruction(eInstructionType instructionType, eAddressingMode addressingMode, eRegisterType register1, 
            eRegisterType register2, eConditionType condition, Byte param)
        {
            InstructionType = instructionType;
            AddressingMode = addressingMode;
            Register1 = register1;
            Register2 = register2;
            ConditionType = condition;
            Parameter = param;
        }


    }
}
