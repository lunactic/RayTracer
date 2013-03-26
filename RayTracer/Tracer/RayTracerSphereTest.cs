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
    public class RayTracerSphereTest : IRayTracer
    {
        private Scene scene;
        private IIntegrator integrator;
        private Film film;
        private Camera camera;

        public RayTracerSphereTest()
        {
            scene = new Scene { BackgroundColor = Color.Black };
            integrator = new BlinnIntegrator(scene);
            camera = new Camera
            {
                FieldOfView = 60.0f,
                ScreenWidth = 512,
                ScreenHeight = 512,
                Eye = new Vector4(0, 0, 0, 1),
                Up = new Vector4(0, 1, 0, 1),
                LookAt = new Vector4(0, 0, -1, 1)
            };
            camera.PreProcess();

            Sphere sphere = new Sphere(new BlinnPhongMaterial(Color.Red,Color.White,20.0f), new Vector3(0f,0f,-2f),1f);
            scene.IntersectableList.Objects.Add(sphere);
            film = new Film(camera.ScreenWidth, camera.ScreenHeight);
            ILight light = new PointLight(new Vector3(0.75f,0.75f,0), Color.White);
            scene.Lights.Add(light);
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
            Tonemapper.SaveImage("C:\\Test\\RayTracerSphereTest.jpg", film);
        }
    }
}
