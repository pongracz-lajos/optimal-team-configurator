using System;
using System.Collections.Generic;

namespace GeneticAlgorithm.Model
{
    /**
     * A candidate solution (genetic representation/individual).
     * 
     * @author Lajos L. Pongracz
     */
    public class CandidateSolution
    {
        #region "Private fields"

        /**
         * The chromosomes (genotype), an array of numbers, where the index is the ID of a person
         * and the value is the ID of the team to which it belongs.
         */
        private int[] solution;

        /**
         * The solution's fitness.
         */
        private double? fitness;

        private int bitSize;

        private FitnessCalculator fitnessCalculator;

        private int groupSize;

        #endregion

        #region "Public properties"

        public int[] Solution
        {
            get
            {
                return solution;
            }
        }

        public double Fitness
        {
            get
            {
                if (!fitness.HasValue)
                {
                    fitness = fitnessCalculator.Calculate(this);
                }

                return fitness.Value;
            }
        }

        #endregion

        /**
         * Constructs a new empty candidate solution.
         */
        public CandidateSolution(FitnessCalculator fitnessCalculator, int bitSize, int length, int groupSize)
        {
            solution = new int[length];
            fitness = null;
            this.bitSize = bitSize;
            this.fitnessCalculator = fitnessCalculator;
            this.groupSize = groupSize;
        }

        public CandidateSolution(CandidateSolution solution)
        {
            this.solution = new int[solution.Solution.Length];
            fitness = null;
            bitSize = solution.bitSize;
            fitnessCalculator = solution.fitnessCalculator;
            groupSize = solution.groupSize;

            for (int member = 0; member < solution.Solution.Length; member++)
            {
                this.solution[member] = solution.Solution[member];
            }
        }

        /**
         * Generates a new individual.
         */
        public void GenerateIndividual()
        {
            Random random = new Random();

            var counter = new Dictionary<int, int>();
            for (int i = 1; i <= bitSize; i++)
            {
                counter.Add(i, 0);
            }

            for (int i = 0; i < solution.Length; i++)
            {
                var reject = true;
                do
                {
                    var bit = random.Next(bitSize) + 1;
                    if (counter[bit] + 1 <= groupSize)
                    {
                        solution[i] = bit;
                        counter[bit] += 1;
                        reject = false;
                    }
                }
                while (reject);

            }
        }
    }
}
