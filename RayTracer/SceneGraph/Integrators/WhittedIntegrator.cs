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

        public Color Integrate(Ray ray, IntersectableList objects, List<ILight> lights, ISampler sampler)
        {
            shadowIntegrator = new ShadowIntegrator();
            Color returnColor = Color.Black;
            HitRecord record = objects.Intersect(ray);

            if (record != null)
            {
                if (!(record.Material is MirrorMaterial))
                {
                    returnColor += shadowIntegrator.Integrate(ray, objects,lights,sampler);
                }
                if (record.Material is MirrorMaterial && record.Material.Specular.R > 0 && record.Material.Specular.G > 0 && record.Material.Specular.B > 0)
                    returnColor += Reflection(ray, record, 0, returnColor, objects,lights, sampler);
            }

            return returnColor;
        }

        private Color Reflection(Ray ray, HitRecord record, int noOfBounce, Color color, IntersectableList objects,List<ILight>lights ,ISampler sampler)
        {
            Color ks = record.Material.Specular;
            if (noOfBounce == Constants.MaximumRecursionDepth)
                return shadowIntegrator.Integrate(ray,objects,lights, sampler) * ks;

            Ray reflectedRay = record.CreateReflectedRay();
            HitRecord reflectRecord = objects.Intersect(reflectedRay);
            if (reflectRecord.HitObject != null)
            {
                if (reflectRecord.Material is MirrorMaterial)
                {
                    Color c = Reflection(reflectedRay, reflectRecord, ++noOfBounce, color, objects,lights, sampler);
                    return c + (shadowIntegrator.Integrate(reflectedRay, objects,lights, sampler) * ks);
                }
                else
                {
                    return shadowIntegrator.Integrate(reflectedRay, objects,lights, sampler) * ks;
                }
            }
            return color;

        }
    }
}
