﻿using System;
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
        public Vector3 PlanePos { get; set; }
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



            if (Intersectables.Objects.Count <= 5 || currentTreeDetph == maxTreeDepth)
            {
                IsLeaf = true;

            }
            else
            {
                Aggregate leftIntersectables = new IntersectableList();
                Aggregate rightIntersectables = new IntersectableList();

                IBoundingBox leftBb = new AxisAlignedBoundingBox();
                IBoundingBox rightBb = new AxisAlignedBoundingBox();
                PlanePos = BoundingBox.Center;
                if (BoundingBox.Dimension.X > BoundingBox.Dimension.Y)
                {
                    //Split along the x-Axis
                    Axis = 0;
                     
                    leftBb.MinVector = BoundingBox.MinVector;
                    leftBb.MaxVector = new Vector3(BoundingBox.MaxVector.X - BoundingBox.Dimension.X / 2, BoundingBox.MaxVector.Y, BoundingBox.MaxVector.Z);

                    rightBb.MaxVector = BoundingBox.MaxVector;
                    rightBb.MinVector = new Vector3(BoundingBox.MinVector.X + BoundingBox.Dimension.X / 2, BoundingBox.MinVector.Y, BoundingBox.MinVector.Z);

    
                }
                else
                {   //Split along the y-Axis
                    Axis = 1;

                    leftBb.MinVector = BoundingBox.MinVector;
                    leftBb.MaxVector = new Vector3(BoundingBox.MaxVector.X,BoundingBox.MaxVector.Y- BoundingBox.Dimension.Y /2,BoundingBox.MaxVector.Z);

                    rightBb.MaxVector = BoundingBox.MaxVector;
                    rightBb.MinVector = new Vector3(BoundingBox.MinVector.X,BoundingBox.MinVector.Y + BoundingBox.Dimension.Y / 2, BoundingBox.MinVector.Z);
                }


                LeftNode = new BspNode();
                RightNode = new BspNode();
                foreach (IIntersectable intersectable in Intersectables.GetObjects())
                {
                    if (intersectable.BoundingBox.Intersect(leftBb)) leftIntersectables.Objects.Add(intersectable);
                    if (intersectable.BoundingBox.Intersect(rightBb)) rightIntersectables.Objects.Add(intersectable);
                    
                }
                LeftNode.BuildSubTree(leftIntersectables, leftBb, maxTreeDepth, currentTreeDetph + 1);
                RightNode.BuildSubTree(rightIntersectables, rightBb, maxTreeDepth, currentTreeDetph + 1);
                Intersectables = null; //Clear up the intersectables in non leaf nodes to save space
            }
        }

        public float DistanceToSplitPlane(Ray ray)
        {

            //int axis = node.SplitAxis();
            //float tplane = (node.SplitPos() - ray.Origin[axis]) * invDir[axis];
            
            //Axis: X = 1; Y = 2; Z = 3
            //TODO: FIX THIS!
            Vector3 rayOrigin = ray.Origin;
            Vector3 rayDirection = ray.Direction;
            Vector3 normal = Vector3.Zero;

            switch (Axis)
            {
                case 0: normal = new Vector3(1, 0, 0); break;
                case 1: normal = new Vector3(0, 1, 0); break;
                case 2: normal = new Vector3(0, 0, 1); break;
                default: break;
            }

            Vector3 p0 = new Vector3(normal);
            p0 = p0*PlanePos[Axis];
            p0 = p0 - rayOrigin;

            return Vector3.Dot(p0,normal) / Vector3.Dot(rayDirection,normal);
            //return (PlanePos[Axis] - ray.Origin[Axis]) * ray.InvDirection[Axis];
        }
    }
}
