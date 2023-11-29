using Microsoft.Xna.Framework;
using System;

namespace PhysicsGame.Physics
{
    public abstract class PhysicsEntity : IPhysicsObject
    {
        public Vector2 Velocity { get; set; }
        public float Elasticity { get; set; } = 1f;

        public float Rotation { get; set; }
        public float RotationVelocity { get; set; }

        public Vector2 Forward => 
            Vector2.Transform(new Vector2(0, 1f), Matrix.CreateRotationZ(Rotation * (MathF.PI / 180f)));

        public abstract void UpdateTransform();
    }
}