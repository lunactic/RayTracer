using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.SceneGraph.Light;
using RayTracer.SceneGraph.Objects;
using RayTracer.Structs;
using RayTracer.Samplers;

namespace RayTracer.SceneGraph.Integrators
{
    public class ShadowIntegrator : IIntegrator
    {

        public Color Integrate(Ray ray, IntersectableList objects, List<ILight> lights, ISampler sampler)
        {
            HitRecord record = objects.Intersect(ray);

            Color returnColor = Color.Black;
            if (record != null)
            {
                foreach (ILight light in lights)
                {
                    Vector3 lightDirection = light.GetLightDirection(record.IntersectionPoint);
                    Vector3 hitPos = record.IntersectionPoint;
                    Vector3 offset = record.RayDirection;
                    offset = -offset;
                    offset *= 0.001f;
                    hitPos += offset;
                    Ray shadowRay = new Ray(hitPos, lightDirection);
                    HitRecord shadowHit = objects.Intersect(shadowRay);
                    Vector3 distance = Vector3.Subtract(light.Position, hitPos);

                    if (shadowHit != null && (shadowHit.Distance > distance.Length))
                    {
                        returnColor += record.HitObject.Material.Shade(record, light.GetLightDirection(record.IntersectionPoint)) * light.GetIncidentColor(record.IntersectionPoint);
                    }

                }


            }
            return returnColor;
        }
    }
}
