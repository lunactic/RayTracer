using RayTracer.SceneGraph.Materials;
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
        private Scene scene;
        private ShadowIntegrator shadowIntegrator;

        public WhittedIntegrator(Scene scene)
        {
            this.scene = scene;
            shadowIntegrator = new ShadowIntegrator(scene);
        }
        public Color Integrate(Ray ray)
        {
            Color returnColor = Color.Black;
            HitRecord record = scene.Intersect(ray);

            if (record != null)
            {
                if (!(record.Material is MirrorMaterial))
                {
                    returnColor += shadowIntegrator.Integrate(ray);
                }
                if (record.Material is MirrorMaterial && record.Material.Specular.R > 0 && record.Material.Specular.G > 0 && record.Material.Specular.B > 0)
                    returnColor += Reflection(ray, record, 0, returnColor);
            }

            return returnColor;
        }

        private Color Reflection(Ray ray, HitRecord record, int noOfBounce, Color color)
        {
            Color ks = record.Material.Specular;
            if (noOfBounce == Constants.MAXIMUM_NUMBER_OF_BOUNCES)
                return shadowIntegrator.Integrate(ray) * ks;

            Ray reflectedRay = record.CreateReflectedRay();
            HitRecord reflectRecord = scene.Intersect(reflectedRay);
            if (reflectRecord.HitObject != null)
            {
                if (reflectRecord.Material is MirrorMaterial)
                {
                    Color c = Reflection(reflectedRay,reflectRecord, ++noOfBounce, color);
                    return c + (shadowIntegrator.Integrate(reflectedRay) * ks);
                }
                else
                {
                    return shadowIntegrator.Integrate(reflectedRay) * ks;
                }
            }
            return color;

        }
    }
}
