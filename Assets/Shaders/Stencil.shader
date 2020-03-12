Shader "Custom/Stencil"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}

        _SRef ("Stencil Ref", Float) = 1
        [Enum(UnityEngine.Rendering.CompareFunction)]
        _SComp("Stencil Comp", Float) = 8
        [Enum(UnityEngine.Rendering.StencilOp)]
        _SOp ("Stencil Op", Float) = 2

    }
    SubShader
    {

        ColorMask 0
        ZWrite Off

        Stencil{
            Ref [_SRef]
            Comp[_SComp]
            Pass[_SOp]
        }

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
