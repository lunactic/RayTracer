using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Structs;

namespace RayTracer.Samplers
{
    public class StratifiedSampler : ISampler
    {
        public int NumberOfSamples { get; set; }
        private Random random;
        private List<LightSample> lightSamples;
        public StratifiedSampler()
        {
            random = new Random();
        }

       
        public List<Sample> CreateSamples()
        {
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
            lightSamples = new List<LightSample>();
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
