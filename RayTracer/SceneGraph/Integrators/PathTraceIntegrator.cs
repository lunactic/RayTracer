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
        public Color Integrate(Ray ray, IIntersectable objects, List<ILight> lights, ISampler sampler, List<List<Sample>> subPathSamples )
        {
            scene = objects;
            this.lights = lights;
            this.sampler = sampler;
            return PathTrace(ray, subPathSamples);
        }

        public Color PathTrace(Ray ray, List<List<Sample>> subPathSamples )
        {
            //Create new LightSamples for this Pixel
            List<LightSample> lightSamples = sampler.GetLightSamples();
           
            int depth = 0;
            Color pixelColor = new Color(0, 0, 0);
            Color alpha = new Color(1, 1, 1);
            bool isGeneratedFromRefraction = false;

            HitRecord record = scene.Intersect(ray);
            if (record == null)
                return pixelColor;
      
            float oneMinusQk = 1;
            while (depth < Constants.MaximalPathLength)
            {
                //Check if we hit a LightSource
                if ((depth == 0 || isGeneratedFromRefraction) && record.HitObject.Light != null) pixelColor.Append(record.HitObject.Light.LightColor);

                //Select one light at random
                ILight light = Randomizer.PickRandomLight(lights);
                //Select a random lightsample
                LightSample sample = Randomizer.PickRandomLightSample(lightSamples);
                //Sample that light
                light.Sample(record, sample);


                pixelColor.Append(alpha.Mult(Shade(record, sample)).Div(sample.Pdf));

                //Break with Russian roulette
                if (depth > 2)
                {
                    oneMinusQk = 0.5f;
                    if (random.NextDouble() > 0.5)
                        return pixelColor;
                }
                List<Sample> directionSamples = subPathSamples[depth];
                //Create the next Ray
                Sample directionSample = Randomizer.PickRandomSample(directionSamples);
                if (record.Material is LambertMaterial || record.Material is BlinnPhongMaterial)
                {
                    Vector3 direction = UniformHemisphereSample(directionSample.X, directionSample.Y, record.SurfaceNormal);
                    record = scene.Intersect(new Ray(record.IntersectionPoint, direction));
                    if (record == null) break;
                    alpha = alpha.Mult(record.Material.Diffuse.Div(oneMinusQk));
                }
                if (record.Material is MirrorMaterial)
                {
                    record = scene.Intersect(record.CreateReflectedRay());
                    isGeneratedFromRefraction = true;
                    
                }
                if (record.Material is RefractiveMaterial)
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
                        alpha = alpha.Mult(1-fresnel);
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
                //Diffuse Shading
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
            Vector3 dist = Vector3.Subtract(sample.Position, pos);
            return (lightHit == null || lightHit.HitObject.Light != null || lightHit.Distance > dist.Length);
        }

        private Vector3 UniformHemisphereSample(float eps1, float eps2, Vector3 normal)
        {
            float x = (float)(Math.Cos(2 * Math.PI * eps2) * Math.Sqrt(eps1));
            float y = (float)(Math.Sin(2 * Math.PI * eps2) * Math.Sqrt(eps1));
            float z = (float)(Math.Sqrt(1 - eps1));
            /*if (z < 0)
                z = -z;
            */
            Vector3 direction = new Vector3(x, y, z);
            
            Vector3 tangentBase = normal.Equals(new Vector3(0, 0, 1)) ? new Vector3(0, 1, 0) : new Vector3(0, 0, 1);

            Vector3 xBase = Vector3.Cross(normal, tangentBase);
            Vector3 yBase = Vector3.Cross(normal, xBase);
            Vector3 zBase = normal;
            Matrix3 mat = new Matrix3()
            {
                R0C0 = xBase.X,
                R0C1 = xBase.Y,
                R0C2 = xBase.Z,
                R1C0 = yBase.X,
                R1C1 = yBase.Y,
                R1C2 = yBase.Z,
                R2C0 = zBase.X,
                R2C1 = zBase.Y,
                R2C2 = zBase.Z,
            };
            Matrix3.Transform(ref mat,ref direction);
            return direction;

        }
    }
}
