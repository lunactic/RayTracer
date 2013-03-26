using RayTracer.SceneGraph.Light;
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
        public abstract Color Specular { get; set; }
        public abstract Color Diffuse { get; set; }
        public abstract Color Ambient { get; }
        public abstract Color Shade(HitRecord record, Scene scene, ILight light);
     }
}
