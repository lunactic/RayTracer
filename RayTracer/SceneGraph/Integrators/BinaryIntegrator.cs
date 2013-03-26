using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Structs;

namespace RayTracer.SceneGraph.Integrators
{
    public class BinaryIntegrator : IIntegrator
    {
        private Scene scene;
        public BinaryIntegrator(Scene scene)
        {
            this.scene = scene;
        }

        public Color Integrate(Ray ray)
        {
            HitRecord record = scene.Intersect(ray);
            if (record != null)
                return Color.White;
            else
                return Color.Black;
        }
    }
}
