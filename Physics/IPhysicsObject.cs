using Microsoft.Xna.Framework.Graphics;

namespace PhysicsGame.Physics
{
    public interface IPhysicsObject
    {
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}