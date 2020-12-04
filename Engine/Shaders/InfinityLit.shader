Shader "InfinityPipeline/InfinityLit"
{
	Pass
	{
		Tags {"Name" = "OpaqueDepth", "Type" = "Graphics"}
		ZWrite On 
		ZTest LEqual 
		Cull Back 
		ColorMask 0 

		HLSLPROGRAM
		#pragma vertexFunc Vertex
		#pragma pixelFunc Pixel

		#include "../Private/Common.hlsl"

		struct Attributes
		{
			float2 uv : TEXCOORD0;
			float4 vertex : POSITION;
		};

		struct Varyings
		{
			float2 uv : TEXCOORD0;
			float4 vertex : SV_POSITION;
		};

		Varyings Vertex(Attributes In)
		{
			Varyings Out = (Varyings)0;

			Out.uv = In.uv;
			float4 WorldPos = mul(UNITY_MATRIX_M, float4(In.vertex.xyz, 1.0));
			Out.vertex = mul(Matrix_ViewJitterProj, WorldPos);
			return Out;
		}

		float4 Pixel(Varyings In) : SV_Target
		{
			/*UNITY_SETUP_INSTANCE_ID(In);
			if (In.uv.x < 0.5) {
				discard;
			}*/
			return 0;
		}
		ENDHLSL
	}

	Pass
	{
		Tags {"Name" = "OpaqueGBuffer", "Type" = "Graphics"}
		ZWrite On 
		ZTest LEqual 
		Cull Back 

		HLSLPROGRAM
		#pragma vertexFunc Vertex
		#pragma pixelFunc Pixel

		#include "../Private/Common.hlsl"

		float _SpecularLevel;
		float4 _BaseColor;
		
		Texture2D _MainTex; SamplerState sampler_MainTex;

		struct Attributes
		{
			float2 uv : TEXCOORD0;
			float3 normal : NORMAL;
			float4 vertex : POSITION;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct Varyings
		{
			float2 uv : TEXCOORD0;
			float3 normal : TEXCOORD1;
			float4 worldPos : TEXCOORD2;
			float4 vertex : SV_POSITION;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};
		
		Varyings Vertex(Attributes In)
		{
			Varyings Out = (Varyings)0;

			Out.uv = In.uv;
			Out.normal = normalize(mul(In.normal, (float3x3)unity_WorldToObject));
			Out.worldPos = mul(UNITY_MATRIX_M, float4(In.vertex.xyz, 1.0));
			Out.vertex = mul(Matrix_ViewJitterProj, Out.worldPos);
			return Out;
		}
		
		void Pixle(Varyings In, out float4 ThinGBufferA : SV_Target0, out uint4 ThinGBufferB : SV_Target1)
		{
			float3 WS_PixelPos = In.worldPos.xyz;
			float3 BaseColor = _MainTex.Sample(sampler_MainTex, In.uv).rgb;
			
			//ThinGBufferA = float4(BaseColor, 1);
			//ThinGBufferB = uint4((In.normal * 127 + 127), 1);
			ThinGBufferData GBufferData;
			GBufferData.WorldNormal = normalize(In.normal);
			GBufferData.BaseColor = BaseColor;
			GBufferData.Roughness = BaseColor.r;
			GBufferData.Specular = _SpecularLevel;
			GBufferData.Reflactance = BaseColor.b;
			EncodeGBuffer(GBufferData, ThinGBufferA, ThinGBufferB);
		}
		ENDHLSL
	}

	//RayTrace AO
	Pass
	{
		Tags {"Name" = "RTAO", "Type" = "HitGroup"}

		HLSLPROGRAM
		#pragma anyHit Anyhit
		#pragma closeHit ClosestHit

		#include "../Private/Common.hlsl"

		float4 _BaseColor;

		[shader("anyhit")]
		void Anyhit(inout AORayPayload RayIntersectionAO : SV_RayPayload, AttributeData attributeData : SV_IntersectionAttributes)
		{
			IgnoreHit();
		}

		[shader("closesthit")]
		void ClosestHit(inout AORayPayload RayIntersectionAO : SV_RayPayload, AttributeData attributeData : SV_IntersectionAttributes)
		{
			RayIntersectionAO.HitDistance = RayTCurrent();
			//Calculate_VertexData(FragInput);
		}
		ENDHLSL
	}
}
