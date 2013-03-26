using RayTracer.Structs;

namespace RayTracer.SceneGraph.Integrators
{
    public interface IIntegrator
    {
        Color Integrate(Ray ray);
    }
}