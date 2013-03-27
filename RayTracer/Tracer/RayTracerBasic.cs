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
    public class RayTracerBasic : AbstractRayTracer
    {

        public RayTracerBasic()
        {
            FileName = "Assignment1_Basic.jpg";
            scene = new Scene { BackgroundColor = Color.Black };
            integrator = new BlinnIntegrator(scene);
            camera = new Camera
            {
                FieldOfView = 60f,
                ScreenWidth = 512,
                ScreenHeight = 512,
                Eye = new Vector4(0, 0, 2, 1),
                Up = new Vector4(0, 1, 0, 1),
                LookAt = new Vector4(0, 0, 0, 1)
            };
            camera.PreProcess();

            film = new Film(camera.ScreenWidth, camera.ScreenHeight);

            //List of objects
            Sphere sphere = new Sphere(new LambertMaterial(new Color(0.8f, 0.8f, 0.8f, 1.0f)), new Vector3(0f, 0f, 0f), 0.2f);
            
            Plane p1 = new Plane(1f, new Vector3(0, 1, 0))
                {
                    Name = "P1",
                    Material = new LambertMaterial(new Color(0f, 0.8f, 0.8f, 1.0f))
                    
                };
            Plane p2 = new Plane(1f, new Vector3(0, 0, 1))
                {
                    Name = "P2",
                    Material = new LambertMaterial(new Color(0.3f, 0.8f, 0.8f, 1.0f))
                };
            Plane p3 = new Plane(1f, new Vector3(-1, 0, 0))
                {
                    Name = "P3",
                    Material = new LambertMaterial(new Color(1.0f, 0.8f, 0.8f, 1.0f))
                };
            Plane p4 = new Plane(1f, new Vector3(1, 0, 0))
                {
                    Name = "P4",
                    Material = new LambertMaterial(new Color(0f, 0.8f, 0f, 1.0f))
                };
            Plane p5 = new Plane(1f, new Vector3(0, -1, 0))
                {
                    Name = "P5",
                    Material = new LambertMaterial(new Color(0.8f, 0.8f, 0.8f, 1.0f))
                };


            scene.IntersectableList.Objects.Add(sphere);
            scene.IntersectableList.Objects.Add(p1);
            scene.IntersectableList.Objects.Add(p2);
            scene.IntersectableList.Objects.Add(p3);
            scene.IntersectableList.Objects.Add(p4);
            scene.IntersectableList.Objects.Add(p5);

            ILight light = new PointLight(new Vector3(0.0f, 0.8f, 0.8f), new Color(0.7f, 0.7f, 0.7f, 1f));
     
            scene.Lights.Add(light);     

         
        }
    }
}
