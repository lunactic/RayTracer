using RayTracer.SceneGraph.Light;
using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.SceneGraph.Materials
{
    public abstract class Material
    {
        public Color Specular { get; set; }
        public Color Diffuse { get; set; }
        public Color Ambient { get; set; }
        public float Shininess { get; set; }
        public float RefractionIndex { get; set; }
        public float Ks { get; set; }

        public Material()
        {
            Specular = Color.Black;
            Diffuse = Color.Black;
            Ambient = Color.Black;
            Shininess = 0f;
            RefractionIndex = 1f;
            Ks = 1;
        }

        public Color Shade(HitRecord record, Vector3 lightDirection)
        {
           
            Color pixelColor = Color.Black;
            Vector3 rayDirection = record.RayDirection;
            rayDirection.Normalize();
            Vector3 normal = record.SurfaceNormal;
            normal.Normalize();

           
            float nDotL = Vector3.Dot(normal, lightDirection);

            if (nDotL > 0)
            {
                //add Diffuse Light
                pixelColor += Diffuse * nDotL;
                //Calculate the Blinn halfVector
                Vector3 h = lightDirection - rayDirection;
                h.Normalize();


                float hDotN = Vector3.Dot(h, normal);
                if (hDotN > 0)
                {
                    float pow = (float)Math.Pow(hDotN, Shininess);
                    pixelColor +=  Specular * pow;
                }
            }
            pixelColor += Ambient;
            pixelColor.Clamp(0f, 1f);
            return pixelColor;
        }
     }
}
