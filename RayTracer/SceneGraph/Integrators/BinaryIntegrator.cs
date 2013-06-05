using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.SceneGraph.Light;
using RayTracer.SceneGraph.Objects;
using RayTracer.SceneGraph.Scenes;
using RayTracer.Structs;
using RayTracer.Samplers;

namespace RayTracer.SceneGraph.Integrators
{
    public class BinaryIntegrator : IIntegrator
    {

        public Color Integrate(Ray ray, IIntersectable objects, List<ILight> lights, ISampler sampler, List<List<Sample>> subPathSamples, LightSample lightSample)
        {
            HitRecord record = objects.Intersect(ray);
            if (record != null)
                return new Color(1,1,1);
            else
                return new Color(0,0,0);
        }
    }
}
