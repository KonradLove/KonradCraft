using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EminaCraft
{
    internal interface IRenderable
    {
        void Render();
        void CleanUp();
        void Bind();
        void Unbind();
    }
}
