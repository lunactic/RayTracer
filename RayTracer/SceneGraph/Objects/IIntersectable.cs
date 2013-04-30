using RayTracer.SceneGraph.Accelerate;
using RayTracer.SceneGraph.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Structs;
using RayTracer.SceneGraph.Light;
using RayTracer.Samplers;

namespace RayTracer.SceneGraph.Objects
{
    public interface IIntersectable
    {
        IBoundingBox BoundingBox { get; set; }
        ILight Light { get; set; }
        Material Material { get; set; }
        HitRecord Intersect(Ray ray);
        void BuildBoundingBox();
        Vector3 GetSamplePoint(LightSample sample);
        float GetArea();

    }
}
