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
        #region "Private fields"

        /**
         * Mutation rate.
         */
        private double mutationRate = 0.015;

        /**
         * The mini population size for selection.
         */
        private int miniPopSize = 5;

        /**
         * Elitism on/off.
         */
        private bool elitism = true;

        #endregion

        public GeneticAlgorithm()
        {

        }

        

	/**
	 * Evolve the population.
	 * @param pop The population.
	 * @return The evolved population.
	 */
	public static Population evolvePopulation(Population population)
        {
            Population newPopulation = new Population(population.populationSize(), false);

            // Elitism.
            int elitismOffset = 0;
            if (elitism)
            {
                newPopulation.saveDrawing(0, population.getFittest());
                elitismOffset = 1;
            }

            // Crossover operation.
            for (int i = elitismOffset; i < newPopulation.populationSize(); i++)
            {
                CandidateSolution parent1 = selection(population);
                CandidateSolution parent2 = selection(population);

                CandidateSolution child = crossover(parent1, parent2);
                newPopulation.saveDrawing(i, child);
            }

            // Mutation.
            for (int i = elitismOffset; i < newPopulation.populationSize(); i++)
            {
                mutate(newPopulation.getDrawing(i));
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
         * Mutation of a drawing.
         * @param drawing The drawing.
         */
        private static void mutate(CandidateSolution drawing)
        {
            float x, y;
            Random random = new Random();

            for (int i = 0; i < drawing.drawingSize(); i++)
            {
                if (random.nextDouble() < mutationRate)
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
        private static CandidateSolution selection(Population population)
        {
            Random random = new Random();
            int randomDrawing;

            Population miniPop = new Population(miniPopSize, false);

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
