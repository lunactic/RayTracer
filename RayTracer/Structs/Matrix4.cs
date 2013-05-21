using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Structs
{
    public struct Matrix4
    {
        #region Fields
        /// <summary>
        /// Top row of the matrix
        /// </summary>
        public Vector4 Row0;
        /// <summary>
        /// 2nd row of the matrix
        /// </summary>
        public Vector4 Row1;
        /// <summary>
        /// 3rd row of the matrix
        /// </summary>
        public Vector4 Row2;
        /// <summary>
        /// Bottom row of the matrix
        /// </summary>
        public Vector4 Row3;

        public Vector4 Translation { get { return Row3; } set { Row3 = value; } }

        #endregion

        /// <summary>
        /// The identity matrix
        /// </summary>
        public static Matrix4 Identity = new Matrix4(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);

        #region Properties
        /// <summary>
        /// Gets or sets the value at row 1, column 1 of this instance.
        /// </summary>
        public float M11 { get { return Row0.X; } set { Row0.X = value; } }
        /// <summary>
        /// Gets or sets the value at row 1, column 2 of this instance.
        /// </summary>
        public float M12 { get { return Row0.Y; } set { Row0.Y = value; } }
        /// <summary>
        /// Gets or sets the value at row 1, column 3 of this instance.
        /// </summary>
        public float M13 { get { return Row0.Z; } set { Row0.Z = value; } }
        /// <summary>
        /// Gets or sets the value at row 1, column 4 of this instance.
        /// </summary>
        public float M14 { get { return Row0.W; } set { Row0.W = value; } }
        /// <summary>
        /// Gets or sets the value at row 2, column 1 of this instance.
        /// </summary>
        public float M21 { get { return Row1.X; } set { Row1.X = value; } }
        /// <summary>
        /// Gets or sets the value at row 2, column 2 of this instance.
        /// </summary>
        public float M22 { get { return Row1.Y; } set { Row1.Y = value; } }
        /// <summary>
        /// Gets or sets the value at row 2, column 3 of this instance.
        /// </summary>
        public float M23 { get { return Row1.Z; } set { Row1.Z = value; } }
        /// <summary>
        /// Gets or sets the value at row 2, column 4 of this instance.
        /// </summary>
        public float M24 { get { return Row1.W; } set { Row1.W = value; } }
        /// <summary>
        /// Gets or sets the value at row 3, column 1 of this instance.
        /// </summary>
        public float M31 { get { return Row2.X; } set { Row2.X = value; } }
        /// <summary>
        /// Gets or sets the value at row 3, column 2 of this instance.
        /// </summary>
        public float M32 { get { return Row2.Y; } set { Row2.Y = value; } }
        /// <summary>
        /// Gets or sets the value at row 3, column 3 of this instance.
        /// </summary>
        public float M33 { get { return Row2.Z; } set { Row2.Z = value; } }
        /// <summary>
        /// Gets or sets the value at row 3, column 4 of this instance.
        /// </summary>
        public float M34 { get { return Row2.W; } set { Row2.W = value; } }
        /// <summary>
        /// Gets or sets the value at row 4, column 1 of this instance.
        /// </summary>
        public float M41 { get { return Row3.X; } set { Row3.X = value; } }
        /// <summary>
        /// Gets or sets the value at row 4, column 2 of this instance.
        /// </summary>
        public float M42 { get { return Row3.Y; } set { Row3.Y = value; } }
        /// <summary>
        /// Gets or sets the value at row 4, column 3 of this instance.
        /// </summary>
        public float M43 { get { return Row3.Z; } set { Row3.Z = value; } }
        /// <summary>
        /// Gets or sets the value at row 4, column 4 of this instance.
        /// </summary>
        public float M44 { get { return Row3.W; } set { Row3.W = value; } }


        /// <summary>
        /// The first column of this matrix
        /// </summary>
        public Vector4 Column0
        {
            get { return new Vector4(Row0.X, Row1.X, Row2.X, Row3.X); }
            set { Row0.X = value.X; Row1.X = value.X; Row2.X = value.X; Row3.X = value.X; }
        }

        /// <summary>
        /// The second column of this matrix
        /// </summary>
        public Vector4 Column1
        {
            get { return new Vector4(Row0.Y, Row1.Y, Row2.Y, Row3.Y); }
            set { Row0.Y = value.Y; Row1.Y = value.Y; Row2.Y = value.Y; Row3.Y = value.Y; }
        }

        /// <summary>
        /// The third column of this matrix
        /// </summary>
        public Vector4 Column2
        {
            get { return new Vector4(Row0.Z, Row1.Z, Row2.Z, Row3.Z); }
            set { Row0.Z = value.Z; Row1.Z = value.Z; Row2.Z = value.Z; Row3.X = value.Z; }
        }

        /// <summary>
        /// The fourth column of this matrix
        /// </summary>
        public Vector4 Column3
        {
            get { return new Vector4(Row0.W, Row1.W, Row2.W, Row3.W); }
            set { Row0.W = value.W; Row1.W = value.W; Row2.W = value.W; Row3.W = value.W; }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="row0">Top row of the matrix</param>
        /// <param name="row1">Second row of the matrix</param>
        /// <param name="row2">Third row of the matrix</param>
        /// <param name="row3">Bottom row of the matrix</param>
        public Matrix4(Vector4 row0, Vector4 row1, Vector4 row2, Vector4 row3)
        {
            Row0 = row0;
            Row1 = row1;
            Row2 = row2;
            Row3 = row3;
        }

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="m00">First item of the first row of the matrix.</param>
        /// <param name="m01">Second item of the first row of the matrix.</param>
        /// <param name="m02">Third item of the first row of the matrix.</param>
        /// <param name="m03">Fourth item of the first row of the matrix.</param>
        /// <param name="m10">First item of the second row of the matrix.</param>
        /// <param name="m11">Second item of the second row of the matrix.</param>
        /// <param name="m12">Third item of the second row of the matrix.</param>
        /// <param name="m13">Fourth item of the second row of the matrix.</param>
        /// <param name="m20">First item of the third row of the matrix.</param>
        /// <param name="m21">Second item of the third row of the matrix.</param>
        /// <param name="m22">Third item of the third row of the matrix.</param>
        /// <param name="m23">First item of the third row of the matrix.</param>
        /// <param name="m30">Fourth item of the fourth row of the matrix.</param>
        /// <param name="m31">Second item of the fourth row of the matrix.</param>
        /// <param name="m32">Third item of the fourth row of the matrix.</param>
        /// <param name="m33">Fourth item of the fourth row of the matrix.</param>
        public Matrix4(float m00, float m01, float m02, float m03,
                float m10, float m11, float m12, float m13,
                float m20, float m21, float m22, float m23,
                float m30, float m31, float m32, float m33)
        {
            Row0 = new Vector4(m00, m01, m02, m03);
            Row1 = new Vector4(m10, m11, m12, m13);
            Row2 = new Vector4(m20, m21, m22, m23);
            Row3 = new Vector4(m30, m31, m32, m33);
        }
        #endregion

        #region Conversion
        /// <summary>
        /// Performs an implicit conversion from <see cref="Ch.Fhnw.ViewerComponentLibrary.Structs.Matrix4"/> to <see cref="System.Single[]"/>.
        /// </summary>
        /// <param name="m">The m.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator float[](Matrix4 m)
        {
            return new float[]
                       {
                           m.M11, m.M12, m.M13, m.M14, m.M21, m.M22, m.M23, m.M24, m.M31, m.M32, m.M33, m.M34, m.M41, m.M42, m.M43, m.M44
                       };
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Single[]"/> to <see cref="Ch.Fhnw.ViewerComponentLibrary.Structs.Matrix4"/>.
        /// </summary>
        /// <param name="arr">The arr.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Matrix4(float[] arr)
        {
            return new Matrix4(arr[0], arr[1], arr[2], arr[3],
                                arr[4], arr[5], arr[6], arr[7],
                                arr[8], arr[9], arr[10], arr[11],
                                arr[12], arr[13], arr[14], arr[15]);
        }
        #endregion

        #region Multiply Functions
        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The result of the operator.</returns>
        public static Matrix4 operator *(Matrix4 left, Matrix4 right)
        {
            return Matrix4.Mult(left, right);
        }

        public static Vector3 operator *(Matrix4 left, Vector3 right)
        {
            float w = left.M41 * right.X + left.M42 * right.Y + left.M43 * right.Z + left.M44;
            return new Vector3((left.M11 * right.X + left.M12 * right.Y + left.M13 * right.Z + left.M14) / w,
                               (left.M21 * right.X + left.M22 * right.Y + left.M23 * right.Z + left.M24) / w,
                               (left.M31 * right.X + left.M32 * right.Y + left.M33 * right.Z + left.M34) / w);
        }


        /// <summary>
        /// Multiplies two instances.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <returns>A new instance that is the result of the multiplication</returns>
        public static Matrix4 Mult(Matrix4 left, Matrix4 right)
        {
            Matrix4 result;
            Mult(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Multiplies two instances.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <param name="result">A new instance that is the result of the multiplication</param>
        public static void Mult(ref Matrix4 left, ref Matrix4 right, out Matrix4 result)
        {
            float lM11 = left.Row0.X, lM12 = left.Row0.Y, lM13 = left.Row0.Z, lM14 = left.Row0.W,
            lM21 = left.Row1.X, lM22 = left.Row1.Y, lM23 = left.Row1.Z, lM24 = left.Row1.W,
            lM31 = left.Row2.X, lM32 = left.Row2.Y, lM33 = left.Row2.Z, lM34 = left.Row2.W,
            lM41 = left.Row3.X, lM42 = left.Row3.Y, lM43 = left.Row3.Z, lM44 = left.Row3.W,
            rM11 = right.Row0.X, rM12 = right.Row0.Y, rM13 = right.Row0.Z, rM14 = right.Row0.W,
            rM21 = right.Row1.X, rM22 = right.Row1.Y, rM23 = right.Row1.Z, rM24 = right.Row1.W,
            rM31 = right.Row2.X, rM32 = right.Row2.Y, rM33 = right.Row2.Z, rM34 = right.Row2.W,
            rM41 = right.Row3.X, rM42 = right.Row3.Y, rM43 = right.Row3.Z, rM44 = right.Row3.W;

            result.Row0.X = (((lM11 * rM11) + (lM12 * rM21)) + (lM13 * rM31)) + (lM14 * rM41);
            result.Row0.Y = (((lM11 * rM12) + (lM12 * rM22)) + (lM13 * rM32)) + (lM14 * rM42);
            result.Row0.Z = (((lM11 * rM13) + (lM12 * rM23)) + (lM13 * rM33)) + (lM14 * rM43);
            result.Row0.W = (((lM11 * rM14) + (lM12 * rM24)) + (lM13 * rM34)) + (lM14 * rM44);
            result.Row1.X = (((lM21 * rM11) + (lM22 * rM21)) + (lM23 * rM31)) + (lM24 * rM41);
            result.Row1.Y = (((lM21 * rM12) + (lM22 * rM22)) + (lM23 * rM32)) + (lM24 * rM42);
            result.Row1.Z = (((lM21 * rM13) + (lM22 * rM23)) + (lM23 * rM33)) + (lM24 * rM43);
            result.Row1.W = (((lM21 * rM14) + (lM22 * rM24)) + (lM23 * rM34)) + (lM24 * rM44);
            result.Row2.X = (((lM31 * rM11) + (lM32 * rM21)) + (lM33 * rM31)) + (lM34 * rM41);
            result.Row2.Y = (((lM31 * rM12) + (lM32 * rM22)) + (lM33 * rM32)) + (lM34 * rM42);
            result.Row2.Z = (((lM31 * rM13) + (lM32 * rM23)) + (lM33 * rM33)) + (lM34 * rM43);
            result.Row2.W = (((lM31 * rM14) + (lM32 * rM24)) + (lM33 * rM34)) + (lM34 * rM44);
            result.Row3.X = (((lM41 * rM11) + (lM42 * rM21)) + (lM43 * rM31)) + (lM44 * rM41);
            result.Row3.Y = (((lM41 * rM12) + (lM42 * rM22)) + (lM43 * rM32)) + (lM44 * rM42);
            result.Row3.Z = (((lM41 * rM13) + (lM42 * rM23)) + (lM43 * rM33)) + (lM44 * rM43);
            result.Row3.W = (((lM41 * rM14) + (lM42 * rM24)) + (lM43 * rM34)) + (lM44 * rM44);
        }
        #endregion

        #region Determinant
        public float Determinant
        {
            get
            {
                return
                    Row0.X * Row1.Y * Row2.Z * Row3.W - Row0.X * Row1.Y * Row2.W * Row3.Z + Row0.X * Row1.Z * Row2.W * Row3.Y - Row0.X * Row1.Z * Row2.Y * Row3.W
                  + Row0.X * Row1.W * Row2.Y * Row3.Z - Row0.X * Row1.W * Row2.Z * Row3.Y - Row0.Y * Row1.Z * Row2.W * Row3.X + Row0.Y * Row1.Z * Row2.X * Row3.W
                  - Row0.Y * Row1.W * Row2.X * Row3.Z + Row0.Y * Row1.W * Row2.Z * Row3.X - Row0.Y * Row1.X * Row2.Z * Row3.W + Row0.Y * Row1.X * Row2.W * Row3.Z
                  + Row0.Z * Row1.W * Row2.X * Row3.Y - Row0.Z * Row1.W * Row2.Y * Row3.X + Row0.Z * Row1.X * Row2.Y * Row3.W - Row0.Z * Row1.X * Row2.W * Row3.Y
                  + Row0.Z * Row1.Y * Row2.W * Row3.X - Row0.Z * Row1.Y * Row2.X * Row3.W - Row0.W * Row1.X * Row2.Y * Row3.Z + Row0.W * Row1.X * Row2.Z * Row3.Y
                  - Row0.W * Row1.Y * Row2.Z * Row3.X + Row0.W * Row1.Y * Row2.X * Row3.Z - Row0.W * Row1.Z * Row2.X * Row3.Y + Row0.W * Row1.Z * Row2.Y * Row3.X;
            }
        }
        #endregion

        #region CreateRotation[XYZ]

        /// <summary>
        /// Builds a rotation matrix for a rotation around the x-axis.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <param name="result">The resulting Matrix4 instance.</param>
        public static void CreateRotationX(float angle, out Matrix4 result)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);

            result.Row0 = Vector4.UnitX;
            result.Row1 = new Vector4(0.0f, cos, sin, 0.0f);
            result.Row2 = new Vector4(0.0f, -sin, cos, 0.0f);
            result.Row3 = Vector4.UnitW;
        }

        /// <summary>
        /// Builds a rotation matrix for a rotation around the x-axis.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <returns>The resulting Matrix4 instance.</returns>
        public static Matrix4 CreateRotationX(float angle)
        {
            Matrix4 result;
            CreateRotationX(angle, out result);
            return result;
        }

        /// <summary>
        /// Builds a rotation matrix for a rotation around the y-axis.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <param name="result">The resulting Matrix4 instance.</param>
        public static void CreateRotationY(float angle, out Matrix4 result)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);

            result.Row0 = new Vector4(cos, 0.0f, -sin, 0.0f);
            result.Row1 = Vector4.UnitY;
            result.Row2 = new Vector4(sin, 0.0f, cos, 0.0f);
            result.Row3 = Vector4.UnitW;
        }

        /// <summary>
        /// Builds a rotation matrix for a rotation around the y-axis.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <returns>The resulting Matrix4 instance.</returns>
        public static Matrix4 CreateRotationY(float angle)
        {
            Matrix4 result;
            CreateRotationY(angle, out result);
            return result;
        }

        /// <summary>
        /// Builds a rotation matrix for a rotation around the z-axis.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <param name="result">The resulting Matrix4 instance.</param>
        public static void CreateRotationZ(float angle, out Matrix4 result)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);

            result.Row0 = new Vector4(cos, sin, 0.0f, 0.0f);
            result.Row1 = new Vector4(-sin, cos, 0.0f, 0.0f);
            result.Row2 = Vector4.UnitZ;
            result.Row3 = Vector4.UnitW;
        }

        /// <summary>
        /// Builds a rotation matrix for a rotation around the z-axis.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <returns>The resulting Matrix4 instance.</returns>
        public static Matrix4 CreateRotationZ(float angle)
        {
            Matrix4 result;
            CreateRotationZ(angle, out result);
            return result;
        }

        #endregion

        #region Transformation
        /// <summary>
        /// Scales the Matrix with the given vector
        /// </summary>
        /// <param name="scaleVec">The scaling vector.</param>
        /// <returns></returns>
        public static Matrix4 CreateScaling(Vector3 scaleVec)
        {
            Matrix4 result;
            result.Row0 = Vector4.UnitX * scaleVec.X;
            result.Row1 = Vector4.UnitY * scaleVec.Y;
            result.Row2 = Vector4.UnitZ * scaleVec.Z;
            result.Row3 = Vector4.UnitW;
            return result;
        }

        public static Matrix4 Scale(float scale)
        {
            return CreateScaling(new Vector3(scale, scale, scale));
        }


        /// <summary>
        /// Creates a translation matrix.
        /// </summary>
        /// <param name="vector">The translation vector.</param>
        /// <param name="result">The resulting Matrix4 instance.</param>
        public static void CreateTranslation(ref Vector3 vector, out Matrix4 result)
        {
            result = Identity;
            result.Row3 = new Vector4(vector.X, vector.Y, vector.Z, 1);
        }
        /// <summary>
        /// Creates a translation matrix.
        /// </summary>
        /// <param name="vector">The translation vector.</param>
        /// <returns>The resulting Matrix4 instance.</returns>
        public static Matrix4 CreateTranslation(Vector3 vector)
        {
            Matrix4 result;
            CreateTranslation(ref vector, out result);
            return result;
        }

        public Vector4 Transform(Vector4 vec)
        {

            Vector4 res;

            float x, y, z;
            x = M11 * vec.X + M12 * vec.Y + M13 * vec.Z + M14 * vec.W;
            y = M21 * vec.X + M22 * vec.Y + M23 * vec.Z + M24 * vec.W;
            z = M31 * vec.X + M32 * vec.Y + M33 * vec.Z + M34 * vec.W;
            res.W = M41 * vec.X + M42 * vec.Y + M43 * vec.Z + M44 * vec.W;
            res.X = x;
            res.Y = y;
            res.Z = z;
            return res;
        }

        #endregion

        #region Transpose
        /// <summary>
        /// Calculate the transpose of the given matrix
        /// </summary>
        /// <param name="mat">The matrix to transpose</param>
        /// <returns>The transpose of the given matrix</returns>
        public static Matrix4 Transpose(Matrix4 mat)
        {
            return new Matrix4(mat.Column0, mat.Column1, mat.Column2, mat.Column3);
        }
        #endregion

        #region Ortho
        /// <summary>
        /// Creates an orthographic projection matrix.
        /// </summary>
        /// <param name="left">The left edge of the projection volume.</param>
        /// <param name="right">The right edge of the projection volume.</param>
        /// <param name="bottom">The bottom edge of the projection volume.</param>
        /// <param name="top">The top edge of the projection volume.</param>
        /// <param name="zNear">The near edge of the projection volume.</param>
        /// <param name="zFar">The far edge of the projection volume.</param>
        /// <param name="result">The resulting Matrix4 instance.</param>
        public static void CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNear, float zFar, out Matrix4 result)
        {
            result = new Matrix4(0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f);

            float invRL = 1 / (right - left);
            float invTB = 1 / (top - bottom);
            float invFN = 1 / (zFar - zNear);

            result.M11 = 2 * invRL;
            result.M22 = 2 * invTB;
            result.M33 = -2 * invFN;

            result.M41 = -(right + left) * invRL;
            result.M42 = -(top + bottom) * invTB;
            result.M43 = -(zFar + zNear) * invFN;
            result.M44 = 1;
        }
        /// <summary>
        /// Creates an orthographic projection matrix.
        /// </summary>
        /// <param name="left">The left edge of the projection volume.</param>
        /// <param name="right">The right edge of the projection volume.</param>
        /// <param name="bottom">The bottom edge of the projection volume.</param>
        /// <param name="top">The top edge of the projection volume.</param>
        /// <param name="zNear">The near edge of the projection volume.</param>
        /// <param name="zFar">The far edge of the projection volume.</param>
        /// <returns>The resulting Matrix4 instance.</returns>
        public static Matrix4 CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNear, float zFar)
        {
            Matrix4 result;
            CreateOrthographicOffCenter(left, right, bottom, top, zNear, zFar, out result);
            return result;
        }
        #endregion

        #region Invert
        /// <summary>
        /// Calculate the inverse of the given matrix
        /// </summary>
        /// <param name="mat">The matrix to invert</param>
        /// <returns>The inverse of the given matrix if it has one, or the input if it is singular</returns>
        /// <exception cref="InvalidOperationException">Thrown if the Matrix4 is singular.</exception>
        public static Matrix4 Invert(Matrix4 mat)
        {
            int[] colIdx = { 0, 0, 0, 0 };
            int[] rowIdx = { 0, 0, 0, 0 };
            int[] pivotIdx = { -1, -1, -1, -1 };

            // convert the matrix to an array for easy looping
            float[,] inverse = {
                                   {mat.Row0.X, mat.Row0.Y, mat.Row0.Z, mat.Row0.W},
                                   {mat.Row1.X, mat.Row1.Y, mat.Row1.Z, mat.Row1.W},
                                   {mat.Row2.X, mat.Row2.Y, mat.Row2.Z, mat.Row2.W},
                                   {mat.Row3.X, mat.Row3.Y, mat.Row3.Z, mat.Row3.W}
                               };
            int icol = 0;
            int irow = 0;
            for (int i = 0; i < 4; i++)
            {
                // Find the largest pivot value
                float maxPivot = 0.0f;
                for (int j = 0; j < 4; j++)
                {
                    if (pivotIdx[j] != 0)
                    {
                        for (int k = 0; k < 4; ++k)
                        {
                            if (pivotIdx[k] == -1)
                            {
                                float absVal = System.Math.Abs(inverse[j, k]);
                                if (absVal > maxPivot)
                                {
                                    maxPivot = absVal;
                                    irow = j;
                                    icol = k;
                                }
                            }
                            else if (pivotIdx[k] > 0)
                            {
                                return mat;
                            }
                        }
                    }
                }
                ++(pivotIdx[icol]);

                // Swap rows over so pivot is on diagonal
                if (irow != icol)
                {
                    for (int k = 0; k < 4; ++k)
                    {
                        float f = inverse[irow, k];
                        inverse[irow, k] = inverse[icol, k];
                        inverse[icol, k] = f;
                    }
                }

                rowIdx[i] = irow;
                colIdx[i] = icol;

                float pivot = inverse[icol, icol];
                // check for singular matrix
                if (pivot == 0.0f)
                {
                    throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
                    //return mat;
                }

                // Scale row so it has a unit diagonal
                float oneOverPivot = 1.0f / pivot;
                inverse[icol, icol] = 1.0f;
                for (int k = 0; k < 4; ++k)
                    inverse[icol, k] *= oneOverPivot;

                // Do elimination of non-diagonal elements
                for (int j = 0; j < 4; ++j)
                {
                    // check this isn't on the diagonal
                    if (icol != j)
                    {
                        float f = inverse[j, icol];
                        inverse[j, icol] = 0.0f;
                        for (int k = 0; k < 4; ++k)
                            inverse[j, k] -= inverse[icol, k] * f;
                    }
                }
            }
            for (int j = 3; j >= 0; --j)
            {
                int ir = rowIdx[j];
                int ic = colIdx[j];
                for (int k = 0; k < 4; ++k)
                {
                    float f = inverse[k, ir];
                    inverse[k, ir] = inverse[k, ic];
                    inverse[k, ic] = f;
                }
            }

            mat.Row0 = new Vector4(inverse[0, 0], inverse[0, 1], inverse[0, 2], inverse[0, 3]);
            mat.Row1 = new Vector4(inverse[1, 0], inverse[1, 1], inverse[1, 2], inverse[1, 3]);
            mat.Row2 = new Vector4(inverse[2, 0], inverse[2, 1], inverse[2, 2], inverse[2, 3]);
            mat.Row3 = new Vector4(inverse[3, 0], inverse[3, 1], inverse[3, 2], inverse[3, 3]);
            return mat;
        }
        #endregion
    }
}
