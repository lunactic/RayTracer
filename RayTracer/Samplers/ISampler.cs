using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Samplers
{
    public interface ISampler
    {
        void CreateLightSamples();
        int NumberOfSamples { get; set; }
        List<Sample> CreateSamples();
        List<LightSample> GetLightSamples();
    }
}
