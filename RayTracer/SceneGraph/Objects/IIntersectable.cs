using RayTracer.SceneGraph.Accelerate;
using RayTracer.SceneGraph.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Structs;
using RayTracer.SceneGraph.Light;

namespace RayTracer.SceneGraph.Objects
{
    public interface IIntersectable
    {
        IBoundingBox BoundingBox { get; set; }
        ILight Light { get; set; }
        Material Material { get; set; }
        HitRecord Intersect(Ray ray);
        void BuildBoundingBox();
        Vector3 GetSamplePoint(float x, float y);
        float GetArea();
        Vector3 GetSampledNormal(float x, float y);

    }
}
