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
    /// <summary>
    /// The implementations for the procedural noise are taken from here: http://www.codermind.com/articles/Raytracer-in-C++-Part-III-Textures.html
    /// </summary>
    public enum Noise
    {
        Turbulence, Marble, Bumb, None
    }

    public abstract class Material
    {
        public Color Specular { get; set; }
        public Color Diffuse { get; set; }
        public Color Ambient { get; set; }
        public float Shininess { get; set; }
        public float RefractionIndex { get; set; }
        public float Ks { get; set; }
        public Noise Noise { get; set; }
        public Texture ColorTexture { get; set; }
        public Texture BumpTexture { get; set; }

        public Material()
        {
            Specular = new Color(0, 0, 0);
            Diffuse = new Color(0, 0, 0);
            Ambient = new Color(0, 0, 0);
            Shininess = 0f;
            RefractionIndex = 1f;
            Ks = 1;
            Noise = Noise.None;
        }

        public Color Shade(HitRecord record, Vector3 wi)
        {

            Color pixelColor = new Color(0, 0, 0);
            Vector3 normal = record.SurfaceNormal;
            normal.Normalize();
            wi.Normalize();

            float noiseCoefficient = 0f;

            switch (Noise)
            {
                case Noise.Turbulence:
                    noiseCoefficient = Turbulence(record.IntersectionPoint);
                    break;
                case Noise.Marble:
                    noiseCoefficient = Marble(record.IntersectionPoint);
                    break;
                case Noise.Bumb:
                    noiseCoefficient = 1f;
                    normal = Bump(record.IntersectionPoint, normal);
                    break;
                case Noise.None:
                    noiseCoefficient = 1f;
                    break;
                default:
                    noiseCoefficient = 1f;
                    break;
            }

            if (BumpTexture != null)
            {
                Color displacementColor = BumpTexture.GetColorFromTexCoordinate(record.HitObject.GetTextudeCoordinates(record));
                Vector3 displacementNormal = normal;
                displacementNormal.X = ((displacementColor.R * 2) - 1);
                displacementNormal.Y = ((displacementColor.G * 2) - 1);
                displacementNormal.Z = displacementColor.B;
                normal = (Vector3.Dot(displacementNormal, normal) < 0) ? -displacementNormal : displacementNormal;
            }

            float nDotL = Vector3.Dot(normal, wi);

            if (nDotL > 0)
            {
                Color diffuse;

                if (Constants.TextureMapping && ColorTexture != null && !(record.HitObject is Plane))
                {
                    diffuse = ColorTexture.GetColorFromTexCoordinate(record.HitObject.GetTextudeCoordinates(record));

                }
                else
                {
                    diffuse = Diffuse;
                }

                //add Diffuse Light
                pixelColor.Append(diffuse.Mult(nDotL).Mult(noiseCoefficient));
                pixelColor.Append(pixelColor.Mult(1f - noiseCoefficient));
                //Calculate the Blinn halfVector
                Vector3 h = wi;
                h = Vector3.Add(h, wi);
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

        private Vector3 Bump(Vector3 hitPoint, Vector3 normal)
        {
            float noiseCoefX = (float)PerlinNoise.Noise(0.1 * hitPoint.X, 0.1 * hitPoint.Y, 0.1 * hitPoint.Z);
            float noiseCoefY = (float)PerlinNoise.Noise(0.1 * hitPoint.Y, 0.1 * hitPoint.Z, 0.1 * hitPoint.X);
            float noiseCoefZ = (float)PerlinNoise.Noise(0.1 * hitPoint.Z, 0.1 * hitPoint.X, 0.1 * hitPoint.Y);

            normal.X += noiseCoefX;
            normal.Y += noiseCoefY;
            normal.Z += noiseCoefZ;

            return normal;
        }


        private float Marble(Vector3 hitPoint)
        {
            float noiseCoefficient = Turbulence(hitPoint);

            noiseCoefficient = (float)(0.5f * (Math.Sin(hitPoint.X + hitPoint.Y) + noiseCoefficient) + 0.5f);

            return noiseCoefficient;
        }

        private float Turbulence(Vector3 hitPoint)
        {
            float noiseCoefficient = 0;
            for (int i = 1; i < 10; i++)
            {
                noiseCoefficient += (1.0f / i) *
                                    Math.Abs(
                                        (float)
                                        (PerlinNoise.Noise(i * hitPoint.X, i * hitPoint.Y, i * hitPoint.Z)));
            }
            return noiseCoefficient;
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

    }
}
