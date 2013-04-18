using RayTracer.SceneGraph.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Structs;

namespace RayTracer.SceneGraph.Objects
{
    public abstract class Aggregate
    {
        public List<IIntersectable> Objects { get; set; }
        public abstract List<IIntersectable> GetObjects();

        public HitRecord Intersect(Ray ray)
        {
            HitRecord hit = null;
            foreach (var intersectable in Objects)
            {
                HitRecord tempHit = intersectable.Intersect(ray);
                if (tempHit != null)
                    if (hit == null) hit = tempHit;
                    //else if (tempHit.Distance < hit.Distance) hit = tempHit;
                    else if (Constants.IsLightSamplingOn)
                    {
                        if (tempHit.Distance < hit.Distance) hit = tempHit;
                    }
                    else if (tempHit.HitObject.Light == null && tempHit.Distance < hit.Distance) hit = tempHit;
            }
            return hit;
        }
    }
}
