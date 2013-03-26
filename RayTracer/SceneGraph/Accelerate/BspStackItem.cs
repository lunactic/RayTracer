using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.SceneGraph.Accelerate
{
    public class BspStackItem
    {
        public BspNode Node { get; set; }
        public float Tmin { get; set; }
        public float Tmax { get; set; }
        public BspStackItem(BspNode node, float tmin, float tmax)
        {
            Node = node;
            Tmin = tmin;
            Tmax = tmax;
        }
    }
}
