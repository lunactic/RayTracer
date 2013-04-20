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
    public class Instance : Aggregate, IIntersectable
    {
        public IIntersectable Intersectable { get; private set; }
        /// <summary>
        /// Object to world transformation
        /// </summary>
        public Matrix4 TransformationMatrix { get; set; }
        /// <summary>
        /// World to object transformation
        /// </summary>
        public Matrix4 InvTransformationMatrix { get; set; }
        /// <summary>
        /// The Transposed Transformation Matrix for convenience
        /// </summary>
        public Matrix4 TransposedTransformationMatrix { get; set; }

        public Instance(IIntersectable intersectable, Matrix4 transMatrix)
        {
            TransformationMatrix = transMatrix;
            Intersectable = intersectable;
            InvTransformationMatrix = Matrix4.Invert(TransformationMatrix);
            TransposedTransformationMatrix = Matrix4.Transpose(TransformationMatrix);
            Material = Intersectable.Material;
        }

        public Material Material
        {
            get; set;
        }

        public new HitRecord Intersect(Ray ray)
        {

            //Transform ray to object coordinate system
            Ray transfRay = RayTransformer.TransformRayToObject(ray,InvTransformationMatrix);
            HitRecord hit = Intersectable.Intersect(transfRay);


            if (hit != null)
            {
                if (Material != null)
                    hit.Material = Material;
                //Transform HitRecrod back to world coordinate system
                RayTransformer.TransformHitToWorld(hit,TransformationMatrix,TransposedTransformationMatrix);
           }
            return hit;
        }

        public override void BuildBoundingBox()
        {
            Intersectable.BuildBoundingBox();
        }
        
        public ILight Light
        {
            get
            {
                return Intersectable.Light;
            }
            set
            {
                Intersectable.Light = value;
            }
        }

        public Vector3 GetSamplePoint(float x, float y)
        {
            return Intersectable.GetSamplePoint(x, y);
        }

        public float GetArea()
        {
            return Intersectable.GetArea();
        }

        public Vector3 GetSampledNormal(float x, float y)
        {
            return Intersectable.GetSampledNormal(x,y);
        }
    }
}
