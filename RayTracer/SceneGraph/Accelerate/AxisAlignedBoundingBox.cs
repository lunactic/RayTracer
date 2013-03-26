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

        public AxisAlignedBoundingBox()
        {
            MinVector = Vector3.Zero;
            MaxVector = Vector3.Zero;
        }
        /// <summary>
        /// Checks if this Bounding Box intersects with the other BoundingBox.
        /// </summary>
        /// <param name="otherBox">The BoundingBox to check against</param>
        /// <returns>An IntersectionType about which kind of intersection occured</returns>
        public bool Intersect(IBoundingBox otherBox)
        {
            bool x, y, z;
            x = otherBox.MinVector.X < MaxVector.X && otherBox.MaxVector.X > MinVector.X;
            y = otherBox.MinVector.Y < MaxVector.Y && otherBox.MaxVector.Y > MinVector.Y;
            z = otherBox.MinVector.Z < MaxVector.Z && otherBox.MaxVector.Z > MinVector.Z;
            return x && y && z;
        }

        public float[] Intersect(Ray ray)
        {
            Vector3[] parameters = new Vector3[]{MinVector,MaxVector};
            float tymin;
            float tymax;
            float tzmin;
            float tzmax;
            float tmin;
            float tmax;
            tmin = (parameters[    ray.Sign[0]].X - ray.Origin.X) * ray.InvDirection.X;
            tmax = (parameters[1 - ray.Sign[0]].X - ray.Origin.X) * ray.InvDirection.X;

            tymin = (parameters[    ray.Sign[1]].Y - ray.Origin.Y) * ray.InvDirection.Y;
            tymax = (parameters[1 - ray.Sign[1]].Y - ray.Origin.Y) * ray.InvDirection.Y;

            if (tmin > tymax || tymin > tmax) return null;
            if (tymin > tmin) tmin = tymin;
            if (tymax < tmax) tmax = tymax;

            tzmin = (parameters[  ray.Sign[2]].Z - ray.Origin.Z) * ray.InvDirection.Z;
            tzmax = (parameters[1-ray.Sign[2]].Z - ray.Origin.Z) * ray.InvDirection.Z;

            if (tmin > tzmax || tzmin > tmax) return null;
            if (tzmin > tmin) tmin = tzmin;
            if (tzmax < tmax) tmax = tzmax;

            if (tmax < 0) return null;

            return new float[] { tmin, tmax };


        }
    }
}
