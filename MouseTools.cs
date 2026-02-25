using OpenTK.Mathematics;

namespace EminaCraft
{
    internal class MouseTools
    {
        public static Vector2 ScreenToWorld(Vector2 position)
        {
            Vector2 mousePos = position * new Vector2(2f / 1920f, 2f / 1080f);
            mousePos -= Vector2.One;
            mousePos.Y = -mousePos.Y;
            return mousePos;
        }
    }
}
