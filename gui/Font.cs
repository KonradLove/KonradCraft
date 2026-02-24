using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonradCraft.gui
{
    internal class Font
    {
        public Font(Texture tex, int characters, char[] charMap, float width)
        {
            texture = tex;
            this.characters = characters;
            this.charMap = charMap;
            this.width = width;
        }

        public Texture texture;
        public int characters;
        public char[] charMap;
        public float width;

        public int fromChar(char c)
        {
            if (c == '\n') return -1;
            for (int i = 0; i < charMap.Length; i++)
            {
                if (charMap[i] == c) return i;
            }
            return 0;
        }
    }
}
