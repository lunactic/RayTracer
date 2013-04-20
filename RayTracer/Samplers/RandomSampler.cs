using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Helper;

namespace RayTracer.Samplers
{
    public class RandomSampler : ISampler
    {
        private Random random;
        private List<LightSample> lightSamples;
        public RandomSampler()
        {
            random = new Random();
        }
        public List<Sample> CreateSamples()
        {
            List<Sample> samples = new List<Sample>();
            for (int i = 0; i < Constants.NumberOfSamples; i++)
            {
                samples.Add(new Sample((float)random.NextDouble(),(float)random.NextDouble()));
            }
            return samples;
        }

        public void CreateLightSamples()
        {
            lightSamples = new List<LightSample>();
            for (int i = 0; i < Constants.NumberOfLightSamples*Constants.NumberOfLightSamples; i++)
            {
                lightSamples.Add(new LightSample((float)random.NextDouble(),(float)random.NextDouble()));
            }
           
        }

        public List<LightSample> GetLightSamples()
        {
            return lightSamples;
        }

    }
}
