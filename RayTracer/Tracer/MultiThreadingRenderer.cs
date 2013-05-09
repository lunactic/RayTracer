using RayTracer.Helper;
using RayTracer.Samplers;
using RayTracer.SceneGraph;
using RayTracer.SceneGraph.Cameras;
using RayTracer.SceneGraph.Integrators;
using RayTracer.SceneGraph.Light;
using RayTracer.SceneGraph.Objects;
using RayTracer.SceneGraph.Scenes;
using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Tracer
{
    public class MultiThreadingRenderer
    {
        private readonly int threadId;
        private readonly ICamera camera;
        private readonly IIntegrator integrator;
        private readonly Film film;
        private readonly ISampler sampler;
        private readonly Aggregate objects;
        private readonly List<ILight> lights;


        public event EventHandler ThreadDone;
        public MultiThreadingRenderer(int i, Aggregate objects, List<ILight> lights, ICamera camera, Film film)
        {
            threadId = i;
            this.camera = camera.Clone();
            this.integrator = (IIntegrator)Activator.CreateInstance(Constants.Integrator);
            this.film = film;
            this.sampler = (ISampler)Activator.CreateInstance(Constants.Sampler);
            this.objects = objects;
            this.lights = lights;
        }
        public void Render()
        {
            for (int i = 0; i < camera.ScreenWidth; i++)
            {
                for (int j = threadId; j < camera.ScreenHeight; j += Constants.NumberOfThreads)
                {
                    List<List<Sample>> subPathSamples = new List<List<Sample>>();
                    if (integrator is PathTraceIntegrator)
                    {
                        for (int s = 0; s < Constants.MaximalPathLength; s++)
                        {
                            subPathSamples.Add(sampler.CreateSamples());
                        }
                    }
                    if (Constants.IsSamplingOn)
                    {
                        List<Sample> samples = sampler.CreateSamples();
                        Color c = new Color(0, 0, 0);
                        foreach (Sample sample in samples)
                        {
                            if (Constants.IsDepthOfFieldCamera)
                            {
                                for (int p = 0; p < 25; p++)
                                {
                                    Color tmp = new Color(0, 0, 0);
                                    Ray ray = camera.CreateRay(i + sample.X, j + sample.Y);
                                    tmp.Append(integrator.Integrate(ray, objects, lights, sampler, subPathSamples));
                                    //tmp.VoidDiv(25);
                                    c.Append(tmp);
                                }
                            }
                            else
                            {
                                Ray ray = camera.CreateRay(i + sample.X, j + sample.Y);
                                c.Append(integrator.Integrate(ray, objects, lights, sampler, subPathSamples));
                            }
                        }
                        c.VoidDiv(samples.Count);
                        film.SetPixel(i, j, c);
                    }
                    else
                    {
                        Ray ray = camera.CreateRay(i, j);
                        Color color = integrator.Integrate(ray, objects, lights, sampler, subPathSamples);
                        film.SetPixel(i, j, color);
                    }

                }
            }
            ThreadDone(this, new EventArgs());
        }
    }
}
