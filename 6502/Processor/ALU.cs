using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6502.Processor
{
    internal class ALU
    {
        public void ADDC(byte value)
        {
            byte carry = (CPU.PS.C ? (byte)1 : (byte)0);
            byte result = (byte)(CPU.A + value + carry);

            //set Flags
            CPU.PS.C = (CPU.A + value + carry) > 0xff;
            CPU.PS.Z = result == 0;
            CPU.PS.N = result >= 0x80;
            CPU.PS.V = (~(CPU.A ^ value) & (CPU.A ^ result) & 0x80) != 0;

            CPU.A = result;
        }
    }
}
