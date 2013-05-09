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

        public ThinLensRayTracer(Scene scene)
        {
            this.scene = scene;
        }

        public void Render()
        {
            DepthOfFieldCamera cam = scene.Camera as DepthOfFieldCamera;
            for (int i = 0; i < scene.Film.Width; i++)
            {
                for (int j = 0; j < scene.Film.Height; j++)
                {
                    ISampler sampler = (ISampler)Activator.CreateInstance(Constants.Sampler);
                    List<Sample> lensSamples = sampler.CreateSamples(); //xl_samples
                    //s = number of samples
                    Color c = new Color(0, 0, 0);
                         
                    for (int s = 0; s < lensSamples.Count; s++)
                    {
                        lensSamples[s].X -= 0.5f;
                        lensSamples[s].Y -= 0.5f;
      
                        Ray ray = scene.Camera.CreateRay(cam.Apperture * lensSamples[s].X, cam.Apperture * lensSamples[s].Y);

                        c.Append(scene.Integrator.Integrate(ray, scene.Objects, scene.Lights, sampler,null));
                    }
                    c.VoidDiv(10f);
                    scene.Film.SetPixel(i, j, c);
                }
            }
            Tonemapper.SaveImage("C:\\Test\\" + scene.FileName, scene.Film);
        }
    }
}
