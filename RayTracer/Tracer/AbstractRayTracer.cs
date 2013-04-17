using RayTracer.Samplers;
using RayTracer.SceneGraph;
using RayTracer.SceneGraph.Integrators;
using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RayTracer.Tracer
{
    public abstract class AbstractRayTracer : IRayTracer
    {
        protected String FileName;
        protected Scene scene;
        protected IIntegrator integrator;
        protected Film film;
        protected Camera camera;
        protected int NumberOfThreads = 0;

        private int finishedThreads;
        
        Stopwatch stopwatch = new Stopwatch();
        
        protected AbstractRayTracer()
        {
            if(Constants.IsLightSamplingOn) Constants.Sampler.CreateLightSamples();
        }

        public void Render()
        {
            stopwatch.Reset();   
            if (NumberOfThreads == 0)
            {
                stopwatch.Start();
                for (int i = 0; i < camera.ScreenWidth; i++)
                {
                    for (int j = 0; j < camera.ScreenHeight; j++)
                    {
                        if(Constants.IsLightSamplingOn)
                        {
                            List<Sample> samples = Constants.Sampler.CreateSamples();
                            Color c = Color.Black;
                            foreach (Sample sample in samples)
                            {
                                Ray ray = camera.CreateRay(i+sample.X, j+sample.Y);
                                c += integrator.Integrate(ray);
                            }
                            c = c/samples.Count;
                            film.SetPixel(i,j,c);
                        }
                        else
                        {
                            Ray ray = camera.CreateRay(i, j);
                            Color color = integrator.Integrate(ray);
                            film.SetPixel(i, j, color);
                        }
                    }
                }
                stopwatch.Stop();
                Debug.WriteLine("Finished rendering in: " + stopwatch.ElapsedMilliseconds + " ms.");
                Tonemapper.SaveImage("C:\\Test\\" + FileName, film);
            }
            else
            {
                Debug.WriteLine("Start rendering with: " + NumberOfThreads + " Threads");
                stopwatch.Start();
                finishedThreads = 0;
                for (int i = 0; i < NumberOfThreads; i++)
                {
                    ISampler sampler = Constants.Sampler;
                    MultiThreadingRenderer renderer = new MultiThreadingRenderer(i,scene,camera,integrator,film,NumberOfThreads,sampler);
                    renderer.ThreadDone += HandleThreadDone;
                    Thread t = new Thread(renderer.Render);
                    t.Start();
                }
            }
        }
        void HandleThreadDone(object sender, EventArgs e)
        {
            finishedThreads++;
            if (finishedThreads == NumberOfThreads)
            {
                stopwatch.Stop();
                Debug.WriteLine("Finished rendering in: " + stopwatch.ElapsedMilliseconds + " ms.");
                Tonemapper.SaveImage("C:\\Test\\" + FileName, film);
            }
        }
    }
    
}
