using Common;
using GeneticAlgorithm.Model;
using System;

namespace GeneticAlgorithm
{
    /**
     * The genetic algorithm implementation.
     * 
     * @author Lajos L. Pongracz
     */
    class GeneticAlgorithm
    {
        #region "Public properties"

        /**
         * Mutation rate.
         */
        public double MutationRate
        {
            get; set;
        }

        /**
         * Elitism on/off.
         */
        public bool Elitism
        {
            get; set;
        }

        #endregion

        public GeneticAlgorithm(SolverConfiguration<GeneticAlgorithmConfiguration> configuration)
        {
            Elitism = configuration.Configuration.Elitism;
            MutationRate = configuration.Configuration.MutationRate;
        }

        /**
         * Evolve the population.
         * @param pop The population.
         * @return The evolved population.
         */
        public Population EvolvePopulation(Population population)
        {
            Population newPopulation = new Population(population.CandidateSolutions.Length, false);

            // Elitism.
            int elitismOffset = 0;
            if (Elitism)
            {
                newPopulation.CandidateSolutions[0] = population.getFittest();
                elitismOffset = 1;
            }

            // Crossover operation.
            for (int i = elitismOffset; i < newPopulation.CandidateSolutions.Length; i++)
            {
                CandidateSolution parent1 = selection(population);
                CandidateSolution parent2 = selection(population);

                CandidateSolution child = crossover(parent1, parent2);

                newPopulation.CandidateSolutions[i] = child;
            }

            // Mutation.
            for (int i = elitismOffset; i < newPopulation.CandidateSolutions.Length; i++)
            {
                mutate(newPopulation.CandidateSolutions[i]);
            }

            return newPopulation;
        }

        /**
         * Two-point crossover.
         * @param parent1 The first parent.
         * @param parent2 The second parent.
         * @return The child drawing.
         */
        private static CandidateSolution crossover(CandidateSolution parent1, CandidateSolution parent2)
        {
            Random random = new Random();
            int startPos = random.nextInt(parent1.drawingSize());
            int endPos = random.nextInt(parent1.drawingSize());

            if (startPos > endPos)
            {
                int tmp = startPos;
                startPos = endPos;
                endPos = tmp;
            }

            CandidateSolution child = new CandidateSolution();

            for (int i = 0; i < parent1.drawingSize(); i++)
            {
                if (startPos <= i && i <= endPos)
                {
                    child.getDrawing().add(new Node(parent2.getNode(i)));
                }
                else
                {
                    child.getDrawing().add(new Node(parent1.getNode(i)));
                }
            }

            return child;
        }

        /**
         * Mutation of a candidate solution.
         * @param drawing The drawing.
         */
        private void mutate(CandidateSolution candidateSolution)
        {
            float x, y;
            Random random = new Random();

            for (int i = 0; i < drawing.drawingSize(); i++)
            {
                if (random.nextDouble() < MutationRate)
                {
                    x = random.nextFloat() * 10.0f;
                    y = random.nextFloat() * 10.0f;

                    drawing.getNode(i).setX(x);
                    drawing.getNode(i).setY(y);
                }
            }
        }

        /**
         * Selection.
         * @param population The population.
         * @return A drawing from the population.
         */
        private CandidateSolution selection(Population population)
        {
            Random random = new Random();
            int randomDrawing;

            Population miniPop = new Population(miniPopSize);

            for (int i = 0; i < miniPopSize; i++)
            {
                randomDrawing = random.nextInt(population.populationSize());
                miniPop.saveDrawing(i, population.getDrawing(randomDrawing));
            }

            CandidateSolution fittest = miniPop.getFittest();
            return fittest;
        }
    }
}
