using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Samplers
{
    public interface ISampler
    {
        void CreateLightSamples();
        List<Sample> CreateSamples();
        List<LightSample> GetLightSamples();
   }
}
