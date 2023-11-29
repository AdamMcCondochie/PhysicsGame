using Microsoft.Xna.Framework;
using System;

namespace Framework
{
    public class PixelPerfectCamera2D
    {
        public enum ScaleType
        {
            HEIGHT,
            WIDTH,
            NONE
        };

        public bool ScaleFromHeight { get; set; } = true; //Scales the screen based on the height, otherwise the width

        public Matrix Transform
        {
            get
            {
                return
                    Matrix.CreateTranslation(
                        -Position.X.RoundToNearest(1f / (_zoom * _xscale)),
                        -Position.Y.RoundToNearest(1f / (_zoom * _yscale)), 0) *

                    Matrix.CreateScale(_zoom * _xscale, _zoom * _yscale, 1f) *
                    Matrix.CreateTranslation((int)(WindowSize.X * Anchor.X), (int)(WindowSize.Y * Anchor.Y), 0);
            }
        }

        public Point WindowSize { get; set; }
        public Point? TargetResolution { get; set; }


        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)(X - (Size.X * Anchor.X)), (int)(Y - (Size.Y * Anchor.Y)), (int)Size.X, (int)Size.Y);
            }
        }


        public Vector2 Size { get { return new Vector2((WindowSize.X / (_xscale * Zoom)), WindowSize.Y / (_yscale * Zoom)); } }

        protected Vector2 _position;
        public Vector2 Position { get { return _position; } set { _position = value; } }

        protected float _zoom = 1f;
        protected float zoom = 1f;
        public float Zoom
        {
            get 
            {
                return _zoom; 
            }
            set
            {
                if (value < MinZoom)
                    value = MinZoom;
                if (value > MaxZoom)
                    value = MaxZoom;

                _zoom = value;
                //_zoom = value - _yscale;
            }
        }

        public float MinZoom { get; set; } = 0.1f;
        public float MaxZoom { get; set; } = 30;

        public float _xscale = 1;
        public float _yscale = 1;

        public float X { get { return Position.X; } set { _position.X = value; } }
        public float Y { get { return Position.Y; } set { _position.Y = value; } }

        public Vector2 Anchor { get; set; } = new Vector2(0.5f);

        public bool SmoothScaling { get; set; } = true;
        public bool PixelPerfect { get; set; } = true;

        public int scale = 1;

        public PixelPerfectCamera2D()
        {
        }

        public virtual Vector2 ScreenToWorld(Vector2 mouse)
        {
            return Vector2.Transform(new Vector2(mouse.X, mouse.Y), Matrix.Invert(Transform));
        }

        public virtual void Resize(int windowWidth, int windowHeight)
        {
            WindowSize = new Point(windowWidth, windowHeight);

            if (TargetResolution == null)
            {
                //_xscale = _yscale = WindowSize.Y/1080f;
                _xscale = _yscale = 1f;
                return;
            }

            if (SmoothScaling)
            {
                if (ScaleFromHeight)
                    _xscale = _yscale = (float)windowHeight / (TargetResolution.Value.Y * scale);
                else
                    _xscale = _yscale = (float)windowWidth / (TargetResolution.Value.X * scale);
            }
            else
            {
                if (ScaleFromHeight)
                    _xscale = _yscale = MathF.Round((float)windowHeight / TargetResolution.Value.Y);
                else
                    _xscale = _yscale = MathF.Round((float)windowWidth / TargetResolution.Value.X);
            }
        }
    }
}
