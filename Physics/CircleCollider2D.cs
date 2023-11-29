using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysicsGame.Physics
{
    public class CircleCollider2D : PhysicsEntity
    {
        public bool IsFixed = false;
        public float Mass
        {
            get => IsFixed ? 0 : 1f / _body.Radius;
        }

        public Vector2 Gravity = Vector2.One;

        private Vector2 _lastAcceleration;
        private Vector2 _acceleration;
        public Vector2 Acceleration
        {
            get => _acceleration;
            set
            {
                if (value != Vector2.Zero)
                    _lastAcceleration = value;
                _acceleration = value;
            }
        }


        public Vector2 Down
        {
            get => Gravity.Normalized();
        }

        public Vector2 Right
        {
            get => Down.Normal();
        }


        public Circle _body;
        public Vector2 Position { get => _body.Position; set => _body.Position = value; }

        public float RadianRotation = 0;


        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public override void UpdateTransform()
        {
        }
    }
}