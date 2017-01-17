using System.Collections.Generic;

namespace TeamConfigurator.Interfaces
{
    public class ProblemResult
    {
        public double Fitness
        {
            get; set;
        }

        public Dictionary<int, List<int>> Groups
        {
            get; set;
        }
    }
}