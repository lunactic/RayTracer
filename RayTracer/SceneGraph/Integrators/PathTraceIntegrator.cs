using RayTracer.Helper;
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
    public class PathTraceIntegrator : IIntegrator
    {
        private IIntersectable scene;
        private List<ILight> lights;
        private ISampler sampler;
        private readonly Random random = new Random();
        public Color Integrate(Ray ray, IIntersectable objects, List<ILight> lights, ISampler sampler, List<List<Sample>> subPathSamples, LightSample lightSample)
        {
            scene = objects;
            this.lights = lights;
            this.sampler = sampler;
            HitRecord record = scene.Intersect(ray);
            if (record == null)
            {
                if (SkyBox.IsSkyBoxLoaded()) return SkyBox.GetSkyBoxColor(ray);
                return new Color(0, 0, 0);
            }

            if (record.HitObject.Light != null) return record.HitObject.Light.LightColor;
            return PathTrace(ray, record, subPathSamples, lightSample);
        }

        public Color PathTrace(Ray ray, HitRecord record, List<List<Sample>> subPathSamples, LightSample lightSample)
        {
            float q = 0f;
            int depth = 0;
            Color pixelColor = new Color(0, 0, 0);
            Color alpha = new Color(1, 1, 1);

            bool isGeneratedFromRefraction = false;

            while (depth < Constants.MaximalPathLength)
            {
                if (record == null) return pixelColor;
                //Select one light at random
                ILight light = Randomizer.PickRandomLight(lights);
                //Sample that light
                light.Sample(record, lightSample);

                if (isGeneratedFromRefraction && record.HitObject.Light != null)
                {
                    isGeneratedFromRefraction = false;
                    pixelColor.Append(record.HitObject.Light.LightColor);
                }

                pixelColor.Append(alpha.Mult(Shade(record, lightSample)).Div(lightSample.Pdf));

                //Break with Russian roulette
                if (depth > 2)
                {
                    q = 0.5f;
                    if (random.NextDouble() <= q)
                        return pixelColor;
                }

                List<Sample> directionSamples = subPathSamples[depth];
                //Create the next Ray
                if (record.Material is LambertMaterial || record.Material is BlinnPhongMaterial)
                {
                    Sample directionSample = Randomizer.PickRandomSample(directionSamples);
                    Vector3 direction = UniformHemisphereSample(directionSample.X, directionSample.Y, record.SurfaceNormal);
                    record = scene.Intersect(new Ray(record.IntersectionPoint, direction));
                    if (record == null) break;
                    alpha = alpha.Mult(record.Material.Diffuse.Div(1 - q));
                }
                else if (record.Material is MirrorMaterial)
                {
                    record = scene.Intersect(record.CreateReflectedRay());
                    isGeneratedFromRefraction = true;
                }
                else if (record.Material is RefractiveMaterial)
                {
                    isGeneratedFromRefraction = true;
                    float fresnel = Reflectance(record);
                    //Pick refraction / reflection path with p=0.5 each
                    if (random.NextDouble() < fresnel)
                    {
                        record = scene.Intersect(record.CreateReflectedRay());
                        alpha = alpha.Mult(fresnel);
                    }
                    else
                    {
                        record = scene.Intersect(record.CreateRefractedRay());
                        alpha = alpha.Mult(1 - fresnel);
                    }
                }

                depth++;
            }
            return pixelColor;
        }

        private Color Shade(HitRecord record, LightSample sample)
        {
            Color pixelColor = new Color(0, 0, 0);
            if (record.Material is LambertMaterial || record.Material is BlinnPhongMaterial)
            {
                if (sample.LightColor.Power > 0 && IsVisible(record, sample))
                {
                    pixelColor.Append(record.Material.Shade(record, sample.Wi).Mult(sample.LightColor));
                }
            }
            return pixelColor;
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

        private bool IsVisible(HitRecord record, LightSample sample)
        {
            Vector3 hitToLight = sample.Wi;
            Vector3 pos = record.IntersectionPoint;
            Vector3 offset = record.RayDirection;
            offset = -offset;
            offset *= 0.001f;
            pos += offset;
            HitRecord lightHit = scene.Intersect(new Ray(pos, hitToLight));
            return (lightHit == null || lightHit.HitObject.Light != null || lightHit.Distance > sample.Distance);
        }

        private Vector3 UniformHemisphereSample(float eps1, float eps2, Vector3 normal)
        {
            float x = (float)Math.Cos(2f * Math.PI * eps2) * (float)Math.Sqrt(eps1);
            float y = (float)Math.Sin(2f * Math.PI * eps2) * (float)Math.Sqrt(eps1);
            float z = (float)(Math.Sqrt(1f - eps1));

            Vector3 direction = new Vector3(x, y, z);

            Vector3 tangentBase = (normal.Equals(new Vector3(0, 0, 1)) || normal.Equals(new Vector3(0, 0, -1))) ? new Vector3(0, 1, 0) : new Vector3(0, 0, 1);


            Vector3 xTransform = Vector3.Cross(normal, tangentBase);
            Vector3 yTransform = Vector3.Cross(normal, xTransform);

            Matrix3 transformMatrix = new Matrix3 { Column0 = xTransform, Column1 = yTransform, Column2 = normal };
            transformMatrix.Transform(ref direction);

            return direction;

        }
    }
}
