#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

uniform matrix WorldViewProjection;
uniform matrix World;

sampler2D Texture;

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
	output.WorldPos = mul(input.Position, World);
    output.UV = input.UV;
    
	return output;
}

float4 hash4( float2 p ) { return frac(sin(float4( 1.0+dot(p,float2(37.0,17.0)), 
                                              2.0+dot(p,float2(11.0,47.0)),
                                              3.0+dot(p,float2(41.0,29.0)),
                                              4.0+dot(p,float2(23.0,31.0))))*103.0); }

float4 textureNoTile( sampler2D samp, in float2 uv )
{
    float2 p = floor( uv );
    float2 f = frac( uv );
	
    // derivatives (for correct mipmapping)
    float2 ddx1 = ddx( uv );
    float2 ddy1 = ddy( uv );
    
    // voronoi contribution
    float4 va = float4( 0.0, 0.0, 0.0, 0.0 );
    float wt = 0.0;
    for( int j=-1; j<=1; j++ )
    for( int i=-1; i<=1; i++ )
    {
        float2 g = float2( float(i), float(j) );
        float4 o = hash4( p + g );
        float2 r = g - f + o.xy;
        float d = dot(r,r);
        float w = exp(-5.0*d );
        float4 c = tex2Dgrad( samp, uv + o.zw, ddx1, ddy1 );
        va += w*c;
        wt += w;
    }
	
    // normalization
    return va/wt;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    return textureNoTile(Texture, input.WorldPos/4.);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};