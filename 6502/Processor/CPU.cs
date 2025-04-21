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

        public static ALU alu = new ALU(); //Arithmetic Logic Unit
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

                if(bus > 0x01ff || bus < 0x0100)
                {
                    Fetch();

                    byte opCode = data;

                    switch(opCode)
                    {
                        case 0x00:
                            return;
                        
                        case 0x01:
                            //ORA_X_ind
                            break;

                        case 0x05:
                            //ORA_zpg
                            break;

                        case 0x06:
                            //ASL_zpg
                            break;

                        case 0x08:
                            //PHP_impl
                            break;

                        case 0x09:
                            //ORA_immediate
                            break;

                        case 0x0a:
                            //ASL_A
                            break;

                        case 0x0d:
                            //ORA_abs
                            break;

                        case 0x0e:
                            //ASL_abs
                            break;

                        case 0x48:
                            PHA();
                            break;

                        case 0xa9:
                            PC++;
                            alu.ADC(Memory.RAM[PC]);
                            PC++;
                            break;

                        //TODO: add default case and the other opCodes
                    }
                }
                else
                {
                    PC++;
                }
            }
        }

        private static void PHA()
        {
            bus = (ushort)(0x0100 + SP);

            data = A;

            Write();

            SP--;
            PC++;
        }

        public static void run(string path)
        {
            ushort address = 0;

            foreach(byte opCode in File.ReadAllBytes(path))
            {
                Memory.RAM[address] = opCode;
                address++;
            }

            Reset();
            execute();
        }

        public static void Dump()
        {
            Console.WriteLine("Accumulator: " + A.ToString("X4"));
            Console.WriteLine("X and Y: " + X.ToString("X4") + " " + Y.ToString("X4"));
            Console.WriteLine("Stack Pointer: " + SP.ToString("X4"));
            Console.WriteLine("Program Counter: " + PC.ToString("X4"));
            Console.WriteLine("current opCode: " + data.ToString("4X"));
            Console.WriteLine("Current Address: " + bus.ToString("X4"));
            Console.WriteLine("C flag: " + SR.C.ToString());
            Console.WriteLine("Z flag: " + SR.Z.ToString());
            Console.WriteLine("I flag: " + SR.I.ToString());
            Console.WriteLine("D flag: " + SR.D.ToString());
            Console.WriteLine("B flag: " + SR.B.ToString());
            Console.WriteLine("V flag: " + SR.V.ToString());
            Console.WriteLine("N flag: " + SR.N.ToString());
            Console.WriteLine(Memory.RAM[0x01FF].ToString("X4"));
        }
    }
}