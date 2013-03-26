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
        Material Material { get; set; }
        void Intersect(Ray ray);
        Vector3 GetNormal(Vector3 hitPosition);
    }
}
