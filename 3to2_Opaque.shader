Shader "Unlit/3to2_Opaque"
{
	Properties
	{
		[Enum(Off, 0, On, 1)] _ZWriteMode ("ZWriteMode", float) = 1
		[Enum(UnityEngine.Rendering.CullMode)] _CullMode ("CullMode", float) = 0
		_Color ("Color", Color) = (1.0,1.0,1.0,1.0)
		_MainTex ("Texture", 2D) = "white" {}
		_DiffusePower ("DiffusePower", Range(0.0,1.0)) = 0.8
		_AmbientPower ("Ambient", Range(0.0,1.0)) = 0.2
		_ShadowThreshold ("ShadowThreshold",Range(-1.0,1.0)) = 0.2
		_ShadowReceiveThreshold ("ShadowReceiveThreshold",Range(-1.0,1.0)) = 0.5
		_ShadowBrightness ("ShadowBrightness", Range(0.0,1.0)) = 0.6
		_ShadowSmoothness ("ShadowSmoothness", Range(0.0,10.0)) = 1.5
		_RimColor ("RimColor", Color) = (1.0,1.0,1.0,1.0)
		_RimThreshold ("RimThreshold", Range(0.0,1.0)) = 0.65
		_Specular ("Specular", Color) = (1.0,1.0,1.0,1.0)
		_SpecularScale ("SpecularScale", Range(0,0.1)) = 0.02
		_RSTex ("RSTex", 2D) = "white" {}
		_EmisColor ("EmisColor", Color) = (0.0,0.0,0.0,1.0)
		_EmisTex ("EmissTexture", 2D) = "white" {}
		//_ShadowMapTexture("_ShadowMapTexture",2D) = "white"{}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque"  "PerformanceChecks"="False"}
		LOD 100


		ZWrite [_ZWriteMode]
		Pass
		{
			Cull [_CullMode]
			Tags { "LightMode" = "ForwardBase" "SHADOWSUPPORT"="true" "RenderType"="Opaque"  "PerformanceChecks"="False"}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase
			
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "Lighting.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				SHADOW_COORDS(3)
				UNITY_FOG_COORDS(4)
				float4 pos : SV_POSITION;
			};

			float4 _MainTex_ST;


			sampler2D _MainTex;
			fixed4 _Color;
			fixed _DiffusePower;
			fixed _AmbientPower;
			fixed _ShadowThreshold;
			fixed _ShadowReceiveThreshold;
			fixed _ShadowBrightness;
			fixed _ShadowSmoothness;
			fixed4 _RimColor;
			fixed _RimThreshold;
			fixed4 _Specular;
			fixed _SpecularScale;
			sampler2D _RSTex;
			sampler2D _EmisTex;
			fixed4 _EmisColor;
			//sampler2D _ShadowMapTexture;
	
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.worldNormal = mul(v.normal,(float3x3)unity_WorldToObject);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

				UNITY_TRANSFER_FOG(o,o.pos);
				TRANSFER_SHADOW(o);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed3 worldNormal = normalize(i.worldNormal);
				fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
				fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
				fixed3 worldHalfDir = normalize(worldLightDir + worldViewDir);

				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 RS = tex2D(_RSTex, i.uv);
				fixed4 spec = dot(worldNormal, worldHalfDir);
				fixed w = fwidth(spec) * _ShadowSmoothness;
				fixed4 specular = _Specular * _LightColor0 * lerp(0,1,smoothstep(-w,w,spec+_SpecularScale-1)) * step(0.001,_SpecularScale) * RS.x;

				fixed diffValue = dot(worldNormal, worldLightDir);
				fixed diffStep = smoothstep(-w + _ShadowThreshold, w + _ShadowThreshold, diffValue);
				fixed rcvShadow = smoothstep(-w + _ShadowReceiveThreshold, w + _ShadowReceiveThreshold, SHADOW_ATTENUATION(i));

				diffStep = min(rcvShadow,diffStep);
				fixed4 diffuse = _LightColor0 * col * (diffStep + (1 - diffStep) * _ShadowBrightness) * _Color;
				fixed4 ambient = UNITY_LIGHTMODEL_AMBIENT  * _Color * col;

				fixed rimValue = 1 - dot(worldNormal,worldViewDir);
				fixed rimStep = smoothstep(-w + _RimThreshold, w + _RimThreshold, rimValue);
				fixed4 rim = _LightColor0 * rimStep * 0.5 * diffStep * _RimColor * RS.y;

				fixed4 emis = tex2D(_EmisTex, i.uv) * _EmisColor;

				fixed4 final = ambient * _AmbientPower + diffuse * _DiffusePower + rim + specular + emis;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, final);
				final.w = col.w;
				return final;
			}
			ENDCG
		}

	}
	FallBack "Diffuse"
}
