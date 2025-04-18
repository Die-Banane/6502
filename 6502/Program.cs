using _6502.Processor;

namespace _6502
{
    internal class Program
    {
        static void Main(string[] args)
        {   
            RAM.memory = new byte[0xffff];

            RAM.memory[0xfffd] = 0xea;
            RAM.memory[0xfffc] = 0xea;

            CPU.Reset();
            Console.ReadKey();
        }
    }
}
