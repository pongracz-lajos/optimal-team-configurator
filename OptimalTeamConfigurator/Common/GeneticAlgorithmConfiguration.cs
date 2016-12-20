using YAXLib;

namespace Common
{
    public class GeneticAlgorithmConfiguration
    {
        public bool Elitism
        {
            get; set;
        }

        public double MutationRate
        {
            get; set;
        }

        public int PopulationSize
        {
            get; set;
        }

        public string File
        {
            get; set;
        }

        [YAXDontSerialize]
        public int PeopleNumber
        {
            get; set;
        }

        [YAXDontSerialize]
        public int GroupSize
        {
            get; set;
        }

        public GeneticAlgorithmConfiguration()
        {
            Elitism = true;
            MutationRate = 0.015d;
        }
    }
}
