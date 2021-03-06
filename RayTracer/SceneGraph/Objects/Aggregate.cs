﻿using RayTracer.Helper;
using RayTracer.SceneGraph.Accelerate;
using RayTracer.SceneGraph.Light;
using RayTracer.SceneGraph.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Structs;
using RayTracer.Samplers;

namespace RayTracer.SceneGraph.Objects
{
    public abstract class Aggregate : IIntersectable
    {

        public List<IIntersectable> Intersectables { get; set; }


        public Aggregate()
        {
            Intersectables = new List<IIntersectable>();
        }


        public void Add(IIntersectable intersectable)
        {
            Intersectables.Add(intersectable);
        }

        public HitRecord Intersect(Ray ray)
        {
            HitRecord hit = null;
            foreach (var intersectable in Intersectables)
            {
                HitRecord tempHit = intersectable.Intersect(ray);
                if (tempHit != null)
                    if (hit == null) hit = tempHit;
                    //else if (tempHit.Distance < hit.Distance) hit = tempHit;
                    else if (Constants.IsLightSamplingOn)
                    {
                        if (tempHit.Distance < hit.Distance) hit = tempHit;
                    }
                    else if (tempHit.HitObject.Light == null && tempHit.Distance < hit.Distance) hit = tempHit;
            }
            return hit;
        }

        public IBoundingBox BoundingBox { get; set; }

        Material IIntersectable.Material
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public List<IIntersectable> GetObjects()
        {
            return Intersectables;
        }
   
        public abstract void BuildBoundingBox();

        public int GetNumberOfComponents()
        {
            int res = 0;
            foreach (IIntersectable inters in Intersectables)
            {
                res += inters.GetNumberOfComponents();
            }
            return res;
        }

        #region unused
        public ILight Light { get; set; }

        public Vector3 GetSamplePoint(LightSample sample)
        {
            throw new NotImplementedException();
        }

        public float GetArea()
        {
            throw new NotImplementedException();
        }

        #endregion


        public Vector2 GetTextudeCoordinates(HitRecord record)
        {
            throw new NotSupportedException();
        }
    }
}
