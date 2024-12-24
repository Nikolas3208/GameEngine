#version 330 core

in vec2 TexCoords;
in vec3 Normal;
in vec3 FragPos;
in vec4 LightSpacePos;

struct Material {
    sampler2D diffuse;
    sampler2D specular;
    sampler2D shadowMap;

    vec3 color;

    float shininess;

    int useTexture;
};

uniform Material material;

uniform int lightType;

struct DirLight {
    vec3 direction;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform DirLight dirLight;

struct PointLight {
    vec3 position;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
};

uniform PointLight pointLight;

struct SpotLight {
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
};

uniform SpotLight spotLight;

uniform vec3 viewPos;
uniform float far_plane;
uniform samplerCube depthMap;
uniform int useShadow;
uniform vec3 lightPos;

out vec4 FragColor;

uniform int usePicking;
uniform int gameObjectId;
uniform int meshId;

float shadow;

// array of offset direction for sampling
vec3 gridSamplingDisk[20] = vec3[]
(
   vec3(1, 1,  1), vec3( 1, -1,  1), vec3(-1, -1,  1), vec3(-1, 1,  1), 
   vec3(1, 1, -1), vec3( 1, -1, -1), vec3(-1, -1, -1), vec3(-1, 1, -1),
   vec3(1, 1,  0), vec3( 1, -1,  0), vec3(-1, -1,  0), vec3(-1, 1,  0),
   vec3(1, 0,  1), vec3(-1,  0,  1), vec3( 1,  0, -1), vec3(-1, 0, -1),
   vec3(0, 1,  1), vec3( 0, -1,  1), vec3( 0, -1, -1), vec3( 0, 1, -1)
);

float ShadowCalculation(vec3 fragPos)
{
    // get vector between fragment position and light position
    vec3 fragToLight = fragPos - lightPos;

    float currentDepth = length(fragToLight);

    float shadow = 0.0;
    float bias = 0.15;
    int samples = 20;
    float viewDistance = length(viewPos - fragPos);
    float diskRadius = (1.0 + (viewDistance / far_plane)) / 25.0;
    for(int i = 0; i < samples; ++i)
    {
        float closestDepth = texture(depthMap, fragToLight + gridSamplingDisk[i] * diskRadius).r;
        closestDepth *= far_plane;   // undo mapping [0;1]
        if(currentDepth - bias > closestDepth)
            shadow += 1.0;
    }
    shadow /= float(samples);
        
    // display closestDepth as debug (to visualize depth cubemap)
    // FragColor = vec4(vec3(closestDepth / far_plane), 1.0);    
        
    return shadow;
}


float ShadowCalculation(vec4 lightSpacePos,vec3 norm, vec3 lightDir)
{
    vec3 projCoords = lightSpacePos.xyz / lightSpacePos.w;
    
    projCoords = projCoords * 0.5 + 0.5;
    
    float closestDepth = texture(material.shadowMap, projCoords.xy).r; 
    
    float currentDepth = projCoords.z;
    
    float bias = max(0.05 * (1.0 - dot(norm, lightDir)), 0.005);

    //float shadow = currentDepth - bias > closestDepth  ? 1.0 : 0.0;
//    // PCF
    float shadow = 0.0;

    vec2 texelSize = 1.0 / textureSize(material.shadowMap, 0);
    for(int x = -1; x <= 1; ++x)
    {
        for(int y = -1; y <= 1; ++y)
        {
            float pcfDepth = texture(material.shadowMap, projCoords.xy + vec2(x, y) * texelSize).r; 
            shadow += currentDepth - bias > pcfDepth  ? 1.0 : 0.0;        
        }    
    }

    shadow /= 9.0;

    if(projCoords.z > 1.0)
        shadow = 0.0;

    return shadow;
}

vec3 CalculatePointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    vec3 lightDir = normalize(light.position - fragPos);

    float diff = max(dot(normal, lightDir), 0.0);

    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);

    float distance    = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));

    vec3 ambient  = light.ambient         * attenuation;
    vec3 diffuse  = light.diffuse  * diff * attenuation;
    vec3 specular = light.specular * spec * attenuation;

    if(useShadow == 1)
        shadow = ShadowCalculation(FragPos);

    if(material.useTexture == 1)
        return (ambient * vec3(texture(material.diffuse, TexCoords)) + diffuse * vec3(texture(material.diffuse, TexCoords)) + specular * vec3(texture(material.specular, TexCoords)));
    else
        return (ambient + ((1 - shadow) * diffuse + specular));
        
}

vec3 CalculateDirLight(DirLight light, vec3 normal, vec3 viewDir)
{
     vec3 lightDir = normalize(-light.direction);
    //diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);
    //specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    //combine results
    vec3 ambient  = light.ambient  * vec3(1);
    vec3 diffuse  = light.diffuse  * diff;
    vec3 specular = light.specular * spec;

    if(useShadow == 1)
        shadow = ShadowCalculation(LightSpacePos, normalize(Normal), normalize(dirLight.direction - FragPos));

   if(material.useTexture == 1)
        return (ambient * vec3(texture(material.diffuse, TexCoords)) + diffuse * vec3(texture(material.diffuse, TexCoords)) + specular * vec3(texture(material.specular, TexCoords)));
    else
        return (ambient + ((1 - shadow) * diffuse + specular));
}

vec3 CalculateSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
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
    vec3 ambient = light.ambient * vec3(1);
    vec3 diffuse = light.diffuse * diff;
    vec3 specular = light.specular * spec;
    ambient  *= attenuation;
    diffuse  *= attenuation * intensity;
    specular *= attenuation * intensity;

    if(material.useTexture == 1)
        return (ambient * vec3(texture(material.diffuse, TexCoords)) + diffuse * vec3(texture(material.diffuse, TexCoords)) + specular * vec3(texture(material.specular, TexCoords)));
    else
        return (ambient + ((1 - shadow) * diffuse + specular));
}

void main()
{
    vec4 result = vec4(1);

    if(useShadow == 0)
        shadow = 0;

    if(lightType == 1)
    {        
        result = vec4(CalculatePointLight(pointLight, normalize(Normal), FragPos, normalize(viewPos - FragPos)) * material.color, 1.0);
    }
    if(lightType == 2)
    {
        result = vec4(CalculateDirLight(dirLight, normalize(Normal), normalize(dirLight.direction - FragPos)) * material.color, 1.0);
    }
    if(lightType == 3)
    {
        result = vec4(CalculateSpotLight(spotLight, normalize(Normal), FragPos, normalize(viewPos - FragPos)) * material.color, 1.0);
    }
    
    if(usePicking == 1)
    {
        result = vec4(gameObjectId, meshId, gl_PrimitiveID, 1);
    }
    
    FragColor = result;
}
