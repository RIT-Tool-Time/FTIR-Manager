float4x4 World;
float4x4 View;
float4x4 Projection;

float frontDepth = 2000;
float rearDepth = 10000;

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float4 Color : COLOR;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float4 Pos : POSITION1;
	float4 Color : COLOR;
};

struct mrt { float4 col0 : COLOR0; float4 col1 : COLOR1; };

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
	float4 projPosition = mul(viewPosition, Projection);
    output.Position = projPosition;
	output.Pos = projPosition;
	output.Color = input.Color;
    return output;
}

mrt PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    mrt m = (mrt)0;
	m.col0 = input.Color;
	float z = input.Pos.z;
	m.col1= float4( (z - frontDepth) / (rearDepth - frontDepth), 0, 0, input.Color.a);
	//mrt.col
    return m;
}

technique Normal
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
