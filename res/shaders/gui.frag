#version 420 core

in vec2 textureCoords;

out vec4 outputColor;

uniform sampler2D guiTexture;
uniform int curSprite;
uniform int numSprites;
uniform vec4 color;


void main(){
    float width = 1.0 / float(numSprites);
    vec2 newTexCoords = vec2(textureCoords.x*width + width*float(curSprite), textureCoords.y);
    outputColor = texture(guiTexture, newTexCoords) * color;
}