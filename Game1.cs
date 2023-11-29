using Framework;
using Framework.Inputs;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PhysicsGame.Physics;
using System;

namespace PhysicsGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Input input;

        public CollisionManager CollisionManager;

        public PixelPerfectCamera2D Camera;

        Texture2D CircleTex;
        CircleCollider2D Player;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += ClientSizeChanged;
        }

        private void ClientSizeChanged(object sender, EventArgs e)
        {
            Camera.Resize(
                Window.ClientBounds.Width, 
                Window.ClientBounds.Height);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            input = new Input();

            CircleTex = createCircleText(100f);

            Camera = new PixelPerfectCamera2D();
            Camera.TargetResolution = new Point(0, 1000);
            Camera.Zoom = 1f;
            Camera.Resize(Window.ClientBounds.Width, Window.ClientBounds.Height);

            CollisionManager = new CollisionManager();
            Player = CollisionManager.Add(-100,-100);

            //CollisionManager.AddCapsule(Vector2.Zero, new Vector2(100, 0), 20f);

            Random ran = new Random();
            var playArea = 800;
            var minRadius = 10;
            var maxRadius = 60;

            CreateBorder(
                new Vector2(-playArea, -playArea),
                new Vector2(playArea, -playArea),
                new Vector2(playArea, playArea),
                new Vector2(-playArea, playArea)
                );

            //Balls
            for (int i = 0; i < 50; i++)
            {
                CollisionManager.Add(
                    ran.Next(-playArea + maxRadius, playArea - maxRadius),
                    ran.Next(-playArea + maxRadius, playArea - maxRadius),
                    ran.Next(minRadius, maxRadius));
            }

            //Walls
            for (int i = 0; i < 3; i++)
            {
                CollisionManager.AddWall(
                    new Vector2(ran.Next(-playArea, playArea), ran.Next(-playArea, playArea)),
                    new Vector2(ran.Next(-playArea, playArea), ran.Next(-playArea, playArea)));
            }
        }


        protected override void Update(GameTime gameTime)
        {
            input.GetState();

            var move = Vector2.Zero;
            move = new Vector2(Input.GetXMovement(), Input.GetYMovement());

            //CollisionManager.Capsules[0].RotationVelocity += Input.GetXMovement();
            //CollisionManager.Capsules[0].Velocity = CollisionManager.Capsules[0].Forward * Input.GetYMovement();

            if (move != Vector2.Zero)
            {
                move.Normalize();
                Player.Acceleration = move * 2f;
            }
            else
                Player.Acceleration = Vector2.Zero;

            if (Input.Keyboard.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Space))
                Player.Acceleration = Player.Forward * 50f;

            CollisionManager.Update(gameTime);

            var d = Camera.Position - Player.Position;
            Camera.Position -= d / 16f;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(transformMatrix: Camera.Transform);

            CollisionManager.Draw(_spriteBatch, CircleTex);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void CreateBorder(params Vector2[] points)
        {
            if (points.Length < 3)
                return;

            for (int i = 0; i < points.Length - 1; i++)
            {
                CollisionManager.AddWall(points[i], points[i + 1]);
            }

            CollisionManager.AddWall(points[points.Length-1], points[0]);
        }


        Texture2D createCircleText(float diameter)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, (int)diameter, (int)diameter);
            Color[] colorData = new Color[(int)(diameter * diameter)];

            float radius = diameter / 2f;
            float radiussqr = radius * radius;

            for (int x = 0; x < diameter; x++)
            {
                for (int y = 0; y < diameter; y++)
                {
                    int index = (int)(x * diameter + y);
                    Vector2 pos = new Vector2(x - radius, y - radius);
                    if (pos.LengthSquared() <= radiussqr)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }
    }
}