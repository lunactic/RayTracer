using RayTracer.Structs;

namespace RayTracer.SceneGraph.Accelerate
{   public enum IntersectionType { Inside, Outside, Intersection };
    public interface IBoundingBox
    {
        Vector3 Center { get; }
        Vector3 MinVector { get; set; }
        Vector3 MaxVector { get; set; }
        Vector3 Dimension{ get; }
        IntersectionType Intersect(IBoundingBox otherBox);
        bool Intersect(Ray ray);
    }
}