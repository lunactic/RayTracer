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
            if (sampler != null)
            {
                for (int i = 0; i < camera.ScreenWidth; i++)
                {
                    for (int j = threadId; j < camera.ScreenHeight; j += Constants.NumberOfThreads)
                    {
                        Color pixelColor = new Color(0, 0, 0);
                        List<List<Sample>> subPathSamples = new List<List<Sample>>();
                        List<Sample> samples = sampler.CreateSamples();
                        List<LightSample> lightSamples = new List<LightSample>();
                        if (integrator is PathTraceIntegrator)
                        {
                            lightSamples = sampler.GetLightSamples(Constants.MaximalPathLength);
                            for (int s = 0; s < Constants.MaximalPathLength; s++)
                            {
                                subPathSamples.Add(sampler.CreateSamples());
                            }
                            foreach (Sample sample in samples)
                            {
                                Ray ray = camera.CreateRay(i + sample.X, j + sample.Y);
                                pixelColor.Append(integrator.Integrate(ray, objects, lights, sampler, subPathSamples, Randomizer.PickRandomLightSample(lightSamples)));
                            }
                        }
                        else
                        {
                            lightSamples = sampler.GetLightSamples();
                            foreach (Sample sample in samples)
                            {
                                Ray ray = camera.CreateRay(i + sample.X, j + sample.Y);
                                pixelColor.Append(integrator.Integrate(ray, objects, lights, sampler, subPathSamples, Randomizer.PickRandomLightSample(lightSamples)));
                            }
                        }
                        pixelColor.VoidDiv(samples.Count);
                        film.SetPixel(i, j, pixelColor);
                    }
                }
            }
            else
            {
                for (int i = 0; i < camera.ScreenWidth; i++)
                {
                    for (int j = threadId; j < camera.ScreenHeight; j += Constants.NumberOfThreads)
                    {
                        Ray ray = camera.CreateRay(i, j);
                        Color pixelColor = integrator.Integrate(ray, objects, lights, null, null, null);
                        film.SetPixel(i,j,pixelColor);
                    }
                }
            }
            ThreadDone(this, new EventArgs());
        }
    }
}
