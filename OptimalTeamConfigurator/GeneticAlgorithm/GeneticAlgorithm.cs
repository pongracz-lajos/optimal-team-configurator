using Common;
using GeneticAlgorithm.Model;
using System;
using TeamConfigurator.Interfaces;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm
{
    /**
     * The genetic algorithm implementation.
     * 
     * @author Lajos L. Pongracz
     */
    public class GeneticAlgorithm : ISolver
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

        public int MiniPopulationSize
        {
            get; set;
        }

        public int PopulationSize
        {
            get; set;
        }

        public int GenotypeLength
        {
            get; set;
        }

        public int BitSize
        {
            get; set;
        }

        public int MaxStep
        {
            get; set;
        }

        public double Epsilon
        {
            get; set;
        }

        #endregion

        public GeneticAlgorithm(GeneticAlgorithmConfiguration configuration)
        {
            Elitism = configuration.Elitism;
            MutationRate = configuration.MutationRate;
            PopulationSize = configuration.PopulationSize;
            GenotypeLength = configuration.PeopleNumber;

            if (configuration.PeopleNumber % configuration.GroupSize == 0)
            {
                BitSize = configuration.PeopleNumber / configuration.GroupSize;
            }
            else
            {
                BitSize = configuration.PeopleNumber / configuration.GroupSize + 1;
            }

            MiniPopulationSize = 5;
        }

        /**
         * Evolve the population.
         * @param pop The population.
         * @return The evolved population.
         */
        private Population EvolvePopulation(Population population)
        {
            Population newPopulation = new Population(PopulationSize, BitSize, GenotypeLength);

            // Elitism.
            int offset = 0;
            if (Elitism)
            {
                newPopulation.CandidateSolutions[0] = population.getFittest();
                offset = 1;
            }

            // Crossover operation.
            for (int i = offset; i < newPopulation.CandidateSolutions.Length; i++)
            {
                CandidateSolution parent1 = selection(population);
                CandidateSolution parent2 = selection(population);

                CandidateSolution child = crossover(parent1, parent2);

                newPopulation.CandidateSolutions[i] = child;
            }

            // Mutation.
            for (int i = offset; i < newPopulation.CandidateSolutions.Length; i++)
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
        private CandidateSolution crossover(CandidateSolution parent1, CandidateSolution parent2)
        {
            Random random = new Random();
            int startPosition = random.Next(GenotypeLength);
            int endPosition = random.Next(GenotypeLength);

            if (startPosition > endPosition)
            {
                int tmp = startPosition;
                startPosition = endPosition;
                endPosition = tmp;
            }

            CandidateSolution child = new CandidateSolution(BitSize, GenotypeLength);

            for (int i = 0; i < GenotypeLength; i++)
            {
                if (startPosition <= i && i <= endPosition)
                {
                    child.Solution[i] = parent2.Solution[i];
                }
                else
                {
                    child.Solution[i] = parent1.Solution[i];
                }
            }

            return child;
        }

        /**
         * Mutation of a candidate solution.
         * @param candidateSolution The candidate solution.
         */
        private void mutate(CandidateSolution candidateSolution)
        {
            Random random = new Random();

            for (int i = 0; i < candidateSolution.Solution.Length; i++)
            {
                if (random.NextDouble() < MutationRate)
                {
                    candidateSolution.Solution[i] = random.Next(BitSize) + 1;
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
            int randomCandidate;

            Population miniPop = new Population(MiniPopulationSize, BitSize, GenotypeLength);

            for (int i = 0; i < MiniPopulationSize; i++)
            {
                randomCandidate = random.Next(population.CandidateSolutions.Length);
                miniPop.CandidateSolutions[i] = population.CandidateSolutions[randomCandidate];
            }

            CandidateSolution fittest = miniPop.getFittest();
            return fittest;
        }

        /**
         * Starts the genetic algorithm.
         * @param The background worker, so we can report progress.
         */
        public void Start(BackgroundWorker worker, ProblemResult result)
        {
            Population population = new Population(MiniPopulationSize, BitSize, GenotypeLength, true);
            int steps = 0;

            while (steps < MaxStep)
            {
                population = EvolvePopulation(population);

                if (steps % 10 == 0)
                {
                    var solution = population.getFittest();
                    var max = solution.Solution.Max();
                    var groups = new Dictionary<int, List<int>>();
                    for (int i = 1; i <= max; i++)
                    {
                        groups.Add(i, new List<int>());
                    }

                    for (int member = 0; member < solution.Solution.Length; member++)
                    {
                        groups[solution.Solution[member]].Add(member);
                    }

                    worker.ReportProgress(steps * 100 / MaxStep, new ProblemResult() { Groups = groups });
                }

                steps++;
            }
        }
    }
}
