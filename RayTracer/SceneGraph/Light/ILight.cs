using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.SceneGraph.Light
{
    public interface ILight
    {
        Vector3 Position { get; set; }
        Color GetIncidentColor(Vector3 v);
        Vector3 GetLightDirection(Vector3 v);
        float GetLightDistance(Vector3 v);
    }
}
