using _6502.Processor;

namespace _6502
{
    internal class Program
    {
        static void Main(string[] args)
        {   
            CPU.run(Console.ReadLine());
            Console.ReadKey();
            CPU.Dump();
            Console.ReadKey();
        }
    }
}
