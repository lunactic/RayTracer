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
        public IntersectionType Intersect(IBoundingBox otherBox)
        {
            if (otherBox.MinVector.X >= MinVector.X && otherBox.MaxVector.X <= MaxVector.X &&
                otherBox.MinVector.Y >= MinVector.Y && otherBox.MaxVector.Y <= MaxVector.Y &&
                otherBox.MinVector.Z >= MinVector.Z && otherBox.MaxVector.Z <= MaxVector.Z)
                return IntersectionType.Inside;

            if (MaxVector.X < otherBox.MaxVector.X || MinVector.X > otherBox.MinVector.X)
                return IntersectionType.Outside;
            if (MaxVector.Y < otherBox.MaxVector.Y || MinVector.Y > otherBox.MinVector.Y)
                return IntersectionType.Outside;
            if (MaxVector.Z < otherBox.MaxVector.Z || MinVector.Z > otherBox.MinVector.Z)
                return IntersectionType.Outside;

            return IntersectionType.Intersection;
        }

        public bool Intersect(Ray ray)
        {
            Vector3 tMin = Vector3.Zero;
            Vector3 tMax = Vector3.Zero;
            Vector3[] parameters = new Vector3[]{MinVector,MaxVector};
            float tymin;
            float tymax;
            float tzmin;
            float tzmax;

            ray.Tmin = (parameters[    ray.Sign[0]].X - ray.Origin.X) * ray.InvDirection.X;
            ray.Tmax = (parameters[1 - ray.Sign[0]].X - ray.Origin.X) * ray.InvDirection.X;

            tymin = (parameters[    ray.Sign[1]].Y - ray.Origin.Y) * ray.InvDirection.Y;
            tymax = (parameters[1 - ray.Sign[1]].Y - ray.Origin.Y) * ray.InvDirection.Y;

            if (ray.Tmin > tymax || tymin > ray.Tmax) return false;
            if (tymin > ray.Tmin) ray.Tmin = tymin;
            if (tymax < ray.Tmax) ray.Tmax = tymax;

            tzmin = (parameters[  ray.Sign[2]].Z - ray.Origin.Z) * ray.InvDirection.Z;
            tzmax = (parameters[1-ray.Sign[2]].Z - ray.Origin.Z) * ray.InvDirection.Z;

            if (ray.Tmin > tzmax || tzmin > ray.Tmax) return false;
            if (tzmin > ray.Tmin) ray.Tmin = tzmin;
            if (tzmax < ray.Tmax) ray.Tmax = tzmax;

            //Maybe need a reference to the nearest intersection...
            return (ray.Tmax > 0);


        }
    }
}
