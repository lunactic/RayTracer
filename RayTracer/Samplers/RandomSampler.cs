using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Samplers
{
    public class RandomSampler : ISampler
    {
        public int NumberOfSamples { get; set; }

        private Random random;

        public RandomSampler(int numOfSamples)
        {
            NumberOfSamples = numOfSamples;
            random = new Random();
        }
        public List<Sample> CreateSamples(int x, int y)
        {
            List<Sample> samples = new List<Sample>();
            for (int i = 0; i < NumberOfSamples; i++)
            {
                int dx = random.Next(-2, 2);
                int dy = random.Next(-2, 2);

                samples.Add(new Sample(x + dx, y + dy));
            }
            return samples;
        }
    }
}
