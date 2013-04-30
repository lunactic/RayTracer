using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Structs;
using RayTracer.SceneGraph.Objects;
using RayTracer.SceneGraph.Materials;
using RayTracer.Helper;
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
        public Ray CreateRefractedRay()
        {
            Vector3 incident = new Vector3(CreateReflectedRay().Direction);
            Vector3 normal = new Vector3(SurfaceNormal);
            float n1, n2;
            float cosI = Vector3.Dot(normal, incident);
            if (cosI < 0)
            {
                //Material to Air
                n1 = Material.RefractionIndex;
                n2 = Refractions.AIR;
                normal = -normal;
                cosI = Vector3.Dot(normal, incident);
            }
            else
            {
                //Air to Material
                n1 = Refractions.AIR;
                n2 = Material.RefractionIndex;
            }

            float n = n1 / n2;

            float sinT2 = (float)(n * n * (1.0 - cosI * cosI));

            if (sinT2 > 1.0) return new Ray(IntersectionPoint, CreateReflectedRay().Direction); //TIR

            float cosT = (float)Math.Sqrt(1.0 - sinT2);

            Vector3 dir = RayDirection;
            dir.Normalize();
            dir = dir * n;
            normal = normal * (float)(n * cosI - cosT);
            dir += normal;

            Vector3 pos = IntersectionPoint;
            Vector3 offset = dir;
            offset *= 0.0001f;
            pos += offset;

            return new Ray(pos, dir);
        }
    }
}
