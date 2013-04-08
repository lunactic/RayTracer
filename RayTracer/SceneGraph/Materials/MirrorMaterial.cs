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
 
        public MirrorMaterial(Color specular)
        {
            Specular = specular;
            Shininess = 35f;
            
        }
    }
}
