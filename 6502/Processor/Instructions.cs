using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        public static void AND(byte operand)
        {
            CPU.A = (byte)(CPU.A & operand);

            CPU.SR.Z = CPU.A == 0x00;
            CPU.SR.N = (CPU.A & 0x80) == 0x80;
        }

        public static byte ASL(byte operand)
        {
            CPU.SR.C = (operand & 0x80) == 0x80;

            operand = (byte)(operand << 1);

            CPU.SR.Z = operand == 0x00;
            CPU.SR.N = (operand & 0x80) == 0x80;

            return operand;
        }

        public static void BIT(byte operand)
        {
            CPU.SR.Z = (CPU.A & operand) == 0x00;
            CPU.SR.N = (operand & 0x80) == 0x80;
            CPU.SR.V = (operand & 0x40) == 0x40;
        }

        public static void CLC()
        {
            CPU.SR.C = false;
        }

        public static void CLD()
        {
            CPU.SR.D = false;
        }

        public static void CLV()
        {
            CPU.SR.V = false;
        }

        public static void CLI()
        {
            CPU.SR.I = false;
        }

        public static void CMP(byte operand)
        {
            operand = (byte)(CPU.A - operand);

            CPU.SR.Z = operand == 0x00;
            CPU.SR.N = (operand & 0x80) == 0x80;
            //TODO: set carry flag right
        }

        public static void CPX(byte operand)
        {
            operand = (byte)(CPU.X - operand);

            CPU.SR.Z = operand == 0x00;
            CPU.SR.N = (operand & 0x80) == 0x80;
            //TODO: set carry flag right
        }

        public static void CPY(byte operand)
        {
            operand = (byte)(CPU.Y - operand);

            CPU.SR.Z = operand == 0x00;
            CPU.SR.N = (operand & 0x80) == 0x80;
            //TODO: set carry flag right
        }

        public static byte DEC(byte operand)
        {
            operand--;

            CPU.SR.Z = operand == 0x00;
            CPU.SR.N = (operand & 0x80) == 0x80;

            return operand;
        }

        public static void DEX()
        {
            CPU.X--;

            CPU.SR.Z = CPU.X == 0x00;
            CPU.SR.N = (CPU.X & 0x80) == 0x80;
        }

        public static void DEY()
        {
            CPU.Y--;

            CPU.SR.Z = CPU.Y == 0x00;
            CPU.SR.N = (CPU.Y & 0x80) == 0x80;
        }
    }
}
