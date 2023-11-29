using Microsoft.Xna.Framework;
using System;

namespace PhysicsGame
{
    public static class Vector2Extensions
    {
        public static Vector2 Normalized(this Vector2 vector)
        {
            float num = 1f / MathF.Sqrt((vector.X * vector.X) + (vector.Y * vector.Y));
            return new Vector2(vector.X * num, vector.Y * num);
        }

        public static Vector2 Normal(this Vector2 vector)
        { 
            return new Vector2(-vector.Y, vector.X).Normalized();
        }

        public static float Angle(Vector2 v1, Vector2 v2)
        {
            return MathF.Acos(Vector2.Dot(v1, v2) / (v1.Length() * v2.Length()));
        }
    }
}