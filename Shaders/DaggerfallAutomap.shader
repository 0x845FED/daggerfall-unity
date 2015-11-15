﻿// Project:         Daggerfall Tools For Unity
// Copyright:       Copyright (C) 2009-2015 Gavin Clayton
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Web Site:        http://www.dfworkshop.net
// Contact:         Gavin Clayton (interkarma@dfworkshop.net)
// Project Page:    https://github.com/Interkarma/daggerfall-unity
// Contributors:    Nystul
// 
// Notes:
//

Shader "Daggerfall/Automap" {
	// Surface Shader for Automap Geometry
	Properties {
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo Map", 2D) = "white" {}
		_BumpMap("Normal Map", 2D) = "bump" {}
		_EmissionMap("Emission Map", 2D) = "white" {}
		_EmissionColor("Emission Color", Color) = (0,0,0)
	}
	SubShader {
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 200
		
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard

		half4 _Color;
		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _EmissionMap;
		half4 _EmissionColor;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float2 uv_EmissionMap;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
			half4 albedo = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			half3 emission = tex2D(_EmissionMap, IN.uv_EmissionMap).rgb * _EmissionColor;
			o.Albedo = albedo.rgb - emission; // Emission cancels out other lights
			o.Alpha = albedo.a;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Emission = emission;
			o.Metallic = 0;
			o.Albedo.r +=0.2f;
			//o.Albedo = half3(1.0f, 1.0f, 0.0f);
		}
		ENDCG
	} 
	FallBack "Standard"
}