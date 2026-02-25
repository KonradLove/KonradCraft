using OpenTK.Windowing.GraphicsLibraryFramework;

namespace EminaCraft
{
    internal class Keyboard
    {
        //Keys that are currently down
        private static List<Keys> keys = new List<Keys>();


        /// <summary>
        /// Check if a key is down
        /// </summary>
        /// <param name="key">The key to checking for</param>
        /// <returns>True if the key is down</returns>
        public static bool getKey(Keys key)
        {
            return keys.Contains(key);
        }
        public static bool getKeyDown(Keys key)
        {
            if (keys.Contains(key))
            {
                keys.Remove(key);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Call when a key has been pressed
        /// </summary>
        /// <param name="key">The key that has been pressed</param>
        public static void keyDown(Keys key)
        {
            if(!keys.Contains(key))
            {
                keys.Add(key);
            }
        }
        /// <summary>
        /// Call when a key has been released
        /// </summary>
        /// <param name="key">The key that has been released</param>
        public static void keyUp(Keys key)
        {
            keys.Remove(key);
        }
    }
}
