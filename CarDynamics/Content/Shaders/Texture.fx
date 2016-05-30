//------------------------------------------------------------
// Microsoft® XNA Game Studio Creator's Guide, Second Edition
// by Stephen Cawood and Pat McGee 
// Copyright (c) McGraw-Hill/Osborne. All rights reserved.
// https://www.mhprofessional.com/product.php?isbn=0071614060
//------------------------------------------------------------
float4x4 wvpMatrix :WORLDVIEWPROJ;	//world view proj matrix
uniform extern texture textureImage;// store texture

// filter (like a brush) for showing texture
sampler textureSampler  = sampler_state
{
	Texture				= <textureImage>;
	magfilter			= LINEAR;	// magfilter - bigger than actual
	minfilter			= LINEAR;	// minfilter - smaller than actual
	mipfilter			= LINEAR;
};

// input to vertex shader
struct VSinput
{									 // input to vertex shader
	float4 position		: POSITION0; // position semantic x,y,z,w
	float4 color		: COLOR0;	 // color semantic r,g,b,a
	float2 uv			: TEXCOORD0; // texture semantic u,v
};

// vertex shader output
struct VStoPS
{ 
	// vertex shader output
	float4 position		: POSITION0; // position semantic x,y,z,w
	float4 color		: COLOR;	 // color semantic r,g,b,a
	float2 uv			: TEXCOORD0; // texture semantic u,v
};

// pixel shader output
struct PSoutput
{									// pixel shader output
	float4 color		: COLOR0;   // colored pixel is output
};

// alter vertex inputs
void VertexShader(in VSinput IN, out VStoPS OUT)
{
	OUT.position = mul(IN.position, wvpMatrix); // transform object
	
	// orient it in camera
	OUT.color  = IN.color;			// send color to p.s.
	OUT.uv	 = IN.uv;				// send uv's to p.s.
}

// convert color and texture data from vertex shader to pixels
void PixelShader(in VStoPS IN, out PSoutput OUT)
{
	// use texture for coloring object
	OUT.color  = tex2D(textureSampler, IN.uv);
	
	// this next line is optional – you can shade the texturized pixel
	// with color to give your textures a tint. Do this by multiplying
	// output by the input color vector.
	OUT.color *= IN.color;
}

// the shader starts here
technique TextureShader
{
	pass p0
	{
		// texture sampler initialized
		sampler[0]		= (textureSampler);
		
		// declare and initialize vs and ps
		vertexshader	= compile vs_1_1 VertexShader();
		pixelshader		= compile ps_1_1 PixelShader();
	}
}

