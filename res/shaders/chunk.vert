#version 330 core

layout (location = 0) in vec3 aPosition;    //Vertex position
layout (location = 1) in vec3 aNormal;      //Normal direction
layout (location = 2) in vec2 aTexCoord;    //Texture Coordinate

out vec2 vTexCoord;
out vec3 vNormal;
out float dist;

uniform vec3 camPos;
uniform mat4 model;                         //Transformation matrix
uniform mat4 projection;                    //Projection matrix
uniform mat4 view;                          //View Matrix

void main(){
    vTexCoord = aTexCoord; //Allow for interpolation
    vNormal = aNormal;
    //Transform the vertex position by the transformation matrix
    mat4 mvp = projection * view * model;
    gl_Position = mvp * vec4(aPosition,1.0);
    vec4 finalposition =  model * vec4(aPosition,1.0);
    dist = distance(camPos,vec3(finalposition.x,finalposition.y,finalposition.z));
}