﻿using Common;
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
            Elitism = configuration.Elitism;
            MutationRate = configuration.MutationRate;
            PopulationSize = configuration.PopulationSize;
            MaxStep = configuration.MaxStep;
            GroupSize = configuration.GroupSize;
            GenotypeLength = configuration.PeopleNumber;

            MiniPopulationSize = 5;

            configuration.Delegate = this;
        }

        /**
         * Evolve the population.
         * @param pop The population.
         * @return The evolved population.
         */
        private Population EvolvePopulation(Population population)
        {
            Population newPopulation = new Population(fitnessCalculator, PopulationSize, BitSize, GenotypeLength, GroupSize);

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
                CandidateSolution child = null;

                bool reject = true;
                do
                {
                    CandidateSolution parent1 = selection(population);
                    CandidateSolution parent2 = selection(population);

                    child = crossover(parent1, parent2);

                    if (IsValid(child))
                    {
                        reject = false;
                    }
                }
                while (reject);
                

                newPopulation.CandidateSolutions[i] = child;
            }

            // Mutation.
            for (int i = offset; i < newPopulation.CandidateSolutions.Length; i++)
            {
                var solution = new CandidateSolution(newPopulation.CandidateSolutions[i]);
                var trial = 0;
                bool reject = true;

                do
                {
                    mutate(solution);

                    if (IsValid(solution))
                    {
                        reject = false;
                    }
                    else
                    {
                        solution = new CandidateSolution(newPopulation.CandidateSolutions[i]);
                    }

                    trial++;
                }
                while (reject && trial < 20);
                
                if (!reject)
                {
                    newPopulation.CandidateSolutions[i] = solution;
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

            Population miniPop = new Population(fitnessCalculator, MiniPopulationSize, BitSize, GenotypeLength, GroupSize);

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
            Population population = new Population(fitnessCalculator, PopulationSize, BitSize, GenotypeLength, GroupSize, true);
            int steps = 0;

            while (steps < MaxStep)
            {
                population = EvolvePopulation(population);

                if (steps % 50 == 0)
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

                    worker.ReportProgress(steps * 100 / MaxStep, new ProblemResult() { Groups = groups, Fitness = solution.Fitness });
                }

                steps++;
            }
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
