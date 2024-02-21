Shader "Naxeex/NaxeexBRDF" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		[HideInInspector] [NoScaleOffset] _Cube ("Reflection Cubemap", Cube) = "_Skybox" {}
		[HideInInspector] _StaticReflectionIntensity ("Static Reflection Intensity", Float) = 1
		[HideInInspector] _CubeReflectionMin ("Cube Reflection Min", Float) = 0.05
		[HideInInspector] _DiffColor ("Main Color For Replace", Vector) = (1,1,1,1)
		[HideInInspector] _DiffColor1 ("Second Color For Replace", Vector) = (1,1,1,1)
		[HideInInspector] _ColorVar0 ("Color 0", Vector) = (0,0,0,1)
		[HideInInspector] _ColorVar1 ("Color 1", Vector) = (0,0,0,1)
		[NoScaleOffset] _BRDFTex ("NdotL NdotH (RGBA)", 2D) = "white" {}
		[NoScaleOffset] _MaskTex ("MASK (R - Main Color G - Second Color, B - Gloss, A - Reflect)", 2D) = "black" {}
		[Header(Color options)] [Toggle(MULCOLOR_ON)] _MulColorOn ("Multiply color", Float) = 0
		[HideInInspector] _Rimlightcolor ("Rimlight color", Vector) = (0,0.8344827,1,1)
		[HideInInspector] _Rimlightscale ("Rimlight scale", Float) = 1
		[HideInInspector] _Rimlightbias ("Rimlight bias", Float) = 0
		[HideInInspector] _FreezeTex ("Freeze Effect Texture", 2D) = "black" {}
		[HideInInspector] _FreezeAmount ("Freeze Effect Amount", Range(0, 1)) = 0
		[HideInInspector] _FreezeDiffMin ("Freeze Diffuse min", Range(0, 1)) = 0.25
		[HideInInspector] _UVOffsetX ("UV Offset X", Float) = 0
		[HideInInspector] _UVOffsetY ("UV Offset Y", Float) = 0
	}
	SubShader {
		LOD 900
		Tags { "LIGHTMODE" = "FORWARDBASE" "PASSFLAGS" = "OnlyDirectional" "PerformanceChecks" = "False" "QUEUE" = "Geometry+10" "RenderType" = "Opaque" }
		UsePass "Hidden/Naxeex/NaxeexBRDF_Ultra/BASE"
		UsePass "VertexLit/SHADOWCASTER"
	}
	SubShader {
		LOD 800
		Tags { "LIGHTMODE" = "FORWARDBASE" "PASSFLAGS" = "OnlyDirectional" "PerformanceChecks" = "False" "QUEUE" = "Geometry+10" "RenderType" = "Opaque" }
		UsePass "Hidden/Naxeex/NaxeexBRDF_High/BASE"
	}
	SubShader {
		LOD 700
		Tags { "FORCENOSHADOWCASTING" = "true" "LIGHTMODE" = "FORWARDBASE" "PASSFLAGS" = "OnlyDirectional" "PerformanceChecks" = "False" "QUEUE" = "Geometry+10" "RenderType" = "Opaque" }
		UsePass "Hidden/Naxeex/NaxeexBRDF_Mid/BASE"
	}
	SubShader {
		LOD 600
		Tags { "FORCENOSHADOWCASTING" = "true" "LIGHTMODE" = "FORWARDBASE" "PASSFLAGS" = "OnlyDirectional" "PerformanceChecks" = "False" "QUEUE" = "Geometry+10" "RenderType" = "Opaque" }
		UsePass "Hidden/Naxeex/NaxeexBRDF_Low/BASE"
	}
	Fallback "Unlit/Texture"
	CustomEditor "Naxeex.Shaders.Editor.NaxeexBRDFShaderGUI"
}