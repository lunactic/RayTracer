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
        private List<LightSample> lightSamples;
        public RandomSampler(int numOfSamples)
        {
            NumberOfSamples = numOfSamples;
            random = new Random();
        }
        public List<Sample> CreateSamples()
        {
            List<Sample> samples = new List<Sample>();
            for (int i = 0; i < NumberOfSamples*NumberOfSamples; i++)
            {
                samples.Add(new Sample((float)random.NextDouble(),(float)random.NextDouble()));
            }
            return samples;
        }

        public void CreateLightSamples()
        {
            lightSamples = new List<LightSample>();
            for (int i = 0; i < NumberOfSamples; i++)
            {
                lightSamples.Add(new LightSample(-0.5f + (float)random.NextDouble(), -0.5f + (float)random.NextDouble()));
            }
           
        }

        public List<LightSample> GetLightSamples()
        {
            return lightSamples;
        }
    }
}
