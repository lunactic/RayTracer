using RayTracer.Helper;
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
 
        public MirrorMaterial(float ks)
        {
            Ks = ks;
            Shininess = 35f;
            Specular = new Color(1, 1, 1);
        }

        public override Color GetBrdf(Vector3 w_o, Vector3 w_i, HitRecord record)
        {
            //Diffuse BRDF
            Vector3 normal = record.SurfaceNormal;

            Color diffuseBrdf = Diffuse.Div((float)Math.PI);
            
            //Use the Torrance-Sparrow Model for the Specular BRDF
            Vector3 w_h = w_o + w_i;
            w_h.Normalize();

            //Blinn Microfacet distribution
            float D_w = (float)((Shininess + 2) / Math.PI * Math.Pow(Vector3.Dot(w_h, normal), Shininess));

            float geom = Math.Min(1, Math.Min((2 * Vector3.Dot(normal, w_h) * Vector3.Dot(normal, w_o)) / Vector3.Dot(w_o, w_h), (2 * Vector3.Dot(normal, w_h) * Vector3.Dot(normal, w_i)) / Vector3.Dot(w_o, w_h)));

            float fresnel = Reflectance(record);

            float cos_o = Vector3.Dot(w_o, normal);
            float cos_i = Vector3.Dot(w_i, normal);

            float multFactor = (D_w * fresnel * geom) / (4 * cos_i * cos_o);

            Color specularBrdf = Specular.Mult(multFactor);

            Color returnColor = new Color(diffuseBrdf.R, diffuseBrdf.G, diffuseBrdf.B);
            returnColor.Append(specularBrdf);

            return returnColor;
        }
   
    }
}
