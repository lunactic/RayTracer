﻿using RayTracer.Helper;
using RayTracer.SceneGraph.Accelerate;
using RayTracer.Structs;
using System;
using RayTracer.SceneGraph.Materials;
using RayTracer.SceneGraph.Light;
using RayTracer.Samplers;

namespace RayTracer.SceneGraph.Objects
{
    public class Triangle : IIntersectable
    {

        public ILight Light { get; set; }

        public Material Material { get; set; }

        public IBoundingBox BoundingBox { get; set; }

        public Vector3 A { get; private set; }
        public Vector3 B { get; private set; }
        public Vector3 C { get; private set; }

        public Vector3 Normal { get; private set; }

        //Vertex normal
        public Vector3 N1 { get; private set; }
        public Vector3 N2 { get; private set; }
        public Vector3 N3 { get; private set; }
        //Texture Coordinates
        public Vector2 Tex1 { get; set; }
        public Vector2 Tex2 { get; set; }
        public Vector2 Tex3 { get; set; }
        private Vector2 InterpolatedTextureCoordinate;

        private Vector3 edge1;
        private Vector3 edge2;
        private bool hasVertexNormals;
        private bool hasTexCoords;
        private float area;

        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            hasVertexNormals = false;
            A = a;
            B = b;
            C = c;
            edge1 = B - A;
            edge2 = C - A;

            Normal = Vector3.Cross(edge1, edge2);
            Normal.Normalize();
            BuildBoundingBox();
            area = CalculateArea();
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
            hasVertexNormals = true;
            Normal = Vector3.Cross(edge1, edge2);
            Normal.Normalize();
            BuildBoundingBox();
            area = CalculateArea();
        }

        public Triangle(Vector3 a, Vector3 n1, Vector2 t1, Vector3 b, Vector3 n2, Vector2 t2, Vector3 c, Vector3 n3, Vector2 t3)
        {

            A = a;
            B = b;
            C = c;
            N1 = n1;
            N2 = n2;
            N3 = n3;
            Tex1 = t1;
            Tex2 = t2;
            Tex3 = t3;
            edge1 = B - A;
            edge2 = C - A;
            hasVertexNormals = true;
            hasTexCoords = true;
            Normal = Vector3.Cross(edge1, edge2);
            Normal.Normalize();
            BuildBoundingBox();
            area = CalculateArea();
        }

        public HitRecord Intersect(Ray ray)
        {

            Vector3 rayDir = ray.Direction;

            if (Equals(Normal.Length, 0.0f)) return null;

            Vector3 w0 = Vector3.Subtract(ray.Origin, A);
            float a = -Vector3.Dot(Normal, w0);
            float b = Vector3.Dot(Normal, rayDir);

            if (Math.Abs(b) < Constants.Epsilon) return null;

            float t = a / b;
            if (t < 0) return null;

            Vector3 hitPoint = ray.Origin + (ray.Direction * t);
            float uu = Vector3.Dot(edge1, edge1);
            float uv = Vector3.Dot(edge1, edge2);
            float vv = Vector3.Dot(edge2, edge2);
            Vector3 w = Vector3.Subtract(hitPoint, A);

            float wu = Vector3.Dot(w, edge1);
            float wv = Vector3.Dot(w, edge2);

            float D = uv * uv - uu * vv;
            float beta = (uv * wv - vv * wu) / D;
            if (beta < 0 || beta > 1) return null;
            float gamma = (uv * wu - uu * wv) / D;
            if (gamma < 0 || (beta + gamma) > 1) return null;
            float alpha = 1 - beta - gamma;
            Vector3 normal = Normal;
            if (hasVertexNormals)
            {
                Vector3 n1Interpolated = N1 * alpha;
                Vector3 n2Interpolated = N2 * beta;
                Vector3 n3Interpolated = N3 * gamma;

                normal = n1Interpolated + n2Interpolated + n3Interpolated;

            }
            if (hasTexCoords)
            {

                Vector2 t1Interpolated = Vector2.Interpolate(Tex1, alpha);
                Vector2 t2Interpolated = Vector2.Interpolate(Tex2, beta);
                Vector2 t3Interpolated = Vector2.Interpolate(Tex3, gamma);
                InterpolatedTextureCoordinate = t1Interpolated + t2Interpolated + t3Interpolated;

            }

            return new HitRecord(t, hitPoint, normal, this, Material, rayDir);
        }

        public void BuildBoundingBox()
        {
            Vector3 minVector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 maxVector = new Vector3(float.Epsilon, float.Epsilon, float.Epsilon);
            
            //Check A
            
            if (A.X < minVector.X) minVector.X = A.X;
            if (A.Y < minVector.Y) minVector.Y = A.Y;
            if (A.Z < minVector.Z) minVector.Z = A.Z;
            if (A.X > maxVector.X) maxVector.X = A.X;
            if (A.Y > maxVector.Y) maxVector.Y = A.Y;
            if (A.Z > maxVector.Z) maxVector.Z = A.Z;
            //check B
            if (B.X < minVector.X) minVector.X = B.X;
            if (B.Y < minVector.Y) minVector.Y = B.Y;
            if (B.Z < minVector.Z) minVector.Z = B.Z;
            if (B.X > maxVector.X) maxVector.X = B.X;
            if (B.Y > maxVector.Y) maxVector.Y = B.Y;
            if (B.Z > maxVector.Z) maxVector.Z = B.Z;
            //check C
            if (C.X < minVector.X) minVector.X = C.X;
            if (C.Y < minVector.Y) minVector.Y = C.Y;
            if (C.Z < minVector.Z) minVector.Z = C.Z;
            if (C.X > maxVector.X) maxVector.X = C.X;
            if (C.Y > maxVector.Y) maxVector.Y = C.Y;
            if (C.Z > maxVector.Z) maxVector.Z = C.Z;
           
            


            BoundingBox = new AxisAlignedBoundingBox(minVector, maxVector);

        }

        public Vector3 GetSamplePoint(LightSample sample)
        {
            float sqrtX = (float)Math.Sqrt(sample.X);
            float x = 1 - sqrtX;
            float y = sample.Y * sqrtX;

            Vector3 aScaled = A * x;
            Vector3 bScaled = B * y;
            Vector3 cScaled = C * (1 - (x + y));

            sample.Normal = Normal;
            sample.Area = area;
            Vector3 samplePoint = aScaled + bScaled + cScaled;
            return samplePoint;
        }

        public float GetArea()
        {
            return area;
        }

        private float CalculateArea()
        {
            Vector3 ab = Vector3.Add(A, B);
            Vector3 ac = Vector3.Add(A, C);
            return (float)(0.5f * Math.Sqrt(ab.Length * ab.Length * ac.Length * ac.Length - Math.Pow(Vector3.Dot(ab, ac), 2)));
        }

        public int GetNumberOfComponents()
        {
            return 1;
        }
        public Vector2 GetTextudeCoordinates(HitRecord record)
        {
            return InterpolatedTextureCoordinate;
        }
    }
}
