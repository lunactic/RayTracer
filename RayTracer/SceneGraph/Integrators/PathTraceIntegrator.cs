using RayTracer.SceneGraph.Light;
using RayTracer.SceneGraph.Materials;
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
        private Scene scene;
        public PathTraceIntegrator(Scene scene)
        {
            this.scene = scene;
        }

        public Color Integrate(Ray ray)
        {
            return PathTrace(ray, 0);
        }

        private Color PathTrace(Ray ray, int depth)
        {
            HitRecord record = scene.Intersect(ray);

            if (record == null)
                return scene.BackgroundColor;

            //Shade The intersection
            Material mat = record.Material;

            Color pointColor = Color.Black;

            //Russian Roulette
            float pathSurvive = 1.0f;

            //Russian Roulette
            if (depth > Constants.MaximumRecursionDepth)
            {
                Color weight = Color.Black;
                if (mat is LambertMaterial) weight = mat.Diffuse;
                if (mat is BlinnPhongMaterial) weight = mat.Diffuse + mat.Specular;
                /* TODO IMPLEMEMNT MIRROR AND REFRACTIVE MATERIAL RESPECTIVELY
                    if (mat is MirrorMaterial) weight = new Color(mat.Ks, mat.Ks, mat.Ks,1.0f);
                    if(mat is RefractiveMaterial) weight = 
                 */
                if (RussianRoulette(weight, pathSurvive))
                    return pointColor;
            }

            //Shade according to the Materials
            if (mat is LambertMaterial)
            {
                //pointColor += DirectIllumintation(record, ray);
            }

            return Color.Black;
        }

        private bool RussianRoulette(Color c, float survivalFactor)
        {
            float p = Math.Max(c.R, Math.Max(c.G, c.B));
            float survivorMultiplicationFactor = 1.0f / p;

            Random rand = new Random();
            if (rand.NextDouble() > p) return true;
            return false;

        }

        private Color DirectIllumination(HitRecord record, Ray ray)
        {
            Material mat = record.Material;
            Color pointColor = Color.Black;

            foreach (ILight light in scene.Lights)
            {
                Color intensity;
                Vector3 lightIncidence;

                

            }

            return Color.Black;
        }
    }
}
