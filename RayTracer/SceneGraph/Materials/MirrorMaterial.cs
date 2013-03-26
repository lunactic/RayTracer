using RayTracer.SceneGraph.Light;
using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.SceneGraph.Materials
{
    public class MirrorMaterial : Material
    {
        private Color diffuse;
        private Color specular;

        public float Shininess { get; set; }
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

        public MirrorMaterial(Color specular)
        {
            this.specular = specular;
            Shininess = 35f;
            
        }

        public override Color Shade(HitRecord record, Scene scene, ILight light)
        {
            Vector3 hitPosition = record.IntersectionPoint;
            Vector3 normal = record.SurfaceNormal;
            Color pixelColor = Color.Black;
            Vector3 rayDirection = record.Ray.Direction;
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
