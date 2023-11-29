using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using C3.MonoGame;
using System;

namespace PhysicsGame.Physics
{
    public class Wall : PhysicsEntity
    {
        public Vector2 StartPos;
        public Vector2 EndPos;

        private Vector2 _tStartPos;
        private Vector2 _tEndPos;

        public float Length => (EndPos - StartPos).Length();
        public Vector2 LineVector => EndPos - StartPos;
        public Vector2 Center => (StartPos + EndPos) * 0.5f;  //StartPos + ((EndPos - StartPos) * 0.5f);

        public Wall(Vector2 v1, Vector2 v2)
        {
            StartPos = v1;
            EndPos = v2;
        }

        public bool IsIntersecting(CircleCollider2D other)
        {
            Vector2 ballToClosest = ClosestPointOnLine(other) - other.Position;
            if (ballToClosest.Length() <= other._body.Radius)
            {
                return true;
            }

            return false;
        }

        public override void UpdateTransform()
        {
            Matrix rot = Matrix.CreateRotationZ(Rotation * (MathF.PI / 180f));
            var lineVec = Vector2.Transform(LineVector.Normalized(), rot);
            _tStartPos = Center + (lineVec * (-Length / 2f));
            _tEndPos = Center + (lineVec * (Length / 2f));
        }

        public void ResolveIntersect(CircleCollider2D other)
        {
            //Collision Resolution
            var penVec = other.Position - ClosestPointOnLine(other);
            other.Position += Vector2.Multiply(penVec.Normalized(), other._body.Radius - penVec.Length());


            //Collision Response
            var normal = (other.Position - ClosestPointOnLine(other)).Normalized();
            var sepVel = Vector2.Dot(other.Velocity, normal);

            var newSepVel = -sepVel * other.Elasticity;
            var vsepDiff = sepVel - newSepVel;

            other.Velocity += normal * -vsepDiff;
        }

        public Vector2 ClosestPointOnLine(CircleCollider2D other)
        {
            var lineNorm = LineVector.Normalized();
            var distFromStart = _tStartPos - other.Position;
            if (Vector2.Dot(lineNorm, distFromStart) > 0) //Points in same direction
            {
                return _tStartPos;
            }

            var distFromEnd = other.Position - _tEndPos;
            if (Vector2.Dot(lineNorm, distFromEnd) > 0) //Points in opposite direction
            {
                return _tEndPos;
            }

            var lengthToClosestPoint = Vector2.Dot(lineNorm, distFromStart);
            var lengthVec = Vector2.Multiply(lineNorm, lengthToClosestPoint);
            return _tStartPos - lengthVec;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawLine(_tStartPos, _tEndPos, Color.White, 4f);
        }
    }
}