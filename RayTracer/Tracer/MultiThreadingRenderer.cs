using RayTracer.Samplers;
using RayTracer.SceneGraph;
using RayTracer.SceneGraph.Integrators;
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
        private int threadId;
        private Scene scene;
        private Camera camera;
        private IIntegrator integrator;
        private Film film;
        private ISampler sampler;

        private int numberOfThreads;

        public event EventHandler ThreadDone;
        public MultiThreadingRenderer(int i,Scene scene, Camera camera,  IIntegrator integrator, Film film, int numberOfThreads, ISampler sampler)
        {
            this.threadId = i;
            this.scene = scene;
            this.camera = camera;
            this.integrator = integrator;
            this.film = film;
            this.numberOfThreads = numberOfThreads;
            

        }
        public void Render()
        {
            for (int i = 0; i < camera.ScreenWidth; i++)
            {
                for (int j = threadId; j < camera.ScreenHeight; j=j+numberOfThreads)
                {

                    if (sampler != null)
                    {
                        List<Sample> samples = sampler.CreateSamples();
                        Color c = Color.Black;
                        foreach (Sample sample in samples)
                        {
                            Ray ray = camera.CreateRay(i+sample.X, j+sample.Y);
                            c += integrator.Integrate(ray);
                        }
                        c = c / samples.Count;
                        film.SetPixel(i, j, c);
                    }
                    else
                    {
                        Ray ray = camera.CreateRay(i, j);
                        Color color = integrator.Integrate(ray);
                        film.SetPixel(i, j, color);
                    }

                }
            }
            ThreadDone(this,new EventArgs());
        }
    }
}
