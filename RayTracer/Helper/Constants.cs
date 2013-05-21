using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Samplers;
using RayTracer.SceneGraph.Cameras;
using RayTracer.SceneGraph.Integrators;
using RayTracer.SceneGraph.Scenes;
using RayTracer.Tracer;

public enum SampleType
{
    Uniform,
    Importance
};
namespace RayTracer.Helper
{
    public class Constants
    {
        public static readonly double Epsilon = 1e-6;
        
        //Recursion for RayTracing
        public static readonly int MaximumRecursionDepth = 3;
        //Path Length for Path Tracing
        public static readonly int MaximalPathLength = 5;
        
        public static readonly bool IsLightSamplingOn = false;
        public static readonly bool IsSamplingOn = false;
        public static readonly bool IsDepthOfFieldCamera = false;


        public static readonly int NumberOfLightSamples = 1;
        public static readonly int NumberOfSamples = 10;
        public static int NumberOfThreads = 1;


        public static bool TextureMapping = true;

        //Collada
        public static Type Sampler = typeof (StratifiedSampler);
        public static Type SceneIndex = typeof(SkyBoxScene);
        public static Type Integrator = typeof (WhittedIntegrator);
        public static Type Camera = typeof (PinholeCamera);
        public static Type RayTracer = typeof(BasicRayTracer);
        

        //PATHTRACE
        /*
        public static Type Sampler = typeof (StratifiedSampler);
        public static Type SceneIndex = typeof(MirrorScene);
        public static Type Integrator = typeof (PathTraceIntegrator);
        public static Type Camera = typeof (PinholeCamera);
        public static Type RayTracer = typeof(BasicRayTracer);
        */
        public static SampleType SampleType = SampleType.Uniform;

    }
}
