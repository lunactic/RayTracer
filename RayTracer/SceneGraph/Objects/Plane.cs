using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Structs;
using RayTracer.SceneGraph.Materials;
using RayTracer.SceneGraph.Light;
namespace RayTracer.SceneGraph.Objects
{
    public class Plane : IIntersectable
    {
        public ILight Light { get; set; }
        //Debug purposes
        public String Name { get; set; }
        /// <summary>
        /// Distance to origin
        /// </summary>
        public double Offset { get; private set; }
        /// <summary>
        /// Plane normal
        /// </summary>
        public Vector3 Normal { get; private set; }
        
        /// <summary>
        /// The Material of the plane
        /// </summary>
        public Material Material { get; set; }

        public Plane(double offset, Vector3 normal)
        {
            
            Normal = normal;
            Normal.Normalize();
            Offset = offset;
           
        }

        /// <summary>
        /// Checks if  a Ray intersects this plane
        /// </summary>
        /// <param name="ray">The Ray to test</param>
        /// <returns>A HitRecord with the needed information about the intersection</returns>
        public HitRecord Intersect(Ray ray)
        {
            Vector3 n = new Vector3(Normal);
            n = -n;
            n = n*(float)Offset;
            n = n - ray.Origin;
            float vd = Vector3.Dot(n, Normal);
            float vn = Vector3.Dot(ray.Direction, Normal);

            float t = vd / vn;
            
            if (t > 0)
            {
                Vector3 hitPoint = ray.Origin + (ray.Direction * t);
                return new HitRecord(t, hitPoint, Normal, this, Material, ray.Direction); ;
            }
            return null;
        }

        public Accelerate.IBoundingBox BoundingBox
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void BuildBoundingBox()
        {
            throw new NotImplementedException();
        }

        public Vector3 GetSamplePoint(float x, float y)
        {
            throw new NotSupportedException();
        }

        public float GetArea()
        {
            return 0f;
        }

        public Vector3 GetSampledNormal(float x, float y)
        {
            throw new NotSupportedException();
        }
    }
}
