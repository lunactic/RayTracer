using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.SceneGraph
{
    public class Ray
    {
        #region Properties
        /// <summary>
        /// Gets the origin in world space.
        /// </summary>
        public Vector3 Origin { get; private set; }
        /// <summary>
        /// Gets the direction world space.
        /// </summary>
        public Vector3 Direction { get; private set; }

        #region AABB Intersection Helpers
        public Vector3 InvDirection { get; set; }
        public int[] Sign { get; set; }
        #endregion

        #endregion

        #region Constructor
        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
            InvDirection = new Vector3(1 / Direction.X, 1 / Direction.Y, 1 / Direction.Z);
            Sign = new int[3];
            Sign[0] = InvDirection.X < 0 ? 1 : 0;
            Sign[1] = InvDirection.Y < 0 ? 1 : 0;
            Sign[2] = InvDirection.Z < 0 ? 1 : 0;

        }
        #endregion

    }
}
