using RayTracer.Helper;
using RayTracer.Samplers;
using RayTracer.SceneGraph.Light;
using RayTracer.SceneGraph.Materials;
using RayTracer.SceneGraph.Objects;
using RayTracer.SceneGraph.Scenes;
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
        public Color Integrate(Ray ray, IIntersectable objects, List<ILight> lights, ISampler sampler)
        {
            return PathTrace(ray, 0, objects, sampler);
        }

        private Color PathTrace(Ray ray, int depth, IIntersectable objects, ISampler sampler)
        {
            HitRecord record = objects.Intersect(ray);

            if (record == null)
                return new Color(0, 0, 0);

            //Shade The intersection
            Material mat = record.Material;

            Color pointColor = new Color(0, 0, 0);

            //Russian Roulette
            float pathSurvive = 1.0f;

            //Russian Roulette
            if (depth > Constants.MaximumRecursionDepth)
            {
                Color weight = new Color(0, 0, 0);
                if (mat is LambertMaterial) weight = mat.Diffuse;
                if (mat is BlinnPhongMaterial)
                    weight = new Color(mat.Diffuse.R + mat.Specular.R, mat.Diffuse.G + mat.Specular.G,mat.Diffuse.B + mat.Specular.B);
                /* TODO IMPLEMEMNT MIRROR AND REFRACTIVE MATERIAL RESPECTIVELY
                    if (mat is MirrorMaterial) weight = new Color(mat.Ks, mat.Ks, mat.Ks,1.0f);
                    if(mat is RefractiveMaterial) weight = 
                 */
                if (RussianRoulette(weight, ref pathSurvive))
                    return pointColor;
            }

            //Shade according to the Materials
            if (mat is LambertMaterial)
            {
                //Direct Illumination
                //pointColor += DirectIllumintation(record, ray);
                //Indirect Illumination
                //pointColor += survival * diffuseInterreflect(ray,record,depth);
            }
            if (mat is BlinnPhongMaterial)
            {
                /*Direct Illumination
                 *   pointColor += directIllumination(record,ray);
                 * Indirect Illumination
                 *  if(glossyRussianRoulette(mat.kS,ref.kD, ref rrMult)
                 *      pointColor += survival * (1.0/(1-1.0/rrMult)) * diffuseInterreflect(ray,record,depth))
                 *  else
                 *      pointColor += survival * rrMult * specularInterreflect(ray, record, depth);
                 *      */
            }
            if (mat is MirrorMaterial)
            {
                //pointColor += survival*mirrorReflect(ray,record,depth)
            }
            if (mat is RefractiveMaterial)
            {
                //Refractive shading
            }

            return new Color(0, 0, 0);
        }

        private bool RussianRoulette(Color c, ref float survivalFactor)
        {
            float p = Math.Max(c.R, Math.Max(c.G, c.B));
            float survivorMultiplicationFactor = 1.0f / p;

            Random rand = new Random();
            if (rand.NextDouble() > p) return true;
            return false;

        }

        private Color DirectIllumination(HitRecord record, Ray ray, Scene scene)
        {
            Material mat = record.Material;
            Color pointColor = new Color(0, 0, 0);

            foreach (ILight light in scene.Lights)
            {
                Color intensity;
                Vector3 lightIncidence;



            }

            return new Color(0, 0, 0);
        }
    }
}
