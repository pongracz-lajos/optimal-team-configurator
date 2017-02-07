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
    public class GeneticAlgorithm : ISolver, IGeneticAlgorithmDelegate
    {
        #region "Public properties"

        /**
         * Mutation rate.
         */
        public double MutationProbability
        {
            get; set;
        }

        /**
         * Elitism on/off.
         */
        public double CrossoverProbability
        {
            get; set;
        }

        public double SurvivalProbability
        {
            get; set;
        }

        public int PopulationSize
        {
            get; set;
        }

        public int GroupSize
        {
            get; set;
        }

        public int GenotypeLength
        {
            get
            {
                return genotypeLength;
            }

            set
            {
                genotypeLength = value;

                if (GenotypeLength % GroupSize == 0)
                {
                    BitSize = GenotypeLength / GroupSize;
                }
                else
                {
                    BitSize = GenotypeLength / GroupSize + 1;
                }
            }
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

        public Dictionary<int, Dictionary<int, bool>> Relationships
        {
            get
            {
                return relationships;
            }
            set
            {
                relationships = value;
                fitnessCalculator = new FitnessCalculator(relationships);
            }
        }

        #endregion

        #region "Private fields"

        private Dictionary<int, Dictionary<int, bool>> relationships;

        private FitnessCalculator fitnessCalculator;

        private int genotypeLength;

        #endregion

        public GeneticAlgorithm(GeneticAlgorithmConfiguration configuration)
        {
            CrossoverProbability = configuration.CrossoverProbability;
            SurvivalProbability = configuration.SurvivalProbability;
            MutationProbability = configuration.MutationProbability;
            PopulationSize = configuration.PopulationSize;
            MaxStep = configuration.MaxStep;
            GroupSize = configuration.GroupSize;
            GenotypeLength = configuration.PeopleNumber;

            configuration.Delegate = this;
        }

        /**
         * Evolve the population.
         * @param pop The population.
         * @return The evolved population.
         */
        private Population EvolvePopulation(Population population)
        {
            Array.Sort(population.CandidateSolutions, delegate (CandidateSolution x, CandidateSolution y)
            {
                if (x.Fitness > y.Fitness)
                {
                    return 1;
                }
                else if (x.Fitness < y.Fitness)
                {
                    return -1;
                }

                return 0;
            });

            Population matingPool = GenerateMatingPool(population);
            Population offsetSpringPopulation = new Population(fitnessCalculator, PopulationSize, BitSize, GenotypeLength, GroupSize);

            // Crossover operation.
            Random random = new Random();
            for (int i = 0; i < offsetSpringPopulation.CandidateSolutions.Length; i++)
            {
                CandidateSolution child = null;
                var probability = random.NextDouble();

                if (CrossoverProbability < probability)
                {
                    child = matingPool.CandidateSolutions[i];
                }
                else
                {
                    CandidateSolution parent1 = matingPool.CandidateSolutions[i];
                    bool reject = true;
                    int trial = 0;

                    do
                    {
                        CandidateSolution parent2 = selection(matingPool);

                        child = crossover(parent1, parent2);

                        if (IsValid(child))
                        {
                            reject = false;
                        }
                        else
                        {
                            trial++;
                        }
                    }
                    while (reject && trial < 10);

                    if (trial == 10)
                    {
                        child = parent1;
                    }
                }

                offsetSpringPopulation.CandidateSolutions[i] = child;
            }

            // Mutation.
            for (int i = 0; i < offsetSpringPopulation.CandidateSolutions.Length; i++)
            {
                mutate(offsetSpringPopulation.CandidateSolutions[i]);
            }

            // Survivor selection
            Array.Sort(offsetSpringPopulation.CandidateSolutions, delegate (CandidateSolution x, CandidateSolution y)
            {
                if (x.Fitness > y.Fitness)
                {
                    return 1;
                }
                else if (x.Fitness < y.Fitness)
                {
                    return -1;
                }

                return 0;
            });

            Population newPopulation = new Population(fitnessCalculator, PopulationSize, BitSize, GenotypeLength, GroupSize);
            var parentCount = (int)(population.CandidateSolutions.Length * SurvivalProbability);
            var offsetSpringCount = (int)(offsetSpringPopulation.CandidateSolutions.Length * (1.0d - SurvivalProbability));

            while (parentCount + offsetSpringCount < PopulationSize)
            {
                if (parentCount % 2 == 1)
                {
                    parentCount++;
                }
                else
                {
                    offsetSpringCount++;
                }
            }

            int counter = 0;
            for (int i = 0; i < newPopulation.CandidateSolutions.Length; i++)
            {
                if (i < parentCount)
                {
                    newPopulation.CandidateSolutions[i] = population.CandidateSolutions[i];
                }
                else
                {
                    newPopulation.CandidateSolutions[i] = offsetSpringPopulation.CandidateSolutions[counter];
                    counter++;
                }
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

            CandidateSolution child = new CandidateSolution(fitnessCalculator, BitSize, GenotypeLength, GroupSize);

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
            if (random.NextDouble() < MutationProbability)
            {
                int i = random.Next(candidateSolution.Solution.Length);
                int j = random.Next(candidateSolution.Solution.Length);

                while (i == j)
                {
                    j = random.Next(candidateSolution.Solution.Length);
                }

                var temp = candidateSolution.Solution[i];
                candidateSolution.Solution[i] = candidateSolution.Solution[j];
                candidateSolution.Solution[j] = temp;
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

            return population.CandidateSolutions[random.Next(population.CandidateSolutions.Length)];
        }

        private Population GenerateMatingPool(Population population)
        {
            var cummulativeProbability = new double[population.CandidateSolutions.Length];
            var probabilities = new double[population.CandidateSolutions.Length];
            double sum = 0;

            for (int i = 0; i < population.CandidateSolutions.Length; i++)
            {
                sum += population.CandidateSolutions[i].Fitness;
            }

            probabilities[0] = population.CandidateSolutions[0].Fitness / sum;
            cummulativeProbability[0] = probabilities[0];
            for (int i = 1; i < population.CandidateSolutions.Length; i++)
            {
                probabilities[i] = population.CandidateSolutions[i].Fitness / sum;
                cummulativeProbability[i] = cummulativeProbability[i - 1] + probabilities[i];
            }

            Random random = new Random();
            double probability;

            Population matingPop = new Population(fitnessCalculator, PopulationSize, BitSize, GenotypeLength, GroupSize);

            for (int i = 0; i < PopulationSize; i++)
            {
                probability = random.NextDouble();
                int candidate = 0;

                for (int j = 1; j < population.CandidateSolutions.Length; j++)
                {
                    if (cummulativeProbability[j - 1] <= probability && cummulativeProbability[j] < probability)
                    {
                        candidate = j;
                        break;
                    }
                }

                matingPop.CandidateSolutions[i] = new CandidateSolution(population.CandidateSolutions[candidate]);
            }

            return matingPop;
        }

        /**
         * Starts the genetic algorithm.
         * @param The background worker, so we can report progress.
         */
        public void Start(BackgroundWorker worker, DoWorkEventArgs args)
        {
            Population population = new Population(fitnessCalculator, PopulationSize, BitSize, GenotypeLength, GroupSize, true);
            int steps = 0;

            CandidateSolution solution;
            int max;
            Dictionary<int, List<int>> groups;

            while (steps < MaxStep)
            {
                population = EvolvePopulation(population);

                if (steps % 2 == 0)
                {
                    solution = population.getFittest();
                    max = solution.Solution.Max();
                    groups = new Dictionary<int, List<int>>();
                    for (int i = 1; i <= max; i++)
                    {
                        groups.Add(i, new List<int>());
                    }

                    for (int member = 0; member < solution.Solution.Length; member++)
                    {
                        groups[solution.Solution[member]].Add(member + 1);
                    }

                    worker.ReportProgress(steps * 100 / MaxStep, new ProblemResult() { Groups = groups, Fitness = solution.Fitness });
                }

                steps++;
            }

            // Finished.
            solution = population.getFittest();
            max = solution.Solution.Max();
            groups = new Dictionary<int, List<int>>();
            for (int i = 1; i <= max; i++)
            {
                groups.Add(i, new List<int>());
            }

            for (int member = 0; member < solution.Solution.Length; member++)
            {
                groups[solution.Solution[member]].Add(member + 1);
            }
            args.Result = new ProblemResult() { Groups = groups, Fitness = solution.Fitness };
        }

        private bool IsValid(CandidateSolution solution)
        {
            var counter = new Dictionary<int, int>();
            for (int i = 1; i <= BitSize; i++)
            {
                counter.Add(i, 0);
            }

            for (int i = 0; i < solution.Solution.Length; i++)
            {
                counter[solution.Solution[i]]++;

                if (counter[solution.Solution[i]] > GroupSize)
                {
                    return false;
                }
            }

            return true;
        }

        public void Refresh(GeneticAlgorithmConfiguration geneticAlgorithmConfiguration)
        {
            GenotypeLength = geneticAlgorithmConfiguration.PeopleNumber;
        }
    }
}
