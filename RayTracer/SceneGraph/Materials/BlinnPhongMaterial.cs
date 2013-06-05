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
    public class BlinnPhongMaterial : Material
    {

        public BlinnPhongMaterial(Color diffuse, Color specular, float shininess)
        {
            Diffuse = diffuse;
            Specular = specular;
            Shininess = shininess;
        }

        public BlinnPhongMaterial(Color diffuse)
        {
            Diffuse = diffuse;
            Specular = new Color(1, 1, 1);
            Shininess = 32f;
        }
    }
}
