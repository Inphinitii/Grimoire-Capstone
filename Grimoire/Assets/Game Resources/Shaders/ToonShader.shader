Shader "Custom/ToonShader" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_BrightColor("Bright Color", Color) = (0,0,0,0)
		_DarkColor("Dark Color", Color) = (0,0,0,0)
		_ThresholdBright("Threshhold Bright To Dark", range(0,1)) = 0.2
		_ThresholdDark("Threshold Middle to Dark", range(0,1)) = 0.9
		_TransitonTexture("TransitionTexture", 2D) = "white" {}
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Toon
		#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}

		half4 LightingToon(SurfaceOutput s, half3 direction, half attend)
		{
			dir = normalize(dir); //Normalize the light direction.
			half DotLightNormal = saturate ( dot (s.Normal, dir));

			half4 colorOutput;

			//Add our calculations to the Surf output and combine them.
			colorOutput.rgb = ((DotLightNormal * 0.2f) + 0.8f) * s.Albedo * _LightColor0;
			colorOutput.a = s.Alpha;
			return colorOutput;
		}

		ENDCG
	} 
	FallBack "Diffuse"
}
