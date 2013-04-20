using System.Collections.Generic;
using RayTracer.Samplers;
using RayTracer.SceneGraph.Light;
using RayTracer.SceneGraph.Objects;
using RayTracer.SceneGraph.Scenes;
using RayTracer.Structs;

namespace RayTracer.SceneGraph.Integrators
{
    public interface IIntegrator
    {
        Color Integrate(Ray ray, IIntersectable objects,List<ILight> lights , ISampler sampler);

    }
}