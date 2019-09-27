Shader "Custom/Out_Glow"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture"n 2D) * "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_GlowScale("Glow Scale", Range(0,1)) = 1
		_GlowColor("Glow Color") = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

		SubShader
		{
			Tangs
			{
				"Queue" = "Transparent"
				"IgnoreProjextor" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Cull Off
			Lighing Off
			ZWirte Off
			Blend One OneMinusSrcAlpha

			Pass
			{
			CGPROGRAM
				#pragma vertex vert

			}
		}
}
