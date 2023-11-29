using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Framework.Inputs
{
    public enum MouseState
    {
        Old,
        New
    }

    public enum MouseInput
    {
        None,
        LeftButton,
        MiddleButton,
        RightButton,
        Button1,
        Button2
    }

    public class Mouse
    {
        public Dictionary<GameInput, MouseInput> Mapping = new Dictionary<GameInput, MouseInput>();
        public Microsoft.Xna.Framework.Input.MouseState Old { get; private set; }
        public Microsoft.Xna.Framework.Input.MouseState New { get; private set; }

        public void SetKey(GameInput input, MouseInput key) => Mapping.Add(input, key);

        public void GetState()
        {
            Old = New;
            New = Microsoft.Xna.Framework.Input.Mouse.GetState();
        }

        public void SetCursor(MouseCursor cursor)
        {
            Microsoft.Xna.Framework.Input.Mouse.SetCursor(cursor);
        }

        public bool IsUp(MouseState state, MouseInput input)
        {
            switch (input)
            {
                case MouseInput.LeftButton:
                    return (state == MouseState.Old ? Old : New).LeftButton == ButtonState.Released;
                case MouseInput.MiddleButton:
                    return (state == MouseState.Old ? Old : New).MiddleButton == ButtonState.Released;
                case MouseInput.RightButton:
                    return (state == MouseState.Old ? Old : New).RightButton == ButtonState.Released;
                case MouseInput.Button1:
                    return (state == MouseState.Old ? Old : New).XButton1 == ButtonState.Released;
                case MouseInput.Button2:
                    return (state == MouseState.Old ? Old : New).XButton2 == ButtonState.Released;
            }
            return false;
        }

        public bool IsDown(MouseState state, MouseInput input)
        {
            switch (input)
            {
                case MouseInput.LeftButton:
                    return (state == MouseState.Old ? Old : New).LeftButton == ButtonState.Pressed;
                case MouseInput.MiddleButton:
                    return (state == MouseState.Old ? Old : New).MiddleButton == ButtonState.Pressed;
                case MouseInput.RightButton:
                    return (state == MouseState.Old ? Old : New).RightButton == ButtonState.Pressed;
                case MouseInput.Button1:
                    return (state == MouseState.Old ? Old : New).XButton1 == ButtonState.Pressed;
                case MouseInput.Button2:
                    return (state == MouseState.Old ? Old : New).XButton2 == ButtonState.Pressed;
            }
            return false;
        }

        public bool IsReleased(MouseInput input) => IsDown(MouseState.Old, input) && IsUp(MouseState.New, input);
        public bool IsPressed(MouseInput input) => IsUp(MouseState.Old, input) && IsDown(MouseState.New, input);
    }
}
