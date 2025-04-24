using System;
using System.Runtime.Intrinsics.X86;
using _6502.Processor;

namespace _6502
{
    public static class Adr_modes
    {
        public static byte Zpg_X()
        {
            CPU.PC++;
            CPU.bus = (ushort)((Memory.RAM[CPU.PC] + CPU.X));

            if(CPU.bus > 0xff)
            {
                CPU.bus = (ushort)(CPU.bus - 256);
            }

            CPU.Fetch();
            return CPU.data;
        }
    }
}