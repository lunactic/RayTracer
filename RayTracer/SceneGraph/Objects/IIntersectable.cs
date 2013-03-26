using RayTracer.SceneGraph.Accelerate;
using RayTracer.SceneGraph.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Structs;

namespace RayTracer.SceneGraph.Objects
{
    public interface IIntersectable
    {
        IBoundingBox BoundingBox { get; set; }
        Material Material { get; set; }
        HitRecord Intersect(Ray ray);
        Vector3 GetNormal(Vector3 hitPosition);
        void BuildBoundingBox();
    }
}
