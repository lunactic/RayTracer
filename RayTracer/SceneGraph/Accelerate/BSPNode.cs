using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Structs;
using RayTracer.SceneGraph.Objects;

namespace RayTracer.SceneGraph.Accelerate
{
    public class BspNode
    {
        public float PlanePos { get; set; }
        /// <summary>
        /// The Split Axis (0 = X-Axis, 1 = Y-Axis, 2 = Z-Axis)
        /// </summary>
        public int Axis { get; set; }
        public Boolean IsLeaf { get; private set; }
        public BspNode LeftNode { get; private set; }
        public BspNode RightNode { get; private set; }
        public Aggregate Intersectables { get; private set; }
        public IBoundingBox BoundingBox { get; set; }


        public BspNode()
        {
            IsLeaf = false;
        }

        public void BuildSubTree(Aggregate intersectables, IBoundingBox boundingBox, int maxTreeDepth, int currentTreeDetph)
        {
            Intersectables = intersectables;
            BoundingBox = boundingBox;

            if (Intersectables.GetNumberOfComponents() <= 5 || currentTreeDetph == maxTreeDepth)
            {
                IsLeaf = true;

            }
            else
            {
                Aggregate leftIntersectables = new IntersectableList();
                Aggregate rightIntersectables = new IntersectableList();
                //Find the maximal Axis
                Axis = 0;

                /*float max = BoundingBox.Dimension.X;
                if (BoundingBox.Dimension.Y > max)
                    Axis = 1;
                if (BoundingBox.Dimension.Z > max)
                    Axis = 2;
                */
                Axis = currentTreeDetph % 3;
                PlanePos = BoundingBox.MaxVector[Axis] - (BoundingBox.MaxVector[Axis] - BoundingBox.MinVector[Axis]) / 2;
                IBoundingBox leftBb = new AxisAlignedBoundingBox();
                IBoundingBox rightBb = new AxisAlignedBoundingBox();

                switch (Axis)
                {
                    case 0:
                        leftBb.MinVector = new Vector3(PlanePos, BoundingBox.MinVector.Y, BoundingBox.MinVector.Z);
                        leftBb.MaxVector = new Vector3(BoundingBox.MaxVector);
                        rightBb.MinVector = new Vector3(BoundingBox.MinVector);
                        rightBb.MaxVector = new Vector3(PlanePos, BoundingBox.MaxVector.Y, BoundingBox.MaxVector.Z);
                        break;
                    case 1:
                        leftBb.MinVector = new Vector3(BoundingBox.MinVector.X, PlanePos, BoundingBox.MinVector.Z);
                        leftBb.MaxVector = new Vector3(BoundingBox.MaxVector);
                        rightBb.MinVector = new Vector3(BoundingBox.MinVector);
                        rightBb.MaxVector = new Vector3(BoundingBox.MaxVector.X, PlanePos, BoundingBox.MaxVector.Z);
                        break;
                    case 2:
                        leftBb.MinVector = new Vector3(BoundingBox.MinVector.X, BoundingBox.MinVector.Y, PlanePos);
                        leftBb.MaxVector = new Vector3(BoundingBox.MaxVector);
                        rightBb.MinVector = new Vector3(BoundingBox.MinVector);
                        rightBb.MaxVector = new Vector3(BoundingBox.MaxVector.X, BoundingBox.MaxVector.Y, PlanePos);
                        break;
                }


                LeftNode = new BspNode();
                RightNode = new BspNode();
                foreach (IIntersectable intersectable in Intersectables.GetObjects())
                {
                    if (intersectable.BoundingBox.Intersect(leftBb)) leftIntersectables.Add(intersectable);
                    if (intersectable.BoundingBox.Intersect(rightBb)) rightIntersectables.Add(intersectable);
                }
                LeftNode.BuildSubTree(leftIntersectables, leftBb, maxTreeDepth, currentTreeDetph + 1);
                RightNode.BuildSubTree(rightIntersectables, rightBb, maxTreeDepth, currentTreeDetph + 1);
                Intersectables = null; //Clear up the intersectables in non leaf nodes to save space
            }
        }

        public float DistanceToSplitPlane(Ray ray)
        {

            Vector3 rayOrigin = ray.Origin;
            Vector3 rayDirection = ray.Direction;
            Vector3 normal = Vector3.Zero;

            switch (Axis)
            {
                case 0: normal = new Vector3(1, 0, 0); break;
                case 1: normal = new Vector3(0, 1, 0); break;
                case 2: normal = new Vector3(0, 0, 1); break;
            }

            Vector3 p0 = new Vector3(normal);
            p0 = p0 * PlanePos;
            p0 = p0 - rayOrigin;

            return Vector3.Dot(p0, normal) / Vector3.Dot(rayDirection, normal);
        }
    }
}
