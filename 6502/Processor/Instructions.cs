using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6502.Processor
{
    public static class Instructions
    {
        public static void ADC(byte operand)
        {
            byte carry = (CPU.SR.C ? (byte)1 : (byte)0);
            byte result = (byte)(CPU.A + operand + carry);

            //set Flags
            CPU.SR.C = (int)(CPU.A + operand + carry) > 0xff;
            CPU.SR.Z = result == 0;
            CPU.SR.N = result >= 0x80;
            CPU.SR.V = ((CPU.A ^ operand) & 0x80) == 0x00 && ((CPU.A ^ result) & 0x80) != 0x00;

            CPU.A = result;
        }

        public static void SBC(byte operand)
        {
            byte carry = (CPU.SR.C ? (byte)1 : (byte)0);
            byte result = (byte)(CPU.A + (~operand) + carry);
            ushort result16 = (ushort)(CPU.A + (~operand) + carry);

            CPU.SR.C = (result16 & 0x100) == 0x00;
            CPU.SR.Z = result == 0;
            CPU.SR.N = result >= 0x00;
            CPU.SR.V = ((CPU.A ^ operand) & 0x80) != 0x00 && ((CPU.A ^ result) & 0x80) != 0x00;

            CPU.A = result;
        }

        public static void PHA()
        {
            CPU.bus = (ushort)(0x0100 + CPU.SP);

            CPU.data = CPU.A;

            CPU.Write();

            CPU.SP--;
            CPU.PC++;
        }

        public static void LDA(byte operand)
        {
            CPU.A = operand;
        }

        public static void LDX(byte operand)
        {
            CPU.X = operand;
        }

        public static void LDY(byte operand)
        {
            CPU.Y = operand;
        }

        public static void CLC()
        {
            CPU.SR.C = false;
        }

        public static void AND(byte operand)
        {
            CPU.A = (byte)(CPU.A & operand);

            CPU.SR.Z = CPU.A == 0x00;
            CPU.SR.N = (CPU.A & 0x80) == 0x80;
        }
    }
}
