using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Structs;

namespace RayTracer.SceneGraph.Accelerate
{
    class AxisAlignedBoundingBox : IBoundingBox
    {


        public Vector3 MinVector { get; set; }
        public Vector3 MaxVector { get; set; }
     
        public Vector3 Center
        {
            get { return (MinVector + MaxVector) / 2f; }
        }
        public Vector3 Dimension
        {
            get { return Vector3.Subtract(MaxVector, MinVector); }
        }

        public AxisAlignedBoundingBox(Vector3 minVector,Vector3 maxVector)
        {
            MinVector = minVector;
            MaxVector = maxVector;
        }

        /// <summary>
        /// Checks if this Bounding Box intersects with the other BoundingBox.
        /// </summary>
        /// <param name="otherBox">The BoundingBox to check against</param>
        /// <returns>An IntersectionType about which kind of intersection occured</returns>
        public bool Intersect(IBoundingBox otherBox)
        {
            bool x = otherBox.MinVector.X < MaxVector.X && otherBox.MaxVector.X > MinVector.X;
            bool y = otherBox.MinVector.Y < MaxVector.Y && otherBox.MaxVector.Y > MinVector.Y;
            bool z = otherBox.MinVector.Z < MaxVector.Z && otherBox.MaxVector.Z > MinVector.Z;
            return x && y && z;
        }

        public float[] Intersect(Ray ray)
        {
            Vector3[] parameters = new[] { MinVector, MaxVector };

            if (MinVector.X == MaxVector.X || MinVector.Y == MaxVector.Y || MinVector.Z == MaxVector.Z) return null;
            
            float tmin = (parameters[    ray.Sign[0]].X - ray.Origin.X) * ray.InvDirection.X;
            float tmax = (parameters[1 - ray.Sign[0]].X - ray.Origin.X) * ray.InvDirection.X;

            float tymin = (parameters[    ray.Sign[1]].Y - ray.Origin.Y) * ray.InvDirection.Y;
            float tymax = (parameters[1 - ray.Sign[1]].Y - ray.Origin.Y) * ray.InvDirection.Y;

            if (tmin > tymax || tymin > tmax) return null;
            if (tymin > tmin) tmin = tymin;
            if (tymax < tmax) tmax = tymax;

            float tzmin = (parameters[  ray.Sign[2]].Z - ray.Origin.Z) * ray.InvDirection.Z;
            float tzmax = (parameters[1-ray.Sign[2]].Z - ray.Origin.Z) * ray.InvDirection.Z;

            if (tmin > tzmax || tzmin > tmax) return null;
            if (tzmin > tmin) tmin = tzmin;
            if (tzmax < tmax) tmax = tzmax;

            return tmax < 0 ? null : new[] { tmin, tmax };
        }


        }
}
