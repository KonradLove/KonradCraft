using OpenTK.Mathematics;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonradCraft
{
    internal class SkyboxShader : ShaderProgram
    {
        UniformMatrix4 projectionU;
        UniformMatrix4 viewU;
        UniformFloat sizeU;
        UniformSampler samplerU;
        UniformVec4 colorU;
        Camera cam;
        public SkyboxShader(Camera cam) : base("skybox")
        {
            this.cam = cam;
            projectionU = new UniformMatrix4("projection", shaderHandle);
            viewU = new UniformMatrix4("view", shaderHandle);
            sizeU = new UniformFloat("size", shaderHandle);
            colorU = new UniformVec4("color", shaderHandle);
            samplerU = new UniformSampler("texSampler", shaderHandle);
        }
        public Skybox? currentSkybox;
        bool firstTime = true;
        public override void uploadUniforms()
        {
            if (currentSkybox == null) return;

            colorU.upload((Vector4)currentSkybox.color);
            samplerU.upload(0);
            viewU.upload(cam.getUntranslatedViewMatrix());
            sizeU.upload(currentSkybox.size);
            if(cam.dirty || firstTime)
            {
                firstTime = false;
                projectionU.upload(cam.generateProjectionMatrix());
            }
        }
    }
}
