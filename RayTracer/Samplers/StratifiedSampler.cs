using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RayTracer.Helper;
using RayTracer.Structs;

namespace RayTracer.Samplers
{
    public class StratifiedSampler : ISampler
    {
        private volatile List<LightSample> lightSamples = new List<LightSample>();

        public StratifiedSampler()
        {
            CreateLightSamples();
        }

        public List<Sample> CreateSamples()
        {
            Random random = new Random();
            List<Sample> samples = new List<Sample>();
            int gridW = (int)Math.Floor(Math.Sqrt(Constants.NumberOfSamples));
            int gridH = gridW;
            float stratW = 1f / gridW;
            float stratH = stratW;

            for (float w = 0; w < 1; w += stratW)
            {
                for (float h = 0; h < 1; h += stratH)
                {
                    samples.Add(new Sample(w + (float)random.NextDouble() / gridW, h + (float)random.NextDouble() / gridH));
                }
            }
            return samples;
        }

        public void CreateLightSamples()
        {
            Random random = new Random();
            int gridW = (int)Math.Floor(Math.Sqrt(Constants.NumberOfSamples * Constants.NumberOfLightSamples));
            int gridH = gridW;
            float stratW = 1f / gridW;
            float stratH = stratW;

            for (float w = 0; w < 1; w += stratW)
            {
                for (float h = 0; h < 1; h += stratH)
                {
                    lightSamples.Add(new LightSample(w + (float)random.NextDouble() / gridW, h + (float)random.NextDouble() / gridH));
                }
            }
        }


        public List<LightSample> GetLightSamples()
        {
            return lightSamples;
            
        }
    }
}
