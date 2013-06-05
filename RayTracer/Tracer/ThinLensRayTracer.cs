using RayTracer.Helper;
using RayTracer.Samplers;
using RayTracer.SceneGraph;
using RayTracer.SceneGraph.Cameras;
using RayTracer.SceneGraph.Scenes;
using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Tracer
{
    public class ThinLensRayTracer : IRayTracer
    {
        private Scene scene;
        private Stopwatch stopwatch;
        private int finishedThreads;
        Random rand = new Random();
        public ThinLensRayTracer(Scene scene)
        {
            this.scene = scene;
        }

        public void Render()
        {
            DepthOfFieldCamera cam = ((DepthOfFieldCamera)scene.Camera);
 
            for (int i = 0; i < scene.Film.Width; i++)
            {
                for (int j = 0; j < scene.Film.Height; j++)
                {
                    Color pixelColor = new Color(0, 0, 0);
                    List<Sample> appertureSamples = ((ISampler)Activator.CreateInstance(Constants.Sampler)).CreateSamples();
                    for (int s = 0; s < 24; s++)
                    {
                        Ray ray = cam.CreateRay(i, j, appertureSamples);
             
                        pixelColor.Append(scene.Integrator.Integrate(ray, scene.Objects, scene.Lights, null, null, null));

                    }
                    pixelColor.VoidDiv(24f);
                    scene.Film.SetPixel(i, j, pixelColor);
                }
            }
            Tonemapper.SaveImage("C:\\Test\\" + scene.FileName, scene.Film);
        }
    }
}
