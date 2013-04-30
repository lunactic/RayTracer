using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Helper;
using RayTracer.SceneGraph.Light;
using RayTracer.SceneGraph.Materials;
using RayTracer.SceneGraph.Objects;
using RayTracer.Structs;
using RayTracer.Samplers;

namespace RayTracer.SceneGraph.Integrators
{
    public class RefractionIntegrator : IIntegrator
    {

        private IIntersectable objects;
        private List<ILight> lights;
        private ISampler sampler;
        private Random random = new Random();
        public Color Integrate(Ray ray, IIntersectable objects, List<ILight> lights, ISampler sampler)
        {
            HitRecord record = objects.Intersect(ray);
            Color retColor = new Color(0, 0, 0);
            this.objects = objects;
            this.lights = lights;
            this.sampler = sampler;
            retColor.Append(GetShadeColor(record, 0));
            return retColor;
        }

        private Color GetShadeColor(HitRecord record, int currentDepth)
        {
            if (record != null && record.Distance > 0 && record.Distance < float.MaxValue)
            {
                if (record.Material is MirrorMaterial && currentDepth <= Constants.MaximumRecursionDepth) //Shade Mirror Objects
                {
                    HitRecord mirrorRecord = objects.Intersect(record.CreateReflectedRay());
                    return GetShadeColor(mirrorRecord, ++currentDepth).Mult(record.Material.Ks);
                }
                if (record.Material is RefractiveMaterial && currentDepth <= Constants.MaximumRecursionDepth) //Shade Reflective Objects
                {
                    float r = Reflectance(record);
                    HitRecord reflectionHit = objects.Intersect(record.CreateReflectedRay());
                    Color reflectedColor = GetShadeColor(reflectionHit, ++currentDepth).Mult(record.Material.Ks);
                    HitRecord refractedHit = objects.Intersect(CreateRefractedRay(record));
                    Color refractedColor = GetShadeColor(refractedHit, ++currentDepth).Mult(record.Material.Ks);
                    Color retColor = reflectedColor.Mult(r);
                    retColor.Append(refractedColor.Mult(1 - r));
                    return retColor;
                }

                if (record.HitObject.Light != null && Constants.IsLightSamplingOn)
                    return record.HitObject.Light.LightColor;

                Color returnColor = new Color(0, 0, 0);
                foreach (ILight light in lights)
                {
                    Color lightColor = new Color(0, 0, 0);

                    if (Constants.IsLightSamplingOn && Constants.NumberOfLightSamples > 0)
                    {
                        List<LightSample> lightSamples = sampler.GetLightSamples();
                        for (int i = 0; i < Constants.NumberOfLightSamples; i++)
                        {
                            LightSample sample = lightSamples[(int)(random.NextDouble() * lightSamples.Count)];
                            lightSamples.Remove(sample);
                            light.Sample(record, sample);
                            if (IsVisible(record, sample) && sample.LightColor.Power > 0)
                            {
                                if (!(light is AreaLight)) lightColor.Append(record.Material.Shade(record, sample.Wi).Mult(sample.LightColor));
                                else
                                    lightColor.Append(record.Material.Shade(record, sample.Wi).Mult(sample.LightColor).Div(sample.Pdf));
                            }
                        }
                        returnColor.Append(lightColor.Div(Constants.NumberOfLightSamples));
                    }

                    if (!(light is AreaLight) && IsVisible(record, light))
                    {
                        returnColor.Append(record.HitObject.Material.Shade(record, light.GetLightDirection(record.IntersectionPoint)).Mult(light.GetIncidentColor(record.IntersectionPoint)));
                    }
                }
                return returnColor;
            }
            return new Color(0, 0, 0);
        }

        private bool IsVisible(HitRecord record, ILight light)
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

            return (shadowHit != null && (shadowHit.Distance > distance.Length));
        }

        private bool IsVisible(HitRecord record, LightSample sample)
        {
            Vector3 hitToLight = sample.Wi;
            Vector3 pos = record.IntersectionPoint;
            Vector3 offset = record.RayDirection;
            offset = -offset;
            offset *= 0.001f;
            pos += offset;
            HitRecord lightHit = objects.Intersect(new Ray(pos, hitToLight));
            Vector3 dist = Vector3.Subtract(sample.Position, pos);
            return (lightHit == null || lightHit.HitObject.Light != null || lightHit.Distance > dist.Length);
        }

        private Ray CreateRefractedRay(HitRecord record)
        {
            Vector3 incident = new Vector3(record.CreateReflectedRay().Direction);
            Vector3 normal = new Vector3(record.SurfaceNormal);
            float n1, n2;
            float cosI = Vector3.Dot(normal, incident);
            if (cosI < 0)
            {
                //Material to Air
                n1 = record.Material.RefractionIndex;
                n2 = Refractions.AIR;
                normal = -normal;
                cosI = Vector3.Dot(normal, incident);
            }
            else
            {
                //Air to Material
                n1 = Refractions.AIR;
                n2 = record.Material.RefractionIndex;
            }

            float n = n1 / n2;

            float sinT2 = (float)(n * n * (1.0 - cosI * cosI));

            if (sinT2 > 1.0) return new Ray(record.IntersectionPoint, record.CreateReflectedRay().Direction); //TIR

            float cosT = (float)Math.Sqrt(1.0 - sinT2);

            Vector3 dir = record.RayDirection;
            dir.Normalize();
            dir = dir * n;
            normal = normal * (float)(n * cosI - cosT);
            dir += normal;

            Vector3 pos = record.IntersectionPoint;
            Vector3 offset = dir;
            offset *= 0.0001f;
            pos += offset;

            return new Ray(pos, dir);
        }

        private float Reflectance(HitRecord record)
        {
            Vector3 incident = new Vector3(record.CreateReflectedRay().Direction);
            Vector3 normal = new Vector3(record.SurfaceNormal);

            float n1, n2;
            float cosI = Vector3.Dot(normal, incident);
            if (cosI < 0)
            {
                //Material to Air
                n1 = record.Material.RefractionIndex;
                n2 = Refractions.AIR;
                normal = -normal;
                cosI = Vector3.Dot(normal, incident);
            }
            else
            {
                //Air to Material
                n1 = Refractions.AIR;
                n2 = record.Material.RefractionIndex;

            }

            double n = n1 / n2;
            double sinT2 = n * n * (1.0 - cosI * cosI);

            if (sinT2 > 1.0) return 1.0f; //Total internal Reflection

            double cosT = Math.Sqrt(1.0 - sinT2);
            double rOrth = (n1 * cosI - n2 * cosT) / (n1 * cosI + n2 * cosT);
            double rPar = (n2 * cosI - n1 * cosT) / (n2 * cosI + n1 * cosT);

            return (float)(rOrth * rOrth + rPar * rPar) / 2.0f;

        }

        private float RSchlick2(HitRecord record)
        {
            Vector3 incident = new Vector3(record.CreateReflectedRay().Direction);
            Vector3 normal = new Vector3(record.SurfaceNormal);
            float n1, n2;

            float cosX = Vector3.Dot(normal, incident);
            if (cosX < 0)
            {
                //Material to Air
                n1 = record.Material.RefractionIndex;
                n2 = Refractions.AIR;
                normal = -normal;
                cosX = Vector3.Dot(normal, incident);
            }
            else
            {
                //Air to Material
                n1 = Refractions.AIR;
                n2 = record.Material.RefractionIndex;
            }
            double r0 = (n1 - n2) / (n1 + n2);
            r0 *= r0;
            if (n1 > n2)
            {
                double n = n1 / n2;
                double sinT2 = n * n * (1.0 - cosX * cosX);
                if (sinT2 > 1.0)
                    return 1.0f;

                cosX = (float)Math.Sqrt(1.0 - sinT2);

            }
            double x = 1.0 - cosX;
            return (float)(r0 + (1.0 - r0) * x * x * x * x * x);
        }

    }


}
