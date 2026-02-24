#version 420 core

in vec2 vTexCoord;
in vec3 vNormal;
in float dist;

out vec4 outputColor;

layout(binding = 0) uniform sampler2D texSampler;

vec3 sunDir = vec3(0.3,0.6,0.1);            //my imaginary sun

uniform vec4 color;

void main(){
    float dotProd = max(0.6,min(0.8,dot(normalize(vNormal),normalize(sunDir))*2.0));
    outputColor = texture(texSampler, vTexCoord) * dotProd;
    if(outputColor.a < 0.1) discard;
}