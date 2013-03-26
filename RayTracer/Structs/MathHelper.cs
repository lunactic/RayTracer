using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Structs
{
    /// <summary>
    /// Provides functionality to some Math.
    /// </summary>
    /// <remarks>Implementation taken from OpenTK, their implementation can be found at: http://opentk.svn.sourceforge.net/viewvc/opentk/trunk/Source/OpenTK/Math/MathHelper.cs?revision=3078&view=markup </remarks>
    public static class MathHelper
    {
        /// <summary>
        /// Returns 2*Pi
        /// </summary>
        public const float TwoPi = (float)Math.PI * 2f;

        /// <summary>
        /// The needed method to calculate Radians from degrees
        /// </summary>
        private const float Deg2Rad = (float)Math.PI / 180.0f;

        /// <summary>
        /// Calculates the radians from given degrees
        /// </summary>
        /// <param name="degrees">The degrees.</param>
        /// <returns></returns>
        public static float DegreesToRadians(float degrees)
        {
            return Deg2Rad * degrees;
        }

        /// <summary>
        /// Calculates the degrees from given radians
        /// </summary>
        /// <param name="radians">The radians.</param>
        /// <returns></returns>
        public static float RadiansToDegrees(float radians)
        {
            return radians / Deg2Rad;
        }
    }
}
