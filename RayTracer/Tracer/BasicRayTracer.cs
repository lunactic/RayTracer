using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RayTracer.Helper;
using RayTracer.Samplers;
using RayTracer.SceneGraph;
using RayTracer.SceneGraph.Scenes;
using RayTracer.Structs;
using RayTracer.SceneGraph.Objects;
using RayTracer.SceneGraph.Materials;
using RayTracer.SceneGraph.Light;
using RayTracer.SceneGraph.Integrators;

namespace RayTracer.Tracer
{
    public class BasicRayTracer : IRayTracer
    {
        private Scene scene;
        private Stopwatch stopwatch;
        private int finishedThreads;

        public BasicRayTracer(Scene scene)
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
                ISampler sampler = (ISampler)Activator.CreateInstance(Constants.Sampler);
                if (Constants.IsSamplingOn && Constants.NumberOfSamples > 0)
                {
                    Console.WriteLine("Using Supersampling with: " + Constants.NumberOfSamples + " samples!");

                    for (int i = 0; i < scene.Film.Width; i++)
                    {
                        for (int j = 0; j < scene.Film.Height; j++)
                        {
                            Color pixelColor = new Color(0, 0, 0);
                            List<List<Sample>> subPathSamples = new List<List<Sample>>();
                            List<LightSample> lightSamples = new List<LightSample>();
                            List<Sample> samples = sampler.CreateSamples();
                            if (scene.Integrator is PathTraceIntegrator)
                            {
                                lightSamples = sampler.GetLightSamples(Constants.MaximalPathLength);
                                for (int s = 0; s < Constants.MaximalPathLength; s++)
                                {
                                    subPathSamples.Add(sampler.CreateSamples());
                                }
                                foreach (Sample sample in samples)
                                {
                                    Ray ray = scene.Camera.CreateRay(i + sample.X, j + sample.Y);
                                    pixelColor.Append(scene.Integrator.Integrate(ray, scene.Objects, scene.Lights, sampler, subPathSamples,Randomizer.PickRandomLightSample(lightSamples)));
                                }
                            }
                            else
                            {
                                lightSamples = sampler.GetLightSamples();
                                foreach (Sample sample in samples)
                                {
                                    Ray ray = scene.Camera.CreateRay(i + sample.X, j + sample.Y);
                                    pixelColor.Append(scene.Integrator.Integrate(ray, scene.Objects, scene.Lights, sampler, subPathSamples, Randomizer.PickRandomLightSample(lightSamples))); 
                                }
                            }
                       
                            pixelColor.VoidDiv(samples.Count);
                            scene.Film.SetPixel(i, j, pixelColor);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < scene.Film.Width; i++)
                    {
                        for (int j = 0; j < scene.Film.Height; j++)
                        {
                            Ray ray = scene.Camera.CreateRay(i, j);
                            Color color = scene.Integrator.Integrate(ray, scene.Objects, scene.Lights,null,null,null);
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
                if (Constants.IsSamplingOn && Constants.NumberOfSamples > 0)
                {
                    Debug.WriteLine("Start rendering with: " + Constants.NumberOfThreads + " Threads");
                    stopwatch.Start();
                    finishedThreads = 0;
                    for (int i = 0; i < Constants.NumberOfThreads; i++)
                    {
                        ISampler sampler = new StratifiedSampler();
                        MultiThreadingRenderer renderer = new MultiThreadingRenderer(i, scene.Objects, scene.Lights,
                                                                                     scene.Camera,
                                                                                     scene.Film);
                        renderer.ThreadDone += HandleThreadDone;
                        Thread t = new Thread(renderer.Render);
                        t.Start();
                    }
                }
                else
                {
                    stopwatch.Start();
                    finishedThreads = 0;
                    for (int i = 0; i < Constants.NumberOfThreads; i++)
                    {
                        MultiThreadingRenderer renderer = new MultiThreadingRenderer(i, scene.Objects, scene.Lights,
                                                                                     scene.Camera,
                                                                                     scene.Film);
                        renderer.ThreadDone += HandleThreadDone;
                        Thread t = new Thread(renderer.Render);
                        t.Start();
                    }
                }
            }
        }

        private void HandleThreadDone(object sender, EventArgs e)
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
