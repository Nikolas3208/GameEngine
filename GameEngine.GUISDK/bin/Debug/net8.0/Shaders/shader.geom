#version 410  core

layout (triangles) in;
layout (triangle_strip, max_vertices = 3) out;

in DATA 
{
    vec2 TexCoords;
    mat4 model;
    vec3 Normal;
    vec3 FragPos;
    vec2 TexCoords2;
    vec4 LightSpacePos;
    mat4 view;
} data_in[];

out VS
{
 vec3 Normal;
 vec3 FragPos;
 vec2 TexCoords;
 vec4 LightSpacePos;
} vs_in;

out mat3 TBN;

void main()
{
    vec3 edge0 = gl_in[1].gl_Position.xyz - gl_in[0].gl_Position.xyz;
    vec3 edge1 = gl_in[2].gl_Position.xyz - gl_in[0].gl_Position.xyz;

    vec2 deltaUV0 = data_in[1].TexCoords2.xy - data_in[0].TexCoords2.xy;
    vec2 deltaUV1 = data_in[2].TexCoords2.xy - data_in[0].TexCoords2.xy;

    float invDet = 1 / (deltaUV0.x * deltaUV1.y - deltaUV1.x * deltaUV0.y);

    vec3 tangent = vec3(invDet * (deltaUV1.y * edge0 - deltaUV0.y * edge1));
    vec3 betangent = vec3(invDet * (-deltaUV1.x * edge0 + deltaUV0.x * edge1));

    vec3 T = normalize(vec3(data_in[0].model * vec4(tangent, 0.0)));
    vec3 B = normalize(vec3(data_in[0].model * vec4(betangent, 0.0)));
    vec3 N = normalize(vec3(data_in[0].model * vec4(cross(edge1, edge0), 0.0)));

    TBN = transpose(mat3(T,B,N));


    gl_Position = gl_in[0].gl_Position * data_in[0].view;
    vs_in.Normal = data_in[0].Normal;
    vs_in.FragPos = data_in[0].FragPos;
    vs_in.TexCoords = data_in[0].TexCoords2;
    vs_in.LightSpacePos = data_in[0].LightSpacePos;
    EmitVertex();

    gl_Position = gl_in[1].gl_Position * data_in[1].view;
    vs_in.Normal = data_in[1].Normal;
    vs_in.FragPos = data_in[1].FragPos;
    vs_in.TexCoords = data_in[1].TexCoords2;
    vs_in.LightSpacePos = data_in[1].LightSpacePos;
    EmitVertex();
    
    gl_Position = gl_in[2].gl_Position * data_in[2].view;
    vs_in.Normal = data_in[2].Normal;
    vs_in.FragPos = data_in[2].FragPos;
    vs_in.TexCoords = data_in[2].TexCoords2;
    vs_in.LightSpacePos = data_in[2].LightSpacePos;
    EmitVertex();


    EndPrimitive();
    
}   