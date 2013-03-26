using RayTracer.SceneGraph.Light;
using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.SceneGraph.Materials
{
    public class BlinnPhongMaterial : Material
    {
        public Color diffuse;
        public Color specular;
        public float Shininess;

        public override Color Specular
        {
            get
            {
                return specular;
            }
            set
            {
                specular = value;
            }
        }

        public override Color Diffuse
        {
            get
            {
                return diffuse;
            }
            set
            {
                diffuse = value;
            }
        }

        public BlinnPhongMaterial(Color diffuse, Color specular, float shininess)
        {
            this.diffuse = diffuse;
            this.specular = specular;
            Shininess = shininess;
        }

        public BlinnPhongMaterial(Color diffuse)
        {
            this.diffuse = diffuse;
            specular = Color.White;
            Shininess = 32f;
        }

        public override Color Shade(HitRecord record, Scene scene, ILight light)
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



        public override Color Ambient
        {
            get { return Color.Black; }
        }
    }
}
