using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

using C3.MonoGame;

namespace PhysicsGame.Physics
{
    public class Rope
    {
        public List<RopeNode> Nodes;
        public float SegmentLength = 25f;

        public Rope(Vector2 pos, int length)
        {
            Nodes = new List<RopeNode>();
            for (int i = 0; i < length; i++)
            {
                Nodes.Add(new RopeNode()
                {
                    Position = new Vector2(pos.X + i * SegmentLength, pos.Y)
                });
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (i == 0)
                    continue;

                Nodes[i].Position += new Vector2(0, 10f);
            }

            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < Nodes.Count - 1; i++)
                {
                    var dist = Nodes[i].Position - Nodes[i + 1].Position;
                    var idealDist = dist.Normalized() * SegmentLength;
                    var d = (dist - idealDist) / 2f;

                    if (i == 1)
                    {
                        Nodes[i + 1].Position += d * 2;
                    }
                    else
                    {
                        Nodes[i + 1].Position += d;
                        Nodes[i].Position += -d;
                    }
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Nodes.Count - 1; i++)
            {
                spriteBatch.DrawLine(Nodes[i].Position, Nodes[i + 1].Position, Color.Blue, 60f);
            }
        }
    }
}