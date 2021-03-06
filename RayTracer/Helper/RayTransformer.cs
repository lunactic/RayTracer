﻿using System;
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
            Vector3 worldIntersectionPoint = transformationMatrix.Transform(intersectionPoint);


            Vector4 normal = new Vector4(record.SurfaceNormal);
            Vector3 worldNormal = transTransformationMatrix.Transform(normal);
            worldNormal.Normalize();

            Vector4 rayDir = transformationMatrix.Transform(new Vector4(record.RayDirection));
            Vector3 worldRayDir = rayDir;
            record.IntersectionPoint = worldIntersectionPoint;
            record.SurfaceNormal = worldNormal;
            record.RayDirection = worldRayDir;

            
        }

        public static Ray TransformRayToObject(Ray ray, Matrix4 invTransformationMatrix)
        {
            Vector4 dir = new Vector4(ray.Direction);
            Vector4 orig = new Vector4(ray.Origin) { W = 1 };

            Vector4 transfDir = invTransformationMatrix.Transform(dir);
            Vector4 transfOrig = invTransformationMatrix.Transform(orig);

            return new Ray(transfOrig, transfDir);
        }

 
    }
}
