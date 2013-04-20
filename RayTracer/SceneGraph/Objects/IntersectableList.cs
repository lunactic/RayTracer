using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Helper;
using RayTracer.SceneGraph.Accelerate;
using RayTracer.Structs;
using RayTracer.SceneGraph.Materials;
using RayTracer.SceneGraph.Light;

namespace RayTracer.SceneGraph.Objects
{
    public class IntersectableList : Aggregate, IIntersectable
    {

        public Material Material { get; set; }
        public Matrix4 TransformationMatrix { get; set; }
        public Matrix4 InvTransformationMatrix { get; set; }
        public Matrix4 TransposedTransformationMatrix { get; set; }

        public override void BuildBoundingBox()
        {
            Vector3 minVector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 maxVector = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (IIntersectable intersectable in Intersectables)
            {
                IBoundingBox box = intersectable.BoundingBox;
                if (box != null)
                {
                    minVector.X = Math.Min(minVector.X, box.MinVector.X);
                    minVector.Y = Math.Min(minVector.Y, box.MinVector.Y);
                    minVector.Z = Math.Min(minVector.Z, box.MinVector.Z);
                    maxVector.X = Math.Max(maxVector.X, box.MaxVector.X);
                    maxVector.Y = Math.Max(maxVector.Y, box.MaxVector.Y);
                    maxVector.Z = Math.Max(maxVector.Z, box.MaxVector.Z);
                }
            }
            BoundingBox = new AxisAlignedBoundingBox { MinVector = minVector, MaxVector = maxVector };
        }

        public new HitRecord Intersect(Ray ray)
        {
            BuildBoundingBox();
            if (BoundingBox.Intersect(ray) == null) return null;

            HitRecord hit = null;
            foreach (var intersectable in Intersectables)
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
