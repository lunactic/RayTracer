using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Helper;
using RayTracer.SceneGraph.Scenes;
using RayTracer.Structs;

namespace RayTracer.Tracer
{
    public class Starter
    {
        public static Scene scene;
        static void Main()
        {
            scene = (Scene)Activator.CreateInstance(Constants.SceneIndex);

            //Create the RayTracer setup you want to use here
            RayTracer tracer = new RayTracer(scene);
            tracer.Render();
        }
    }
}
