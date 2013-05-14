using RayTracer.Helper;
using RayTracer.SceneGraph.Light;
using RayTracer.SceneGraph.Objects;
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

        public Texture Texture { get; set; }

        public Material()
        {
            Specular = new Color(0,0,0);
            Diffuse = new Color(0,0,0);
            Ambient = new Color(0,0,0);
            Shininess = 0f;
            RefractionIndex = 1f;
            Ks = 1;
        }

        public Color Shade(HitRecord record, Vector3 lightDirection)
        {
           
            Color pixelColor = new Color(0,0,0);
            Vector3 rayDirection = record.RayDirection;
            rayDirection.Normalize();
            Vector3 normal = record.SurfaceNormal;
            normal.Normalize();

           
            float nDotL = Vector3.Dot(normal, lightDirection);

            if (nDotL > 0)
            {
                Color diffuse;

                if (Constants.TextureMapping && record.Material.Texture != null && !(record.HitObject is Plane))
                {
                    diffuse = Texture.GetColorFromTexCoordinate(record.HitObject.GetTextudeCoordinates(record));
                }
                else
                {
                    diffuse = Diffuse;
                }

                //add Diffuse Light
                pixelColor.Append(diffuse.Mult(nDotL));
                //Calculate the Blinn halfVector
                Vector3 h = lightDirection - rayDirection;
                h.Normalize();


                float hDotN = Vector3.Dot(h, normal);
                if (hDotN > 0)
                {
                    float pow = (float)Math.Pow(hDotN, Shininess);
                    pixelColor.Append(Specular.Mult(pow));
                }
            }
            pixelColor.Append(Ambient);
            pixelColor.Clamp(0f, 1f);
            return pixelColor;
        }

        protected float Reflectance(HitRecord record)
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

        public abstract Color GetBrdf(Vector3 w_o, Vector3 w_i, HitRecord record);
    }
}
