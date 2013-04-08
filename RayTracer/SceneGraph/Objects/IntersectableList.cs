using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Structs;
using RayTracer.SceneGraph.Materials;

namespace RayTracer.SceneGraph.Objects
{
    public class IntersectableList : Aggregate, IIntersectable
    {
        public Material Material {get; set; }
        public Matrix4 TransformationMatrix { get; set; }
        public Matrix4 InvTransformationMatrix { get; set; }
        public Matrix4 TransposedTransformationMatrix { get; set; }
        public IntersectableList()
        {
               Objects = new List<IIntersectable>();
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

        public override List<IIntersectable> GetObjects()
        {
            return Objects;
        }
    }
}
