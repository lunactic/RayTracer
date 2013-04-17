using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.SceneGraph.Light
{
    public class DirectionalLight : ILight
    {
        public Vector3 Position
        {
            get;
            set;
        }
        public Color LightColor { get; set; }
        private Vector3 Direction;
        

        public DirectionalLight(Vector3 direction, Color color)
        {
            Direction = direction;
            LightColor = color;
        }

        public Color GetIncidentColor(Structs.Vector3 v)
        {
            return LightColor;
        }

        public Vector3 GetLightDirection(Structs.Vector3 v)
        {
            return Direction;
        }

        public float GetLightDistance(Structs.Vector3 v)
        {
            return float.PositiveInfinity;
        }

        public void Sample(HitRecord record, Samplers.LightSample sample)
        {
            sample.LightColor = LightColor;
            sample.Position = Direction;
            sample.Wi = Direction;
            sample.Distance = float.PositiveInfinity;
        }

        
    }
}
