using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Structs;
using RayTracer.SceneGraph.Materials;

namespace RayTracer.SceneGraph.Objects
{
    public class IntersectableList : Aggregate, IIntersectable
    {
        public Material Material {get; set; }
        public List<IIntersectable> Objects { get; set; }

        public IntersectableList()
        {
               Objects = new List<IIntersectable>();
        }

        public void Intersect(Ray ray)
        {
            foreach (var intersectable in Objects)
            {
                intersectable.Intersect(ray);
                
            }
        }

        public Vector3 GetNormal(Vector3 hitPosition)
        {
            throw new NotImplementedException();
        }
    }
}
