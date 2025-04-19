using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

        public static Status PS; //Processor Status

        public static ALU alu; //Arithmetic Logic Unit
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

        private static void push()
        {
            RAM.memory[0x0100 + SP] = data;
            SP--;
        }

        private static void pull()
        {
            SP++;
            data = RAM.memory[0x100 + SP];
        }

        public static void Fetch()
        {
            if(bus >= 0x0100 && bus <= 0x01ff)
            {
                pull();
            }
            else
            {
                data = RAM.memory[bus];
            }
        }

        public static void Reset()
        {
            bus = 0xfffc;
            Fetch();
            byte vector_low = data;

            bus = 0xfffd;
            Fetch();
            byte vector_high = data;

            PS.B = true;
            PS.D = false;
            PS.I = true;
            PS.U = true;

            PC = (ushort)((vector_high << 8) | vector_low);
            SP = 0xff;
        }
    }
}
