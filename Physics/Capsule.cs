using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using C3.MonoGame;
using System;

namespace PhysicsGame.Physics
{
    public class Capsule : PhysicsEntity
    {
        public float Radius { get; set; }
        public Vector2 StartPoint { get; set; }
        public Vector2 EndPoint { get; set; }

        private Vector2 _tstartPoint;
        private Vector2 _tendPoint;

        public Vector2 Center => (StartPoint + EndPoint) * 0.5f;
        public float Length => (EndPoint - StartPoint).Length();
        public Vector2 LineVector => EndPoint - StartPoint;

        public Capsule(Vector2 start, Vector2 end, float radius)
        {
            StartPoint = start;
            EndPoint = end;
            Radius = radius;
        }

        public bool IsIntersecting()
        {
            return false;
        }

        public override void UpdateTransform()
        {
            Matrix rot = Matrix.CreateRotationZ(Rotation * (MathF.PI / 180f));
            var lineVec = Vector2.Transform(LineVector.Normalized(), rot);
            _tstartPoint = Center + (lineVec * (-Length / 2f));
            _tendPoint = Center + (lineVec * (Length / 2f));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawLine(_tstartPoint, _tendPoint, Color.White, Radius * 2);

            spriteBatch.DrawCircle(_tstartPoint, Radius, 16, Color.White);
            spriteBatch.DrawCircle(_tendPoint, Radius, 16, Color.White);
        }
    }
}