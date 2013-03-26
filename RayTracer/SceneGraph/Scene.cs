using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.SceneGraph.Objects;
using RayTracer.Structs;
using RayTracer.SceneGraph.Light;

namespace RayTracer.SceneGraph
{
    public class Scene
    {
        public IntersectableList IntersectableList { get; set; }
        public List<ILight> Lights { get; set; }
        public Color BackgroundColor { get; set; }
        public Color Ambient { get; set; }

        public Scene()
        {
            IntersectableList = new IntersectableList();
            Lights = new List<ILight>();
        }

        public HitRecord Intersect(Ray ray)
        {
            /*
             * ray tracing algorithm
             */
            return IntersectableList.Intersect(ray);

        }
    }
}
