﻿using System.Diagnostics;
using RayTracer.SceneGraph.Accelerate;
using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.SceneGraph.Materials;
using RayTracer.SceneGraph.Light;
using RayTracer.Samplers;
using RayTracer.Helper;

namespace RayTracer.SceneGraph.Objects
{
    public class Sphere : IIntersectable
    {
        public IBoundingBox BoundingBox { get; set; }
        public Material Material { get; set; }
        public Vector3 Center { get; private set; }
        public float Radius { get; private set; }

        public ILight Light { get; set; }


        public Sphere(Material material, Vector3 center, float radius)
        {
            Material = material;
            Center = center;
            Radius = radius;
            BuildBoundingBox();
        }


        /// <summary>
        /// Calculates the Ray-Sphere Intersection, as described in <see cref="http://www.siggraph.org/education/materials/HyperGraph/raytrace/rtinter1.htm"/>
        /// With information taken from this site: <see cref="http://wiki.cgsociety.org/index.php/Ray_Sphere_Intersection"/>
        /// </summary>
        /// <param name="ray">The ray to calculate the intersection for</param>
        /// <returns>A new HitRecord if an intersection occurs, null otherwise</returns>
        public HitRecord Intersect(Ray ray)
        {

            //Compute A,B,C
            float a = Vector3.Dot(ray.Direction, ray.Direction);
            float b = 2 * Vector3.Dot(ray.Direction, ray.Origin - Center);
            float c = Vector3.Dot(ray.Origin - Center, ray.Origin - Center) - (Radius * Radius);

            //Calculate discriminant
            float disc = b * b - 4 * a * c;

            if (disc < 0)
                return null;

            float discSqrt = (float)Math.Sqrt(disc);

            //Compute q
            float q;
            if (b < 0)
                q = (-b + discSqrt) / 2.0f;
            else
                q = (-b - discSqrt) / 2.0f;

            //Compute t0,t1
            float t0 = q / a;
            float t1 = c / q;
            float t;
            if (t0 > t1)
            {
                float tmp = t0;
                t0 = t1;
                t1 = tmp;
            }

            if (t1 < 0)
                return null;

            if (t0 < 0)
                t = t1;
            else
                t = t0;

            if (float.IsNaN(t))
                return null;

            Vector3 hitPoint = ray.Origin + (ray.Direction * t);

            return new HitRecord(t, hitPoint, GetNormal(hitPoint), this, Material, ray.Direction);

        }

        private Vector3 GetNormal(Vector3 hitPosition)
        {
            Vector3 normal = hitPosition - Center;
            normal *= (1 / Radius);

            return normal;
        }

        public void BuildBoundingBox()
        {
            BoundingBox = new AxisAlignedBoundingBox(new Vector3(Center.X - Radius, Center.Y - Radius, Center.Z - Radius), new Vector3(Center.X + Radius, Center.Y + Radius, Center.Z + Radius));
        }



        public Vector3 GetSamplePoint(LightSample sample)
        {
            throw new NotSupportedException();
        }

        public float GetArea()
        {
            return 0f;
        }



        public Vector2 GetTextudeCoordinates(HitRecord record)
        {
            //Calculate TextureCoordinates
            double theta = Math.Acos((record.IntersectionPoint.Y - Center.Y) / Radius);
            double phi = Math.Atan2(record.IntersectionPoint.Z - Center.Y, record.IntersectionPoint.X - Center.X);
            phi = (phi < 0 ? phi + 2 * Math.PI : phi);
            double u = 1 - (phi / (2 * Math.PI));
            double v = (Math.PI - theta) / Math.PI;
            Vector2 texCoord = new Vector2((float)u, (float)v);

            return texCoord;
        }
        public int GetNumberOfComponents()
        {
            return 1;
        }
    }
}
