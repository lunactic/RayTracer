using RayTracer.SceneGraph.Accelerate;
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

    
        public List<float[]> Vertices { get; private set; }
        public List<float[]> Normals { get; private set; }
        public Mesh()
        {

            Triangles = new List<Triangle>();
            BoundingBox = new AxisAlignedBoundingBox();

        }

        public new HitRecord Intersect(Ray ray)
        {
            HitRecord hit = null;
            HitRecord tempHit;
            foreach (Triangle t in Triangles)
            {

                tempHit = t.Intersect(ray);
                if (tempHit != null)
                {
                    if (hit == null) hit = tempHit;
                    else if (tempHit.Distance < hit.Distance) hit = tempHit;
                }
            }
            return hit;
        }


        public void CreateMeshFromObjectFile(String filename, float scale)
        {
            List<float[]> vertices = new List<float[]>();
            List<float[]> texCoords = new List<float[]>();
            List<float[]> normals = new List<float[]>();
            List<int[,]> faces = new List<int[,]>();

            ObjReader.Read(filename, scale, vertices, texCoords, normals, faces);
            Vertices = vertices;
            Normals = normals;
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
                    Triangle t = new Triangle(p1, n1, p2, n2, p3, n3) { Material = Material };
                    t.BuildBoundingBox();
                    Triangles.Add(t);
                }
                else
                {
                    Triangle t = new Triangle(p1, p2, p3) { Material = Material };
                    t.BuildBoundingBox();
                    Triangles.Add(t);

                }
            }
            BuildBoundingBox();
        }

        public override void BuildBoundingBox()
        {
            Vector3 minVector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 maxVector = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (Triangle t in Triangles)
            {
                //x component
                IBoundingBox box = t.BoundingBox;
                minVector.X = Math.Min(minVector.X, box.MinVector.X);
                minVector.Y = Math.Min(minVector.Y, box.MinVector.Y);
                minVector.Z = Math.Min(minVector.Z, box.MinVector.Z);

                maxVector.X = Math.Max(maxVector.X, box.MaxVector.X);
                maxVector.Y = Math.Max(maxVector.Y, box.MaxVector.Y);
                maxVector.Z = Math.Max(maxVector.Z, box.MaxVector.Z);

            }
            BoundingBox.MaxVector = maxVector;
            BoundingBox.MinVector = minVector;

        }
    }
}
