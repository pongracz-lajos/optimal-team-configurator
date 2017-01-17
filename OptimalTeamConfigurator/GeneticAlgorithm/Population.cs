using GeneticAlgorithm.Model;

namespace GeneticAlgorithm
{
    class Population
    {
        #region "Private members."

        /**
         * The concrete population.
         */
        private CandidateSolution[] candidateSolutions;

        private FitnessCalculator fitnessCalculator;

        private int groupSize;

        #endregion

        #region "Public properties"

        public CandidateSolution[] CandidateSolutions
        {
            get
            {
                return candidateSolutions;
            }
        }

        #endregion

        /**
         * Constructs a new population.
         * @param size The size of the population.
         * @param initialise True, if the population will be initialised.
         */
        public Population(FitnessCalculator fitnessCalculator, int size, int bitSize, int genotypeLength, int groupSize, bool initialise = false)
        {
            candidateSolutions = new CandidateSolution[size];
            this.fitnessCalculator = fitnessCalculator;
            this.groupSize = groupSize;

            if (initialise)
            {
                for (int i = 0; i < candidateSolutions.Length; i++)
                {
                    CandidateSolution candidateSolution = new CandidateSolution(fitnessCalculator, bitSize, genotypeLength, groupSize);
                    candidateSolution.GenerateIndividual();
                    candidateSolutions[i] = candidateSolution;
                }
            }
        }

        /**
         * @return The solution with the highest fitness.
         */
        public CandidateSolution getFittest()
        {
            CandidateSolution fittest = candidateSolutions[0];

            for (int i = 1; i < candidateSolutions.Length; i++)
            {
                if (fittest.Fitness < candidateSolutions[i].Fitness)
                {
                    fittest = candidateSolutions[i];
                }
            }

            return fittest;
        }
    }
}