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
            byte result = (byte)(CPU.A + value + carry);

            //set Flags
            CPU.PS.C = (int)(CPU.A + value + carry) > 0xff;
            CPU.PS.Z = result == 0;
            CPU.PS.N = result >= 0x80;
            CPU.PS.V = ((CPU.A ^ value) & 0x80) == 0x00 && ((CPU.A ^ result) & 0x80) != 0x00;

            CPU.A = result;
        }

        public void SBC(byte value)
        {
            byte carry = (CPU.PS.C ? (byte)1 : (byte)0);
            byte result = (byte)(CPU.A + (~value) + carry);
            ushort result16 = (ushort)(CPU.A + (~value) + carry);

            CPU.PS.C = (result16 & 0x100) == 0x00;
            CPU.PS.Z = result == 0;
            CPU.PS.N = result >= 0x00;
            CPU.PS.V = ((CPU.A ^ value) & 0x80) != 0x00 && ((CPU.A ^ result) & 0x80) != 0x00;

            CPU.A = result;
        }
    }
}
