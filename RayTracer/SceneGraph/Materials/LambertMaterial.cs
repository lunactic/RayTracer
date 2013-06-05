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

        public LambertMaterial(Color diffuse)
        {
            Diffuse = diffuse;
        }
    }
}
