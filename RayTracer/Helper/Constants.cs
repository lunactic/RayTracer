using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Samplers;

namespace RayTracer.Structs
{
    public class Constants
    {
        public static readonly double Epsilon = 1e-6;
        public static readonly int MaximumRecursionDepth = 8;

        public static readonly bool IsLightSamplingOn = true;
        public static readonly bool IsSamplingOn = true;
        public static readonly int NumberOfLightSamples = 12;
        public static readonly int NumberOfSamples = 30;
        public static readonly ISampler Sampler = new StratifiedSampler();

        public static int SceneIndex = 1;
        public static int NumberOfThreads = 8;
    }
}
