using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace _6502.Processor
{
    static class CPU
    {
        public static byte A; //Accumulator
        public static byte X, Y; //Index Register
        public static byte SP; //Stack Pointer 

        public static ushort PC; //Program Counters

        public static byte data; //data bus

        public static ushort bus; //address bus

        public static Status SR; //Processor Status
        public struct Status 
        {
            public bool C; //Carry
            public bool Z; //Zero Flag
            public bool I; //Interrupt
            public bool D; //Decimal
            public bool B; //Break
            public bool U; //Unused
            public bool V; //Overflow
            public bool N; //Negative
        }

        public static byte SRToByte()
        {
        byte status = 0;

        if (CPU.SR.C) status |= 0x01;
        if (CPU.SR.Z) status |= 0x02;
        if (CPU.SR.I) status |= 0x04;
        if (CPU.SR.D) status |= 0x08;
        if (CPU.SR.B) status |= 0x10;
        status |= 0x20;              
        if (CPU.SR.V) status |= 0x40;
        if (CPU.SR.N) status |= 0x80;

        return status;
        }

        public static void ByteToSR(byte value)
        {
            CPU.SR.N = (value & 0x80) != 0;
            CPU.SR.V = (value & 0x40) != 0;
            CPU.SR.U = true;               
            CPU.SR.B = (value & 0x10) != 0;
            CPU.SR.D = (value & 0x08) != 0;
            CPU.SR.I = (value & 0x04) != 0;
            CPU.SR.Z = (value & 0x02) != 0;
            CPU.SR.C = (value & 0x01) != 0;
        }

        public static void Fetch()
        {
            data = Memory.RAM[bus];
        }

        public static void Write()
        {
            Memory.RAM[bus] = data;
        }

        private static void Reset()
        {
            bus = 0xfffc;
            Fetch();
            byte vector_low = data;

            bus = 0xfffd;
            Fetch();
            byte vector_high = data;

            SR.B = true;
            SR.D = false;
            SR.I = true;
            SR.U = true;

            PC = (ushort)((vector_high << 8) | vector_low);
            SP = 0x00ff;
        }
        private static void execute()
        {
            while(true)
            {
                bus = PC;
                data = 0x00;

                Fetch();

                byte opCode = data;

                #region opCodes
                    switch(opCode)
                    {
                        case 0x00:
                            Console.WriteLine("die Ausführung des Programms wurde erfolgreich beendet");
                            return;
                        
                        case 0x01:
                            Instructions.ORA(Adr_modes.Pre_Indexed());
                            PC++;
                            break;

                        case 0x05:
                            Instructions.ORA(Adr_modes.Zpg());
                            PC++;
                            break;

                        case 0x06:
                            data = Instructions.ASL(Adr_modes.Zpg());
                            Write();
                            PC++;
                            break;

                        case 0x08:
                            Instructions.PHP();
                            PC++;
                            break;

                        case 0x09:
                            Instructions.ORA(Adr_modes.Immediate());
                            PC++;
                            break;

                        case 0x0a:
                            A = Instructions.ASL(A);
                            PC++;
                            break;

                        case 0x0d:
                            Instructions.ORA(Adr_modes.Absolute());
                            PC++;
                            break;

                        case 0x0e:
                            data = Instructions.ASL(Adr_modes.Absolute());
                            Write();
                            PC++;
                            break;

                        case 0x10:
                            Instructions.BPL(Adr_modes.Relative());
                            break;

                        case 0x11:
                            Instructions.ORA(Adr_modes.Post_Indexed());
                            PC++;
                            break;

                        case 0x15:
                            Instructions.ORA(Adr_modes.Indexed_Zpg(X));
                            PC++;
                            break;
                        
                        case 0x16:
                            data = Instructions.ASL(Adr_modes.Indexed(X));
                            Write();
                            PC++;
                            break;

                        case 0x18:
                            Instructions.CLC();
                            PC++;
                            break;

                        case 0x19:
                            Instructions.ORA(Adr_modes.Indexed(Y));
                            PC++;
                            break;

                        case 0x1d:
                            Instructions.ORA(Adr_modes.Indexed(X));
                            PC++;
                            break;
                        case 0x1e:
                            data = Instructions.ASL(Adr_modes.Indexed(X));
                            Write();
                            PC++;
                            break;

                        case 0x20:
                            Adr_modes.Absolute();
                            Instructions.JSR(bus);
                            PC++;
                            break;

                        case 0x21:
                            Instructions.AND(Adr_modes.Pre_Indexed());
                            PC++;
                            break;

                        case 0x24:
                            Instructions.BIT(Adr_modes.Zpg());
                            PC++;
                            break;

                        case 0x25:
                            Instructions.AND(Adr_modes.Zpg());
                            PC++;
                            break;

                        case 0x26:
                            data = Instructions.ROL(Adr_modes.Zpg());
                            Write();
                            PC++;
                            break;

                        case 0x28:
                            Instructions.PLP();
                            PC++;
                            break;

                        case 0x29:
                            Instructions.AND(Adr_modes.Immediate());
                            PC++;
                            break;

                        case 0x2a:
                            A = Instructions.ROL(A);
                            PC++;
                            break;

                        case 0x2c:
                            Instructions.BIT(Adr_modes.Absolute());
                            PC++;
                            break;

                        case 0x2d:
                            Instructions.AND(Adr_modes.Absolute());
                            PC++;
                            break;

                        case 0x2e:
                            data = Instructions.ROL(Adr_modes.Absolute());
                            Write();
                            PC++;
                            break;

                        case 0x30:
                            Instructions.BMI(Adr_modes.Relative());
                            break;

                        case 0x31:
                            Instructions.AND(Adr_modes.Post_Indexed());
                            PC++;
                            break;

                        case 0x35:
                            Instructions.AND(Adr_modes.Indexed_Zpg(X));
                            PC++;
                            break;

                        case 0x36:
                            data = Instructions.ROL(Adr_modes.Indexed_Zpg(X));
                            Write();
                            PC++;
                            break;

                        case 0x38:
                            Instructions.SEC();
                            PC++;
                            break;

                        case 0x39:
                            Instructions.AND(Adr_modes.Indexed(Y));
                            PC++;
                            break;

                        case 0x3d:
                            Instructions.AND(Adr_modes.Indexed(X));
                            PC++;
                            break;

                        case 0x3e:
                            data = Instructions.ROL(Adr_modes.Indexed(X));
                            Write();
                            PC++;
                            break;

                        case 0x40:
                            Instructions.RTI();
                            break;

                        case 0x41:
                            Instructions.EOR(Adr_modes.Pre_Indexed());
                            PC++;
                            break;

                        case 0x45:
                            Instructions.EOR(Adr_modes.Zpg());
                            PC++;
                            break;

                        case 0x46:
                            data = Instructions.LSR(Adr_modes.Zpg());
                            Write();
                            PC++;
                            break;
                            
                        case 0x48:
                            Instructions.PHA();
                            PC++;
                            break;

                        case 0x49:
                            Instructions.EOR(Adr_modes.Immediate());
                            PC++;
                            break;
                        
                        case 0x4a:
                            A = Instructions.LSR(A);
                            PC++;
                            break;

                        case 0x4c:
                            Adr_modes.Absolute();
                            Instructions.JMP(bus);
                            break;

                        case 0x4d:
                            Instructions.EOR(Adr_modes.Absolute());
                            PC++;
                            break;

                        case 0x4e:
                            data = Instructions.LSR(Adr_modes.Absolute());
                            Write();
                            PC++;
                            break;

                        case 0x50:
                            Instructions.BVC(Adr_modes.Relative());
                            break;

                        case 0x51:
                            Instructions.EOR(Adr_modes.Post_Indexed());
                            PC++;
                            break;

                        case 0x55:
                            Instructions.EOR(Adr_modes.Indexed_Zpg(X));
                            PC++;
                            break;

                        case 0x56:
                            data = Instructions.LSR(Adr_modes.Indexed_Zpg(X));
                            Write();
                            PC++;
                            break;

                        case 0x58:
                            Instructions.CLI();
                            PC++;
                            break;

                        case 0x59:
                            Instructions.EOR(Adr_modes.Indexed(Y));
                            PC++;
                            break;

                        case 0x5d:
                            Instructions.EOR(Adr_modes.Indexed(X));
                            PC++;
                            break;

                        case 0x5e:
                            data = Instructions.LSR(Adr_modes.Indexed(X));
                            Write();
                            PC++;
                            break;

                        case 0x60:
                            Instructions.RTS();
                            break;

                        case 0x61:
                            Instructions.ADC(Adr_modes.Pre_Indexed());
                            PC++;
                            break;

                        case 0x65:
                            Instructions.ADC(Adr_modes.Zpg());
                            PC++;
                            break;

                        case 0x66:
                            data = Instructions.ROR(Adr_modes.Zpg());
                            Write();
                            PC++;
                            break;

                        case 0x68:
                            Instructions.PLA();
                            PC++;
                            break;

                        case 0x69:
                            Instructions.ADC(Adr_modes.Immediate());
                            PC++;
                            break;

                        case 0x6a:
                            A = Instructions.ROR(A);
                            PC++;
                            break;

                        case 0x6c:
                            Instructions.JMP(Adr_modes.Indirect());
                            break;

                        case 0x6d:
                            Instructions.ADC(Adr_modes.Absolute());
                            PC++;
                            break;

                        case 0x6e:
                            data =Instructions.ROR(Adr_modes.Absolute());
                            Write();
                            PC++;
                            break;

                        case 0x70:
                            Instructions.BVS(Adr_modes.Relative());
                            break;

                        case 0x71:
                            Instructions.ADC(Adr_modes.Post_Indexed());
                            PC++;
                            break;

                        case 0x75:
                            Instructions.ADC(Adr_modes.Indexed_Zpg(X));
                            PC++;
                            break;

                        case 0x76:
                            data = Instructions.ROR(Adr_modes.Indexed_Zpg(X));
                            Write();
                            PC++;
                            break;

                        case 0x7e:
                            data = Instructions.ROR(Adr_modes.Indexed(X));
                            Write();
                            PC++;
                            break;

                        case 0x81:
                            Adr_modes.Pre_Indexed();
                            Instructions.STA(bus);
                            PC++;
                            break;

                        case 0x84:
                            Adr_modes.Zpg();
                            Instructions.STY(bus);
                            PC++;
                            break;

                        case 0x85:
                            Adr_modes.Zpg();
                            Instructions.STA(bus);
                            PC++;
                            break;

                        case 0x86:
                            Adr_modes.Zpg();
                            Instructions.STX(bus);
                            PC++;
                            break;

                        case 0x88:
                            Instructions.DEY();
                            PC++;
                            break;

                        case 0x8a:
                            Instructions.TXA();
                            PC++;
                            break;

                        case 0x8c:
                            Adr_modes.Absolute();
                            Instructions.STY(bus);
                            PC++;
                            break;

                        case 0x8d:
                            Adr_modes.Absolute();
                            Instructions.STA(bus);
                            PC++;
                            break;

                        case 0x8e:
                            Adr_modes.Absolute();
                            Instructions.STX(bus);
                            PC++;
                            break;

                        case 0x90:
                            Instructions.BCC(Adr_modes.Relative());
                            break;

                        case 0x91:
                            Adr_modes.Post_Indexed();
                            Instructions.STA(bus);
                            PC++;
                            break;

                        case 0x94:
                            Adr_modes.Indexed_Zpg(X);
                            Instructions.STY(bus);
                            PC++;
                            break;

                        case 0x95:
                            Adr_modes.Indexed_Zpg(X);
                            Instructions.STA(bus);
                            PC++;
                            break;

                        case 0x96:
                            Adr_modes.Indexed_Zpg(Y);
                            Instructions.STX(bus);
                            PC++;
                            break;

                        case 0x98:
                            Instructions.TYA();
                            PC++;
                            break;

                        case 0x99:
                            Adr_modes.Indexed(Y);
                            Instructions.STA(bus);
                            PC++;
                            break;

                        case 0x9a:
                            Instructions.TXS();
                            PC++;
                            break;

                        case 0x9d:
                            Adr_modes.Indexed(X);
                            Instructions.STA(bus);
                            PC++;
                            break;

                        case 0xa0:
                            Instructions.LDY(Adr_modes.Immediate());
                            PC++;
                            break;

                        case 0xa2:
                            Instructions.LDX(Adr_modes.Immediate());
                            PC++;
                            break;

                        case 0xa4:
                            Instructions.LDY(Adr_modes.Zpg());
                            PC++;
                            break;

                        case 0xa8:
                            Instructions.TAY();
                            PC++;
                            break;

                        case 0xa9:
                            Instructions.LDA(Adr_modes.Immediate());
                            PC++;
                            break;

                        case 0xaa:
                            Instructions.TAX();
                            PC++;
                            break;

                        case 0xac:
                            Instructions.LDY(Adr_modes.Absolute());
                            PC++;
                            break;

                        case 0xb4:
                            Instructions.LDY(Adr_modes.Indexed_Zpg(X));
                            PC++;
                            break;

                        case 0xb8:
                            Instructions.CLV();
                            PC++;
                            break;

                        case 0xba:
                            Instructions.TSX();
                            PC++;
                            break;

                        case 0xbc:
                            Instructions.LDY(Adr_modes.Indexed(X));
                            PC++;
                            break;

                        case 0xc0:
                            Instructions.CPY(Adr_modes.Immediate());
                            PC++;
                            break;

                        case 0xc1:
                            Instructions.CMP(Adr_modes.Pre_Indexed());
                            PC++;
                            break;

                        case 0xc4:
                            Instructions.CPY(Adr_modes.Zpg());
                            PC++;
                            break;

                        case 0xc5:
                            Instructions.CMP(Adr_modes.Zpg());
                            PC++;
                            break;

                        case 0xc6:
                            data = Instructions.DEC(Adr_modes.Zpg());
                            Write();
                            PC++;
                            break;

                        case 0xc8:
                            Instructions.INY();
                            PC++;
                            break;

                        case 0xc9:
                            Instructions.CMP(Adr_modes.Immediate());
                            PC++;
                            break;

                        case 0xca:
                            Instructions.DEX();
                            PC++;
                            break;

                        case 0xcc:
                            Instructions.CPY(Adr_modes.Absolute());
                            PC++;
                            break;

                        case 0xcd:
                            Instructions.CMP(Adr_modes.Absolute());
                            PC++;
                            break;

                        case 0xce:
                            data = Instructions.DEC(Adr_modes.Absolute());
                            Write();
                            PC++;
                            break;

                        case 0xd1:
                            Instructions.CMP(Adr_modes.Post_Indexed());
                            PC++;
                            break;

                        case 0xd5:
                           Instructions.CMP(Adr_modes.Indexed_Zpg(X));
                           PC++;
                           break;

                        case 0xd6:
                            data = Instructions.DEC(Adr_modes.Indexed_Zpg(X));
                            Write();
                            PC++;
                            break;

                        case 0xd8:
                            Instructions.CLD();
                            PC++;
                            break;

                        case 0xd9:
                            Instructions.CMP(Adr_modes.Indexed(Y));
                            PC++;
                            break;

                        case 0xdd:
                            Instructions.CMP(Adr_modes.Indexed(X));
                            PC++;
                            break;

                        case 0xde:
                            data = Instructions.DEC(Adr_modes.Indexed(X));
                            Write();
                            PC++;
                            break;

                        case 0xe0:
                            Instructions.CPX(Adr_modes.Immediate());
                            PC++;
                            break;

                        case 0xe4:
                            Instructions.CPX(Adr_modes.Zpg());
                            PC++;
                            break;

                        case 0xe6:
                            data = Instructions.INC(Adr_modes.Zpg());
                            Write();
                            PC++;
                            break;

                        case 0xe8:
                            Instructions.INX();
                            PC++;
                            break;

                        case 0xe9:
                            Instructions.SBC(Adr_modes.Immediate());
                            PC++;
                            break;

                        case 0xea:
                            Instructions.NOP();
                            break;

                        case 0xec:
                            Instructions.CPX(Adr_modes.Absolute());
                            PC++;
                            break;

                        case 0xee:
                            data = Instructions.INC(Adr_modes.Absolute());
                            Write();
                            PC++;
                            break;

                        case 0xf6:
                            data = Instructions.INC(Adr_modes.Indexed_Zpg(X));
                            Write();
                            PC++;
                            break;

                        case 0xfe:
                            data = Instructions.INC(Adr_modes.Indexed(X));
                            Write();
                            PC++;
                            break;

                        default:
                            Console.WriteLine("invalide opcode: " + opCode);
                            return;
                    }
                    #endregion opCodes
            }
        }

        public static void run(string path)
        {
            ushort address = 0x0200;

            foreach(byte opCode in File.ReadAllBytes(path))
            {
                Memory.RAM[address] = opCode;
                address++;
            }

            Memory.RAM[0xfffc] = 0x00;
            Memory.RAM[0xfffd] = 0x02;

            Reset();
            execute();
        }

        public static void Dump_temp()
        {
            Console.WriteLine("Accumulator: " + A.ToString("X4"));
            Console.WriteLine("X and Y: " + X.ToString("X4") + " " + Y.ToString("X4"));
            Console.WriteLine("Stack Pointer: " + SP.ToString("X4"));
            Console.WriteLine("Program Counter: " + PC.ToString("X4"));
            Console.WriteLine("C flag: " + SR.C.ToString());
            Console.WriteLine("Z flag: " + SR.Z.ToString());
            Console.WriteLine("I flag: " + SR.I.ToString());
            Console.WriteLine("D flag: " + SR.D.ToString());
            Console.WriteLine("B flag: " + SR.B.ToString());
            Console.WriteLine("V flag: " + SR.V.ToString());
            Console.WriteLine("N flag: " + SR.N.ToString());
            Console.WriteLine(Memory.RAM[0x400].ToString("X4"));
        }

        public static void Dump()
        {
            string dir = Path.Combine(AppContext.BaseDirectory, "output");
            
            if(!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string fullPath = Path.Combine(dir, "output1.bin");
            File.WriteAllBytes(fullPath, Memory.RAM);
        }
    }
}