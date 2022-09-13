// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "OnlyGlitter"
{
	Properties
	{
		_BasesColor("Bases Color", Color) = (0,0,0,0)
		_ColorMixVSColor("ColorMixVSColor", Range( 0 , 1)) = 0
		[Header(Color Mix)][Space]_MixColorA("Mix Color A", Color) = (0,0,0,0)
		_MixColorB("Mix Color B", Color) = (0,0,0,0)
		_MixColorC("Mix Color C", Color) = (0,0,0,0)
		_ColorsOffset("Colors Offset", Range( 0 , 0.5)) = 0
		_ColorsFalloff("Colors Falloff", Range( 0 , 0.5)) = 0
		[Header(Noise)][Sapce]_ColorNoiseStrength("Color Noise Strength", Range( 0 , 0.2)) = 0
		_ColorNoiseScale("Color NoiseS cale", Float) = 1
		[Header(Coat Spec)][Space]_SpecularA("Specular A", Range( 0 , 1)) = 0
		_SpecularB("Specular B", Range( 0 , 1)) = 0
		_SmoothnessA("Smoothness A", Range( 0 , 1)) = 0
		_SmoothnessB("Smoothness B", Range( 0 , 1)) = 0
		_SpecularColor("Specular Color", Color) = (0,0,0,0)
		_Scale("Scale", Float) = 1
		_Falloff("Falloff", Float) = 1
		[Header(Glitter)][NoScaleOffset][Space]_Gliter("Gliter", 2D) = "white" {}
		[Toggle(_USEGLITER_ON)] _UseGliter("UseGliter", Float) = 0
		_GlitterXScale("Glitter X Scale", Float) = 1
		_GlitterYScale("Glitter Y Scale", Float) = 1
		[HDR]_GlitterColor("Glitter Color", Color) = (0,0,0,0)
		_GlitterContrust("Glitter Contrust", Range( 1 , 2)) = 1
		_GlitterBrightness("Glitter Brightness", Range( 1 , 2)) = 1
		[Header(Spec 2)][Space]_Spec2Mix("Spec 2 Mix", Range( 0 , 1)) = 0
		_Spec2Shininess("Spec 2 Shininess", Range( 0 , 10)) = 0
		_Spec2Falloff("Spec 2 Falloff", Range( -1 , 1)) = 0
		[Header(Color Maps)][NoScaleOffset][Space]_BaseColorMap("Base Color Map", 2D) = "white" {}
		[NoScaleOffset]_SpecularColorMap("Specular Color Map", 2D) = "white" {}
		[NoScaleOffset]_GlitterColorMap("Glitter Color Map", 2D) = "white" {}
		[NoScaleOffset]_GlitterMask("Glitter Mask", 2D) = "white" {}
		[NoScaleOffset]_Stamp("Stamp", 2D) = "black" {}
		_StampVisability("StampVisability", Range( 0 , 1)) = 0
		_NormalMap("NormalMap", 2D) = "bump" {}
		_NormalScale("NormalScale", Range( 0 , 10)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_local __ _USEGLITER_ON
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform float _NormalScale;
		uniform float4 _BasesColor;
		uniform sampler2D _BaseColorMap;
		uniform float4 _MixColorA;
		uniform float4 _MixColorB;
		uniform float _ColorNoiseStrength;
		uniform float _ColorNoiseScale;
		uniform float _ColorsOffset;
		uniform float _ColorsFalloff;
		uniform float4 _MixColorC;
		uniform float _ColorMixVSColor;
		uniform sampler2D _Stamp;
		uniform float _StampVisability;
		uniform float4 _GlitterColor;
		uniform float _Spec2Falloff;
		uniform float _Spec2Shininess;
		uniform float _Spec2Mix;
		uniform sampler2D _Gliter;
		uniform float _GlitterXScale;
		uniform float _GlitterYScale;
		uniform float _GlitterContrust;
		uniform float _GlitterBrightness;
		uniform sampler2D _GlitterMask;
		uniform sampler2D _GlitterColorMap;
		uniform float4 _SpecularColor;
		uniform sampler2D _SpecularColorMap;
		uniform float _Scale;
		uniform float _Falloff;
		uniform float _SpecularA;
		uniform float _SpecularB;
		uniform float _SmoothnessA;
		uniform float _SmoothnessB;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
			float3 normalOut245 = UnpackScaleNormal( tex2D( _NormalMap, uv_NormalMap ), _NormalScale );
			o.Normal = normalOut245;
			float2 uv_BaseColorMap231 = i.uv_texcoord;
			float4 tex2DNode231 = tex2D( _BaseColorMap, uv_BaseColorMap231 );
			float simplePerlin2D54 = snoise( i.uv_texcoord*_ColorNoiseScale );
			float noise67 = ( _ColorNoiseStrength * simplePerlin2D54 );
			float temp_output_83_0 = ( 0.5 - _ColorsOffset );
			float falloff14 = _ColorsFalloff;
			float falloffMinA31 = ( temp_output_83_0 - falloff14 );
			float falloffMaxA33 = ( temp_output_83_0 + falloff14 );
			float smoothstepResult51 = smoothstep( 0.0 , 1.0 , saturate( (0.0 + (( noise67 + i.uv_texcoord.x ) - falloffMinA31) * (1.0 - 0.0) / (falloffMaxA33 - falloffMinA31)) ));
			float rampA40 = smoothstepResult51;
			float4 lerpResult45 = lerp( _MixColorA , _MixColorB , rampA40);
			float temp_output_85_0 = ( _ColorsOffset + 0.5 );
			float falloffMinB61 = ( temp_output_85_0 - falloff14 );
			float falloffMaxB62 = ( temp_output_85_0 + falloff14 );
			float smoothstepResult75 = smoothstep( 0.0 , 1.0 , saturate( (0.0 + (( noise67 + i.uv_texcoord.x ) - falloffMinB61) * (1.0 - 0.0) / (falloffMaxB62 - falloffMinB61)) ));
			float rampB78 = smoothstepResult75;
			float4 lerpResult79 = lerp( lerpResult45 , _MixColorC , rampB78);
			float4 colorMixOut47 = lerpResult79;
			float4 lerpResult259 = lerp( ( ( _BasesColor * _BasesColor.a ) * tex2DNode231 ) , ( tex2DNode231 * colorMixOut47 ) , _ColorMixVSColor);
			float2 uv_Stamp273 = i.uv_texcoord;
			float4 tex2DNode273 = tex2D( _Stamp, uv_Stamp273 );
			float temp_output_296_0 = ( tex2DNode273.a * _StampVisability );
			float stampAlpha275 = temp_output_296_0;
			float4 lerpResult274 = lerp( lerpResult259 , tex2DNode273 , stampAlpha275);
			float4 _AlbedoColor261 = lerpResult274;
			o.Albedo = _AlbedoColor261.rgb;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float4 transform131 = mul(unity_ObjectToWorld,float4( ase_vertexNormal , 0.0 ));
			float3 appendResult137 = (float3(transform131.x , transform131.y , transform131.z));
			float3 normalizeResult147 = normalize( reflect( -ase_worldlightDir , appendResult137 ) );
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 normalizeResult143 = normalize( ase_worldViewDir );
			float dotResult154 = dot( normalizeResult147 , normalizeResult143 );
			float specular2Mask173 = pow( saturate( (0.0 + (dotResult154 - _Spec2Falloff) * (1.0 - 0.0) / (1.0 - _Spec2Falloff)) ) , _Spec2Shininess );
			float2 break119 = i.uv_texcoord;
			float2 appendResult128 = (float2(( break119.x * _GlitterXScale ) , ( break119.y * _GlitterYScale )));
			float2 GlitterUV132 = appendResult128;
			float4 tex2DNode148 = tex2D( _Gliter, GlitterUV132 );
			float fresnelNdotV112 = dot( ase_worldNormal, ase_worldlightDir );
			float fresnelNode112 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV112, 0.5 ) );
			float simplePerlin2D117 = snoise( i.uv_texcoord*5.0 );
			simplePerlin2D117 = simplePerlin2D117*0.5 + 0.5;
			float fresnelNdotV122 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode122 = ( -1.0 + simplePerlin2D117 * pow( 1.0 - fresnelNdotV122, 0.5 ) );
			float2 appendResult126 = (float2(saturate( ( 1.0 - fresnelNode112 ) ) , fresnelNode122));
			float simplePerlin2D129 = snoise( appendResult126*4.0 );
			simplePerlin2D129 = simplePerlin2D129*0.5 + 0.5;
			float glitterBands135 = simplePerlin2D129;
			float lerpResult153 = lerp( tex2DNode148.r , tex2DNode148.b , saturate( glitterBands135 ));
			#ifdef _USEGLITER_ON
				float staticSwitch290 = lerpResult153;
			#else
				float staticSwitch290 = 1.0;
			#endif
			float glitterTex172 = saturate( ( pow( staticSwitch290 , _GlitterContrust ) * _GlitterBrightness ) );
			float temp_output_191_0 = ( specular2Mask173 * glitterTex172 );
			float4 lerpResult207 = lerp( float4( 0,0,0,0 ) , _GlitterColor , saturate( (0.0 + (( ( specular2Mask173 * _Spec2Mix ) + temp_output_191_0 ) - 0.0) * (1.0 - 0.0) / (0.5 - 0.0)) ));
			float4 glitterColorOut218 = lerpResult207;
			float glitterMask282 = tex2D( _GlitterMask, i.uv_texcoord ).r;
			float4 lerpResult237 = lerp( float4( 0,0,0,0 ) , glitterColorOut218 , glitterMask282);
			float2 uv_GlitterColorMap236 = i.uv_texcoord;
			float4 _EmissionColor264 = ( lerpResult237 * tex2D( _GlitterColorMap, uv_GlitterColorMap236 ) );
			o.Emission = _EmissionColor264.rgb;
			float2 uv_SpecularColorMap226 = i.uv_texcoord;
			float fresnelNdotV201 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode201 = ( 0.0 + _Scale * pow( 1.0 - fresnelNdotV201, _Falloff ) );
			float lerpResult196 = lerp( _SpecularA , _SpecularB , glitterMask282);
			float specular198 = lerpResult196;
			float glitterSpecOut217 = ( temp_output_191_0 + ( fresnelNode201 * specular198 ) );
			float lerpResult233 = lerp( 0.0 , glitterSpecOut217 , glitterMask282);
			float4 _SpecularColor268 = ( ( _SpecularColor * tex2D( _SpecularColorMap, uv_SpecularColorMap226 ) ) * lerpResult233 );
			o.Specular = _SpecularColor268.rgb;
			float lerpResult240 = lerp( _SmoothnessA , _SmoothnessB , glitterMask282);
			float _Smoothness271 = lerpResult240;
			o.Smoothness = _Smoothness271;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardSpecular keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandardSpecular o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardSpecular, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18909
-1920;0;1920;1019;4992.771;-1036.085;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;108;-7052.868,1122.414;Inherit;False;2026.713;648.2758;Comment;12;135;129;126;122;121;117;116;114;113;112;110;109;Glitter Bands;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;109;-6896.932,1340.375;Inherit;False;Constant;_Float1;Float 1;16;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;111;-7037.168,1931.955;Inherit;False;1323.088;468.853;Comment;8;132;128;127;124;120;119;118;115;Glitter UV;1,1,1,1;0;0
Node;AmplifyShaderEditor.FresnelNode;112;-6659.392,1189.846;Inherit;False;Standard;WorldNormal;LightDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;113;-7022.3,1429.387;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;110;-6798.969,1496.77;Inherit;False;Constant;_scale;scale;13;0;Create;True;0;0;0;False;0;False;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;115;-6987.168,1984.756;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;114;-6788.625,1629.479;Inherit;False;Constant;_power;power;14;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;116;-6415.781,1192.668;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;117;-6589.321,1418.912;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;120;-6748.609,2221.032;Inherit;False;Property;_GlitterYScale;Glitter Y Scale;19;0;Create;True;0;0;0;False;0;False;1;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;119;-6697.992,1981.955;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.FresnelNode;122;-6304.788,1332.21;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;121;-6235.081,1200.043;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;118;-6746.607,2117.333;Inherit;False;Property;_GlitterXScale;Glitter X Scale;18;0;Create;True;0;0;0;False;0;False;1;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;123;-7057.176,70.93118;Inherit;False;2395.55;912.718;Comment;16;173;167;164;163;161;155;154;147;143;142;141;137;136;131;130;125;Glitter Spec;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;-6501.85,1983.631;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;124;-6493.469,2127.801;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;126;-6011.579,1285.109;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NormalVertexDataNode;125;-6998.054,385.7281;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;128;-6324.242,2053.976;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;129;-5568.658,1296.98;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;41;-9437.897,1337.321;Inherit;False;1823.947;848.5981;Comment;18;62;61;60;59;33;31;28;64;32;14;65;17;16;82;83;85;87;86;Ramp Falloff;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;130;-6866.162,139.5631;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;131;-6809.76,385.4474;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;132;-5938.08,2185.314;Inherit;False;GlitterUV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;135;-5302.454,1318.979;Inherit;False;glitterBands;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;133;-7042.357,2473.513;Inherit;False;1628.997;710.8497;Comment;13;152;146;139;148;138;172;170;165;160;156;153;290;291;Glitter Tex;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;42;-9447.882,55.871;Inherit;False;1409.64;1212.633;Comment;24;78;75;40;51;74;73;39;29;72;71;70;77;35;76;57;34;69;68;67;55;54;56;58;36;Ramp;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-9370.057,1597.45;Inherit;False;Property;_ColorsOffset;Colors Offset;5;0;Create;True;0;0;0;False;0;False;0;0.087;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-9400.299,334.3925;Inherit;False;Property;_ColorNoiseScale;Color NoiseS cale;8;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;36;-9402.135,210.6454;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;137;-6599.601,393.5392;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;138;-7031.22,2731.381;Inherit;False;132;GlitterUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;139;-7020.957,2979.751;Inherit;False;135;glitterBands;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;136;-6589.957,206.0133;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-9344.992,133.097;Inherit;False;Property;_ColorNoiseStrength;Color Noise Strength;7;1;[Header];Create;True;1;Noise;0;0;False;1;Sapce;False;0;0;0;0.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;86;-9019.819,1524.114;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-8857.095,1610.976;Inherit;False;Property;_ColorsFalloff;Colors Falloff;6;0;Create;True;0;0;0;False;0;False;0;0.083;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-8744.385,1416.365;Inherit;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;54;-9107.726,214.621;Inherit;False;Simplex2D;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;148;-6864.105,2721.512;Inherit;True;Property;_Gliter;Gliter;16;2;[Header];[NoScaleOffset];Create;True;1;Glitter;0;0;False;1;Space;False;-1;27734a44c603f2a4b9380852e981d1ce;27734a44c603f2a4b9380852e981d1ce;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ReflectOpNode;142;-6385.555,273.5802;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;141;-6734.195,782.3951;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SaturateNode;146;-6814.57,2983.909;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;83;-8546.896,1424.89;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-8874.681,164.5582;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;14;-8565.905,1610.124;Inherit;False;falloff;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;87;-9059.65,1755.467;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-8738.539,1871.473;Inherit;False;Constant;_ProgressionB;ProgressionB;2;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;147;-6209.87,271.3663;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;291;-6704.589,2642.33;Inherit;False;Constant;_gliterValue;gliterValue;42;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;143;-6247.974,791.8531;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;153;-6553.976,2796.792;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;290;-6397.589,2687.33;Inherit;False;Property;_UseGliter;UseGliter;17;0;Create;True;0;0;0;False;0;False;1;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;154;-5897.627,454.0153;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;32;-8299.456,1415.292;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;64;-8586.671,2026.964;Inherit;False;14;falloff;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;67;-8673.894,147.2432;Inherit;False;noise;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;85;-8539.572,1791.385;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-8296.078,1641.238;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;155;-5864.378,687.399;Inherit;False;Property;_Spec2Falloff;Spec 2 Falloff;25;0;Create;True;0;0;0;False;0;False;0;-1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;152;-6437.63,2947.23;Inherit;False;Property;_GlitterContrust;Glitter Contrust;21;0;Create;True;0;0;0;False;0;False;1;1.215;1;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;68;-9348.859,496.5275;Inherit;False;67;noise;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;160;-6298.086,3063.49;Inherit;False;Property;_GlitterBrightness;Glitter Brightness;22;0;Create;True;0;0;0;False;0;False;1;1.111;1;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;161;-5650.955,459.3541;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;59;-8301.396,1795.258;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;31;-8136.965,1409.156;Inherit;False;falloffMinA;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;69;-9374.835,589.4132;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;156;-6171.09,2656.893;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;60;-8298.02,2021.203;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;33;-8161.857,1639.119;Inherit;False;falloffMaxA;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;35;-9166.612,727.7702;Inherit;False;33;falloffMaxA;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;57;-9117.689,516.4611;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;34;-9164.826,629.6736;Inherit;False;31;falloffMinA;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;76;-9381.182,976.7233;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;285;-10379.41,2392.004;Inherit;False;849.4355;406.1511;Comment;4;157;283;284;282;Gliter Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.SaturateNode;163;-5363.361,459.2972;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;164;-5452.367,622.8551;Inherit;False;Property;_Spec2Shininess;Spec 2 Shininess;24;0;Create;True;0;0;0;False;0;False;0;3.19;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;62;-8163.799,2019.086;Inherit;False;falloffMaxB;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;165;-5985.088,2657.091;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;61;-8138.908,1789.122;Inherit;False;falloffMinB;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;77;-9355.207,883.8381;Inherit;False;67;noise;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;70;-9171.173,1016.983;Inherit;False;61;falloffMinB;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;71;-9124.037,903.7719;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;283;-10294.12,2639.155;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;157;-10329.41,2442.004;Inherit;True;Property;_GlitterMask;Glitter Mask;32;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TFHCRemapNode;29;-8910.764,561.1837;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;72;-9171.829,1115.081;Inherit;False;62;falloffMaxB;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;167;-5152.074,456.2562;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;170;-5839.113,2658.6;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;174;-7049.909,3232.128;Inherit;False;1801.687;1181.564;Comment;23;218;217;208;207;206;205;203;201;200;198;197;196;195;193;192;191;189;188;186;185;184;183;289;Glitter Comp;1,1,1,1;0;0
Node;AmplifyShaderEditor.SaturateNode;39;-8713.498,562.1305;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;73;-8917.113,948.4936;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;284;-10049.67,2507.5;Inherit;True;Property;_TextureSample0;Texture Sample 0;27;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;173;-4939.951,448.8102;Inherit;False;specular2Mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;172;-5671.834,2652.023;Inherit;False;glitterTex;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;184;-7012.681,3444.021;Inherit;False;Property;_Spec2Mix;Spec 2 Mix;23;1;[Header];Create;True;1;Spec 2;0;0;False;1;Space;False;0;0.344;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;185;-6986.177,3644.248;Inherit;False;172;glitterTex;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;186;-7014.221,3549.581;Inherit;False;173;specular2Mask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;74;-8719.846,949.4406;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;51;-8538.021,562.6085;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;282;-9753.979,2509.383;Inherit;False;glitterMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;183;-7015.493,3345.072;Inherit;False;173;specular2Mask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;188;-7000.76,4071.331;Inherit;False;Property;_SpecularB;Specular B;10;0;Create;True;1;Coat Spec;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;189;-6996.134,3980.929;Inherit;False;Property;_SpecularA;Specular A;9;1;[Header];Create;True;1;Coat Spec;0;0;False;1;Space;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;289;-6973.314,4205.833;Inherit;False;282;glitterMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;75;-8544.369,949.9184;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;40;-8330.341,551.0729;Inherit;False;rampA;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;192;-6652.477,3417.08;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;191;-6656.157,3592.265;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;52;-10694.89,38.55083;Inherit;False;1181.097;1018.902;Comment;8;47;80;44;43;79;45;81;46;Color Mix;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;193;-7003.361,3794.691;Inherit;False;Property;_Scale;Scale;14;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;197;-6403.73,3525.607;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;43;-10644.21,88.55083;Inherit;False;Property;_MixColorA;Mix Color A;2;1;[Header];Create;True;1;Color Mix;0;0;False;1;Space;False;0,0,0,0;0.09732112,0.764151,0.4230542,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;46;-10606.64,468.6353;Inherit;False;40;rampA;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;196;-6665.104,4027.337;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;78;-8336.688,938.3825;Inherit;False;rampB;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;195;-7000.093,3887.462;Inherit;False;Property;_Falloff;Falloff;15;0;Create;True;0;0;0;False;0;False;1;1.16;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;44;-10644.89,282.0685;Inherit;False;Property;_MixColorB;Mix Color B;3;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.5213439,0.2400765,0.6132076,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;45;-10295.22,212.6228;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;201;-6676.035,3765.665;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;198;-6418.392,4013.068;Inherit;False;specular;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;80;-10320,468.5361;Inherit;False;Property;_MixColorC;Mix Color C;4;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,0.6465514,0.1462264,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;200;-6215.048,3465.138;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.5;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;81;-10283.94,683.1124;Inherit;False;78;rampB;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;206;-6425.718,3308.341;Inherit;False;Property;_GlitterColor;Glitter Color;20;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0,0.2980392,0.8313726,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;205;-5988.588,3480.712;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;203;-6198.195,3809.631;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;260;-4506.458,124.2837;Inherit;False;1674.009;917.7014;Comment;19;257;258;247;234;238;255;231;256;259;249;261;273;274;275;295;296;297;298;299;Albedo;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;79;-9977.399,484.7611;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;234;-4484,306.4275;Inherit;False;Property;_BasesColor;Bases Color;0;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.06666667,0.1568628,0.282353,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;47;-9765.377,481.6054;Inherit;False;colorMixOut;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;208;-6020.652,3787.413;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;207;-5812.017,3394.158;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;218;-5637.846,3383.171;Inherit;False;glitterColorOut;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;257;-4473.844,757.9648;Inherit;False;47;colorMixOut;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;231;-4476.509,532.3552;Inherit;True;Property;_BaseColorMap;Base Color Map;29;2;[Header];[NoScaleOffset];Create;True;1;Color Maps;0;0;False;1;Space;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;267;-4907.908,1789.519;Inherit;False;1566.41;808.1072;Comment;8;235;219;226;227;233;248;268;287;SpecularColor;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;217;-5848.535,3782.598;Inherit;False;glitterSpecOut;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;299;-4266.479,380.0812;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;263;-4911.498,1120.274;Inherit;False;1612.006;603.9673;Comment;6;221;236;237;246;264;286;Emission;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;295;-4101.806,943.7961;Inherit;False;Property;_StampVisability;StampVisability;34;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;273;-4107.07,744.129;Inherit;True;Property;_Stamp;Stamp;33;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;9a5d215afc1babd4a8b8da2167ae28f0;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;258;-4140.597,555.1017;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;256;-4486.467,832.2537;Inherit;False;Property;_ColorMixVSColor;ColorMixVSColor;1;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;249;-4125.299,430.1971;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;202;-5197.366,3790.716;Inherit;False;1462.473;602.3887;Comment;4;245;292;293;294;Normal;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;226;-4642.741,2111.54;Inherit;True;Property;_SpecularColorMap;Specular Color Map;30;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;270;-4867.752,2666.138;Inherit;False;1605.009;455.1695;Comment;6;271;241;240;230;229;288;Smoothness;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;227;-4549.857,2308.037;Inherit;False;217;glitterSpecOut;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;219;-4562.051,1940.05;Inherit;False;Property;_SpecularColor;Specular Color;13;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.7028302,0.9235162,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;296;-3809.504,903.9173;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;221;-4520.61,1183.643;Inherit;False;218;glitterColorOut;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;287;-4740.617,2352.988;Inherit;False;282;glitterMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;286;-4565.052,1309.313;Inherit;False;282;glitterMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;259;-3973.111,508.8856;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;292;-5112.273,3868.342;Inherit;True;Property;_NormalMap;NormalMap;35;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;bump;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;230;-4422.551,2725.741;Inherit;False;Property;_SmoothnessA;Smoothness A;11;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;288;-4325.759,2907.498;Inherit;False;282;glitterMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;235;-4316.579,1979.828;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;294;-5081.833,4066.12;Inherit;False;Property;_NormalScale;NormalScale;36;0;Create;True;0;0;0;False;0;False;0;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;233;-4335.237,2306.998;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;275;-3416.519,680.3787;Inherit;False;stampAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;237;-4273.002,1260.443;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;236;-4616.599,1464.031;Inherit;True;Property;_GlitterColorMap;Glitter Color Map;31;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;229;-4421.622,2825.676;Inherit;False;Property;_SmoothnessB;Smoothness B;12;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;246;-4083.036,1383.239;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;240;-4089.486,2834.572;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.UnpackScaleNormalNode;293;-4621.786,3977.516;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;248;-4143.501,2091.906;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;274;-3243.797,502.8139;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;264;-3596.582,1376.369;Inherit;False;_EmissionColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;261;-3037.226,473.5244;Inherit;False;_AlbedoColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;245;-4010.759,4011.375;Inherit;False;normalOut;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;268;-3536.713,2084.558;Inherit;False;_SpecularColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;271;-3512.538,2827.952;Inherit;False;_Smoothness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;269;-3059.786,2098.942;Inherit;False;268;_SpecularColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;241;-4126.382,2725.886;Inherit;False;Property;_FrenchSmoothness;French Smoothness;28;0;Create;True;0;0;0;False;0;False;0;0.96;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;298;-3669.806,791.9647;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;254;-3039.763,1925.237;Inherit;False;245;normalOut;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;262;-3035.908,1833.335;Inherit;False;261;_AlbedoColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;272;-3040.952,2185.534;Inherit;False;271;_Smoothness;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;297;-3501.017,825.6931;Inherit;False;Property;_FrenchAlwaysVisable;FrenchAlwaysVisable;27;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;255;-3704.006,429.7993;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;266;-3044.282,2007.503;Inherit;False;264;_EmissionColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;238;-3972.822,321.321;Inherit;False;Property;_FrenchColor;French Color;26;1;[Header];Create;True;1;French;0;0;False;1;Space;False;0,0,0,0;1,0.9429423,0.9009434,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;247;-3949.794,653.3878;Inherit;False;-1;;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-2695.142,1926.224;Float;False;True;-1;2;ASEMaterialInspector;0;0;StandardSpecular;OnlyGlitter;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;16;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;112;3;109;0
WireConnection;116;0;112;0
WireConnection;117;0;113;0
WireConnection;117;1;110;0
WireConnection;119;0;115;0
WireConnection;122;2;117;0
WireConnection;122;3;114;0
WireConnection;121;0;116;0
WireConnection;127;0;119;0
WireConnection;127;1;118;0
WireConnection;124;0;119;1
WireConnection;124;1;120;0
WireConnection;126;0;121;0
WireConnection;126;1;122;0
WireConnection;128;0;127;0
WireConnection;128;1;124;0
WireConnection;129;0;126;0
WireConnection;131;0;125;0
WireConnection;132;0;128;0
WireConnection;135;0;129;0
WireConnection;137;0;131;1
WireConnection;137;1;131;2
WireConnection;137;2;131;3
WireConnection;136;0;130;0
WireConnection;86;0;82;0
WireConnection;54;0;36;0
WireConnection;54;1;58;0
WireConnection;148;1;138;0
WireConnection;142;0;136;0
WireConnection;142;1;137;0
WireConnection;146;0;139;0
WireConnection;83;0;16;0
WireConnection;83;1;86;0
WireConnection;55;0;56;0
WireConnection;55;1;54;0
WireConnection;14;0;17;0
WireConnection;87;0;82;0
WireConnection;147;0;142;0
WireConnection;143;0;141;0
WireConnection;153;0;148;1
WireConnection;153;1;148;3
WireConnection;153;2;146;0
WireConnection;290;1;291;0
WireConnection;290;0;153;0
WireConnection;154;0;147;0
WireConnection;154;1;143;0
WireConnection;32;0;83;0
WireConnection;32;1;14;0
WireConnection;67;0;55;0
WireConnection;85;0;87;0
WireConnection;85;1;65;0
WireConnection;28;0;83;0
WireConnection;28;1;14;0
WireConnection;161;0;154;0
WireConnection;161;1;155;0
WireConnection;59;0;85;0
WireConnection;59;1;64;0
WireConnection;31;0;32;0
WireConnection;156;0;290;0
WireConnection;156;1;152;0
WireConnection;60;0;85;0
WireConnection;60;1;64;0
WireConnection;33;0;28;0
WireConnection;57;0;68;0
WireConnection;57;1;69;1
WireConnection;163;0;161;0
WireConnection;62;0;60;0
WireConnection;165;0;156;0
WireConnection;165;1;160;0
WireConnection;61;0;59;0
WireConnection;71;0;77;0
WireConnection;71;1;76;1
WireConnection;29;0;57;0
WireConnection;29;1;34;0
WireConnection;29;2;35;0
WireConnection;167;0;163;0
WireConnection;167;1;164;0
WireConnection;170;0;165;0
WireConnection;39;0;29;0
WireConnection;73;0;71;0
WireConnection;73;1;70;0
WireConnection;73;2;72;0
WireConnection;284;0;157;0
WireConnection;284;1;283;0
WireConnection;173;0;167;0
WireConnection;172;0;170;0
WireConnection;74;0;73;0
WireConnection;51;0;39;0
WireConnection;282;0;284;1
WireConnection;75;0;74;0
WireConnection;40;0;51;0
WireConnection;192;0;183;0
WireConnection;192;1;184;0
WireConnection;191;0;186;0
WireConnection;191;1;185;0
WireConnection;197;0;192;0
WireConnection;197;1;191;0
WireConnection;196;0;189;0
WireConnection;196;1;188;0
WireConnection;196;2;289;0
WireConnection;78;0;75;0
WireConnection;45;0;43;0
WireConnection;45;1;44;0
WireConnection;45;2;46;0
WireConnection;201;2;193;0
WireConnection;201;3;195;0
WireConnection;198;0;196;0
WireConnection;200;0;197;0
WireConnection;205;0;200;0
WireConnection;203;0;201;0
WireConnection;203;1;198;0
WireConnection;79;0;45;0
WireConnection;79;1;80;0
WireConnection;79;2;81;0
WireConnection;47;0;79;0
WireConnection;208;0;191;0
WireConnection;208;1;203;0
WireConnection;207;1;206;0
WireConnection;207;2;205;0
WireConnection;218;0;207;0
WireConnection;217;0;208;0
WireConnection;299;0;234;0
WireConnection;299;1;234;4
WireConnection;258;0;231;0
WireConnection;258;1;257;0
WireConnection;249;0;299;0
WireConnection;249;1;231;0
WireConnection;296;0;273;4
WireConnection;296;1;295;0
WireConnection;259;0;249;0
WireConnection;259;1;258;0
WireConnection;259;2;256;0
WireConnection;235;0;219;0
WireConnection;235;1;226;0
WireConnection;233;1;227;0
WireConnection;233;2;287;0
WireConnection;275;0;296;0
WireConnection;237;1;221;0
WireConnection;237;2;286;0
WireConnection;246;0;237;0
WireConnection;246;1;236;0
WireConnection;240;0;230;0
WireConnection;240;1;229;0
WireConnection;240;2;288;0
WireConnection;293;0;292;0
WireConnection;293;1;294;0
WireConnection;248;0;235;0
WireConnection;248;1;233;0
WireConnection;274;0;259;0
WireConnection;274;1;273;0
WireConnection;274;2;275;0
WireConnection;264;0;246;0
WireConnection;261;0;274;0
WireConnection;245;0;293;0
WireConnection;268;0;248;0
WireConnection;271;0;240;0
WireConnection;298;0;247;0
WireConnection;298;1;296;0
WireConnection;297;0;296;0
WireConnection;297;1;298;0
WireConnection;255;0;238;0
WireConnection;255;1;259;0
WireConnection;0;0;262;0
WireConnection;0;1;254;0
WireConnection;0;2;266;0
WireConnection;0;3;269;0
WireConnection;0;4;272;0
ASEEND*/
//CHKSM=23039CDF29C0AF69FD0431AC8A7817BB8EBA719B