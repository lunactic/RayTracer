using RayTracer.SceneGraph;
using RayTracer.SceneGraph.Integrators;
using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

        public void Render()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < camera.ScreenWidth; i++)
            {
                for (int j = 0; j < camera.ScreenHeight; j++)
                {

                    Ray ray = camera.CreateRay(i, j);
                    Color color = integrator.Integrate(ray);
                    film.SetPixel(i, j, color);

                }
            }
            stopwatch.Stop();
            Debug.WriteLine("Finished rendering in: " + stopwatch.ElapsedMilliseconds + " ms.");
            Tonemapper.SaveImage("C:\\Test\\"+ FileName, film);
        }
    }
}
