using RayTracer.SceneGraph.Accelerate;
using RayTracer.SceneGraph.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Structs;
using RayTracer.Helper;
using RayTracer.Samplers;
using RayTracer.SceneGraph.Light;
using System.Runtime.CompilerServices;

namespace RayTracer.SceneGraph.Objects
{
    public class Mesh : Aggregate, IIntersectable
    {

        public Material Material { get; set; }
        public List<Triangle> Triangles { get; private set; }
        private ILight light;
        public ILight Light { get { return light; } set { foreach (Triangle t in Triangles) { t.Light = value; } light = value; } }
        private float area;

        public List<float[]> Vertices { get; private set; }
        public List<float[]> Normals { get; private set; }
        private List<int> Indices { get; set; }
        public Mesh()
        {

            Triangles = new List<Triangle>();
            BoundingBox = new AxisAlignedBoundingBox();

        }

        public Mesh(Material material, int[] indices, params float[] vertices)
        {
            Material = material;
            Triangles = new List<Triangle>();
            BoundingBox = new AxisAlignedBoundingBox();

            Vertices = new List<float[]>();
            for (int i = 0; i < vertices.Length; i += 3)
            {
                float[] vertex = new float[3];
                vertex[0] = vertices[i];
                vertex[1] = vertices[i + 1];
                vertex[2] = vertices[i + 2];
                Vertices.Add(vertex);
            }
            for (int j = 0; j < indices.Length; j += 3)
            {
                Triangles.Add(new Triangle(new Vector3(Vertices[indices[j    ]][0], Vertices[indices[j    ]][1], Vertices[indices[j    ]][2]),
                                           new Vector3(Vertices[indices[j + 1]][0], Vertices[indices[j + 1]][1], Vertices[indices[j + 1]][2]),
                                           new Vector3(Vertices[indices[j + 2]][0], Vertices[indices[j + 2]][1], Vertices[indices[j + 2]][2])) { Material = material });
            }
            BuildBoundingBox();
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

        public new Vector3 GetSamplePoint(LightSample sample)
        {
            if (Indices == null) CreateIndices();
            IIntersectable sampleTriangle = Triangles[(int)Math.Floor(new Random().NextDouble() * Triangles.Count)];
            sample.Normal = ((Triangle)sampleTriangle).Normal;
            sample.Area = ((Triangle)sampleTriangle).GetArea();

            return sampleTriangle.GetSamplePoint(sample);

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void CreateIndices()
        {
            Indices = new List<int>();
            float smallestArea = float.PositiveInfinity;
            int index = 0;
            for (int i = 0; i < Triangles.Count; i++)
            {
                if (Triangles[i].GetArea() < smallestArea)
                {
                    smallestArea = Triangles[i].GetArea();
                    index = i;
                }
            }
            Indices.Add(index);
            for (int j = 0; j < Triangles.Count; j++)
            {
                int k = (int)(Math.Ceiling((Triangles[j].GetArea() / Triangles[index].GetArea())));
                for (int l = 0; l <= k; l++)
                {
                    Indices.Add(j);
                }
            }
        }

        public new float GetArea()
        {
            if (area == 0f)
            {
                foreach (Triangle t in Triangles)
                {
                    area += t.GetArea();
                }
            }
            return area;
        }

    }
}
