using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.SceneGraph.Objects;
using RayTracer.Structs;
using RayTracer.SceneGraph.Light;

namespace RayTracer.SceneGraph.Integrators
{
    public class BlinnIntegrator : IIntegrator
    {
        private Scene scene;
        public BlinnIntegrator(Scene scene)
        {
            this.scene = scene;
        }
        public Color Integrate(Ray ray)
        {
            Color returnColor = Color.Black;
            /*
             * Core Ray Tracing algorithm
             */
            HitRecord record = scene.Intersect(ray);

            if (record != null && record.Distance > 0 && record.Distance < float.PositiveInfinity)
            {
                returnColor = scene.Lights.Aggregate(returnColor, (current, light) => current + record.HitObject.Material.Shade(record, scene, light));
            }
            return returnColor;
        }
    }
}
