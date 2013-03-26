using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Structs
{
    /// <summary>
    /// Represents a 3D vector using three single-precision floating-point numbers.
    /// </summary>
    /// <remarks> Implementation partly taken from OpenTK, their implementation can be found at: <see cref="http://opentk.svn.sourceforge.net/viewvc/opentk/trunk/Source/OpenTK/Math/Vector3.cs?revision=3078&view=markup"/> </remarks>
    public struct Vector3 : IEquatable<Vector3>
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
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3"/> struct.
        /// </summary>
        /// <param name="x">The x component.</param>
        /// <param name="y">The y component.</param>
        /// <param name="z">The z component.</param>
        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        /// <summary>
        /// Constructs a new Vector3 from the given Vector3.
        /// </summary>
        /// <param name="v">The Vector3 to copy components from.</param>
        public Vector3(Vector3 v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        /// <summary>
        /// Constructs a new Vector3 from the given Vector4.
        /// </summary>
        /// <param name="v">The Vector4 to copy components from.</param>
        public Vector3(Vector4 v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        /// <summary>
        /// Defines a zero-length Vector3.
        /// </summary>
        public static readonly Vector3 Zero = new Vector3(0.0f, 0.0f, 0.0f);
        #endregion

        #region indexer
        public float this[int index]
        {
            get {
                switch (index)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    case 2:
                        return Z;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set {
                switch (index)
                {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    case 2:
                        Z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
        #endregion
        #region Add
        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            left.X += right.X;
            left.Y += right.Y;
            left.Z += right.Z;
            return left;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Result of operation.</returns>
        public static Vector3 Add(Vector3 a, Vector3 b)
        {
            Add(ref a, ref b, out a);
            return a;
        }
        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <param name="result">Result of operation.</param>
        public static void Add(ref Vector3 a, ref Vector3 b, out Vector3 result)
        {
            result = new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        #endregion

        #region Subtract
        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            return left;
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="vec">The vector.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector3 operator -(Vector3 vec)
        {
            vec.X = -vec.X;
            vec.Y = -vec.Y;
            vec.Z = -vec.Z;
            return vec;
        }

        /// <summary>
        /// Subtract one Vector from another
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <returns>Result of subtraction</returns>
        public static Vector3 Subtract(Vector3 a, Vector3 b)
        {
            Subtract(ref a, ref b, out a);
            return a;
        }

        /// <summary>
        /// Subtract one Vector from another
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <param name="result">Result of subtraction</param>
        public static void Subtract(ref Vector3 a, ref Vector3 b, out Vector3 result)
        {
            result = new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }
        #endregion

        #region Multiply
        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="scalar">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        public static Vector3 Multiply(Vector3 left, float scalar)
        {
            Multiply(ref left, scalar, out left);
            return left;
        }
        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="scalar">Right operand.</param>
        /// <param name="result">Result of the operation.</param>
        private static void Multiply(ref Vector3 left, float scalar, out Vector3 result)
        {
            result = new Vector3(left.X * scalar, left.Y * scalar, left.Z * scalar);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="vec">The vector.</param>
        /// <param name="scale">The scale factor.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector3 operator *(Vector3 vec, float scale)
        {
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            return vec;
        }
        #endregion

        #region Divide
        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="vec">The vector.</param>
        /// <param name="scale">The scale factor.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector3 operator /(Vector3 vec, float scale)
        {
            float mult = 1.0f / scale;
            vec.X *= mult;
            vec.Y *= mult;
            vec.Z *= mult;
            return vec;
        }
        #endregion

        #region Length
        /// <summary>
        /// Gets the length (magnitude) of the vector.
        /// </summary>
        public float Length
        {
            get
            {
                return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
            }
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
        }
        #endregion Normalize

        #region Cross
        /// <summary>
        /// Caclulate the cross (vector) product of two vectors
        /// </summary>
        /// <param name="left">First operand</param>
        /// <param name="right">Second operand</param>
        /// <returns>The cross product of the two inputs</returns>
        public static Vector3 Cross(Vector3 left, Vector3 right)
        {
            Vector3 result;
            Cross(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Caclulate the cross (vector) product of two vectors
        /// </summary>
        /// <param name="left">First operand</param>
        /// <param name="right">Second operand</param>
        /// <returns>The cross product of the two inputs</returns>
        /// <param name="result">The cross product of the two inputs</param>
        public static void Cross(ref Vector3 left, ref Vector3 right, out Vector3 result)
        {
            result = new Vector3(left.Y * right.Z - left.Z * right.Y,
            left.Z * right.X - left.X * right.Z,
            left.X * right.Y - left.Y * right.X);
        }
        #endregion

        #region Dot
        /// <summary>
        /// Calculate the dot (scalar) product of two vectors
        /// </summary>
        /// <param name="left">First operand</param>
        /// <param name="right">Second operand</param>
        /// <returns>The dot product of the two inputs</returns>
        public static float Dot(Vector3 left, Vector3 right)
        {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
        }

        /// <summary>
        /// Calculate the dot (scalar) product of two vectors
        /// </summary>
        /// <param name="left">First operand</param>
        /// <param name="right">Second operand</param>
        /// <param name="result">The dot product of the two inputs</param>
        public static void Dot(ref Vector3 left, ref Vector3 right, out float result)
        {
            result = left.X * right.X + left.Y * right.Y + left.Z * right.Z;
        }
        #endregion

        #region Transform
        /// <summary>Transform a Vector by the given Matrix</summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed vector</returns>
        public static Vector3 Transform(Vector3 vec, Matrix4 mat)
        {
            Vector3 result;
            Transform(ref vec, ref mat, out result);
            return result;
        }
        /// <summary>Transform a Vector by the given Matrix</summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <param name="result">The transformed vector</param>
        public static void Transform(ref Vector3 vec, ref Matrix4 mat, out Vector3 result)
        {
            Vector4 v4 = new Vector4(vec.X, vec.Y, vec.Z, 1.0f);
            Vector4.Transform(ref v4, ref mat, out v4);
            result = v4.Xyz;
        }
        /// <summary>Transform a direction vector by the given Matrix
        /// Assumes the matrix has a bottom row of (0,0,0,1), that is the translation part is ignored.
        /// </summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed vector</returns>
        public static Vector3 TransformVector(Vector3 vec, Matrix4 mat)
        {
            Vector3 v;
            v.X = Dot(vec, new Vector3(mat.Column0));
            v.Y = Dot(vec, new Vector3(mat.Column1));
            v.Z = Dot(vec, new Vector3(mat.Column2));
            return v;
        }
        /// <summary>Transform a Vector3 by the given Matrix, and project the resulting Vector4 back to a Vector3</summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed vector</returns>
        public static Vector3 TransformPerspective(Vector3 vec, Matrix4 mat)
        {
            Vector3 result;
            TransformPerspective(ref vec, ref mat, out result);
            return result;
        }
        /// <summary>Transform a Vector3 by the given Matrix, and project the resulting Vector4 back to a Vector3</summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <param name="result">The transformed vector</param>
        public static void TransformPerspective(ref Vector3 vec, ref Matrix4 mat, out Vector3 result)
        {
            Vector4 v = new Vector4(vec.X, vec.Y, vec.Z, 1);
            Vector4.Transform(ref v, ref mat, out v);
            result.X = v.X / v.W;
            result.Y = v.Y / v.W;
            result.Z = v.Z / v.W;
        }


        #endregion

        #region IEquatable<Vector3>

        /// <summary>Indicates whether the current vector is equal to another vector.</summary>
        /// <param name="other">A vector to compare with this vector.</param>
        /// <returns>true if the current vector is equal to the vector parameter; otherwise, false.</returns>
        public bool Equals(Vector3 other)
        {
            return
                X == other.X &&
                Y == other.Y &&
                Z == other.Z;
        }
        #endregion

        #region Equality
        /// <summary>
        /// Returns the hashcode for this instance.
        /// </summary>
        /// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Vector3))
                return false;

            return this.Equals((Vector3)obj);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Vector3 left, Vector3 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Vector3 left, Vector3 right)
        {
            return !left.Equals(right);
        }
        #endregion

        #region Overloaded ToString

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "" + X + "; " + Y + "; " + Z;
        }

        #endregion
    }
}
