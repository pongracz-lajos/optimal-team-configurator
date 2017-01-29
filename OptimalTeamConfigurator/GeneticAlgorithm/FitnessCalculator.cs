using GeneticAlgorithm.Model;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm
{
    public class FitnessCalculator
    {
        private Dictionary<int, Dictionary<int, bool>> relationships;

        public FitnessCalculator(Dictionary<int, Dictionary<int, bool>> relationships)
        {
            this.relationships = relationships;
        }

        public double Calculate(CandidateSolution solution)
        {
            double sum = 0d;
            var groups = GetGroups(solution);

            foreach (var group in groups.Keys)
            {
                var groupSum = 0;
                var members = groups[group].ToArray();

                for (int memberOne = 0; memberOne < members.Length; memberOne++)
                {
                    for (int memberTwo = 0; memberTwo < members.Length; memberTwo++)
                    {
                        if (memberOne != memberTwo)
                        {
                            if (relationships[members[memberOne]].ContainsKey(members[memberTwo]))
                            {
                                groupSum++;
                            }
                        }
                    }
                }

                sum += groupSum;
            }

            return sum;
        }

        private Dictionary<int, List<int>> GetGroups(CandidateSolution solution)
        {
            var max = solution.Solution.Max();
            var groups = new Dictionary<int, List<int>>();
            for (int i = 1; i <= max; i++)
            {
                groups.Add(i, new List<int>());
            }

            for (int member = 0; member < solution.Solution.Length; member++)
            {
                groups[solution.Solution[member]].Add(member + 1);
            }

            return groups;
        }
    }
}