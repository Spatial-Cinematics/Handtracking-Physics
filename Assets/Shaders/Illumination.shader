Shader "CStamp/Illumination"
{
    Properties
    {
        _myCube ("Example Cube", CUBE) = "" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        samplerCUBE _myCube;

        struct Input
        {
            float3 worldRefl;
        };


        void surf (Input IN, inout SurfaceOutputStandard o)
        {
           // o.Albedo = texCube(_myCube, IN.worldRefl).rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
