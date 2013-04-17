﻿using RayTracer.Samplers;
using RayTracer.SceneGraph.Objects;
using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Pdf = (1f/Shape.GetArea());
        }

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

        public void Sample(HitRecord record, LightSample sample)
        {
            Vector3 position = Shape.GetSamplePoint(sample.X, sample.Y);
            Vector3 normal = Shape.GetSampledNormal(position.X,position.Y);
            Vector3 wi = position;
            wi-= record.IntersectionPoint;
            wi.Normalize();
            position-=record.IntersectionPoint;
            float ldist = position.Length;
            Vector3 wiNegated = -wi;
            float cos1 = Vector3.Dot(record.SurfaceNormal,wi);
            float cos2 = Vector3.Dot(normal, wiNegated);
            if (cos2 <= 0) sample.LightColor = new Color(0,0,0,1f);
            if (cos1 > 0 && cos1 <= 1 && cos2 > 0 && cos2 <= 1)
            {
                float geom = (cos1 * cos2) / (ldist * ldist);
                sample.LightColor = LightColor * geom;
                sample.Normal = normal;
                sample.Position = position;
                sample.Wi = wi;
                sample.Pdf = Pdf;
            }
        }
    }
}