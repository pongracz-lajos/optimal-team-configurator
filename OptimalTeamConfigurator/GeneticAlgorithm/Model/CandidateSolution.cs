using System;

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
        private double fitness;

        private int bitSize;

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
                if (fitness == 0d)
                {

                }

                return fitness;
            }
        }

        #endregion

        /**
         * Constructs a new empty candidate solution.
         */
        public CandidateSolution(int bitSize, int length)
        {
            solution = new int[length];
            fitness = 0d;
            this.bitSize = bitSize;
        }

        /**
         * Generates a new individual.
         */
        public void GenerateIndividual()
        {
            Random random = new Random();

            for (int i = 0; i < solution.Length; i++)
            {
                solution[i] = random.Next(bitSize) + 1;
            }
        }
    }
}
