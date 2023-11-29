using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Framework.Inputs
{
    public class Keyboard
    {
        public Dictionary<GameInput, Keys> Mapping = new Dictionary<GameInput, Keys>();

        public void SetKey(GameInput input, Keys key) => Mapping.Add(input, key);

        public KeyboardState[] Keys = new KeyboardState[10];
        public KeyboardState Old { get => Keys[1]; }
        public KeyboardState New { get => Keys[0]; }

        public void GetState()
        {
            for (int i = Keys.Length - 1; i > 0; i--)
            {
                Keys[i] = Keys[i - 1];
            }
            Keys[0] = Microsoft.Xna.Framework.Input.Keyboard.GetState();

            //New = Microsoft.Xna.Framework.Input.Keyboard.GetState();
        }

        public bool IsKeyUp(Keys key) => New.IsKeyUp(key);
        public bool WasKeyUp(Keys key) => Old.IsKeyUp(key);

        public bool IsKeyHeld(Keys key)
        {
            if (New.IsKeyDown(key))
            {

                return true;
            }
            else
                return false;
        }

        public bool IsKeyDown(Keys key) => New.IsKeyDown(key);
        public bool WasKeyDown(Keys key) => Old.IsKeyDown(key);

        public bool IsKeyReleased(Keys key) => WasKeyDown(key) && IsKeyUp(key);

        public bool IsKeyPressed(Keys key) => WasKeyUp(key) && IsKeyDown(key);

        public bool IsHoldingControl() => 
            IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl) ||
            IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl);


        protected Keys lastKey;
        protected float keyDownTime;

        public event Action<TextInputEventArgs> OnCharacterTyped = delegate { };

        public void TextInputHandler(object sender, TextInputEventArgs args)
        {
            OnCharacterTyped.Invoke(args);
        }
    }
}
