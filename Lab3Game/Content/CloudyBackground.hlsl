﻿#if OPENGL
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

static const float cloudscale = 0.4;
static const float speed = 0.005;
static const float clouddark = 0.5;
static const float cloudlight = 0.3;
static const float cloudcover = 0.2;
static const float cloudalpha = 8.0;
static const float skytint = 0.5;
static const float3 skycolour1 = float3(0.2, 0.4, 0.6);
static const float3 skycolour2 = float3(0.4, 0.7, 1.0);

static const float2x2 m = float2x2( 1.6,  1.2, -1.2,  1.6 );

float2 hash( float2 p ) {
	p = float2(dot(p,float2(127.1,311.7)), dot(p,float2(269.5,183.3)));
	return -1.0 + 2.0*frac(sin(p)*43758.5453123);
}

float noise( in float2 p ) {
    const float K1 = 0.366025404; // (sqrt(3)-1)/2;
    const float K2 = 0.211324865; // (3-sqrt(3))/6;
	float2 i = floor(p + (p.x+p.y)*K1);	
    float2 a = p - i + (i.x+i.y)*K2;
    float2 o = (a.x>a.y) ? float2(1.0,0.0) : float2(0.0,1.0); //vec2 of = 0.5 + 0.5*vec2(sign(a.x-a.y), sign(a.y-a.x));
    float2 b = a - o + K2;
	float2 c = a - 1.0 + 2.0*K2;
    float3 h = max(0.5-float3(dot(a,a), dot(b,b), dot(c,c) ), 0.0 );
	float3 n = h*h*h*h*float3( dot(a,hash(i+0.0)), dot(b,hash(i+o)), dot(c,hash(i+1.0)));
    return dot(n, float3(70.0,70.0,70.0));	
}

float fbm(float2 n) {
	float total = 0.0, amplitude = 0.1;
	for (int i = 0; i < 7; i++) {
		total += noise(n) * amplitude;
		n = mul(m, n);
		amplitude *= 0.4;
	}
	return total;
}

// -----------------------------------------------

//void mainImage( out float4 fragColor, in float2 fragCoord ) {
float4 MainPS(VertexShaderOutput input) : COLOR
{
    float2 p = input.WorldPos.xy/5.;
	float2 uv = p;
    float time = Time * speed;
    float q = fbm(uv * cloudscale * 0.5);
    
    //ridged noise shape
	float r = 0.0;
	uv *= cloudscale;
    uv -= q - time;
    float weight = 0.8;
    for (int i=0; i<8; i++){
		r += abs(weight*noise( uv ));
        uv = mul(m,uv) + time;
		weight *= 0.7;
    }
    
    //noise shape
	float f = 0.0;
    uv = p;
	uv *= cloudscale;
    uv -= q - time;
    weight = 0.7;
    for (i=0; i<8; i++){
		f += weight*noise( uv );
        uv = mul(m,uv) + time;
		weight *= 0.6;
    }
    
    f *= r + f;
    
    //noise colour
    float c = 0.0;
    time = Time * speed * 2.0;
    uv = p;
	uv *= cloudscale*2.0;
    uv -= q - time;
    weight = 0.4;
    for (i=0; i<7; i++){
		c += weight*noise( uv );
        uv = mul(m,uv) + time;
		weight *= 0.6;
    }
    
    //noise ridge colour
    float c1 = 0.0;
    time = Time * speed * 3.0;
    uv = p;
	uv *= cloudscale*3.0;
    uv -= q - time;
    weight = 0.4;
    for (i=0; i<7; i++){
		c1 += abs(weight*noise( uv ));
        uv = mul(m,uv) + time;
		weight *= 0.6;
    }
	
    c += c1;
    
    float3 skycolour = lerp(skycolour2, skycolour1, p.y);
    float3 cloudcolour = float3(1.1, 1.1, 0.9) * clamp((clouddark + cloudlight*c), 0.0, 1.0);
   
    f = cloudcover + cloudalpha*f*r;
    
    float3 result = lerp(skycolour, clamp(skytint * skycolour + cloudcolour, 0.0, 1.0), clamp(f + c, 0.0, 1.0));
    
	return float4( result, 1.0 );
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};