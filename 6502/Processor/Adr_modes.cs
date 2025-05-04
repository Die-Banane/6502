using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using _6502.Processor;

namespace _6502
{
    public static class Adr_modes
    {
        public static byte Immediate()
        {
            CPU.PC++;
            CPU.bus = CPU.PC;
            CPU.Fetch();
            return CPU.data;
        }

        public static byte Absolute()
        {
            CPU.PC++;
            CPU.bus = CPU.PC;
            CPU.Fetch();
            byte low = CPU.data;

            CPU.PC++;
            CPU.bus = CPU.PC;
            CPU.Fetch();
            byte high = CPU.data;

            CPU.bus = (ushort)((high << 8) | low);
            CPU.Fetch();

            return CPU.data;
        }
        public static byte Zpg()
        {
            CPU.PC++;
            CPU.bus = CPU.PC;
            CPU.Fetch();
            CPU.bus = CPU.data;
            CPU.Fetch();
            return CPU.data;
        }

        public static byte Indexed(byte Register)
        {
            CPU.PC++;
            CPU.bus = CPU.PC;
            CPU.Fetch();
            byte low = CPU.data;

            CPU.PC++;
            CPU.bus = CPU.PC;
            CPU.Fetch();
            byte high = CPU.data;

            CPU.bus = (ushort)(((high << 8) | low) + Register);
            CPU.Fetch();
            return CPU.data;
        }
        
        public static byte Indexed_Zpg(byte Register)
        {
            CPU.PC++;
            CPU.bus = CPU.PC;
            CPU.Fetch();

            CPU.bus = (ushort)(CPU.data + Register);
            
            if(CPU.bus > 0xff)
            {
                CPU.bus = (ushort)(CPU.bus - 256);
            }

            CPU.Fetch();

            return CPU.data;
        }

        public static ushort Indirect()
        {
            CPU.PC++;
            CPU.bus = CPU.PC;
            CPU.Fetch();

            byte adr_low = CPU.data;

            CPU.PC++;
            CPU.bus = CPU.PC;
            CPU.Fetch();

            byte adr_high = CPU.data;

            ushort adr = (ushort)((adr_high << 8) | adr_low);

            CPU.bus = adr;
            CPU.Fetch();
            byte low = CPU.data;

            CPU.bus = (ushort)(adr + 1);
            CPU.Fetch();
            byte high = CPU.data;

            return (ushort)((high << 8) | low);
        }

        public static byte Pre_Indexed()
        {
            CPU.PC++;
            CPU.bus = CPU.PC;
            CPU.Fetch();

            CPU.bus = (ushort)(CPU.data + CPU.X);

            if(CPU.bus > 0xff)
            {
                CPU.bus = (ushort)(CPU.bus - 256);
            }

            CPU.Fetch();
            byte low = CPU.data;

            CPU.bus++;

            CPU.Fetch();
            byte high = CPU.data;

            CPU.bus = (ushort)((high << 8) | low);
            CPU.Fetch();

            return CPU.data;
        }

        public static byte Post_Indexed()
        {
            CPU.PC++;
            CPU.bus = CPU.PC;
            CPU.Fetch();

            CPU.bus = CPU.data;
            CPU.Fetch();

            byte low = CPU.data;

            CPU.bus++;

            if(CPU.bus > 0xff)
            {
                CPU.bus = (ushort)(CPU.bus - 256);
            }

            CPU.Fetch();

            byte high = CPU.data;

            CPU.bus = (ushort)(((high << 8) | low) + CPU.Y);

            CPU.Fetch();

            return CPU.data;
        }
    }
}