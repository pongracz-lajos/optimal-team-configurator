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
            Elitism = true;
            MutationRate = 0.015d;
            MaxStep = 1000;
            PopulationSize = 100;
            GroupSize = 10;
        }
    }
}
