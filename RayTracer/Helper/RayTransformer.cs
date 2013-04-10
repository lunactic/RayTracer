using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.SceneGraph.Accelerate;
using RayTracer.SceneGraph.Materials;
using RayTracer.Structs;

namespace RayTracer.SceneGraph.Objects
{
    public static class RayTransformer
    {
    
        public static void TransformHitToWorld(HitRecord record, Matrix4 transformationMatrix, Matrix4 transTransformationMatrix)
        {
            
            Vector4 intersectionPoint = new Vector4(record.IntersectionPoint) { W = 1 };
            Vector3 worldIntersectionPoint = Vector4.Transform(intersectionPoint, transformationMatrix);


            Vector4 normal = new Vector4(record.SurfaceNormal);
            Vector3 worldNormal = Vector4.Transform(normal, transTransformationMatrix);
            worldNormal.Normalize();

            Vector4 rayDir = Vector4.Transform(new Vector4(record.RayDirection), transformationMatrix);
            Vector3 worldRayDir = rayDir;
            record.IntersectionPoint = worldIntersectionPoint;
            record.SurfaceNormal = worldNormal;
            record.RayDirection = worldRayDir;
        }

        public static Ray TransformRayToObject(Ray ray, Matrix4 invTransformationMatrix)
        {
            Vector4 dir = new Vector4(ray.Direction);
            Vector4 orig = new Vector4(ray.Origin) { W = 1 };

            Vector4 transfDir = Vector4.Transform(dir, invTransformationMatrix);
            Vector4 transfOrig = Vector4.Transform(orig, invTransformationMatrix);

            return new Ray(transfOrig, transfDir);
        }

 
    }
}
