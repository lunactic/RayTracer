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

        public HitRecord Intersect(Ray ray)
        {
            HitRecord hit = null;
            foreach (var intersectable in Objects)
            {
                HitRecord tempHit = intersectable.Intersect(ray);
                if (tempHit != null)
                    if (hit == null) hit = tempHit;
                    else if (tempHit.Distance < hit.Distance) hit = tempHit;
            }
            return hit;
        }

        public Vector3 GetNormal(Vector3 hitPosition)
        {
            throw new NotImplementedException();
        }

        public Accelerate.IBoundingBox BoundingBox
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void BuildBoundingBox()
        {
            throw new NotImplementedException();
        }

        public override List<IIntersectable> GetObjects()
        {
            return Objects;
        }
    }
}
