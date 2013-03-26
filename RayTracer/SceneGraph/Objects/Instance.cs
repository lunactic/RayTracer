using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Structs;
using RayTracer.SceneGraph.Materials;

namespace RayTracer.SceneGraph.Objects
{
    public class Instance : IIntersectable
    {
        public IIntersectable Intersectable { get; private set; }
        /// <summary>
        /// Object to world transformation
        /// </summary>
        public Matrix4 TransformationMatrix { get; private set; }
        /// <summary>
        /// World to object transformation
        /// </summary>
        public Matrix4 InvTransformationMatrix { get; private set; }
        /// <summary>
        /// The Transposed Transformation Matrix for convenience
        /// </summary>
        public Matrix4 TransposedTransformationMatrix { get; private set; }

        public Accelerate.IBoundingBox BoundingBox
        {
            get { return Intersectable.BoundingBox; }
            set { Intersectable.BoundingBox = value; }
        }


        public Instance(IIntersectable intersectable, Matrix4 transMatrix)
        {
            TransformationMatrix = transMatrix;
            Intersectable = intersectable;
            InvTransformationMatrix = Matrix4.Invert(TransformationMatrix);
            TransposedTransformationMatrix = Matrix4.Transpose(TransformationMatrix);
        }

        public Material Material
        {
            get { return Intersectable.Material; }
            set { Intersectable.Material = value; }
        }

        public HitRecord Intersect(Ray ray)
        {

            //Transform ray to object coordinate system
            Ray transfRay = TransformRayToObject(ray);
            HitRecord hit = Intersectable.Intersect(transfRay);
         

            if (hit != null)
            {
                if (Material != null)
                    hit.Material = Material;
                //Transform HitRecrod back to world coordinate system
                Vector4 intersectionPoint = new Vector4(hit.IntersectionPoint) { W = 1 };
                Vector3 worldIntersectionPoint = Vector4.Transform(intersectionPoint, TransformationMatrix);


                Vector4 normal = new Vector4(hit.SurfaceNormal);
                Vector3 worldNormal = Vector4.Transform(normal, TransposedTransformationMatrix);
                worldNormal.Normalize();

                Vector4 rayDir = Vector4.Transform(new Vector4(hit.RayDirection), TransformationMatrix);
                Vector3 worldRayDir = rayDir;
                hit.IntersectionPoint = worldIntersectionPoint;
                hit.SurfaceNormal = worldNormal;
                hit.RayDirection = worldRayDir;
            }
            return hit;
        }


        private Ray TransformRayToObject(Ray ray)
        {
            Vector4 dir = new Vector4(ray.Direction);
            Vector4 orig = new Vector4(ray.Origin) { W = 1 };

            Vector4 transfDir = Vector4.Transform(dir, InvTransformationMatrix);
            Vector4 transfOrig = Vector4.Transform(orig, InvTransformationMatrix);

            return new Ray(transfOrig, transfDir);
        }

        public Vector3 GetNormal(Vector3 hitPosition)
        {
            return Intersectable.GetNormal(hitPosition);
        }

        public void BuildBoundingBox()
        {
            Intersectable.BuildBoundingBox();
        }
    }
}
