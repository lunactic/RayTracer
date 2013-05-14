using RayTracer.Helper;
using RayTracer.Samplers;
using RayTracer.SceneGraph.Accelerate;
using RayTracer.SceneGraph.Light;
using RayTracer.SceneGraph.Materials;
using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.SceneGraph.Objects
{
    public class Rectangle : IIntersectable
    {
        public IBoundingBox BoundingBox { get; set; }

        public ILight Light { get; set; }

        public Material Material { get; set; }

        public Vector3 P0 { get; set; }
        public Vector3 P1 { get; set; }
        public Vector3 P2 { get; set; }
        public Vector3 P3 { get; set; }


        public Vector3 A { get; set; }
        public Vector3 B { get; set; }

        private readonly float area;
        private Vector3 normal;

        public Rectangle(Vector3 p0, Vector3 a, Vector3 b)
        {
            P0 = p0;
            P1 = P0 + a;
            P2 = P0 + b;
            P3 = P0 + a + b;
            A = a;
            B = b;

            area = a.Length * b.Length;
            normal = Vector3.Cross(a, b);
            normal.Normalize();
            BoundingBox = new AxisAlignedBoundingBox();
            BuildBoundingBox();
        }

        public HitRecord Intersect(Ray ray)
        {
            Vector3 rayOrig = ray.Origin;
            Vector3 rayDir = ray.Direction;
            Vector3 point = Vector3.Subtract(P0,rayOrig);

            Vector3 n = normal * (1f / Vector3.Dot(rayDir, normal));
            float t = Vector3.Dot(point, n);

            if (t <= Constants.Epsilon) return null;

            Vector3 rayDir2 = rayDir * t;
            rayOrig += rayDir2;
            rayOrig -= P0; 

            float ddota = Vector3.Dot(rayOrig, A);
            if (ddota < 0f || ddota > A.Length * A.Length) return null;


            float ddotb = Vector3.Dot(rayOrig, B);
            if (ddotb < 0f || ddotb > B.Length * B.Length) return null;

            Vector3 hitPoint = ray.Origin + (rayDir * t);
            return new HitRecord(t, hitPoint, normal, this, Material, rayDir);
        }

        public void BuildBoundingBox()
        {
            float xMin = Math.Min(P0.X, Math.Min(P1.X, Math.Min(P2.X, P3.X)));
            float yMin = Math.Min(P0.Y, Math.Min(P1.Y, Math.Min(P2.Y, P3.Y)));
            float zMin = Math.Min(P0.Z, Math.Min(P1.Z, Math.Min(P2.Z, P3.Z)));

            float xMax = Math.Max(P0.X, Math.Max(P1.X, Math.Max(P2.X, P3.X)));
            float yMax = Math.Max(P0.Y, Math.Max(P1.Y, Math.Max(P2.Y, P3.Y)));
            float zMax = Math.Max(P0.Z, Math.Max(P1.Z, Math.Max(P2.Z, P3.Z)));


            BoundingBox.MinVector = new Vector3(xMin, yMin, zMin);
            BoundingBox.MaxVector = new Vector3(xMax, yMax, zMax);
        }

        public Vector3 GetSamplePoint(LightSample sample)
        {
            Vector3 samplePoint = P0;
            Vector3 aScaled = A*sample.X;
            Vector3 bScaled = B*sample.Y;
            
            samplePoint = samplePoint+aScaled+bScaled;
            sample.Normal = normal;
            sample.Area = area;
            return samplePoint;
        }

        public float GetArea()
        {
            return area;
        }

        public Vector3 GetSampledNormal(float x, float y)
        {
            return normal;
        }

        public int GetNumberOfComponents()
        {
            return 1;
        }
        public Vector2 GetTextudeCoordinates(HitRecord record)
        {
            throw new NotImplementedException();
        }
    }
}
