﻿using RayTracer.Helper;
using RayTracer.Samplers;
using RayTracer.SceneGraph.Light;
using RayTracer.SceneGraph.Materials;
using RayTracer.SceneGraph.Objects;
using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.SceneGraph.Integrators
{
    public class WhittedIntegrator : IIntegrator
    {
 
        private ShadowIntegrator shadowIntegrator;

        public Color Integrate(Ray ray, IIntersectable objects, List<ILight> lights, ISampler sampler, List<List<Sample>> subPathSamples, LightSample lightSample)
        {
            shadowIntegrator = new ShadowIntegrator();
            Color returnColor = new Color(0,0,0);
            HitRecord record = objects.Intersect(ray);

            if (record != null)
            {
                if (!(record.Material is MirrorMaterial))
                {
                    returnColor.Append(shadowIntegrator.Integrate(ray, objects, lights, sampler, subPathSamples, lightSample));
                }
                if (record.Material is MirrorMaterial && record.Material.Specular.R > 0 && record.Material.Specular.G > 0 && record.Material.Specular.B > 0)
                    returnColor.Append(Reflection(ray, record, 0, returnColor, objects, lights, sampler, subPathSamples, lightSample));
            }
            else
            {
                if (SkyBox.IsSkyBoxLoaded())
                    returnColor = SkyBox.GetSkyBoxColor(ray);
            }
            return returnColor;
        }

        private Color Reflection(Ray ray, HitRecord record, int noOfBounce, Color color, IIntersectable objects, List<ILight> lights, ISampler sampler, List<List<Sample>> subPathSamples, LightSample lightSample)
        {
            Color ks = record.Material.Specular;
            if (noOfBounce == Constants.MaximumRecursionDepth)
                return shadowIntegrator.Integrate(ray, objects, lights, sampler, subPathSamples, lightSample).Mult(ks);

            Ray reflectedRay = record.CreateReflectedRay();
            HitRecord reflectRecord = objects.Intersect(reflectedRay);
            if (reflectRecord.HitObject != null)
            {
                if (reflectRecord.Material is MirrorMaterial)
                {
                    Color c = Reflection(reflectedRay, reflectRecord, ++noOfBounce, color, objects, lights, sampler, subPathSamples, lightSample);
                    c.Append(shadowIntegrator.Integrate(reflectedRay, objects, lights, sampler, subPathSamples, lightSample).Mult(ks));
                    return c;
                }
                else
                {
                    return shadowIntegrator.Integrate(reflectedRay, objects, lights, sampler, subPathSamples, lightSample).Mult(ks);
                }
            }
            else
            {
                if (SkyBox.IsSkyBoxLoaded())
                    color = SkyBox.GetSkyBoxColor(ray);
            }
            return color;

        }
    }
}
