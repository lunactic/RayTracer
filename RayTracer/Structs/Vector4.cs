using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Structs
{
    public struct Vector4
    {
        #region Fields
        /// <summary>
        /// The X component
        /// </summary>
        public float X;

        /// <summary>
        /// The Y component
        /// </summary>
        public float Y;

        /// <summary>
        /// The Z component
        /// </summary>
        public float Z;

        /// <summary>
        /// The W component
        /// </summary>
        public float W;

        /// <summary>
        /// Defines a unit-length Vector4 that points towards the X-axis.
        /// </summary>
        public static Vector4 UnitX = new Vector4(1, 0, 0, 0);

        /// <summary>
        /// Defines a unit-length Vector4 that points towards the Y-axis.
        /// </summary>
        public static Vector4 UnitY = new Vector4(0, 1, 0, 0);

        /// <summary>
        ///  Defines a unit-length Vector4 that points towards the Z-axis.
        /// </summary>
        public static Vector4 UnitZ = new Vector4(0, 0, 1, 0);

        /// <summary>
        /// Defines a unit-length Vector4 that points towards the W-axis.
        /// </summary>
        public static Vector4 UnitW = new Vector4(0, 0, 0, 1);
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a new Vector4.
        /// </summary>
        /// <param name="x">The x component of the Vector4.</param>
        /// <param name="y">The y component of the Vector4.</param>
        /// <param name="z">The z component of the Vector4.</param>
        /// <param name="w">The w component of the Vector4.</param>
        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> struct.
        /// </summary>
        /// <param name="v">The Vector3 to copy components from.</param>
        public Vector4(Vector3 v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = 0.0f;
        }
        #endregion

        #region Conversion
        /// <summary>
        /// Performs an implicit conversion from <see cref="Ch.Fhnw.ViewerComponentLibrary.Structs.Vector4"/> to <see cref="System.Single[]"/>.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator float[](Vector4 vector)
        {
            return new float[] { vector.X, vector.Y, vector.Z, vector.W };
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Ch.Fhnw.ViewerComponentLibrary.Structs.Vector4"/> to <see cref="Ch.Fhnw.ViewerComponentLibrary.Structs.Vector3"/>.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Vector3(Vector4 vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }

        #endregion

        #region Scale
        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="vec">The vector.</param>
        /// <param name="scale">The scale factor.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector4 operator *(Vector4 vec, float scale)
        {
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            vec.W *= scale;
            return vec;
        }
        #endregion

        #region Subtract
        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector4 operator -(Vector4 left, Vector4 right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            left.W -= right.W;
            return left;
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="vec">The vector.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector4 operator -(Vector4 vec)
        {
            vec.X = -vec.X;
            vec.Y = -vec.Y;
            vec.Z = -vec.Z;
            vec.W = -vec.W;
            return vec;
        }
        #endregion
        #region Add
        public static Vector4 operator +(Vector4 left, Vector4 right)
        {
            left.X += right.X;
            left.Y += right.Y;
            left.Z += right.Z;
            left.W += right.W;
            return left;
        }
        #endregion

        #region Normalize
        /// <summary>
        /// Scales the Vector3 to unit length.
        /// </summary>
        public void Normalize()
        {

            float scale = 1.0f / Length;
            X *= scale;
            Y *= scale;
            Z *= scale;
            W *= scale;
        }
        #endregion Normalize

        #region Length
        /// <summary>
        /// Gets the length (magnitude) of the vector.
        /// </summary>
        public float Length
        {
            get
            {
                return (float)Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
            }
        }
        #endregion

        #region Swizzle
        /// <summary>
        /// Gets or sets an Vector3 with the X, Y and Z components of this instance.
        /// </summary>
        public Vector3 Xyz { get { return new Vector3(X, Y, Z); } set { X = value.X; Y = value.Y; Z = value.Z; } }
        #endregion

        #region Transform

        /// <summary>Transform a Vector by the given Matrix</summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <param name="result">The transformed vector</param>
        public static void Transform(ref Vector4 vec, ref Matrix4 mat, out Vector4 result)
        {
            result = new Vector4(
                                vec.X * mat.Row0.X + vec.Y * mat.Row1.X + vec.Z * mat.Row2.X + vec.W * mat.Row3.X,
                                vec.X * mat.Row0.Y + vec.Y * mat.Row1.Y + vec.Z * mat.Row2.Y + vec.W * mat.Row3.Y,
                                vec.X * mat.Row0.Z + vec.Y * mat.Row1.Z + vec.Z * mat.Row2.Z + vec.W * mat.Row3.Z,
                                vec.X * mat.Row0.W + vec.Y * mat.Row1.W + vec.Z * mat.Row2.W + vec.W * mat.Row3.W);
        }

        /// <summary>Transform a Vector by the given Matrix</summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed vector</returns>
        public static Vector4 Transform(Vector4 vec, Matrix4 mat)
        {
            Vector4 result;
            Transform(ref vec, ref mat, out result);
            return result;
        }
        #endregion
    }
}