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

            Vector3 position = Shape.GetSamplePoint(sample);
            sample.Position = position;

            Vector3 wi = position;
            wi = wi - record.IntersectionPoint;

            sample.Pdf = (float)(1f / Math.PI * sample.Area);

            
            float ldist = wi.Length;
            sample.Distance = ldist;
            wi.Normalize();
            sample.Wi = wi;

            Vector3 wiNegated = -wi;
            Vector3 recordNormal = record.SurfaceNormal;
            Vector3 lightNormal = sample.Normal;

            float cos1 = Math.Min(Math.Max(Vector3.Dot(recordNormal, wi), 0), 1);
            float cos2 = Math.Min(Math.Max(Vector3.Dot(lightNormal, wiNegated), 0), 1);
            float geom = (cos1 * cos2) / (ldist * ldist);
            float multFact = geom * Shape.GetArea();
            sample.LightColor = LightColor.Mult(geom);
          
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
