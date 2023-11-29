using Microsoft.Xna.Framework;

namespace Framework
{
    public static class RoundingExtensions
    {
        public static Point RoundToNearest(this Point number, int nearestNumber)
        {
            return new Point(((int)(number.X / (float)nearestNumber)) * nearestNumber,
                            ((int)(number.Y / (float)nearestNumber)) * nearestNumber);
        }

        public static int RoundToNearest(this int number, int nearestNumber)
        {
            return ((int)(number / (float)nearestNumber)) * nearestNumber;
        }

        public static float RoundToNearest(this float number, float nearestNumber)
        {
            return ((int)(number / (float)nearestNumber)) * nearestNumber;
        }
    }
}
