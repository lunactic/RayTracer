using RayTracer.SceneGraph.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Structs;
using RayTracer.Helper;

namespace RayTracer.SceneGraph.Objects
{
    public class Mesh : Aggregate, IIntersectable
    {
        public Material Material { get; set; }
        public List<Triangle> Triangles { get; private set; }

        public Mesh()
        {
            Triangles = new List<Triangle>();
        }

        public void Intersect(Ray ray)
        {
            Ray rayCopy = new Ray(ray.Origin, ray.Direction);
            foreach (Triangle t in Triangles)
            {
                t.Intersect(rayCopy);
            }
            if (rayCopy.HitRecord.HitObject != null && rayCopy.HitRecord.Distance < ray.HitRecord.Distance)
            {
                ray.HitRecord = rayCopy.HitRecord;
            }
        }


        public Vector3 GetNormal(Vector3 hitPosition)
        {
            throw new NotSupportedException();
        }

        public void CreateMeshFromObjectFile(String filename, float scale)
        {
            List<float[]> vertices = new List<float[]>();
            List<float[]> texCoords = new List<float[]>();
            List<float[]> normals = new List<float[]>();
            List<int[,]> faces = new List<int[,]>();

            ObjReader.Read(filename, scale, vertices, texCoords, normals, faces);

            //loop over all faces to create the Triangle instaces
            foreach (int[,] face in faces)
            {
                float[] point;

                point = vertices[face[0, 0] - 1];
                Vector3 p1 = new Vector3(point[0], point[1], point[2]);
                point = vertices[face[1, 0] - 1];
                Vector3 p2 = new Vector3(point[0], point[1], point[2]);
                point = vertices[face[2, 0] - 1];
                Vector3 p3 = new Vector3(point[0], point[1], point[2]);

                if (normals.Count > 0)
                {
                    float[] normal;
                    normal = normals[face[0, 2] - 1];
                    Vector3 n1 = new Vector3(normal[0], normal[1], normal[2]);
                    normal = normals[face[1, 2] - 1];
                    Vector3 n2 = new Vector3(normal[0], normal[1], normal[2]);
                    normal = normals[face[2, 2] - 1];
                    Vector3 n3 = new Vector3(normal[0], normal[1], normal[2]);
                    Triangles.Add(new Triangle(p1, n1, p2, n2, p3, n3));
                }
                else
                {
                    Triangles.Add(new Triangle(p1, p2, p3));
                }
            }
        }
    }
}
