using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RayTracer.Samplers;
using RayTracer.SceneGraph;
using RayTracer.SceneGraph.Scenes;
using RayTracer.Structs;
using RayTracer.SceneGraph.Objects;
using RayTracer.SceneGraph.Materials;
using RayTracer.SceneGraph.Light;

namespace RayTracer.Tracer
{
    public class RayTracer
    {
        private Scene scene;
        private Stopwatch stopwatch;
        private int finishedThreads;
        public RayTracer(Scene scene)
        {
            this.scene = scene;
            
        }

        public void Render()
        {
            stopwatch = new Stopwatch();
            stopwatch.Reset();
            if (Constants.NumberOfThreads == 1) //One Thread
            {
                stopwatch.Start();
                for (int i = 0; i < scene.Camera.ScreenWidth; i++)
                {
                    for (int j = 0; j < scene.Camera.ScreenHeight; j++)
                    {
                        if (Constants.IsLightSamplingOn)
                        {
                            List<Sample> samples = Constants.Sampler.CreateSamples();
                            Color c = Color.Black;
                            foreach (Sample sample in samples)
                            {
                                Ray ray = scene.Camera.CreateRay(i + sample.X, j + sample.Y);
                                c += scene.Integrator.Integrate(ray, scene.IntersectableList,scene.Lights, Constants.Sampler);
                            }
                            c = c / samples.Count;
                            scene.Film.SetPixel(i, j, c);
                        }
                        else
                        {
                            Ray ray = scene.Camera.CreateRay(i, j);
                            Color color = scene.Integrator.Integrate(ray, scene.IntersectableList,scene.Lights, Constants.Sampler);
                            scene.Film.SetPixel(i, j, color);
                        }
                    }
                }
                stopwatch.Stop();
                Debug.WriteLine("Finished rendering in: " + stopwatch.ElapsedMilliseconds + " ms.");
                Tonemapper.SaveImage("C:\\Test\\" + scene.FileName, scene.Film);
            }
            else //More than 1 thread
            {
                Debug.WriteLine("Start rendering with: " + Constants.NumberOfThreads + " Threads");
                stopwatch.Start();
                finishedThreads = 0;
                for (int i = 0; i < Constants.NumberOfThreads; i++)
                {
                    ISampler sampler = new StratifiedSampler();
                    MultiThreadingRenderer renderer = new MultiThreadingRenderer(i, scene.IntersectableList,scene.Lights, scene.Camera, scene.Integrator, scene.Film, Constants.NumberOfThreads, sampler);
                    renderer.ThreadDone += HandleThreadDone;
                    Thread t = new Thread(renderer.Render);
                    t.Start();
                }
            }
        }
        void HandleThreadDone(object sender, EventArgs e)
        {
            finishedThreads++;
            if (finishedThreads == Constants.NumberOfThreads)
            {
                stopwatch.Stop();
                Debug.WriteLine("Finished rendering in: " + stopwatch.ElapsedMilliseconds + " ms.");
                Tonemapper.SaveImage("C:\\Test\\" + scene.FileName, scene.Film);
            }
        }

    }
}
