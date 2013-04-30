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


        public override Color GetBrdf(Vector3 w_o, Vector3 w_i, HitRecord record)
        {
            return Diffuse.Div((float)Math.PI);
        }
    }
}
