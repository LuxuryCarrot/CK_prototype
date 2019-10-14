Shader "Custom/NewSurfaceShader"
{
    Properties
    {
		[PerRendererData] _SpriteTex ("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags
		{
			"RenderType"="Opaque"
		}

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };


        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

        }
        ENDCG
    }
    FallBack "Diffuse"
}
