using Microsoft.Xna.Framework.Input.Touch;

namespace Framework.Inputs
{
    public class Touch
    {
        public TouchCollection Old { get;  set; }
        public TouchCollection New { get;  set; }

        public Touch()
        {

        }

        public void GetState()
        {
            Old = New;
            New = TouchPanel.GetState();
        }

        public bool IsDown()
        {
            return New[0].State == TouchLocationState.Pressed;
        }
    }
}
