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

        public Color Integrate(Ray ray, IntersectableList objects, List<ILight> lights, ISampler sampler)
        {
            HitRecord record = objects.Intersect(ray);
            if (record != null)
                return Color.White;
            else
                return Color.Black;
        }
    }
}
