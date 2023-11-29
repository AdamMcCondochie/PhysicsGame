using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;

namespace Framework.Inputs
{
    public enum GameInput
    {
        None,

        Switch,

        Attack,
        Jump,
        Dash,
        Crouch,

        Up,
        Down,
        Left,
        Right,

        Escape,
    }

    public class Controller
    {
        public void Update()
        {
            if (GamePad.GetState(0).IsConnected)
            {
                OldGamePad = NewGamePad;
                NewGamePad = GamePad.GetState(0);
            }
        }

        public static Dictionary<GameInput, Buttons> Mapping = new Dictionary<GameInput, Buttons>();
        public static GamePadState OldGamePad { get; protected set; }
        public static GamePadState NewGamePad { get; protected set; }


       public bool IsDown(Buttons button) => NewGamePad.IsButtonDown(button);

       public bool IsUp(Buttons button) => NewGamePad.IsButtonDown(button);

       public bool IsPressed(Buttons button) => OldGamePad.IsButtonUp(button) && NewGamePad.IsButtonDown(button);

       public bool IsReleased(Buttons button) => OldGamePad.IsButtonDown(button) && NewGamePad.IsButtonUp(button);



    }

    public class Input
    {
        public static Keyboard Keyboard { get; protected set; } = new Keyboard();

        public static Controller Controller { get; protected set; } = new Controller();

        public static Mouse Mouse { get; protected set; } = new Mouse();
        public static Touch Touch { get; protected set; } = new Touch();

        public Input()
        {
            Keyboard.SetKey(GameInput.Up, Keys.W);
            Keyboard.SetKey(GameInput.Left, Keys.A);
            Keyboard.SetKey(GameInput.Down, Keys.S);
            Keyboard.SetKey(GameInput.Right, Keys.D);

            Keyboard.SetKey(GameInput.Jump, Keys.Space);
            SetKey(GameInput.Jump, Buttons.A);

            Keyboard.SetKey(GameInput.Switch, Keys.E);
            SetKey(GameInput.Switch, Buttons.Y);

            Keyboard.SetKey(GameInput.Dash, Keys.LeftShift);
            SetKey(GameInput.Dash, Buttons.RightTrigger);

            Keyboard.SetKey(GameInput.Crouch, Keys.S);
            SetKey(GameInput.Crouch, Buttons.LeftStick);
        }

        public void GetState()
        {
            Keyboard?.GetState();
            Mouse?.GetState();
            Touch?.GetState();
        }

        public void SetKey(GameInput input, Buttons key) => Controller.Mapping.Add(input, key);

        public static bool IsDown(GameInput input)
        {
            Keys key = Keyboard.Mapping.TryGetValue(input, out key) ? key : default(Keys);
            MouseInput mouse = Mouse.Mapping.TryGetValue(input, out mouse) ? mouse : default(MouseInput);
            Buttons gamePadKey = Controller.Mapping.TryGetValue(input, out gamePadKey) ? gamePadKey : Buttons.BigButton;

            return
                Keyboard.IsKeyDown(key) ||
                Mouse.IsDown(MouseState.New, mouse) ||
                Controller.IsDown(gamePadKey);
        }

        public static bool IsUp(GameInput input)
        {
            Keys key = Keyboard.Mapping.TryGetValue(input, out key) ? key : default(Keys);
            MouseInput mouse = Mouse.Mapping.TryGetValue(input, out mouse) ? mouse : default(MouseInput);
            Buttons gamePadKey = Controller.Mapping.TryGetValue(input, out gamePadKey) ? gamePadKey : Buttons.BigButton;

            return Keyboard.IsKeyUp(key) || Mouse.IsUp(MouseState.New, mouse) || Controller.IsUp(gamePadKey);
        }

        public static bool IsPressed(GameInput input)
        {
            Keys key = Keyboard.Mapping.TryGetValue(input, out key) ? key : default(Keys);
            MouseInput mouse = Mouse.Mapping.TryGetValue(input, out mouse) ? mouse : default(MouseInput);
            Buttons gamePadKey = Controller.Mapping.TryGetValue(input, out gamePadKey) ? gamePadKey : default(Buttons);

            return Keyboard.IsKeyPressed(key) || Mouse.IsPressed(mouse) || Controller.IsPressed(gamePadKey);
        }

        public bool IsReleased(GameInput input)
        {
            Keys key = Keyboard.Mapping.TryGetValue(input, out key) ? key : default(Keys);
            return Keyboard.IsKeyReleased(key);
        }

        public static float GetXMovement()
        {
           // return (IsDown(GameInput.Left) || GamePad.GetState(0).ThumbSticks.Left.X < 0 ? -1 : 0)
           //     - (IsDown(GameInput.Right) || GamePad.GetState(0).ThumbSticks.Left.X > 0 ? -1 : 0);

            float xMovement = GamePad.GetState(0).ThumbSticks.Left.X;

            if (IsDown(GameInput.Left)) xMovement += -1;
            if (IsDown(GameInput.Right)) xMovement += 1;

            return xMovement;
        }

        public static float GetYMovement()
        {
            //return (IsDown(GameInput.Up) || GamePad.GetState(0).ThumbSticks.Left.Y > 0 ? -1 : 0)
           //    - (IsDown(GameInput.Down) || GamePad.GetState(0).ThumbSticks.Left.Y < 0 ? -1 : 0);

            float yMovement = -GamePad.GetState(0).ThumbSticks.Left.Y;

            if (IsDown(GameInput.Up)) yMovement += -1;
            if (IsDown(GameInput.Down)) yMovement += 1;

            return yMovement;
        }

        public static Keys CharToKey(char character)
        {
            switch (character)
            {
                case 'q': return Keys.Q;
                case 'w': return Keys.W;
                case 'e': return Keys.E;
                case 'r': return Keys.R;
                case 't': return Keys.T;
                case 'y': return Keys.Y;
                case 'u': return Keys.U;
                case 'i': return Keys.I;
                case 'o': return Keys.O;
                case 'p': return Keys.P;
                case 'a': return Keys.A;
                case 's': return Keys.S;
                case 'd': return Keys.D;
                case 'f': return Keys.F;
                case 'g': return Keys.G;
                case 'h': return Keys.H;
                case 'j': return Keys.J;
                case 'k': return Keys.K;
                case 'l': return Keys.L;
                case 'z': return Keys.Z;
                case 'x': return Keys.X;
                case 'c': return Keys.C;
                case 'v': return Keys.V;
                case 'b': return Keys.B;
                case 'n': return Keys.N;
                case 'm': return Keys.M;

                case ',': return Keys.OemComma;
                case '\'': return Keys.OemQuotes;
                case '/': return Keys.OemQuestion;
                case '.': return Keys.OemPeriod;
                case ' ': return Keys.Space;

                default:
                    return Keys.None;
            }
        }

        //
        //Source: https://roy-t.nl/2010/02/11/code-snippet-converting-keyboard-input-to-text-in-xna.html
        //
        double pauseTime;
        bool pressing;
        public bool TryConvertKeyboardInput(bool capLock, out char key)
        {
            Keys[] keys = Keyboard.New.GetPressedKeys();
            bool shift = Keyboard.New.IsKeyDown(Keys.LeftShift) || Keyboard.New.IsKeyDown(Keys.RightShift);
            if (capLock) shift = !shift;

            //if (keys.Length > 0 && IsPressAndHold(keys[0], gameTime, ref pauseTime, ref pressing))
            if (keys.Length > 0 && !Keyboard.Old.IsKeyDown(keys[0]))
            {

                switch (keys[0])
                {
                    //Alphabet keys
                    case Keys.A: if (shift) { key = 'A'; } else { key = 'a'; } return true;
                    case Keys.B: if (shift) { key = 'B'; } else { key = 'b'; } return true;
                    case Keys.C: if (shift) { key = 'C'; } else { key = 'c'; } return true;
                    case Keys.D: if (shift) { key = 'D'; } else { key = 'd'; } return true;
                    case Keys.E: if (shift) { key = 'E'; } else { key = 'e'; } return true;
                    case Keys.F: if (shift) { key = 'F'; } else { key = 'f'; } return true;
                    case Keys.G: if (shift) { key = 'G'; } else { key = 'g'; } return true;
                    case Keys.H: if (shift) { key = 'H'; } else { key = 'h'; } return true;
                    case Keys.I: if (shift) { key = 'I'; } else { key = 'i'; } return true;
                    case Keys.J: if (shift) { key = 'J'; } else { key = 'j'; } return true;
                    case Keys.K: if (shift) { key = 'K'; } else { key = 'k'; } return true;
                    case Keys.L: if (shift) { key = 'L'; } else { key = 'l'; } return true;
                    case Keys.M: if (shift) { key = 'M'; } else { key = 'm'; } return true;
                    case Keys.N: if (shift) { key = 'N'; } else { key = 'n'; } return true;
                    case Keys.O: if (shift) { key = 'O'; } else { key = 'o'; } return true;
                    case Keys.P: if (shift) { key = 'P'; } else { key = 'p'; } return true;
                    case Keys.Q: if (shift) { key = 'Q'; } else { key = 'q'; } return true;
                    case Keys.R: if (shift) { key = 'R'; } else { key = 'r'; } return true;
                    case Keys.S: if (shift) { key = 'S'; } else { key = 's'; } return true;
                    case Keys.T: if (shift) { key = 'T'; } else { key = 't'; } return true;
                    case Keys.U: if (shift) { key = 'U'; } else { key = 'u'; } return true;
                    case Keys.V: if (shift) { key = 'V'; } else { key = 'v'; } return true;
                    case Keys.W: if (shift) { key = 'W'; } else { key = 'w'; } return true;
                    case Keys.X: if (shift) { key = 'X'; } else { key = 'x'; } return true;
                    case Keys.Y: if (shift) { key = 'Y'; } else { key = 'y'; } return true;
                    case Keys.Z: if (shift) { key = 'Z'; } else { key = 'z'; } return true;

                    //Decimal keys
                    case Keys.D0: if (shift) { key = ')'; } else { key = '0'; } return true;
                    case Keys.D1: if (shift) { key = '!'; } else { key = '1'; } return true;
                    case Keys.D2: if (shift) { key = '@'; } else { key = '2'; } return true;
                    case Keys.D3: if (shift) { key = '#'; } else { key = '3'; } return true;
                    case Keys.D4: if (shift) { key = '$'; } else { key = '4'; } return true;
                    case Keys.D5: if (shift) { key = '%'; } else { key = '5'; } return true;
                    case Keys.D6: if (shift) { key = '^'; } else { key = '6'; } return true;
                    case Keys.D7: if (shift) { key = '&'; } else { key = '7'; } return true;
                    case Keys.D8: if (shift) { key = '*'; } else { key = '8'; } return true;
                    case Keys.D9: if (shift) { key = '('; } else { key = '9'; } return true;

                    //Decimal numpad keys
                    case Keys.NumPad0: key = '0'; return true;
                    case Keys.NumPad1: key = '1'; return true;
                    case Keys.NumPad2: key = '2'; return true;
                    case Keys.NumPad3: key = '3'; return true;
                    case Keys.NumPad4: key = '4'; return true;
                    case Keys.NumPad5: key = '5'; return true;
                    case Keys.NumPad6: key = '6'; return true;
                    case Keys.NumPad7: key = '7'; return true;
                    case Keys.NumPad8: key = '8'; return true;
                    case Keys.NumPad9: key = '9'; return true;

                    //Special keys
                    case Keys.OemTilde: if (shift) { key = '~'; } else { key = '`'; } return true;
                    case Keys.OemSemicolon: if (shift) { key = ':'; } else { key = ';'; } return true;
                    case Keys.OemQuotes: if (shift) { key = '"'; } else { key = '\''; } return true;
                    case Keys.OemQuestion: if (shift) { key = '?'; } else { key = '/'; } return true;
                    case Keys.OemPlus: if (shift) { key = '+'; } else { key = '='; } return true;
                    case Keys.OemPipe: if (shift) { key = '|'; } else { key = '\\'; } return true;
                    case Keys.OemPeriod: if (shift) { key = '>'; } else { key = '.'; } return true;
                    case Keys.OemOpenBrackets: if (shift) { key = '{'; } else { key = '['; } return true;
                    case Keys.OemCloseBrackets: if (shift) { key = '}'; } else { key = ']'; } return true;
                    case Keys.OemMinus: if (shift) { key = '_'; } else { key = '-'; } return true;
                    case Keys.OemComma: if (shift) { key = '<'; } else { key = ','; } return true;
                    case Keys.Space: key = ' '; return true;
                }
            }

            key = (char)0;
            return false;
        }
    }
}
