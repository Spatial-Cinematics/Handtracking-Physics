Shader "Custom/BumpDiffuseStencil"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Bump ("Normal", 2D) = "bump" {}
        _RimColor ("Rim Color", Color) = (.5, .5, 1, 1)
        _RimPower ("Rim Power", Range (.5, 8)) = 3
        _Slider ("Bump Amount", Range (0,5)) = 1

        _SRef ("Stencil Ref", Float) = 1
        [Enum(UnityEngine.Rendering.CompareFunction)]
        _SComp("Stencil Comp", Float) = 8
        [Enum(UnityEngine.Rendering.StencilOp)]
        _SOp ("Stencil Op", Float) = 2

    }
    SubShader
    {

        Stencil{
            Ref [_SRef]
            Comp[_SComp]
            Pass[_SOp]
        }

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        sampler2D _Bump;
        float4 _RimColor;
        float _RimPower;
        half _Slider;
        float4 _Color;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_Bump;
            float3 viewDir;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Normal = UnpackNormal(tex2D(_Bump, IN.uv_Bump)) * float3(_Slider, _Slider, 1);

            half rim = 1 - saturate(dot(normalize(IN.viewDir), o.Normal));
            o.Emission = _RimColor.rgb * pow (rim, _RimPower);

        }
        ENDCG
    }
    FallBack "Diffuse"
}
