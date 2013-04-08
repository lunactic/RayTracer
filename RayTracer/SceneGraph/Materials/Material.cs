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

        public Material()
        {
            Specular = Color.Black;
            Diffuse = Color.Black;
            Ambient = Color.Black;
            Shininess = 0f;
            RefractionIndex = 1f;
        }

        public Color Shade(HitRecord record, Scene scene, ILight light)
        {
            Vector3 hitPosition = record.IntersectionPoint;
            Vector3 normal = record.SurfaceNormal;
            Color pixelColor = Color.Black;
            Vector3 rayDirection = record.RayDirection;
            rayDirection.Normalize();


            Color cLi = light.GetIncidentColor(hitPosition);
            Vector3 lightDirection = light.GetLightDirection(hitPosition);
            float nDotL = Vector3.Dot(normal, lightDirection);

            if (nDotL > 0)
            {
                //add Diffuse Light
                pixelColor += cLi * nDotL * Diffuse;
                //Calculate the Blinn halfVector
                Vector3 h = lightDirection - rayDirection;
                h.Normalize();


                float nDotH = Vector3.Dot(normal, h);
                if (nDotH > 0)
                {
                    float pow = (float)Math.Pow(nDotH, Shininess);
                    pixelColor += cLi * pow * Specular;
                }
            }
            pixelColor += scene.Ambient * cLi;
            pixelColor.Clamp(0f, 1f);
            return pixelColor;
        }
     }
}
