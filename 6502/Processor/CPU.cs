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

        public static void Fetch(byte[] memory)
        {
            data = memory[bus];
        }

        public static void Reset()
        {
            bus = 0xfffd;
            Fetch(RAM.memory);
            byte vector_1 = data;
            bus = 0xfffc;
            Fetch(RAM.memory);
            byte vector_2 = data;

            ushort vector = (ushort)((vector_1 << 8) | vector_2);

            Console.WriteLine(vector.ToString("X4"));
        }
    }
}
