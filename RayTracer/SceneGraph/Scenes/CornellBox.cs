using RayTracer.SceneGraph.Cameras;
using RayTracer.SceneGraph.Integrators;
using RayTracer.SceneGraph.Light;
using RayTracer.SceneGraph.Materials;
using RayTracer.SceneGraph.Objects;
using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.SceneGraph.Scenes
{
    public class CornellBox : Scene
    {
        public CornellBox()
        {
            FileName = "CornellBox.jpg";

            Integrator = new RefractionIntegrator();


            // Make camera and film
            Camera = new PinholeCamera()
            {
                Eye = new Vector4(278f, 273f, -800f, 1f),
                LookAt = new Vector4(278f, 273f, 0f, 1f),
                Up = new Vector4(0f, 1f, 0f, 1f),
                FieldOfView = 38.5f,
                ScreenHeight = 512,
                ScreenWidth = 512
            };
            Camera.PreProcess();
            Film = new Film(Camera.ScreenWidth, Camera.ScreenHeight);
            // Colors
            LambertMaterial white = new LambertMaterial(new Color(1, 1, 1));
            LambertMaterial green = new LambertMaterial(new Color(0, 1, 0));
            LambertMaterial red = new LambertMaterial(new Color(1, 0, 0));

            // List of objects
            Objects = new IntersectableList();

            // Floor
            int[] i = { 0, 1, 2, 0, 2, 3 };
            Mesh floor = new Mesh(white, i,
                            556f, 0.0f, 0.0f,
                            0.0f, 0.0f, 0.0f,
                            0.0f, 0.0f, 559.2f,
                            556f, 0.0f, 559.2f);
            Objects.Add(floor);

            // Ceiling
            Mesh ceiling = new Mesh(white, i,
                            556.0f, 548.8f, 0.0f,
                            556.0f, 548.8f, 559.2f,
                            0.0f, 548.8f, 559.2f,
                            0.0f, 548.8f, 0.0f);
            Objects.Add(ceiling);

            // Back wall
            Mesh backWall = new Mesh(white, i,
                            556f, 0.0f, 559.2f,
                            0.0f, 0.0f, 559.2f,
                            0.0f, 548.8f, 559.2f,
                            556.0f, 548.8f, 559.2f);
            Objects.Add(backWall);

            // Left wall
            Mesh leftWall = new Mesh(red, i,
                            556f, 0.0f, 0.0f,
                            556f, 0.0f, 559.2f,
                            556.0f, 548.8f, 559.2f,
                            556.0f, 548.8f, 0.0f);
            Objects.Add(leftWall);

            // Right wall
            Mesh rightWall = new Mesh(green, i,
                            0.0f, 0.0f, 559.2f,
                            0.0f, 0.0f, 0.0f,
                            0.0f, 548.8f, 0.0f,
                            0.0f, 548.8f, 559.2f);
            Objects.Add(rightWall);

            // Short block
            int[] j = {0, 1, 2, 0, 2, 3,
                       4, 3, 2, 4, 2, 7,
                       8, 0, 3, 8, 3, 4,
                       12, 1, 0, 12, 0, 8,
                       7, 2, 1, 7, 1, 12};
            Mesh shortBlock = new Mesh(white, j,
                            130.0f, 165.0f, 65.0f,
                            82.0f, 165.0f, 225.0f,
                            240.0f, 165.0f, 272.0f,
                            290.0f, 165.0f, 114.0f,
                            290.0f, 0.0f, 114.0f,
                            290.0f, 165.0f, 114.0f,
                            240.0f, 165.0f, 272.0f,
                            240.0f, 0.0f, 272.0f,
                            130.0f, 0.0f, 65.0f,
                            130.0f, 165.0f, 65.0f,
                            290.0f, 165.0f, 114.0f,
                            290.0f, 0.0f, 114.0f,
                            82.0f, 0.0f, 225.0f,
                            82.0f, 165.0f, 225.0f,
                            130.0f, 165.0f, 65.0f,
                            130.0f, 0.0f, 65.0f,
                            240.0f, 0.0f, 272.0f,
                            240.0f, 165.0f, 272.0f,
                            82.0f, 165.0f, 225.0f,
                            82.0f, 0.0f, 225.0f);
            //Objects.Add(shortBlock);

            // Tall block
            Mesh tallBlock = new Mesh(white, j,
                            423.0f, 330.0f, 247.0f,
                            265.0f, 330.0f, 296.0f,
                            314.0f, 330.0f, 456.0f,
                            472.0f, 330.0f, 406.0f,
                            423.0f, 0.0f, 247.0f,
                            423.0f, 330.0f, 247.0f,
                            472.0f, 330.0f, 406.0f,
                            472.0f, 0.0f, 406.0f,
                            472.0f, 0.0f, 406.0f,
                            472.0f, 330.0f, 406.0f,
                            314.0f, 330.0f, 456.0f,
                            314.0f, 0.0f, 456.0f,
                            314.0f, 0.0f, 456.0f,
                            314.0f, 330.0f, 456.0f,
                            265.0f, 330.0f, 296.0f,
                            265.0f, 0.0f, 296.0f,
                            265.0f, 0.0f, 296.0f,
                            265.0f, 330.0f, 296.0f,
                            423.0f, 330.0f, 247.0f,
                            423.0f, 0.0f, 247.0f);
            //Objects.Add(tallBlock);


            // List of lights
            Lights = new List<ILight>();

            //Triangle triangle = new Triangle(new Vector3(343.0f, 548.7f, 227.0f), new Vector3(343.0f, 548.7f, 332.0f), new Vector3(213.0f, 548.7f, 332.0f)) { Material = white };
            //Objects.Add(triangle);
            //Rectangle rect = new Rectangle(new Vector3(445.5f, 548.7f, 279, 5f),);
            //ILight light = new AreaLight(new Color(10, 10, 10), rect);
            ILight light = new PointLight(new Vector3(278, 548.7f, 279.5f), new Color(1f, 0.85f, 0.43f));
            Lights.Add(light);

        }
    }
}
