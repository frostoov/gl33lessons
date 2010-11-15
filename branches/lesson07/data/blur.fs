// текстуры
uniform sampler2D colorTexture, depthTexture;

// параметры полученные из вершинного шейдера
in Vertex
{
	vec2 texcoord;
} Vert;

layout(location = FRAG_OUTPUT0) out vec4 color;

#define KERNEL_SIZE 9

const float kernel[KERNEL_SIZE] = float[](
	0.0625, 0.125, 0.0625,
	0.1250, 0.250, 0.1250,
	0.0625, 0.125, 0.0625
);

const vec2 offset[KERNEL_SIZE] = vec2[](
	vec2(-1.0,-1.0), vec2( 0.0,-1.0), vec2( 1.0,-1.0),
	vec2(-1.0, 0.0), vec2( 0.0, 0.0), vec2( 1.0, 0.0),
	vec2(-1.0, 1.0), vec2( 0.0, 1.0), vec2( 1.0, 1.0)
);

vec3 filter(in vec2 texcoord)
{
	vec2 pstep = vec2(2.0) / vec2(textureSize(colorTexture, 0));
	vec4 res   = vec4(0.0);

	for (int i = 0; i < KERNEL_SIZE; ++i)
		res += texture(colorTexture, texcoord + offset[i] * pstep) * kernel[i];

	return vec3(res);
}

void main(void)
{
	vec3 texel = Vert.texcoord.x < 0.5 ? filter(Vert.texcoord)
		: texture(colorTexture, Vert.texcoord).rgb;

	color = vec4(texel, 1.0);
}
