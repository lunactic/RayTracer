using RayTracer.Samplers;
using RayTracer.SceneGraph;
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
        private readonly Camera camera;
        private readonly IIntegrator integrator;
        private readonly Film film;
        private readonly ISampler sampler;
        private readonly IntersectableList objects;
        private readonly List<ILight> lights; 


        public event EventHandler ThreadDone;
        public MultiThreadingRenderer(int i, IntersectableList objects,List<ILight> lights , Camera camera, IIntegrator integrator, Film film, int numberOfThreads, ISampler sampler)
        {
            threadId = i;
            this.camera = camera;
            this.integrator = integrator;
            this.film = film;
            this.sampler = sampler;
            this.objects = objects;
            this.lights = lights;
        }
        public void Render()
        {
            for (int i = 0; i < camera.ScreenWidth; i++)
            {
                for (int j = threadId; j < camera.ScreenHeight; j += Constants.NumberOfThreads)
                {

                    if (Constants.IsSamplingOn)
                    {
                        List<Sample> samples = sampler.CreateSamples();
                        Color c = Color.Black;
                        foreach (Sample sample in samples)
                        {
                            Ray ray = camera.CreateRay(i + sample.X, j + sample.Y);
                            c += integrator.Integrate(ray,objects,lights,sampler);
                        }
                        c = c / samples.Count;
                        film.SetPixel(i, j, c);
                    }
                    else
                    {
                        Ray ray = camera.CreateRay(i, j);
                        Color color = integrator.Integrate(ray,objects,lights,sampler);
                        film.SetPixel(i, j, color);
                    }

                }
            }
            ThreadDone(this, new EventArgs());
        }
    }
}
