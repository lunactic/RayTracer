using RayTracer.Samplers;
using RayTracer.SceneGraph.Objects;
using RayTracer.Structs;
using System;

namespace RayTracer.SceneGraph.Light
{
    public class AreaLight : ILight
    {
        public Vector3 Position { get { throw new NotSupportedException(); } set { throw new NotSupportedException(); } }
        public IIntersectable Shape { get; set; }
        public float Pdf { get; set; }
        public Color LightColor { get; set; }

        public AreaLight(Color c, IIntersectable shape)
        {
            Shape = shape;
            LightColor = c;
            shape.Light = this;
            Pdf = (1f / Shape.GetArea());
        }

        public void Sample(HitRecord record, LightSample sample)
        {
            //position = sampPoint
            Vector3 position = Shape.GetSamplePoint(sample);
            Vector3 normal = sample.Normal;
            Vector3 wi = position;
            //wi = incidence
            wi = wi - record.IntersectionPoint;
            //wi.Normalize();
            position = position - record.IntersectionPoint; 
            //ldist = inlen
            float ldist = wi.Length;
            Vector3 wiNegated = -wi;
            float cos1 = Math.Max(Vector3.Dot(record.SurfaceNormal, wi), 0);
            float cos2 = Math.Max(Vector3.Dot(normal, wiNegated), 0);
            float geom = (cos1 * cos2) / (ldist * ldist);
            float multFact = geom * Shape.GetArea();
            sample.LightColor = LightColor.Mult(multFact);
            sample.Normal = normal;
            sample.Position = position;
            sample.Wi = wi;
            sample.Pdf = (float)(1f / Shape.GetArea());
        }

        #region unused

        public Color GetIncidentColor(Structs.Vector3 v)
        {
            throw new NotSupportedException();
        }

        public Vector3 GetLightDirection(Structs.Vector3 v)
        {
            throw new NotSupportedException();
        }

        public float GetLightDistance(Structs.Vector3 v)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
