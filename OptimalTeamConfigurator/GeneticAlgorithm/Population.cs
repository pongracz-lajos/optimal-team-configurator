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
        public Population(int size, int bitSize, int genotypeLength, bool initialise = false)
        {
            candidateSolutions = new CandidateSolution[size];

            if (initialise)
            {
                for (int i = 0; i < candidateSolutions.Length; i++)
                {
                    CandidateSolution candidateSolution = new CandidateSolution(bitSize, genotypeLength);
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