using Microsoft.Xna.Framework;

namespace PhysicsGame
{
    public struct Circle
    {
        public Vector2 Position { get; set; }
        public float Radius { get; set; }

        public Circle()
        {
            Position = Vector2.Zero;
            Radius = 40;
        }

        public bool Intersects(Circle other)
        {
            var r = Radius + other.Radius;
            r *= r;

            var distance = Vector2.DistanceSquared(Position, other.Position);
            return distance < r;
        }
    }
}