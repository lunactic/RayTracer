using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.SceneGraph.Materials;
using RayTracer.SceneGraph.Objects;
using RayTracer.Structs;
using RayTracer.SceneGraph.Light;

namespace RayTracer.SceneGraph.Accelerate
{
    public class BspAccelerator : IIntersectable
    {
        public enum OrderType { XOrdered, YOrdered }
        public ILight Light { get; set; }
        public BspNode RootNode { get; private set; }
        public IBoundingBox BoundingBox { get; set; }
        public Material Material { get; set; }
        public Matrix4 TransformationMatrix { get; set; }
        public Matrix4 InvTransformationMatrix { get; set; }
        public Matrix4 TransposedTransformationMatrix { get; set; }

        private int maxTreeDepth;

        public void Construct(Aggregate aggregate)
        {
            Material = ((IIntersectable)aggregate).Material;
            Aggregate objects = new IntersectableList();
      
            foreach (IIntersectable intersectable in aggregate.GetObjects())
            {
                objects.Add(intersectable);
            }

            BoundingBox = ((IIntersectable)aggregate).BoundingBox;
            //Calculate max treed depth = 8+1.3*log(n);
            maxTreeDepth = (int)Math.Ceiling(8 + (1.3 * Math.Log(objects.Intersectables.Count)));

            RootNode = new BspNode();


            //Kick off the recursive tree construction
            RootNode.BuildSubTree(objects, BoundingBox, maxTreeDepth, 0);
        }

        public HitRecord Intersect(Ray ray)
        {
            #region BSP Tree Traversal
            BspNode node = RootNode;
            HitRecord hit = null;
            Stack<BspStackItem> nodeStack = new Stack<BspStackItem>();
            BspNode first, second;

            float[] boxIntersections = node.BoundingBox.Intersect(ray);
            if (boxIntersections == null) return null;
            float tmin = boxIntersections[0];
            float tmax = boxIntersections[1];

            float isect = float.MaxValue;

            while (node != null)
            {

                if (isect < tmin) return hit;
                if (!node.IsLeaf)
                {
                    //process interior node
                    //tsplit = compute ray-split plane intersection
                    //<order of children>
                    //<process of children>

                    float tSplit = node.DistanceToSplitPlane(ray);
                    //Order children
                    if (ray.Origin[node.Axis] < node.PlanePos)
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
                    if (tSplit < 0 || tSplit > tmax || (tSplit == 0.0f && ray.Direction[node.Axis] > 0))
                    {
                        node = first;
                    }
                    else if (tSplit < tmin || (tSplit == 0.0f && ray.Direction[node.Axis] < 0))
                    {
                        node = second;
                    }
                    else
                    {
                        node = first;
                        BspStackItem item = new BspStackItem(second, tSplit, tmax);
                        nodeStack.Push(item);
                        tmax = tSplit;
                    }
                }
                else
                {
                    //Intersection = first intersection in leaf
                    HitRecord tmpHitRecord = node.Intersectables.Intersect(ray); ;

                    if (tmpHitRecord != null)
                    {
                        isect = tmpHitRecord.Distance;
                        hit = tmpHitRecord;

                    }

                    if (nodeStack.Count > 0)
                    {
                        BspStackItem sItem = nodeStack.Pop();
                        node = sItem.Node;
                        tmin = sItem.Tmin;
                        tmax = sItem.Tmax;
                    }
                    else
                        node = null;
                }
            }

            return hit;

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



        public Vector3 GetNormal(Vector3 hitPosition)
        {
            throw new NotImplementedException();
        }

        public void BuildBoundingBox()
        {
            throw new NotSupportedException();
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
