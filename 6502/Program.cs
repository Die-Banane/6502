﻿using _6502.Processor;

namespace _6502
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CPU.PS.U = true;
            CPU.alu = new ALU();
            CPU.A = 0x50;
            CPU.PS.C = true;
            CPU.alu.ADC(0x30);
            Console.WriteLine(CPU.A.ToString("X") + " " + CPU.PS.C.ToString() + " " + CPU.PS.Z.ToString() + " " + CPU.PS.N.ToString() + " " + CPU.PS.V.ToString());
            Console.ReadKey();
        }
    }
}
