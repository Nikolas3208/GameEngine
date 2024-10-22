#version 330

layout (location = 0) in vec3 aPos;

out vec3 WorldPos;

uniform mat4 view;
uniform mat4 projection;
uniform mat4 model;

uniform float gGridSize = 100.0;
uniform vec3 gCameraWorldPos;

const vec3 Pos[4] = vec3[4](
    vec3(-1.0, 0.0, -1.0),      // bottom left
    vec3( 1.0, 0.0, -1.0),      // bottom right
    vec3( 1.0, 0.0,  1.0),      // top right
    vec3(-1.0, 0.0,  1.0)       // top left
);

const int Indices[6] = int[6](0, 2, 1, 2, 0, 3);


void main()
{
    int Index = Indices[gl_VertexID];
    vec3 vPos3 = aPos * gGridSize;

    vPos3.x += gCameraWorldPos.x;
    vPos3.z += gCameraWorldPos.z;

    vec4 vPos4 = vec4(vPos3, 1.0);

    gl_Position = vPos4 * model * view * projection;

    WorldPos = vPos3;
}