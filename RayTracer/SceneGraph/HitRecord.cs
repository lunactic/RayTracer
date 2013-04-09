using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Structs;
using RayTracer.SceneGraph.Objects;
using RayTracer.SceneGraph.Materials;
namespace RayTracer.SceneGraph
{

    public class HitRecord
    {
        public Vector3 RayDirection { get; set; }
        public Vector3 InvRayDirection { get; set; }
        public Material Material { get; set; }
        public Vector3 SurfaceNormal { get; set; }
        public Vector3 IntersectionPoint { get; set; }
        public IIntersectable HitObject { get; set; }
        public float Distance { get; set; }
        public HitRecord()
        {
            Distance = float.MaxValue;
        }

        public HitRecord(float t, Vector3 hitPoint, Vector3 normal, IIntersectable hitObject, Material material, Vector3 rayDirection)
        {
            
            Distance = t;
            IntersectionPoint = hitPoint;
            SurfaceNormal = normal;
            HitObject = hitObject;
            Material = material;
            RayDirection = rayDirection;
            InvRayDirection = new Vector3(1 / RayDirection.X, 1 / RayDirection.Y, 1 / RayDirection.Z);
        }
        
        public Ray CreateReflectedRay()
        {
            float cosI = -Vector3.Dot(SurfaceNormal, RayDirection);
            Vector3 direction = RayDirection + (SurfaceNormal*cosI*2);
            direction.Normalize();
            Vector3 newOrigin = IntersectionPoint;
            Vector3 offset = direction * 0.001f;
            newOrigin += offset;

            return new Ray(newOrigin, direction);
        }
    }
}
