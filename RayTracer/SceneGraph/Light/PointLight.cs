using RayTracer.Samplers;
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
        public Color LightColor { get; set; }
        public Vector3 Position { get; set; }
        public PointLight(Vector3 position, Color color)
        {
            LightColor = color;
            Position = position;
        }

        public Color GetIncidentColor(Vector3 v)
        {
            Vector3 length = Vector3.Subtract(Position, v);
            float dist = length.Length*length.Length;
            return LightColor.Div(dist);
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


        public void Sample(HitRecord record, LightSample sample)
        {
            Vector3 wi = new Vector3(Position);
            wi -= record.IntersectionPoint;
            wi.Normalize();
            Vector3 dist = Vector3.Subtract(Position, record.IntersectionPoint);
            float ldist = dist.Length;

            sample.LightColor = LightColor.Div(ldist*ldist);
            sample.Wi = wi;
            sample.Distance = ldist;
            sample.Position = Position;
            sample.Pdf = 1;
        }
    }
}
