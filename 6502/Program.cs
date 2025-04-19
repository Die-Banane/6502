using _6502.Processor;

namespace _6502
{
    internal class Program
    {
        static void Main(string[] args)
        {   
            RAM.memory = new byte[0xffff];

            RAM.memory[0xfffd] = 0x80;
            RAM.memory[0xfffc] = 0x00;

            CPU.Reset();
            Console.WriteLine(CPU.PC.ToString("X4"));
            Console.ReadKey();
        }
    }
}
