using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6502.Processor
{
    internal class ALU
    {
        public void ADC(byte value)
        {
            byte carry = (CPU.PS.C ? (byte)1 : (byte)0);
            CPU.A = (byte)(CPU.A + value + carry);

            //set Flags
            CPU.PS.C = (CPU.A + value + carry) > 0xff;
            CPU.PS.Z = CPU.A == 0;
            CPU.PS.N = CPU.A >= 0x80;
            CPU.PS.V = (~(CPU.A ^ value) & (CPU.A ^ CPU.A) & 0x80) != 0;
        }
    }
}
