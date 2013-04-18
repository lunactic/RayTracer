using RayTracer.Structs;

namespace RayTracer.SceneGraph.Accelerate
{
    public interface IBoundingBox
    {
        Vector3 Center { get; }
        Vector3 MinVector { get; set; }
        Vector3 MaxVector { get; set; }
        Vector3 Dimension { get; }
        bool Intersect(IBoundingBox otherBox);
        float[] Intersect(Ray ray);
    }
}