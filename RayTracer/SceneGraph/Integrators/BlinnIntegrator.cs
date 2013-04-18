using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.SceneGraph.Objects;
using RayTracer.Structs;
using RayTracer.SceneGraph.Light;
using RayTracer.Samplers;

namespace RayTracer.SceneGraph.Integrators
{
    public class BlinnIntegrator : IIntegrator
    {
        public Color Integrate(Ray ray, IntersectableList objects, List<ILight> lights, ISampler sampler)
        {
            Color returnColor = Color.Black;
            /*
             * Core Ray Tracing algorithm
             */
            HitRecord record = objects.Intersect(ray);

            if (record != null && record.Distance > 0 && record.Distance < float.PositiveInfinity)
            {
                returnColor = lights.Aggregate(returnColor, (current, light) => current + record.HitObject.Material.Shade(record,light.GetLightDirection(record.IntersectionPoint))*light.GetIncidentColor(record.IntersectionPoint));
            }
            return returnColor;
        }
    }
}
