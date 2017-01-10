using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generating problem input.");

            var distrib1 = "IGD";
            var distrib2 = "IGD";

            ICollection<Tuple<int, int>> list = new List<Tuple<int, int>>();
            // Number of people.
            int N = 1000;
            double p = 0.5;

            //var random1 = new Normal(0.0, 0.5);
            //var random2 = new Normal(0.0, 0.5);

            //var random1 = new Normal(0.0, 0.5);
            //var random2 = new InverseGamma(3.0, 1.0);

            //var random1 = new Normal(0.0, 0.5);
            //var random2 = new LogNormal(0.5, 0.0);

            //var random1 = new Normal(0.0, 0.5);
            //var random2 = new ContinuousUniform(0.0, 1.0);

            //var random1 = new ContinuousUniform(0.0, 1.0);
            //var random2 = new ContinuousUniform(0.0, 1.0);

            //var random1 = new ContinuousUniform(0.0, 1.0);
            //var random2 = new InverseGamma(3.0, 1.0);

            //var random1 = new ContinuousUniform(0.0, 1.0);
            //var random2 = new LogNormal(0.5, 0.0);

            //var random1 = new LogNormal(0.5, 0.0);
            //var random2 = new LogNormal(0.5, 0.0);

            //var random1 = new InverseGamma(3.0, 1.0);
            //var random2 = new LogNormal(0.5, 0.0);

            var random1 = new InverseGamma(3.0, 1.0);
            var random2 = new InverseGamma(3.0, 1.0);

            for (int i = 1; i <= N; i++)
            {
                for (int j = 1; j <= N; j++)
                {
                    if (i != j)
                    {
                        double value = 0.0;
                        if (i < j)
                        {
                            value = random1.Sample();
                        }
                        else
                        {
                            value = random2.Sample();
                        }

                        if (value < p)
                        {
                            list.Add(new Tuple<int, int>(i, j));
                        }
                    }
                }
            }

            var file = new StreamWriter(string.Format("input-{0}-{1}-{2}-{3}.dat", distrib1, distrib2, N, list.Count));
            file.WriteLine(string.Format("{0} {1}", N, list.Count));

            foreach (var pair in list)
            {
                file.WriteLine(string.Format("{0} {1}", pair.Item1, pair.Item2));
            }

            file.Close();

            Console.WriteLine("Input for problem generated.");
            Console.ReadKey();
        }
    }
}
