using OpenTK.Mathematics;

namespace KonradCraft.shaders
{
    internal class EntityShader : ShaderProgram
    {
        protected UniformMatrix4 transformationU;
        protected UniformMatrix4 projectionU;
        protected UniformMatrix4 viewU;
        protected UniformInteger numSpritesU;
        protected UniformInteger curSpriteU;
        protected UniformVec4 colorU;
        protected Camera cam;
        public EntityShader(Camera cam, string name) : base(name)
        {
            transformationU = new UniformMatrix4("model", shaderHandle);
            projectionU = new UniformMatrix4("projection", shaderHandle);
            viewU = new UniformMatrix4("view", shaderHandle);
            numSpritesU = new UniformInteger("numSprites", shaderHandle);
            curSpriteU = new UniformInteger("curSprite", shaderHandle);
            colorU = new UniformVec4("color", shaderHandle);
            this.cam = cam;
        }
        public EntityShader(Camera cam, string vert, string frag) : base(vert,frag)
        {
            transformationU = new UniformMatrix4("model", shaderHandle);
            projectionU = new UniformMatrix4("projection", shaderHandle);
            viewU = new UniformMatrix4("view", shaderHandle);
            numSpritesU = new UniformInteger("numSprites", shaderHandle);
            curSpriteU = new UniformInteger("curSprite", shaderHandle);
            colorU = new UniformVec4("color", shaderHandle);
            this.cam = cam;
        }
        public Entity? currentEntity;
        public override void uploadUniforms()
        {
            if (currentEntity == null) return;

            numSpritesU.upload(currentEntity.numSprites);
            curSpriteU.upload(currentEntity.curSprite);
            transformationU.upload(
                Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(currentEntity.rotation)) *
                Matrix4.CreateScale(currentEntity.scale) *
                Matrix4.CreateTranslation(currentEntity.position)
            );
            colorU.upload((Vector4)currentEntity.color);
            viewU.upload(cam.viewMatrix);
            projectionU.upload(cam.generateProjectionMatrix());
        }
    }
}
