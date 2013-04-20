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
            switch (Constants.SceneIndex)
            {
                case 0: scene = new MirrorScene();
                    break;
                case 1: scene = new MirrorScene();
                    break;
                case 2: scene = new MirrorScene();
                    break;
                case 3: scene = new MirrorScene();
                    break;
                case 4: scene = new MirrorScene();
                    break;
                case 5: scene = new MirrorScene();
                    break;
                case 6: scene = new MirrorScene();
                    break;
                case 7: scene = new MirrorScene();
                    break;
            }

            //Create the RayTracer setup you want to use here
            RayTracer tracer = new RayTracer(scene);
            tracer.Render();
        }
    }
}
