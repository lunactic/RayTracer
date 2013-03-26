using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.SceneGraph.Light
{
    public class PointLight : ILight
    {
        public Color Color { get; set; }
        public Vector3 Position { get; set; }
        public PointLight(Vector3 position, Color color)
        {
            Color = color;
            Position = position;
        }

        public Color GetIncidentColor(Vector3 v)
        {
            Vector3 length = Vector3.Subtract(Position, v);
            float dist = length.Length*length.Length;
            return Color * (1/dist);
        }

        public Vector3 GetLightDirection(Vector3 v)
        {
            Vector3 dir = Vector3.Subtract(Position, v);
            dir.Normalize();
            return dir;
        }
        public float GetLightDistance(Vector3 v)
        {
            return Vector3.Subtract(Position, v).Length;
        }
    }
}
