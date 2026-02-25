
using System.Diagnostics;

namespace EminaCraft
{
    internal class Time
    {
        private static Stopwatch sw;
        public static float deltaTime;
        private static float lastTime;
        public static float time
        {
            get
            {
                return sw.ElapsedMilliseconds/1000f;
            }
        }
        public static double timeDouble
        {
            get
            {
                return sw.ElapsedTicks / 10000000.0;
            }
        }
        public static void init()
        {

            sw = new Stopwatch();
            sw.Start();
        }
        public static void update()
        {
            deltaTime = (sw.ElapsedMilliseconds/1000f) - lastTime;
            lastTime = sw.ElapsedMilliseconds/1000f;
        }
    }
}
