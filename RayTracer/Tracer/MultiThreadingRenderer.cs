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
        private int numberOfThreads;

        public event EventHandler ThreadDone;
        public MultiThreadingRenderer(int i,Scene scene, Camera camera,  IIntegrator integrator, Film film, int numberOfThreads)
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

                    Ray ray = camera.CreateRay(i, j);
                    Color color = integrator.Integrate(ray);
                    film.SetPixel(i, j, color);

                }
            }
            ThreadDone(this,new EventArgs());
        }
    }
}
