using GeneticAlgorithm.Model;

namespace GeneticAlgorithm
{
    class Population
    {
        /**
         * The list of drawings.
         */
        CandidateSolution[] candidateSolutions;

        /**
         * Constructs a new population.
         * @param populationSize The size of the population.
         * @param initialise True, if the population will be initialised.
         */
        public Population(int populationSize, bool initialise)
        {
            candidateSolutions = new CandidateSolution[populationSize];

            if (initialise)
            {
                for (int i = 0; i < this.populationSize(); i++)
                {
                    CandidateSolution drawing = new CandidateSolution(5);
                    drawing.GenerateIndividual();
                    saveCandidateSolution(i, drawing);
                }
            }
        }

        /**
         * Saves the candidate solution at the given index.
         * @param index Index.
         * @param candidateSolutions CandidateSolution.
         */
        public void saveCandidateSolution(int index, CandidateSolution candidateSolution)
        {
            candidateSolutions[index] = candidateSolution;
        }

        /**
         * @param index Index.
         * @return The drawing at the index.
         */
        public CandidateSolution getDrawing(int index)
        {
            return candidateSolutions[index];
        }

        /**
         * @return The drawing with the highest fitness.
         */
        public CandidateSolution getFittest()
        {
            CandidateSolution fittest = candidateSolutions[0];

            for (int i = 1; i < populationSize(); i++)
            {
                if (fittest.Fitness <= getDrawing(i).Fitness)
                {
                    fittest = getDrawing(i);
                }
            }

            return fittest;
        }

        /**
         * @return The size of the population.
         */
        public int populationSize()
        {
            return candidateSolutions.Length;
        }
    }
}