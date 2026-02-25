using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EminaCraft.shaders
{
    class CursorShader : EntityShader
    {
        private UniformFloat time;
        public CursorShader(Camera cam) : base(cam, "default", "cursor")
        {
            time = new UniformFloat("time", shaderHandle);
        }
        public override void uploadUniforms()
        {
            time.upload(Time.time);
            base.uploadUniforms();
        }
    }
}
