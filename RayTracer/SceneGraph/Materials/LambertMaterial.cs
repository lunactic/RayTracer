using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.SceneGraph.Light;
using RayTracer.Structs;

namespace RayTracer.SceneGraph.Materials
{
    public class LambertMaterial : Material
    {
        private Color diffuse;
        public override Color Diffuse { get { return diffuse; } set { diffuse = value; } }
        public override Color Specular { get; set; }

        public LambertMaterial(Color diffuse)
        {
            this.diffuse = diffuse;
        }

        public override Color Shade(HitRecord record, Scene scene, ILight light)
        {
            Vector3 hitPosition = record.IntersectionPoint;
            Vector3 normal = record.SurfaceNormal;
          
            Color lightColor = Color.Black;
            Vector3 lightDirection = light.GetLightDirection(hitPosition);

            float nDotL = Vector3.Dot(normal, lightDirection);

            if (nDotL >= 0)
                lightColor += Diffuse * light.GetIncidentColor(hitPosition) * nDotL;

            lightColor += scene.Ambient * light.GetIncidentColor(hitPosition);
            lightColor.Clamp(0.0f, 1.0f);

            return lightColor;
        }



        public override Color Ambient
        {
            get { return Color.Black; }
        }
    }
}
