using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EminaCraft.gui
{
    internal class Fonts
    {
        public static Font? FONT_LARGE;
        public static Font? FONT_SMALL;

        public static void initFonts()
        {
            char[] characters1 = new char[47];
            for (int i = 0; i < 10; i++)
            {
                characters1[i + 1] = (char)('0' + i);
            }
            for (int i = 0; i < 26; i++)
            {
                characters1[i + 11] = (char)('A' + i);
            }
            characters1[37] = '?';
            characters1[38] = '\'';
            characters1[39] = '\"';
            characters1[40] = '@';
            characters1[41] = '$';
            characters1[42] = '^';
            characters1[43] = '>';
            characters1[44] = '<';
            characters1[45] = '*';
            characters1[46] = '+';
            FONT_LARGE = new Font(new Texture("gui/Letters", TextureMinFilter.Nearest), 47, characters1, 1f);
            char[] characters2 = new char[78];
            for (int i = 0; i < 26; i++)
            {
                characters2[i + 1] = (char)('A' + i);
            }
            for (int i = 0; i < 26; i++)
            {
                characters2[i + 27] = (char)('a' + i);
            }
            for (int i = 0; i < 10; i++)
            {
                characters2[i + 53] = (char)('0' + i);
            }

            characters2[0] = ' ';
            characters2[63] = '.';
            characters2[64] = ',';
            characters2[65] = ':';
            characters2[66] = '!';
            characters2[67] = '?';
            characters2[68] = '\'';
            characters2[69] = '~';
            characters2[70] = '-';
            characters2[71] = '*';
            characters2[72] = '|';
            characters2[73] = '&';
            characters2[74] = '(';
            characters2[75] = ')';
            characters2[76] = '“';
            characters2[77] = '”';
            FONT_LARGE = new Font(new Texture("gui/Letters", TextureMinFilter.Nearest), 47, characters1, 1f);
            FONT_SMALL = new Font(new Texture("gui/FontSmall", TextureMinFilter.Nearest), 78, characters2, 0.5f);
        }
    }
}
