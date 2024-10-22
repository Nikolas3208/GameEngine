#version 410

uniform int gameObjectId;
uniform int meshId;

out vec4 FragColor;

void main()
{
   FragColor = vec4(gameObjectId, meshId, gl_PrimitiveID, 1);
}