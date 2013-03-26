using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.SceneGraph;
using RayTracer.SceneGraph.Integrators;
using RayTracer.SceneGraph.Light;
using RayTracer.SceneGraph.Materials;
using RayTracer.SceneGraph.Objects;
using RayTracer.Structs;

namespace RayTracer.Tracer
{
    public class RayTracerFirst : IRayTracer
    {

        private Scene scene;
        private IIntegrator integrator;
        private Film film;
        private Camera camera;

        public RayTracerFirst()
        {
            scene = new Scene { BackgroundColor = Color.Black };
            integrator = new BinaryIntegrator(scene);
            camera = new Camera
            {
                FieldOfView = 60.0f,
                ScreenWidth = 512,
                ScreenHeight = 512,
                Eye = new Vector4(0, 0, 2, 1),
                Up = new Vector4(0, 1, 0, 1),
                LookAt = new Vector4(0, 0, 0, 1)
            };
            camera.PreProcess();

            Plane p = new Plane(1.0f,new Vector3(0,1,0));
            scene.IntersectableList.Objects.Add(p);
            film = new Film(camera.ScreenWidth, camera.ScreenHeight);
            integrator = new BinaryIntegrator(scene);


        }
        public void Render()
        {
            for (int y = 0; y < camera.ScreenHeight; y++)
            {
                for (int x = 0; x < camera.ScreenWidth; x++)
                {

                    Ray ray = camera.CreateRay(x, y);
                    Color color = integrator.Integrate(ray);
                    film.SetPixel(x, y, color);

                }
            }
            Tonemapper.SaveImage("C:\\Test\\RayTracerFirst.jpg", film);
        }
    }
}
