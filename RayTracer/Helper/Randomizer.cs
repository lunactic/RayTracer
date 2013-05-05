using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Samplers;
using RayTracer.SceneGraph.Light;

namespace RayTracer.Helper
{
    public static class Randomizer
    {
        private static Random rand = new Random();
        public static ILight PickRandomLight(List<ILight> lights)
        {
            return lights[rand.Next(0, lights.Count - 1)];
        }
        public static LightSample PickRandomLightSample(List<LightSample> samples)
        {
            LightSample sample = samples[rand.Next(0, samples.Count - 1)];
            samples.Remove(sample);
            return sample;
        }
        public static Sample PickRandomSample(List<Sample> samples)
        {
            Sample sample = samples[rand.Next(0, samples.Count - 1)];
            samples.Remove(sample);
            return sample;
        }
    }
}
