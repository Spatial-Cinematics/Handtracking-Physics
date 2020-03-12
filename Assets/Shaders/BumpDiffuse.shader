﻿Shader "CStamp/BumpDiffuse"
{
    Properties
    {
        _myDiffuse ("Diffuse Texture", 2D) = "white" {}
        _myBump ("Bump Texture", 2D) = "bump" {}
        _mySlider ("Bump Amount", Range(0,10)) = 1
        _myBrightness ("Brightness", Range(0,1)) = 1
        _myCube ("Example Cube", CUBE) = "white" {}
    }
    SubShader
    {
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _myDiffuse;
        sampler2D _myBump;
        half _mySlider;
        half _myBrightness;
        samplerCUBE _myCube;

        struct Input
        {
            float2 uv_myDiffuse;
            float2 uv_myBump;
            float3 worldRefl; INTERNAL_DATA
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = tex2D(_myDiffuse, IN.uv_myDiffuse).rgb;
            o.Normal = UnpackNormal(tex2D(_myBump, IN.uv_myBump)) *  _myBrightness;
            o.Normal *= float3(_mySlider, _mySlider, 1);
            o.Emission = texCUBE(_myCube, WorldReflectionVector(IN, o.Normal)).rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
