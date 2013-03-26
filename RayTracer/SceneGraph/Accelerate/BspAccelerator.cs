using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.SceneGraph.Objects;
using RayTracer.Structs;

namespace RayTracer.SceneGraph.Accelerate
{
    public class BspAccelerator : IIntersectable
    {
        public enum OrderType { XOrdered, YOrdered }

        public BspNode RootNode { get; private set; }

        private int maxTreeDepth;

        public BspAccelerator()
        {
        }

        public void Construct(Aggregate aggregate)
        {
            List<IIntersectable> objects = aggregate.GetObjects();
            //Calculate max treed depth = 8+1.3*log(n);
            maxTreeDepth = (int)Math.Ceiling(8 + (1.3 * Math.Log(objects.Count)));

            RootNode = new BspNode();

            //Calculate the Boundig Box for the Root-Note;
            IBoundingBox rootBox = new AxisAlignedBoundingBox();
            Vector3 minVector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 maxVector = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            foreach (IIntersectable item in objects)
            {
                IBoundingBox box = item.BoundingBox;

                if (box.MinVector.X < minVector.X) minVector.X = box.MinVector.X;
                if (box.MinVector.Y < minVector.Y) minVector.Y = box.MinVector.Y;
                if (box.MinVector.Z < minVector.Z) minVector.Z = box.MinVector.Z;

                if (box.MaxVector.X > maxVector.X) maxVector.X = box.MaxVector.X;
                if (box.MaxVector.Y > maxVector.Y) maxVector.Y = box.MaxVector.Y;
                if (box.MaxVector.Z > maxVector.Z) maxVector.Z = box.MaxVector.Z;
            }
            rootBox.MinVector = minVector;
            rootBox.MaxVector = maxVector;

            List<IIntersectable> xOrdered = objects.OrderBy(intersectable => intersectable.BoundingBox.Center.X).ToList();
            List<IIntersectable> yOrdered = objects.OrderBy(intersectable => intersectable.BoundingBox.Center.Y).ToList();

            Dictionary<OrderType, List<IIntersectable>> orderedIntersectables = new Dictionary<OrderType, List<IIntersectable>>();
            orderedIntersectables.Add(OrderType.XOrdered, xOrdered);
            orderedIntersectables.Add(OrderType.YOrdered, yOrdered);

            //Kick off the recursive tree construction
            RootNode.BuildSubTree(objects, rootBox, maxTreeDepth, 0);




        }


        public IBoundingBox BoundingBox
        {
            get { return RootNode.BoundingBox; }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Materials.Material Material
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

        public HitRecord Intersect(Ray ray)
        {


            #region BSP Tree Traversal
            BspNode node = RootNode;
            HitRecord hit = null;
            Stack<BspNode> nodeStack = new Stack<BspNode>();
            BspNode first, second;


            float isect = float.MaxValue;

            while (node != null)
            {
                node.BoundingBox.Intersect(ray);
                if (isect < ray.Tmin) break;
                if (!node.IsLeaf)
                {
                    //process interior node
                    //tsplit = compute ray-split plane intersection
                    //<order of children>
                    //<process of children>

                    float tSplit = node.DistanceToSplitPlane(ray);
                    //Order children
                    if (ray.Origin[node.Axis] < node.PlanePos[node.Axis])
                    {
                        first = node.RightNode;
                        second = node.LeftNode;
                    }
                    else
                    {
                        first = node.LeftNode;
                        second = node.RightNode;
                    }
                    //Process children
                    if (tSplit < 0 || tSplit > ray.Tmax)
                    {
                        node = first;
                    }
                    else if (tSplit < ray.Tmin)
                    {
                        node = second;
                    }
                    else
                    {
                        node = first;
                        nodeStack.Push(second);
                    }
                }
                else
                {
                    //Intersection = first intersection in leaf
                    HitRecord tmpHitRecord = null;
                    int i = 0;
                    while (tmpHitRecord == null && i < node.Intersectables.Count)
                    {
                        tmpHitRecord = node.Intersectables[i].Intersect(ray);
                        i++;
                        //get node from stack
                        //Compute new tmin,tmax with nodeBB and ray
                    }
                    if (tmpHitRecord != null)
                    {
                        if (hit == null) hit = tmpHitRecord;
                        if (tmpHitRecord.Distance < hit.Distance) hit = tmpHitRecord;

                        if (hit != null) isect = hit.Distance;
                    }

                    if (nodeStack.Count > 0)
                        node = nodeStack.Pop();
                    else
                        node = null;
                }
            }
       
            return  hit;
             
            #endregion
            #region simple tree traversal
            /**
             *Traverse the tree and do intersection 
             */
            /*Stack<BspNode> nodesToVisit = new Stack<BspNode>();
            nodesToVisit.Push(RootNode);
            HitRecord hit = null;
            HitRecord tempHit;
            while (nodesToVisit.Count > 0)
            {
                BspNode currentNode = nodesToVisit.Pop();
                if (currentNode.IsLeaf)
                {
                    foreach (IIntersectable intersectable in currentNode.Intersectables)
                    {
                        tempHit = intersectable.Intersect(ray);
                        if (tempHit != null)
                        {
                            if (hit == null) hit = tempHit;
                            else if (tempHit.Distance < hit.Distance) hit = tempHit;
                        }
                    }
                }
                else
                {
                    nodesToVisit.Push(currentNode.LeftNode);
                    nodesToVisit.Push(currentNode.RightNode);
                }
            }
            return hit;*/
            #endregion
        }



        public Structs.Vector3 GetNormal(Structs.Vector3 hitPosition)
        {
            throw new NotImplementedException();
        }

        public void BuildBoundingBox()
        {
            throw new NotSupportedException();
        }
    }
}
