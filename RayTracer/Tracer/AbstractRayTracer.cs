using RayTracer.Samplers;
using RayTracer.SceneGraph;
using RayTracer.SceneGraph.Integrators;
using RayTracer.SceneGraph.Scenes;
using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RayTracer.Tracer
{
    public abstract class AbstractRayTracer
    {
        protected String FileName;
        protected Scene scene;
        protected IIntegrator integrator;
        protected Film film;
        protected Camera camera;
        protected int NumberOfThreads = 0;

        private int finishedThreads;

        readonly Stopwatch stopwatch = new Stopwatch();
        
        protected AbstractRayTracer()
        {
            Constants.Sampler.CreateLightSamples();
        }

        public void Render()
        {
        }
    }
    
}
