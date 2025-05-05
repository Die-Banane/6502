using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Security.Cryptography;
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

        public static void EOR(byte operand)
        {
            CPU.A = (byte)(CPU.A ^ operand);

            CPU.SR.Z = CPU.A == 0x00;
            CPU.SR.N = (CPU.A & 0x80) == 0x80;
        }

        public static byte INC(byte operand)
        {
            operand++;

            CPU.SR.Z = operand == 0x00;
            CPU.SR.N = (operand & 0x80) == 0x80;

            return operand;
        }

        public static void INX()
        {
            CPU.X++;

            CPU.SR.Z = CPU.X == 0x00;
            CPU.SR.N = (CPU.X & 0x80) == 0x80;            
        }

        public static void INY()
        {
            CPU.Y++;

            CPU.SR.Z = CPU.Y == 0x00;
            CPU.SR.N = (CPU.Y & 0x80) == 0x80;  
        }

        public static void JMP(ushort address)
        {
            CPU.PC = (ushort)(address + 0x199);
        }

        public static void JSR(ushort address)
        {
            CPU.bus = (ushort)(0x0100 + CPU.SP);

            CPU.data = (byte)(CPU.PC + 2);

            CPU.Write();
            CPU.SP--;

            JMP(address);
        }

        public static byte LSR(byte operand)
        {
            CPU.SR.C = (operand & 0x01) == 0x01;

            operand = (byte)(operand >> 1);

            CPU.SR.Z = operand == 0x00;
            CPU.SR.N = false;

            return operand;
        }

        public static void NOP()
        {
            CPU.PC++;
        }

        public static void ORA(byte operand)
        {
            CPU.A = (byte)(CPU.A | operand);

            CPU.SR.N = (CPU.A & 0x80) == 0x80;
            CPU.SR.Z = CPU.A == 0x00;
        }

        public static void PHP()
        {
            CPU.bus = (ushort)(0x100 + CPU.SP);
            CPU.data = CPU.SRToByte();
            CPU.Write();
            CPU.SP--;
        }

        public static void PLA()
        {
            CPU.bus = (ushort)(0x100 + CPU.SP);
            CPU.Fetch();

            CPU.A = CPU.data;

            CPU.SR.N = (CPU.A & 0x80) == 0x80;
            CPU.SR.Z = CPU.A == 0x00;

            CPU.SP++;
        }

        public static byte ROL(byte operand)
        {
            bool carryTemp = CPU.SR.C;
            CPU.SR.C = (operand & 0x80) == 0x80;

            operand = (byte)(operand << 1);
            operand |= carryTemp ? (byte)1 : (byte)0;

            CPU.SR.N = (operand & 0x80) == 0x80;
            CPU.SR.Z = operand == 0x00;

            return operand;
        }

        public static byte ROR(byte operand)
        {
            bool carryTemp = CPU.SR.C;
            CPU.SR.C = (operand & 0x01) == 0x01;

            operand = (byte)(operand >> 1);
            operand |= carryTemp ? (byte)0x80 : (byte)0x00;

            CPU.SR.N = (operand & 0x80) == 0x80;
            CPU.SR.Z = operand == 0x00;

            return operand;
        }

        public static void RTI()
        {
            CPU.bus = (ushort)(0x100 + CPU.SP);
            CPU.Fetch();
            CPU.ByteToSR(CPU.data);
            
            CPU.SP++;

            CPU.bus = (ushort)(0x100 + CPU.SP);
            CPU.Fetch();
            CPU.PC = CPU.data;

            CPU.SP++;
        }

        public static void RTS()
        {
            CPU.bus = (ushort)(0x100 + CPU.SP);
            CPU.Fetch();
            CPU.PC = CPU.data;

            CPU.SP++;

            CPU.PC++;
        }

        public static void STA(ushort address)
        {
            Memory.RAM[address] = CPU.A;
        }

        public static void STX(ushort address)
        {
            Memory.RAM[address] = CPU.X;
        }

        public static void STY(ushort address)
        {
            Memory.RAM[address] = CPU.Y;
        }

        public static void TAX()
        {
            CPU.X = CPU.A;

            CPU.SR.N = (CPU.X & 0x80) == 0x80;
            CPU.SR.Z = CPU.X == 0x00;
        }

        public static void TAY()
        {
            CPU.Y = CPU.A;

            CPU.SR.N = (CPU.Y & 0x80) == 0x80;
            CPU.SR.Z = CPU.Y == 0x00;
        }

        public static void TSX()
        {
            CPU.X = CPU.SP;

            CPU.SR.N = (CPU.X & 0x80) == 0x80;
            CPU.SR.Z = CPU.X == 0x00;
        }

        public static void TXA()
        {
            CPU.A = CPU.X;

            CPU.SR.N = (CPU.A & 0x80) == 0x80;
            CPU.SR.Z = CPU.A == 0x00;
        }

        public static void TXS()
        {
            CPU.SP = CPU.X;
        }

        public static void TYA()
        {
            CPU.A = CPU.Y;

            CPU.SR.N = (CPU.A & 0x80) == 0x80;
            CPU.SR.Z = CPU.A == 0x00;
        }

        public static void BCC(sbyte offset)
        {
            if(!CPU.SR.C)
            {
                CPU.PC = (ushort)(sbyte)(CPU.PC + offset);
            }
            else
            {
                CPU.PC++;
            }
        }

        public static void BCS(sbyte offset)
        {
            if(CPU.SR.C)
            {
                CPU.PC = (ushort)(sbyte)(CPU.PC + offset);
            }
            else
            {
                CPU.PC++;
            }
        }

        public static void BEQ(sbyte offset)
        {
            if(CPU.SR.Z)
            {
                CPU.PC = (ushort)(sbyte)(CPU.PC + offset);
            }
            else
            {
                CPU.PC++;
            }
        }

        public static void BMI(sbyte offset)
        {
            if(CPU.SR.N)
            {
                CPU.PC = (ushort)(sbyte)(CPU.PC + offset);
            }
            else
            {
                CPU.PC++;
            }
        }

        public static void BNE(sbyte offset)
        {
            if(!CPU.SR.Z)
            {
                CPU.PC = (ushort)(sbyte)(CPU.PC + offset);
            }
            else
            {
                CPU.PC++;
            }
        }

        public static void BPL(sbyte offset)
        {
            if(!CPU.SR.N)
            {
                CPU.PC = (ushort)(sbyte)(CPU.PC + offset);
            }
            else
            {
                CPU.PC++;
            }
        }

        public static void BVC(sbyte offset)
        {
            if(!CPU.SR.V)
            {
                CPU.PC = (ushort)(sbyte)(CPU.PC + offset);
            }
            else
            {
                CPU.PC++;
            }
        }

        public static void BVS(sbyte offset)
        {
            if(CPU.SR.V)
            {
                CPU.PC = (ushort)(sbyte)(CPU.PC + offset);
            }
            else
            {
                CPU.PC++;
            }
        }

        public static void SEC()
        {
            CPU.SR.C = true;
        }

        public static void PLP()
        {
            CPU.bus = (ushort)(0x100 + CPU.SP);
            CPU.Fetch();
            CPU.ByteToSR(CPU.data);

            CPU.SP++;
        }
    }
}