#version 420 core

in vec2 vTexCoord;
in vec3 vNormal;

out vec4 outputColor;

layout(binding = 0) uniform sampler2D texSampler;

vec3 sunDir = vec3(0.3,0.6,0.4);            //my imaginary sun
uniform float blend;

uniform int curSprite;
uniform int numSprites;

uniform vec4 color;

float noise(vec2 pos){
    return fract(sin(dot(pos,vec2(12.9898,78.233)))*43758.5453);
}

void main(){
    float dotProd = min(1.0,dot(normalize(vNormal),sunDir)*2.0)-0.2;
    float width = 1.0 / float(numSprites);
    vec2 newTexCoords = vec2(vTexCoord.x*width + width*float(curSprite), vTexCoord.y);
    outputColor = texture(texSampler, newTexCoords)*max(min(dotProd*1.7,1.7),0.5) * color;
    if(outputColor.a<0.5) discard;
    if(blend < 1.0){
        float ns = noise(vTexCoord);
        if(ns>blend) discard;
    }
}