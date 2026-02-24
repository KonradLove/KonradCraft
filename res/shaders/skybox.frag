#version 420 core

in vec3 vTexCoord;

out vec4 outputColor;

uniform vec4 color;
uniform samplerCube texSampler;

void main(){
    outputColor = texture(texSampler, vTexCoord) * color;
}