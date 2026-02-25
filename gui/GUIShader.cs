using OpenTK.Mathematics;

namespace EminaCraft.gui
{
    internal class GUIShader : ShaderProgram
    {
        private UniformVec2 positionU;
        private UniformVec2 scaleU;
        private UniformVec4 colorU;
        private UniformInteger numSpritesU;
        private UniformInteger curSpriteU;
        private UniformFloat aspectU;
        private UniformSampler samplerU;

        private float aspect;
        public GUIShader(float aspect) : base("gui")
        {
            positionU = new UniformVec2("position", shaderHandle);
            scaleU = new UniformVec2("scale", shaderHandle);
            samplerU = new UniformSampler("guiTexture", shaderHandle);
            aspectU = new UniformFloat("aspect", shaderHandle);
            colorU = new UniformVec4("color", shaderHandle);
            numSpritesU = new UniformInteger("numSprites", shaderHandle);
            curSpriteU = new UniformInteger("curSprite", shaderHandle);

            this.aspect = aspect;
        }
        public Vector2 scale;
        public Vector2 position;
        public Color4 color;
        public int numSprites;
        public int curSprite;

        public void uploadValues(GUIObject obj)
        {
            scale = obj.size;
            position = obj.position;
            color = obj.color;
            numSprites = obj.numSprites;
            curSprite = obj.curSprite;
        }

        public override void uploadUniforms()
        {
            aspectU.upload(aspect);
            samplerU.upload(0);
            scaleU.upload(scale);
            positionU.upload(position);
            colorU.upload((Vector4)color);
            numSpritesU.upload(numSprites);
            curSpriteU.upload(curSprite);
        }
    }
}
