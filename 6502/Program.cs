using _6502.Processor;

namespace _6502
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = Console.ReadLine();
            
            CPU.run(path);
            Console.ReadKey();
            CPU.Dump_temp();
            CPU.Dump();
            Console.ReadKey();
        }
    }
}