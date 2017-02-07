using YAXLib;

namespace Common
{
    public class GeneticAlgorithmConfiguration
    {
        public double SurvivalProbability
        {
            get; set;
        }

        public double MutationProbability
        {
            get; set;
        }

        public double CrossoverProbability
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

        public int GroupSize
        {
            get; set;
        }

        public int MaxStep
        {
            get; set;
        }

        [YAXDontSerialize]
        public int PeopleNumber
        {
            get
            {
                return peopleNumber;
            }
            set
            {
                peopleNumber = value;
                Delegate.Refresh(this);
            }
        }

        public IGeneticAlgorithmDelegate Delegate
        {
            get; set;
        }

        private int peopleNumber;

        public GeneticAlgorithmConfiguration()
        {
            SurvivalProbability = 0.5;
            MutationProbability = 0.015d;
            MaxStep = 1000;
            PopulationSize = 100;
            GroupSize = 10;
        }
    }
}
