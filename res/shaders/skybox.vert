#version 330 core

layout (location = 0) in vec3 aPosition;    //Vertex position

out vec3 vTexCoord;

uniform mat4 projection;                    //Projection matrix
uniform mat4 view;                          //View Matrix

uniform float size;

void main(){
    vTexCoord = aPosition; //Allow for interpolation
    //Transform the vertex position by the transformation matrix
    gl_Position = projection * view * vec4(aPosition*500.0,1.0);
}