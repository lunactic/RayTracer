using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Samplers;

namespace RayTracer.Structs
{
    public static class Constants
    {
        public const double Epsilon = 1e-6;
        public const int MaximumRecursionDepth = 8;

        public const bool IsLightSamplingOn = true;
        public const int NumberOfLightSamples = 12;
        public const int NumberOfSamples = 64;

        public static readonly ISampler Sampler = new StratifiedSampler();
    }
}
