using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.SceneGraph.Materials;

namespace RayTracer.SceneGraph.Objects
{
    public class Triangle : IIntersectable
    {
        public Vector3 A { get; private set; }
        public Vector3 B { get; private set; }
        public Vector3 C { get; private set; }

        public Vector3 Normal { get; private set; }

        public Material Material { get; set; }

        //Vertex normal
        public Vector3 N1 { get; private set; }
        public Vector3 N2 { get; private set; }
        public Vector3 N3 { get; private set; }

        private Vector3 edge1;
        private Vector3 edge2;

        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            A = a;
            B = b;
            C = c;
            edge1 = B - A;
            edge2 = C - A;

            Normal = Vector3.Cross(edge1, edge2);
        }

        public Triangle(Vector3 a, Vector3 n1, Vector3 b, Vector3 n2, Vector3 c, Vector3 n3)
        {
            A = a;
            B = b;
            C = c;
            N1 = n1;
            N2 = n2;
            N3 = n3;
            edge1 = B - A;
            edge2 = C - A;

            Normal = Vector3.Cross(edge1, edge2);
        }

        public void Intersect(Ray ray)
        {
            double a = A.X - B.X;
            double b = A.X - C.X;
            double c = ray.Direction.X;
            double d = A.X - ray.Origin.X;
            double e = A.Y - B.Y;
            double f = A.Y - C.Y;
            double g = ray.Direction.Y;
            double h = A.Y - ray.Origin.Y;
            double i = A.Z - B.Z;
            double j = A.Z - C.Z;
            double k = ray.Direction.Z;
            double l = A.Z - ray.Origin.Z;

            double m = f * k - g * j;
            double n = h * k - g * l;
            double p = f * l - h * j;
            double q = g * i - e * k;
            double s = e * j - f * i;

            double inv_denom = 1.0 / (a * m + b * q + c * s);

            double e1 = d * m - b * n - c * p;
            double beta = e1 * inv_denom;

            if (beta < 0.0)
                return;

            double r = e * l - h * i;
            double e2 = a * n + d * q + c * r;
            double gamma = e2 * inv_denom;

            if (gamma < 0.0 || beta + gamma > 1.0)
                return;

            double e3 = a * p - b * r + d * s;

            double t = e3 * inv_denom;

            if (t > Constants.EPSILON)
            {
                ray.HitRecord.UpdateRecord((float)t, this);
                if (N1 != Vector3.Zero && N2 != Vector3.Zero && N3 != Vector3.Zero)
                {
                    double alpha = 1 - gamma - beta;
                    Vector3 n1Interpolated = N1 * (float)alpha;
                    Vector3 n2Interpolated = N2 * (float)beta;
                    Vector3 n3Interpolated = N3 * (float)gamma;

                    Vector3 interpolatedNormal = n1Interpolated + n2Interpolated + n3Interpolated;
                    ray.HitRecord.SurfaceNormal = interpolatedNormal;
                }
            }

        }

        public Vector3 GetNormal(Vector3 hitPosition)
        {
            return Normal;
        }
    }
}
