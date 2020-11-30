using System;

namespace Physics
{
    public class Vector2 : ICloneable
    {
        #region Fields

        private Vector3 x;
        private Vector3 y;

        #endregion

        #region Static Operators

        public static Vector2 operator +(Vector2 a, double b)
        {
            return new Vector2(
                a.X + b,
                a.Y + b);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(
                a.X + b.X,
                a.Y + b.Y);
        }
        public static Vector2 operator -(Vector2 a, double b)
        {
            return new Vector2(
                a.X - b,
                a.Y - b);
        }
        public static Vector2 operator *(Vector2 a, double b)
        {
            return new Vector2(
                a.X * b,
                a.Y * b);
        }
        public static Vector2 operator /(Vector2 a, double b)
        {
            return new Vector2(
                a.X / b,
                a.Y / b);
        }

        #endregion

        #region Properties

        public Vector3 X
        {
            get { return x; }
            set { x = value; }
        }

        public Vector3 Y
        {
            get { return y; }
            set { y = value; }
        }

        #endregion

        public Vector2(Vector3 x, Vector3 y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return "Position: " + x + "\nVelocity: " + y;
        }

        public object Clone()
        {
            return new Vector2(X, Y);
        }
    }
}
