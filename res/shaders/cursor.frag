#version 420 core

in vec2 vTexCoord;
in vec3 vNormal;

out vec4 outputColor;

layout(binding = 0) uniform sampler2D texSampler;

vec3 sunDir = vec3(0.3,0.6,0.1);            //my imaginary sun

uniform int curSprite;
uniform int numSprites;

uniform float time;

uniform vec4 color;

float noise(vec2 pos){
    return fract(sin(dot(pos,vec2(12.9898,78.233)))*43758.5453);
}

void main(){
    vec2 newCoords = vec2(vTexCoord.x+time/100.0,vTexCoord.y+time/50.0);
    outputColor = texture(texSampler,newCoords);
}