#version 330 core

layout (location = 0) in vec2 aPosition;    //Vertex position

out vec2 textureCoords;

uniform vec2 position;
uniform vec2 scale;

uniform float aspect;

void main(){
    textureCoords = vec2((aPosition.x+1.0)/2.0, 1 - (aPosition.y+1.0)/2.0); //Allow for interpolation
    
    vec2 newPos = aPosition;
    newPos.x = newPos.x * aspect;
    newPos /= 4.0;
    gl_Position = vec4(position/4.0+newPos*scale,0.0,1.0);
}