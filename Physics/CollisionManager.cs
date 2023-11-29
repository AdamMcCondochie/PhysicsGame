using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

using C3.MonoGame;
using System;

namespace PhysicsGame.Physics
{
    public class CollisionManager
    {
        public List<CircleCollider2D> Colliders = new List<CircleCollider2D>();
        public List<Wall> Walls = new List<Wall>();
        public List<Capsule> Capsules = new List<Capsule>();

        public float Friction = 0.1f;

        public Capsule AddCapsule(Vector2 p1, Vector2 p2, float r)
        {
            var capsule = new Capsule(p1, p2, r);
            Capsules.Add(capsule);
            return capsule;
        }

        public Wall AddWall(Vector2 v1, Vector2 v2)
        {
            var wall = new Wall(v1, v2);
            Walls.Add(wall);
            return wall;
        }

        public CircleCollider2D Add(float x = 0, float y = 0, float radius = 20)
        {
            var collider = new CircleCollider2D();
            collider._body.Radius = radius;
            collider.Position = new Vector2(x, y);
            Colliders.Add(collider);
            return collider;
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Walls.Count; i++)
            {
                Walls[i].StartPos += Walls[i].Velocity;
                Walls[i].EndPos += Walls[i].Velocity;
                Walls[i].Rotation += Walls[i].RotationVelocity;

                Walls[i].UpdateTransform();

                Walls[i].Velocity *= 1f - Friction;
                Walls[i].RotationVelocity *= 1f - Friction;
            }

            for (int i = 0; i < Capsules.Count; i++)
            {
                Capsules[i].StartPoint += Capsules[i].Velocity;
                Capsules[i].EndPoint += Capsules[i].Velocity;
                Capsules[i].Rotation += Capsules[i].RotationVelocity;

                Capsules[i].UpdateTransform();

                Capsules[i].Velocity *= 1f - Friction;
                Capsules[i].RotationVelocity *= 1f - Friction;
            }

            for (int i = 0; i < Colliders.Count; i++)
            {
                Colliders[i].Velocity += Colliders[i].Acceleration;
                Colliders[i].Position += Colliders[i].Velocity;

                Colliders[i].Rotation += Colliders[i].RotationVelocity;

                Colliders[i].Update(gameTime);

                //Damping
                Colliders[i].Velocity *= 1f - Friction;
                Colliders[i].RotationVelocity *= 1f - Friction;
            }



            DetectCollisions();
        }

        public void DetectCollisions()
        {
            for (int i = 0; i < Colliders.Count; i++)
            {
                for (int j = i + 1; j < Colliders.Count; j++)
                {
                    for (int k = 0; k < Walls.Count; k++)
                    {
                        if (Walls[k].IsIntersecting(Colliders[i]))
                        {
                            Walls[k].ResolveIntersect(Colliders[i]);
                        }
                    }

                    if (Colliders[i]._body.Intersects(Colliders[j]._body))
                    {
                        //Collision Resolution
                        var c1 = Colliders[i];
                        var c2 = Colliders[j];

                        var dist = Colliders[i].Position - Colliders[j].Position;
                        var depth = c1._body.Radius + c2._body.Radius - dist.Length();

                        var pen_res = Vector2.Multiply(dist.Normalized(),
                            depth / (c1.Mass + c2.Mass));
                        c1.Position += Vector2.Multiply(pen_res, c1.Mass);
                        c2.Position += Vector2.Multiply(pen_res, -c2.Mass);

                        //Collision Response
                        var normal = (c1.Position - c2.Position).Normalized();
                        var relVel = c1.Velocity - c2.Velocity;
                        var sepVel = Vector2.Dot(relVel, normal);
                        var newSep = -sepVel * MathF.Min(c1.Elasticity, c2.Elasticity);

                        var vsepDiff = newSep - sepVel;
                        var impulse = vsepDiff / (c1.Mass + c2.Mass);
                        var impulseVec = Vector2.Multiply(normal, impulse);

                        c1.Velocity += Vector2.Multiply(impulseVec, c1.Mass);
                        c2.Velocity += Vector2.Multiply(impulseVec, -c2.Mass);
                    }
                }


            }

        }

        public void Draw(SpriteBatch spriteBatch, Texture2D CircleTex)
        {
            for (int i = 0; i < Colliders.Count; i++)
            {
                spriteBatch.Draw(
                    CircleTex,
                    Colliders[i].Position,
                    CircleTex.Bounds,
                    Color.White,
                    0,
                    CircleTex.Bounds.Center.ToVector2(),
                    Colliders[i]._body.Radius * 2 / 100f,
                    SpriteEffects.None,
                    0f);
            }

            for (int i = 0; i < Colliders.Count; i++)
            {
                spriteBatch.DrawLine(Colliders[i].Position, Colliders[i].Position + Colliders[i].Velocity * 20f, Color.Red, 3);
                spriteBatch.DrawLine(Colliders[i].Position, Colliders[i].Position + Colliders[i].Acceleration * 20f, Color.Green, 3);
                spriteBatch.DrawLine(Colliders[i].Position, Colliders[i].Position + Colliders[i].Acceleration.Normal() * 20f, Color.Green, 3);
            }

            for (int i = 0; i < Capsules.Count; i++)
            {
                Capsules[i].Draw(spriteBatch);
            }

            for (int i = 0; i < Walls.Count; i++)
            {
                Walls[i].Draw(spriteBatch);
            }
        }

        public void ResolveCollisions()
        {
        }
    }
}