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
        public Color Integrate(Ray ray, IIntersectable objects, List<ILight> lights, ISampler sampler, List<List<Sample>> subPathSamples, LightSample lightSample)
        {
            Color returnColor = new Color(0,0,0);
            /*
             * Core Ray Tracing algorithm
             */
            HitRecord record = objects.Intersect(ray);

            if (record != null && record.Distance > 0 && record.Distance < float.PositiveInfinity)
            {
                foreach (ILight light in lights)
                {
                    returnColor.Append(record.Material.Shade(record, light.GetLightDirection(record.IntersectionPoint)).Mult(light.GetIncidentColor(record.IntersectionPoint)));
                }
            }
            return returnColor;
        }
    }
}
