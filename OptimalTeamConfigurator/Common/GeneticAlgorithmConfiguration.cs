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

        public GeneticAlgorithmConfiguration()
        {
            Elitism = true;
            MutationRate = 0.015d;
        }
    }
}
