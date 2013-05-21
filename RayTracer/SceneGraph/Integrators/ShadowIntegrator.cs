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

        public Color Integrate(Ray ray, IIntersectable objects, List<ILight> lights, ISampler sampler, List<List<Sample>> subPathSamples)
        {
            HitRecord record = objects.Intersect(ray);

            Color returnColor = new Color(0,0,0);
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

                    //DEBUGGING
                    /*if (shadowHit != null && (shadowHit.Distance > distance.Length))
                    {
                        returnColor.Append(record.HitObject.Material.Shade(record, light.GetLightDirection(record.IntersectionPoint)).Mult(light.GetIncidentColor(record.IntersectionPoint)));
                    }*/
                    returnColor.Append(record.HitObject.Material.Shade(record, light.GetLightDirection(record.IntersectionPoint)).Mult(light.GetIncidentColor(record.IntersectionPoint)));
                 
                }


            }
            return returnColor;
        }

      
    }
}
