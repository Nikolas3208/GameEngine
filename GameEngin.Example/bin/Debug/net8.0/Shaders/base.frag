#version 330 core

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoords;

struct Material
{
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    
    vec4 color;

    float shininess;

    sampler2D texDiffuse;
    sampler2D texSpecular;
    sampler2D texNormal;
};

struct Light {
    vec3 position;
    vec3 direction;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;

    float cutOff;
    float outerCutOff;

    int type;
};

uniform Material material;

#define MaxLight 100

uniform Light lights[MaxLight];

uniform vec3 viewPos;
uniform int lightCount;

out vec4 FragColor;

vec3 DirectionLightCalculate(Light light, vec3 normal, vec3 viewDir);
vec3 PointLightCalculate(Light light, vec3 normal, vec3 fragPos, vec3 viewDir);
vec3 SpotLightCalculate(Light light, vec3 normal, vec3 fragPos, vec3 viewDir);

void main()
{
    //properties
    vec3 norm = normalize(Normal);
    vec3 viewDir = normalize(viewPos - FragPos);

    vec3 result = vec3(0);

    for(int i = 0; i < lightCount && i < MaxLight; i++)
    {
        if(lights[i].type == 0)
        {
            result += DirectionLightCalculate(lights[i], norm, viewDir);
        }
        else if(lights[i].type == 1)
        {
            result += PointLightCalculate(lights[i], norm, FragPos, viewDir);
        }
        else if(lights[i].type == 2)
        {
            result += SpotLightCalculate(lights[i], norm, FragPos, viewDir);
        }
    }

    FragColor = vec4(result, 1.0);
}

vec3 DirectionLightCalculate(Light light, vec3 normal, vec3 viewDir)
{
    vec3 lightDir = normalize(-light.direction);
    //diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);
    //specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    //combine results
    vec3 ambient  = light.ambient * material.ambient * vec3(texture(material.texDiffuse, TexCoords)) * material.color.xyz;
    vec3 diffuse  = light.diffuse  * diff * material.diffuse * vec3(texture(material.texDiffuse, TexCoords)) * material.color.xyz;
    vec3 specular = light.specular * spec * material.specular * vec3(texture(material.texSpecular, TexCoords)) * material.color.xyz;
    return (ambient + diffuse + specular);
}

vec3 PointLightCalculate(Light light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    vec3 lightDir = normalize(light.position - fragPos);
    //diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);
    //specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    //attenuation
    float distance    = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance +
    light.quadratic * (distance * distance));
    //combine results
    vec3 ambient  = light.ambient * material.ambient * vec3(texture(material.texDiffuse, TexCoords)) * material.color.xyz;
    vec3 diffuse  = light.diffuse  * diff * material.diffuse * vec3(texture(material.texDiffuse, TexCoords)) * material.color.xyz;
    vec3 specular = light.specular * spec * material.specular * vec3(texture(material.texSpecular, TexCoords)) * material.color.xyz;
    ambient  *= attenuation;
    diffuse  *= attenuation;
    specular *= attenuation;
    return (ambient + diffuse + specular);
}

vec3 SpotLightCalculate(Light light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
     //diffuse shading
    vec3 lightDir = normalize(light.position - FragPos);
    float diff = max(dot(normal, lightDir), 0.0);

    //specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);

    //attenuation
    float distance    = length(light.position - FragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance +
    light.quadratic * (distance * distance));

    //spotlight intensity
    float theta     = dot(lightDir, normalize(-light.direction));
    float epsilon   = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);

    //combine results
    vec3 ambient = light.ambient * material.ambient * vec3(texture(material.texDiffuse, TexCoords)) * material.color.xyz;
    vec3 diffuse = light.diffuse * diff * material.diffuse * vec3(texture(material.texDiffuse, TexCoords)) * material.color.xyz;
    vec3 specular = light.specular * spec * material.specular * vec3(texture(material.texSpecular, TexCoords)) * material.color.xyz;
    ambient  *= attenuation;
    diffuse  *= attenuation * intensity;
    specular *= attenuation * intensity;
    return (ambient + diffuse + specular);
}
