Shader "InfinityPipeline/InfinityLit"
{
	Pass
	{
		Tags {"Name" = "OpaqueDepth", "Type" = "Graphics"}
		ZWrite On ZTest LEqual Cull Back 
		ColorMask 0 

		HLSLPROGRAM
		#pragma vertex vert
		#pragma fragment frag

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

		Varyings vert(Attributes In)
		{
			Varyings Out = (Varyings)0;

			Out.uv = In.uv;
			float4 WorldPos = mul(UNITY_MATRIX_M, float4(In.vertex.xyz, 1.0));
			Out.vertex = mul(Matrix_ViewJitterProj, WorldPos);
			return Out;
		}

		float4 frag(Varyings In) : SV_Target
		{
			return 0;
		}
		ENDHLSL
	}

	Pass
	{
		Tags {"Name" = "OpaqueGBuffer", "Type" = "Graphics"}
		ZWrite On ZTest LEqual Cull Back 

		HLSLPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		#include "../Private/Common.hlsl"

		float _SpecularLevel;
		float4 _BaseColor;
		
		Texture2D _MainTex; SamplerState sampler_MainTex;

		struct Attributes
		{
			float2 uv : TEXCOORD0;
			float3 normal : NORMAL;
			float4 vertex : POSITION;
		};

		struct Varyings
		{
			float2 uv : TEXCOORD0;
			float3 normal : TEXCOORD1;
			float4 worldPos : TEXCOORD2;
			float4 vertex : SV_POSITION;
		};
		
		Varyings vert(Attributes In)
		{
			Varyings Out = (Varyings)0;

			Out.uv = In.uv;
			Out.normal = normalize(mul(In.normal, (float3x3)unity_WorldToObject));
			Out.worldPos = mul(UNITY_MATRIX_M, float4(In.vertex.xyz, 1.0));
			Out.vertex = mul(Matrix_ViewJitterProj, Out.worldPos);
			return Out;
		}
		
		void frag(Varyings In, out float4 GBufferA : SV_Target0, out uint4 GBufferB : SV_Target1)
		{
			float3 WS_PixelPos = In.worldPos.xyz;
			float3 BaseColor = _MainTex.Sample(sampler_MainTex, In.uv).rgb;
			
			GBufferA = float4(BaseColor, 1);
			GBufferB = uint4((In.normal * 127 + 127), 1);
		}
		ENDHLSL
	}

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
