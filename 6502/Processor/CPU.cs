using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
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

        static byte low;
        static byte high;
        static byte zpAdr;
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
                            //ORA_X_ind
                            break;

                        case 0x05:
                            //ORA_zpg
                            break;

                        case 0x06:
                            data = Instructions.ASL(Adr_modes.Zpg());
                            Write();
                            PC++;
                            break;

                        case 0x08:
                            //PHP_impl
                            break;

                        case 0x09:
                            //ORA_immediate
                            break;

                        case 0x0a:
                            A = Instructions.ASL(A);
                            PC++;
                            break;

                        case 0x0d:
                            //ORA_abs
                            break;

                        case 0x0e:
                            data = Instructions.ASL(Adr_modes.Absolute());
                            Write();
                            PC++;
                            break;

                        case 0x10:
                            //BPL_rel
                            break;

                        case 0x11:
                            //ORA_ind_Y
                            break;

                        case 0x15:
                            //ORA_zpg_X
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
                            //ORA_abs_Y
                            break;

                        case 0x1d:
                            //ORA_abs_X
                            break;
                        case 0x1e:
                            data = Instructions.ASL(Adr_modes.Indexed(X));
                            Write();
                            PC++;
                            break;

                        case 0x20:
                            //JSR_abs
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
                            //RQL_zpg
                            break;

                        case 0x28:
                            //PLP_impl
                            break;

                        case 0x29:
                            Instructions.AND(Adr_modes.Immediate());
                            PC++;
                            break;

                        case 0x2a:
                            //ROL_A
                            break;

                        case 0x2c:
                            Instructions.BIT(Adr_modes.Absolute());
                            PC++;
                            break;

                        case 0x2d:
                            Instructions.AND(Adr_modes.Absolute());
                            break;

                        case 0x2e:
                            //ROL_abs
                            break;

                        case 0x30:
                            //BMI_rel
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
                            //ROL_zpg_X
                            break;

                        case 0x38:
                            //SEC_impl
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
                            //ROL_abs_X
                            break;

                        case 0x40:
                            //RTI
                            break;

                        case 0x41:
                            //EOR_X_ind
                            break;

                        case 0x45:
                            //EOR_zpg
                            break;

                        case 0x46:
                            //LSR_zpg
                            break;
                            
                        case 0x48:
                            Instructions.PHA();
                            PC++;
                            break;

                        case 0x49:
                            //EOR_immediate
                            break;
                        
                        case 0x4a:
                            //LSR_A
                            break;

                        case 0x4c:
                            //JMP_abs
                            break;

                        case 0x4d:
                            //EOR_abs
                            break;

                        case 0x4e:
                            //LSR_abs
                            break;

                        case 0x50:
                            //BVC_rel
                            break;

                        case 0x51:
                            //EOR_rel
                            break;

                        case 0x55:
                            //EOR_zpg_X
                            break;

                        case 0x56:
                            //LSR_zpg_X
                            break;

                        case 0x58:
                            Instructions.CLI();
                            PC++;
                            break;

                        case 0x59:
                            //EOR_abs_Y
                            break;

                        case 0x5d:
                            //EOR_abs_X
                            break;

                        case 0x5e:
                            //LSR_abs_X
                            break;

                        case 0x60:
                            //RTS_impl
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
                            //ROR_zpg
                            break;

                        case 0x68:
                            //PLA_impl
                            break;

                        case 0x69:
                            Instructions.ADC(Adr_modes.Immediate());
                            PC++;
                            break;

                        case 0x6a:
                            //ROR_A
                            break;

                        case 0x6c:
                            //JMP_ind
                            break;

                        case 0x6d:
                            Instructions.ADC(Adr_modes.Absolute());
                            PC++;
                            break;

                        case 0x6e:
                            //ROR_abs
                            break;

                        case 0x70:
                            //BVS_rel
                            break;

                        case 0x71:
                            Instructions.ADC(Adr_modes.Post_Indexed());
                            PC++;
                            break;

                        case 0x75:
                            Instructions.ADC(Adr_modes.Indexed_Zpg(X));
                            PC++;
                            break;

                        case 0xa2:
                            Instructions.LDX(Adr_modes.Immediate());
                            PC++;
                            break;

                        case 0xa9:
                            Instructions.LDA(Adr_modes.Immediate());
                            PC++;
                            break;

                        case 0xc5:
                            Instructions.CMP(Adr_modes.Zpg());
                            PC++;
                            break;

                        case 0xc9:
                            Instructions.CMP(Adr_modes.Immediate());
                            PC++;
                            break;

                        case 0xb8:
                            Instructions.CLV();
                            PC++;
                            break;

                        case 0xd5:
                           // Instructions.CMP(Adr_modes.pre

                        case 0xd8:
                            Instructions.CLD();
                            PC++;
                            break;

                        //TODO: add default case and the other opCodes
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

        public static void Dump()
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
            Console.WriteLine(Memory.RAM[0x80].ToString("X4"));
        }
    }
}