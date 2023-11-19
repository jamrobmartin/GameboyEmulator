using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameboyEmulator
{
    public enum eInstructionType
    {
        None,
        NOP,
        STOP,
        LD,
        LDH,
        JP,
        JR,
        CALL,
        RET,
        ADD,
        ADC,
        SUB,
        SBC,
        AND,
        XOR,
        OR,
        CP,
        CB,
        INC,
        DEC
    }

    public enum eAddressingMode
    {
        None,
        Implied,
        Register,
        MemoryRegister,
        Register_Register,
        Register_MemoryRegister,
        MemoryRegister_Register,
        Register_D8,
        Register_A8,
        Register_R8,
        A8_Register,
        MemoryRegister_D8,
        Register_A16,
        A16_Register,
        Register_D16,
        HLI_Register,
        Register_HLI,
        HLD_Register,
        Register_HLD,
        HL_SPR,
        D16,
        D8
    }

    public enum eRegisterType
    {
        None,
        A,
        F,
        B,
        C,
        D,
        E,
        H,
        L,
        AF,
        BC,
        DE,
        HL,
        SP,
        PC
    }

    public enum eConditionType
    {
        None,
        NZ,
        Z,
        NC,
        C
    }

    public class Instruction
    {
        public Byte OpCode = 0xDD;
        public eInstructionType InstructionType = eInstructionType.None;
        public eAddressingMode AddressingMode = eAddressingMode.None;
        public eRegisterType Register1 = eRegisterType.None;
        public eRegisterType Register2 = eRegisterType.None;
        public eConditionType ConditionType = eConditionType.None;
        public Byte Parameter = 0;
        public Byte Parameter2 = 0;

        public bool DestinationIsMemory { get; set; } = false;
        public Word DestinationAddress { get; set; } = 0;
        public bool SourceIsMemory {  get; set; } = false;
        public Word SourceAddress { get; set; } = 0;

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
                case 0x00: SetUpInstruction(eInstructionType.NOP); break;
                case 0x01: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_D16, eRegisterType.BC); break;
                case 0x02: SetUpInstruction(eInstructionType.LD, eAddressingMode.MemoryRegister_Register, eRegisterType.BC, eRegisterType.A); break;
                case 0x03: SetUpInstruction(eInstructionType.INC, eAddressingMode.Register, eRegisterType.BC); break;
                case 0x04: SetUpInstruction(eInstructionType.INC, eAddressingMode.Register, eRegisterType.B); break;
                case 0x05: SetUpInstruction(eInstructionType.DEC, eAddressingMode.Register, eRegisterType.B); break;
                case 0x06: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_D8, eRegisterType.B); break;
                case 0x07: SetUpInstruction(eInstructionType.None); break;
                case 0x08: SetUpInstruction(eInstructionType.LD, eAddressingMode.A16_Register, eRegisterType.None, eRegisterType.SP); break;
                case 0x09: SetUpInstruction(eInstructionType.None); break;
                case 0x0A: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_MemoryRegister, eRegisterType.A, eRegisterType.BC); break;
                case 0x0B: SetUpInstruction(eInstructionType.DEC, eAddressingMode.Register, eRegisterType.BC); break;
                case 0x0C: SetUpInstruction(eInstructionType.INC, eAddressingMode.Register, eRegisterType.C); break;
                case 0x0D: SetUpInstruction(eInstructionType.DEC, eAddressingMode.Register, eRegisterType.C); break;
                case 0x0E: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_D8, eRegisterType.C); break;
                case 0x0F: SetUpInstruction(eInstructionType.None); break;

                case 0x10: SetUpInstruction(eInstructionType.STOP); break;
                case 0x11: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_D16, eRegisterType.DE); break;
                case 0x12: SetUpInstruction(eInstructionType.LD, eAddressingMode.MemoryRegister_Register, eRegisterType.DE, eRegisterType.A); break;
                case 0x13: SetUpInstruction(eInstructionType.INC, eAddressingMode.Register, eRegisterType.DE); break;
                case 0x14: SetUpInstruction(eInstructionType.INC, eAddressingMode.Register, eRegisterType.D); break;
                case 0x15: SetUpInstruction(eInstructionType.DEC, eAddressingMode.Register, eRegisterType.D); break;
                case 0x16: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_D8, eRegisterType.D); break;
                case 0x17: SetUpInstruction(eInstructionType.None); break;
                case 0x18: SetUpInstruction(eInstructionType.JR, eAddressingMode.D8); break;
                case 0x19: SetUpInstruction(eInstructionType.None); break;
                case 0x1A: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_MemoryRegister, eRegisterType.A, eRegisterType.DE); break;
                case 0x1B: SetUpInstruction(eInstructionType.DEC, eAddressingMode.Register, eRegisterType.DE); break;
                case 0x1C: SetUpInstruction(eInstructionType.INC, eAddressingMode.Register, eRegisterType.E); break;
                case 0x1D: SetUpInstruction(eInstructionType.DEC, eAddressingMode.Register, eRegisterType.E); break;
                case 0x1E: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_D8, eRegisterType.E); break;
                case 0x1F: SetUpInstruction(eInstructionType.None); break;

                case 0x20: SetUpInstruction(eInstructionType.JR, eAddressingMode.D8, eRegisterType.None, eRegisterType.None, eConditionType.NZ); break;
                case 0x21: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_D16, eRegisterType.HL); break;
                case 0x22: SetUpInstruction(eInstructionType.LD, eAddressingMode.HLI_Register, eRegisterType.HL, eRegisterType.A); break;
                case 0x23: SetUpInstruction(eInstructionType.INC, eAddressingMode.Register, eRegisterType.HL); break;
                case 0x24: SetUpInstruction(eInstructionType.INC, eAddressingMode.Register, eRegisterType.H); break;
                case 0x25: SetUpInstruction(eInstructionType.DEC, eAddressingMode.Register, eRegisterType.H); break;
                case 0x26: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_D8, eRegisterType.H); break;
                case 0x27: SetUpInstruction(eInstructionType.None); break;
                case 0x28: SetUpInstruction(eInstructionType.JR, eAddressingMode.D8, eRegisterType.None, eRegisterType.None, eConditionType.Z); break;
                case 0x29: SetUpInstruction(eInstructionType.None); break;
                case 0x2A: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_HLI, eRegisterType.A, eRegisterType.HL); break;
                case 0x2B: SetUpInstruction(eInstructionType.DEC, eAddressingMode.Register, eRegisterType.HL); break;
                case 0x2C: SetUpInstruction(eInstructionType.INC, eAddressingMode.Register, eRegisterType.L); break;
                case 0x2D: SetUpInstruction(eInstructionType.DEC, eAddressingMode.Register, eRegisterType.L); break;
                case 0x2E: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_D8, eRegisterType.L); break;
                case 0x2F: SetUpInstruction(eInstructionType.None); break;

                case 0x30: SetUpInstruction(eInstructionType.JR, eAddressingMode.D8, eRegisterType.None, eRegisterType.None, eConditionType.NC); break;
                case 0x31: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_D16, eRegisterType.SP); break;
                case 0x32: SetUpInstruction(eInstructionType.LD, eAddressingMode.HLD_Register, eRegisterType.HL, eRegisterType.A); break;
                case 0x33: SetUpInstruction(eInstructionType.INC, eAddressingMode.Register, eRegisterType.SP); break;
                case 0x34: SetUpInstruction(eInstructionType.INC, eAddressingMode.MemoryRegister, eRegisterType.HL); break;
                case 0x35: SetUpInstruction(eInstructionType.DEC, eAddressingMode.MemoryRegister, eRegisterType.HL); break;
                case 0x36: SetUpInstruction(eInstructionType.LD, eAddressingMode.MemoryRegister_D8, eRegisterType.HL); break;
                case 0x37: SetUpInstruction(eInstructionType.None); break;
                case 0x38: SetUpInstruction(eInstructionType.JR, eAddressingMode.D8, eRegisterType.None, eRegisterType.None, eConditionType.C); break;
                case 0x39: SetUpInstruction(eInstructionType.None); break;
                case 0x3A: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_HLD, eRegisterType.A, eRegisterType.HL); break;
                case 0x3B: SetUpInstruction(eInstructionType.DEC, eAddressingMode.Register, eRegisterType.SP); break;
                case 0x3C: SetUpInstruction(eInstructionType.INC, eAddressingMode.Register, eRegisterType.A); break;
                case 0x3D: SetUpInstruction(eInstructionType.DEC, eAddressingMode.Register, eRegisterType.A); break;
                case 0x3E: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_D8, eRegisterType.A); break;
                case 0x3F: SetUpInstruction(eInstructionType.None); break;

                case 0x40: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.B, eRegisterType.B); break;
                case 0x41: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.B, eRegisterType.C); break;
                case 0x42: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.B, eRegisterType.D); break;
                case 0x43: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.B, eRegisterType.E); break;
                case 0x44: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.B, eRegisterType.H); break;
                case 0x45: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.B, eRegisterType.L); break;
                case 0x46: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_MemoryRegister, eRegisterType.B, eRegisterType.HL); break;
                case 0x47: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.B, eRegisterType.A); break;
                case 0x48: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.C, eRegisterType.B); break;
                case 0x49: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.C, eRegisterType.C); break;
                case 0x4A: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.C, eRegisterType.D); break;
                case 0x4B: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.C, eRegisterType.E); break;
                case 0x4C: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.C, eRegisterType.H); break;
                case 0x4D: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.C, eRegisterType.L); break;
                case 0x4E: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_MemoryRegister, eRegisterType.C, eRegisterType.HL); break;
                case 0x4F: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.C, eRegisterType.A); break;

                case 0x50: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.D, eRegisterType.B); break;
                case 0x51: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.D, eRegisterType.C); break;
                case 0x52: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.D, eRegisterType.D); break;
                case 0x53: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.D, eRegisterType.E); break;
                case 0x54: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.D, eRegisterType.H); break;
                case 0x55: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.D, eRegisterType.L); break;
                case 0x56: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_MemoryRegister, eRegisterType.D, eRegisterType.HL); break;
                case 0x57: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.D, eRegisterType.A); break;
                case 0x58: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.E, eRegisterType.B); break;
                case 0x59: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.E, eRegisterType.C); break;
                case 0x5A: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.E, eRegisterType.D); break;
                case 0x5B: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.E, eRegisterType.E); break;
                case 0x5C: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.E, eRegisterType.H); break;
                case 0x5D: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.E, eRegisterType.L); break;
                case 0x5E: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_MemoryRegister, eRegisterType.E, eRegisterType.HL); break;
                case 0x5F: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.E, eRegisterType.A); break;

                case 0x60: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.H, eRegisterType.B); break;
                case 0x61: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.H, eRegisterType.C); break;
                case 0x62: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.H, eRegisterType.D); break;
                case 0x63: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.H, eRegisterType.E); break;
                case 0x64: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.H, eRegisterType.H); break;
                case 0x65: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.H, eRegisterType.L); break;
                case 0x66: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_MemoryRegister, eRegisterType.H, eRegisterType.HL); break;
                case 0x67: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.H, eRegisterType.A); break;
                case 0x68: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.L, eRegisterType.B); break;
                case 0x69: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.L, eRegisterType.C); break;
                case 0x6A: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.L, eRegisterType.D); break;
                case 0x6B: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.L, eRegisterType.E); break;
                case 0x6C: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.L, eRegisterType.H); break;
                case 0x6D: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.L, eRegisterType.L); break;
                case 0x6E: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_MemoryRegister, eRegisterType.L, eRegisterType.HL); break;
                case 0x6F: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.L, eRegisterType.A); break;

                case 0x70: SetUpInstruction(eInstructionType.LD, eAddressingMode.MemoryRegister_Register, eRegisterType.HL, eRegisterType.B); break;
                case 0x71: SetUpInstruction(eInstructionType.LD, eAddressingMode.MemoryRegister_Register, eRegisterType.HL, eRegisterType.C); break;
                case 0x72: SetUpInstruction(eInstructionType.LD, eAddressingMode.MemoryRegister_Register, eRegisterType.HL, eRegisterType.D); break;
                case 0x73: SetUpInstruction(eInstructionType.LD, eAddressingMode.MemoryRegister_Register, eRegisterType.HL, eRegisterType.E); break;
                case 0x74: SetUpInstruction(eInstructionType.LD, eAddressingMode.MemoryRegister_Register, eRegisterType.HL, eRegisterType.H); break;
                case 0x75: SetUpInstruction(eInstructionType.LD, eAddressingMode.MemoryRegister_Register, eRegisterType.HL, eRegisterType.L); break;
                case 0x76: SetUpInstruction(eInstructionType.None); break; // HALT
                case 0x77: SetUpInstruction(eInstructionType.LD, eAddressingMode.MemoryRegister_Register, eRegisterType.HL, eRegisterType.A); break;
                case 0x78: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.A, eRegisterType.B); break;
                case 0x79: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.A, eRegisterType.C); break;
                case 0x7A: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.A, eRegisterType.D); break;
                case 0x7B: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.A, eRegisterType.E); break;
                case 0x7C: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.A, eRegisterType.H); break;
                case 0x7D: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.A, eRegisterType.L); break;
                case 0x7E: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_MemoryRegister, eRegisterType.A, eRegisterType.HL); break;
                case 0x7F: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.A, eRegisterType.A); break;

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

                case 0xC0: SetUpInstruction(eInstructionType.RET, eAddressingMode.Implied, eRegisterType.None, eRegisterType.None, eConditionType.NZ); break;
                case 0xC1: SetUpInstruction(eInstructionType.None); break;
                case 0xC2: SetUpInstruction(eInstructionType.JP, eAddressingMode.D16, eRegisterType.None, eRegisterType.None, eConditionType.NZ); break;
                case 0xC3: SetUpInstruction(eInstructionType.JP, eAddressingMode.D16); break;
                case 0xC4: SetUpInstruction(eInstructionType.CALL, eAddressingMode.D16, eRegisterType.None, eRegisterType.None, eConditionType.NZ); break;
                case 0xC5: SetUpInstruction(eInstructionType.None); break;
                case 0xC6: SetUpInstruction(eInstructionType.ADD, eAddressingMode.Register_D8, eRegisterType.A); break;
                case 0xC7: SetUpInstruction(eInstructionType.None); break;
                case 0xC8: SetUpInstruction(eInstructionType.RET, eAddressingMode.Implied, eRegisterType.None, eRegisterType.None, eConditionType.Z); break;
                case 0xC9: SetUpInstruction(eInstructionType.RET, eAddressingMode.Implied); break;
                case 0xCA: SetUpInstruction(eInstructionType.JP, eAddressingMode.D16, eRegisterType.None, eRegisterType.None, eConditionType.Z); break;
                case 0xCB: SetUpInstruction(eInstructionType.CB, eAddressingMode.D8); break;
                case 0xCC: SetUpInstruction(eInstructionType.CALL, eAddressingMode.D16, eRegisterType.None, eRegisterType.None, eConditionType.Z); break;
                case 0xCD: SetUpInstruction(eInstructionType.CALL, eAddressingMode.D16); break;
                case 0xCE: SetUpInstruction(eInstructionType.ADC, eAddressingMode.Register_D8, eRegisterType.A); break;
                case 0xCF: SetUpInstruction(eInstructionType.None); break;

                case 0xD0: SetUpInstruction(eInstructionType.RET, eAddressingMode.Implied, eRegisterType.None, eRegisterType.None, eConditionType.NC); break;
                case 0xD1: SetUpInstruction(eInstructionType.None); break;
                case 0xD2: SetUpInstruction(eInstructionType.JP, eAddressingMode.D16, eRegisterType.None, eRegisterType.None, eConditionType.NC); break;
                case 0xD3: SetUpInstruction(eInstructionType.None); break;
                case 0xD4: SetUpInstruction(eInstructionType.CALL, eAddressingMode.D16, eRegisterType.None, eRegisterType.None, eConditionType.NC); break;
                case 0xD5: SetUpInstruction(eInstructionType.None); break;
                case 0xD6: SetUpInstruction(eInstructionType.SUB, eAddressingMode.Register_D8, eRegisterType.A); break;
                case 0xD7: SetUpInstruction(eInstructionType.None); break;
                case 0xD8: SetUpInstruction(eInstructionType.RET, eAddressingMode.Implied, eRegisterType.None, eRegisterType.None, eConditionType.C); break;
                case 0xD9: SetUpInstruction(eInstructionType.None); break;
                case 0xDA: SetUpInstruction(eInstructionType.JP, eAddressingMode.D16, eRegisterType.None, eRegisterType.None, eConditionType.C); break;
                case 0xDB: SetUpInstruction(eInstructionType.None); break;
                case 0xDC: SetUpInstruction(eInstructionType.CALL, eAddressingMode.D16, eRegisterType.None, eRegisterType.None, eConditionType.C); break;
                case 0xDD: SetUpInstruction(eInstructionType.None); break;
                case 0xDE: SetUpInstruction(eInstructionType.SBC, eAddressingMode.Register_D8, eRegisterType.A); break;
                case 0xDF: SetUpInstruction(eInstructionType.None); break;

                case 0xE0: SetUpInstruction(eInstructionType.LDH, eAddressingMode.A8_Register, eRegisterType.None, eRegisterType.A); break;
                case 0xE1: SetUpInstruction(eInstructionType.None); break;
                case 0xE2: SetUpInstruction(eInstructionType.LD, eAddressingMode.MemoryRegister_Register, eRegisterType.C, eRegisterType.A); break;
                case 0xE3: SetUpInstruction(eInstructionType.None); break;
                case 0xE4: SetUpInstruction(eInstructionType.None); break;
                case 0xE5: SetUpInstruction(eInstructionType.None); break;
                case 0xE6: SetUpInstruction(eInstructionType.AND, eAddressingMode.Register_D8, eRegisterType.A); break;
                case 0xE7: SetUpInstruction(eInstructionType.None); break;
                case 0xE8: SetUpInstruction(eInstructionType.ADD, eAddressingMode.Register_R8, eRegisterType.SP); break;
                case 0xE9: SetUpInstruction(eInstructionType.JP, eAddressingMode.Register, eRegisterType.HL); break;
                case 0xEA: SetUpInstruction(eInstructionType.LD, eAddressingMode.A16_Register, eRegisterType.None, eRegisterType.A); break;
                case 0xEB: SetUpInstruction(eInstructionType.None); break;
                case 0xEC: SetUpInstruction(eInstructionType.None); break;
                case 0xED: SetUpInstruction(eInstructionType.None); break;
                case 0xEE: SetUpInstruction(eInstructionType.XOR, eAddressingMode.Register_D8, eRegisterType.A); break;
                case 0xEF: SetUpInstruction(eInstructionType.None); break;

                case 0xF0: SetUpInstruction(eInstructionType.LDH, eAddressingMode.Register_A8, eRegisterType.A); break;
                case 0xF1: SetUpInstruction(eInstructionType.None); break;
                case 0xF2: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_MemoryRegister, eRegisterType.A, eRegisterType.C); break;
                case 0xF3: SetUpInstruction(eInstructionType.None); break;
                case 0xF4: SetUpInstruction(eInstructionType.None); break;
                case 0xF5: SetUpInstruction(eInstructionType.None); break;
                case 0xF6: SetUpInstruction(eInstructionType.OR, eAddressingMode.Register_D8, eRegisterType.A); break;
                case 0xF7: SetUpInstruction(eInstructionType.None); break;
                case 0xF8: SetUpInstruction(eInstructionType.LD, eAddressingMode.HL_SPR, eRegisterType.HL, eRegisterType.SP); break;
                case 0xF9: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_Register, eRegisterType.SP, eRegisterType.HL); break;
                case 0xFA: SetUpInstruction(eInstructionType.LD, eAddressingMode.Register_A16, eRegisterType.A); break;
                case 0xFB: SetUpInstruction(eInstructionType.None); break;
                case 0xFC: SetUpInstruction(eInstructionType.None); break;
                case 0xFD: SetUpInstruction(eInstructionType.None); break;
                case 0xFE: SetUpInstruction(eInstructionType.CP, eAddressingMode.Register_D8, eRegisterType.A); break;
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

        public string Mnemonic()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(InstructionType);

            // If the instruction is any of the Command instruction, just display the command name
            switch(InstructionType)
            {
                case eInstructionType.CB:
                    return sb.ToString();
            }

            switch (AddressingMode)
            {
                case eAddressingMode.None:
                    break;
                case eAddressingMode.Register:
                    sb.Append(' ');
                    sb.Append(Register1.ToString());
                    break;
                case eAddressingMode.MemoryRegister:
                    sb.Append(' ');
                    sb.Append("(" + Register1.ToString() + ")");
                    break;

                case eAddressingMode.Register_Register:
                    sb.Append(' ');
                    sb.Append(Register1.ToString() + "," + Register2.ToString());
                    break;
                case eAddressingMode.Register_MemoryRegister:
                    sb.Append(' ');
                    sb.Append(Register1.ToString() + "," + "(" + Register2.ToString() + ")");
                    break;
                case eAddressingMode.MemoryRegister_Register:
                    sb.Append(' ');
                    sb.Append("(" + Register1.ToString() + ")" + "," + Register2.ToString());
                    break;

                case eAddressingMode.Register_D8:
                    sb.Append(' ');
                    sb.Append(Register1.ToString() + "," + "D8");
                    break;
                case eAddressingMode.MemoryRegister_D8:
                    sb.Append(' ');
                    sb.Append("(" + Register1.ToString() + ")," + "D8");
                    break;
                case eAddressingMode.A8_Register:
                    sb.Append(' ');
                    sb.Append("(A8)" + "," + Register2.ToString());
                    break;
                case eAddressingMode.Register_A8:
                    sb.Append(' ');
                    sb.Append(Register1.ToString() + "," + "(A8)");
                    break;
                case eAddressingMode.A16_Register:
                    sb.Append(' ');
                    sb.Append("(A16)" + "," + Register2.ToString());
                    break;
                case eAddressingMode.Register_A16:
                    sb.Append(' ');
                    sb.Append(Register1.ToString() + "," + "(A16)");
                    break;
                case eAddressingMode.Register_D16:
                    sb.Append(' ');
                    sb.Append(Register1.ToString() + "," + "D16");
                    break;
                case eAddressingMode.Register_R8:
                    sb.Append(' ');
                    sb.Append(Register1.ToString() + "," + "R8");
                    break;
                case eAddressingMode.HLI_Register:
                    sb.Append(' ');
                    sb.Append("(" + Register1.ToString() + "+)," + Register2.ToString());
                    break;
                case eAddressingMode.Register_HLI:
                    sb.Append(' ');
                    sb.Append(Register1.ToString() + ",(" + Register2.ToString() + "+)");
                    break;
                case eAddressingMode.HLD_Register:
                    sb.Append(' ');
                    sb.Append("(" + Register1.ToString() + "-)," + Register2.ToString());
                    break;
                case eAddressingMode.Register_HLD:
                    sb.Append(' ');
                    sb.Append(Register1.ToString() + ",(" + Register2.ToString() + "-)"); 
                    break;
                case eAddressingMode.HL_SPR:
                    sb.Append(' ');
                    sb.Append(Register1.ToString() + "," + Register2.ToString() + "+R8");
                    break;
                case eAddressingMode.D16:
                    sb.Append(' ');
                    if (ConditionType != eConditionType.None)
                        sb.Append(ConditionType.ToString() + ",");
                    sb.Append("A16");
                    break;
                case eAddressingMode.D8:
                    sb.Append(' ');
                    if (ConditionType != eConditionType.None)
                        sb.Append(ConditionType.ToString() + ",");
                    sb.Append("R8");
                    break;

                case eAddressingMode.Implied:
                    sb.Append(' ');
                    if (ConditionType != eConditionType.None)
                        sb.Append(ConditionType.ToString());
                    break;

                default:
                    break;
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(OpCode.ToHexString());
            sb.Append(" - ");

            sb.Append(InstructionType.ToString());
            sb.Append(" - ");

            sb.Append(AddressingMode.ToString());
            sb.Append(" - ");

            sb.Append(Register1.ToString());
            sb.Append(" - ");

            sb.Append(Register2.ToString());
            sb.Append(" - ");

            sb.Append(ConditionType.ToString());
            sb.Append(" - ");

            sb.Append(Parameter.ToHexString());
            sb.Append(" - ");

            sb.Append(Parameter2.ToHexString());

            return sb.ToString();
        }


    }
}
