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
        /**
         * The chromosomes (genotype), an array of numbers, where the index is the ID of a person
         * and the value is the ID of the team to which it belongs.
         */
        private int[] solution;

        /**
         * The solution's fitness.
         */
        private double fitness;

        /**
         * Constructs a new empty candidate solution.
         */
        public CandidateSolution(int length)
        {
            solution = new int[length];
            fitness = 0d;
        }

        /**
         * Constructs a copy of a candidate solution from a genotype.
         * @param solution The genotype.
         */
        public CandidateSolution(int[] solution)
        {
            this.solution = new int[solution.Length];

            for (int i = 0; i < solution.Length; i++)
            {
                this.solution[i] = solution[i];
            }

            fitness = 0d;
        }

        /**
         * Generates a new individual.
         */
        public void GenerateIndividual()
        {
            Random random = new Random();
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
    }
}
