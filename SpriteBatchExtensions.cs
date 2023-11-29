using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using C3.MonoGame;

namespace PhysicsGame
{
    public static class SpriteBatchExtensions
    {
        public static void DrawVector(this SpriteBatch spriteBatch, Vector2 vector, Color color)
        {
            spriteBatch.DrawLine(Vector2.Zero, vector, color, 2f);
        }
    }
}