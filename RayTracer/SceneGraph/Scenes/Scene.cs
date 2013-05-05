using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Helper;
using RayTracer.SceneGraph.Cameras;
using RayTracer.SceneGraph.Integrators;
using RayTracer.SceneGraph.Objects;
using RayTracer.Structs;
using RayTracer.SceneGraph.Light;

namespace RayTracer.SceneGraph.Scenes
{
    public abstract class Scene
    {
        public Aggregate Objects { get; set; }
        public List<ILight> Lights { get; set; }
        public Color BackgroundColor { get; set; }
        public Color Ambient { get; set; }
        public Stopwatch Stopwatch { get; set; }
        public ICamera Camera { get; set; }
        public Film Film { get; set; }
        public IIntegrator Integrator { get; set; }
        public Tonemapper Tonemapper { get; set; }
        public String FileName { get; set; }
      

        protected Scene()
        {
            Camera = (ICamera)Activator.CreateInstance(Constants.Camera);
        }
    }
}
