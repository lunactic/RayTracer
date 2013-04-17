﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Helper;
using RayTracer.SceneGraph.Light;
using RayTracer.SceneGraph.Materials;
using RayTracer.Structs;
using RayTracer.Samplers;

namespace RayTracer.SceneGraph.Integrators
{
    public class RefractionIntegrator : IIntegrator
    {
        private Scene scene;
        private ISampler sampler;
        public RefractionIntegrator(Scene scene, ISampler sampler)
        {
            this.scene = scene;
            this.sampler = sampler;
        }

        public Color Integrate(Ray ray)
        {
            HitRecord record = scene.Intersect(ray);
            Color retColor = Color.Black;
            retColor += GetShadeColor(record, 0);
            return retColor;
        }

        private Color GetShadeColor(HitRecord record, int currentDepth)
        {
            if (record != null)
            {
                if (record.Material is MirrorMaterial && currentDepth <= Constants.MaximumRecursionDepth) //Shade Mirror Objects
                {
                    HitRecord mirrorRecord = scene.Intersect(record.CreateReflectedRay());
                    return GetShadeColor(mirrorRecord, ++currentDepth) * record.Material.Ks;
                }
                if (record.Material is RefractiveMaterial && currentDepth <= Constants.MaximumRecursionDepth) //Shade Reflective Objects
                {
                    float r = Reflectance(record);
                    HitRecord reflectionHit = scene.Intersect(record.CreateReflectedRay());
                    Color reflectedColor = GetShadeColor(reflectionHit, ++currentDepth) * record.Material.Ks;
                    HitRecord refractedHit = scene.Intersect(CreateRefractedRay(record));
                    Color refractedColor = GetShadeColor(refractedHit, ++currentDepth) * record.Material.Ks;
                    Color retColor = reflectedColor * r;
                    retColor += refractedColor * (1 - r);
                    return retColor;
                }
                else //Shade 'normal' Objects
                {
                    Color returnColor = Color.Black;

                    if (record.HitObject.Light != null && Constants.IsLightSamplingOn) 
                        return record.HitObject.Light.LightColor;

                    foreach (ILight light in scene.Lights)
                    {
                        Color lightColor = Color.Black;

                        if (Constants.IsLightSamplingOn && Constants.NumberOfLightSamples > 0)
                        {
                            List<LightSample> lightSamples = sampler.GetLightSamples();
                            for(int i = 0; i<Constants.NumberOfLightSamples; i++)
                            {
                                LightSample sample = lightSamples[(int)new Random().NextDouble()*lightSamples.Count];
                                
                                light.Sample(record, sample);
                                if (IsVisible(record, sample) && sample.LightColor.Power > 0)
                                {
                                    if (!(light is AreaLight)) lightColor += record.Material.Shade(record, sample.Wi) * sample.LightColor;
                                    else lightColor += record.Material.Shade(record, sample.Wi) * sample.LightColor * (1f/sample.Pdf);
                                }
                            }
                            returnColor += lightColor * (1f / Constants.NumberOfLightSamples);
                        }

                        if (!(light is AreaLight) && IsVisible(record,light))
                        {
                            returnColor += record.HitObject.Material.Shade(record,light.GetLightDirection(record.IntersectionPoint)) * light.GetIncidentColor(record.IntersectionPoint);
                        }
                    }
                    return returnColor;
                }
            }
            return Color.Black;
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
            HitRecord shadowHit = scene.Intersect(shadowRay);
            Vector3 distance = Vector3.Subtract(light.Position, hitPos);

            return (shadowHit != null && (shadowHit.Distance > distance.Length));
        }

        private bool IsVisible(HitRecord record, LightSample sample)
        {
            Vector3 hitToLight = sample.Wi;
            Vector3 pos = new Vector3(record.IntersectionPoint);
            Vector3 offset = record.RayDirection;
            offset = -offset;
            offset *= 0.001f;
            pos+=offset;
            HitRecord lightHit = scene.Intersect(new Ray(pos,hitToLight));
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

            return new Ray(pos,dir);
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
            double r0 = (n1 - n2)/(n1 + n2);
            r0 *= r0;
           if (n1 > n2)
            {
                double n = n1/n2;
                double sinT2 = n*n*(1.0 - cosX*cosX);
                if (sinT2 > 1.0)
                    return 1.0f;

                cosX = (float)Math.Sqrt(1.0 - sinT2);

            }
            double x = 1.0 - cosX;
            return (float)(r0 + (1.0 - r0)*x*x*x*x*x);
        }
    }


}
