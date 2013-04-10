using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Samplers
{
    public class StratifiedSampler : ISampler
    {
        public int NumberOfSamples { get; set; }
        public StratifiedSampler(int numOfSamples)
        {
            NumberOfSamples = numOfSamples;
        }

       
        public List<Sample> CreateSamples(int x, int y)
        {
            List<Sample> samples = new List<Sample>();

            for (int dy = -NumberOfSamples/2; dy < (NumberOfSamples + 1)/2; dy++)
            {
                for (int dx = -NumberOfSamples/2; dx < (NumberOfSamples + 1)/2; dx++)
                {
                    Sample s = new Sample(x + dx, y + dy);
                    samples.Add(s);
                }
            }
            return samples;
        }
    }
}
