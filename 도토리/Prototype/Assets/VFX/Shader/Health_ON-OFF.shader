Shader "Custom/Health_ON-OFF"
{
    Properties
    {
        _FullTex ("Full", 2D) = "white" {}
	    _LowTex ("Low", 2D) = "white" {}
		_ONOFF("On / Off", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        #pragma target 3.0
        sampler2D _FullTex;
		sampler2D _LowTex;

        struct Input
        {
            float2 uv_FullTex;
			float2 uv_LowTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_FullTex, IN.uv_FullTex);
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
