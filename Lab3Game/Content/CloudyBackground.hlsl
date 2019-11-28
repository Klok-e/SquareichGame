#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

uniform float Time;
uniform matrix WorldViewProjection;
uniform matrix World;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float2 UV : TEXCOORD0;
	float4 WorldPos : TEXCOORD1;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output;
    
	output.Position = mul(input.Position, WorldViewProjection);
	//output.Position.x = output.Position.x + sin(output.Position.x+Time*10.)*0.2;
	//output.Position.y = output.Position.y + cos( output.Position.y+Time*10.)*0.2;
	output.WorldPos = mul(input.Position, World);
    output.UV = input.UV;
    
	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    //input.WorldPos;
    //input.Position;
	return float4(0.,0. + (sin(Time*2.)*0.5+0.5)*0.4,8.,1.);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};