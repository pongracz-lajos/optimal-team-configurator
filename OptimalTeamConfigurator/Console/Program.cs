using Common;
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Algorith type (ga):");
            var algorithmType = Console.ReadLine();

            Console.WriteLine("Input:");
            var input = Console.ReadLine();

            if (args.Length != 2)
            {
                Console.Error.WriteLine("Error: few arguments! usage: solver {algorithm_type} {input_configuration}");
            }
            else
            {
                switch (args[0])
                {
                    case "ga":
                        var solverConfiguration = new SolverConfiguration<GeneticAlgorithmConfiguration>();
                        solverConfiguration.Path = args[1];

                        var solver = new GeneticAlgorithm.GeneticAlgorithm(solverConfiguration.Configuration);
                        solver.Solve();
                        break;
                }
            }

            Console.Read();
        }
    }
}
