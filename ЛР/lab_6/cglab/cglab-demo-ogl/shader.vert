#version 330 core
in vec3 position;
in vec3 normal;

out vec3 Normal;
out vec3 FragPos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform float time;

void main()
{    
    vec3 Vector_Position = position;
	Vector_Position.x = Vector_Position.x * cos(time);
    gl_Position = projection * view * model * vec4(Vector_Position, 1.0f);
    FragPos = vec3(view * model * vec4(Vector_Position, 1.0f));
    Normal = mat3(transpose(inverse(view * model))) * normal;  
} 