shader "Morph"
{
    Properties
    {
        [Toggle] _RecieveLight ("Reiceve Light", Float) = 0
        _Color ("Color", Color) = (1,1,1,1)
        _RimColor ("Rim Color", Color) = (.5, .5, 1, 1)
        _RimPower ("Rim Power", Range (.5, 8)) = 3
        _Intensity ("Rim Intensity", Range (1, 5)) = 3
        _Warp ("Affector Strength", Range (0, 1)) = 1
        _Affector ("_Affector", vector) = (0,0,0)
        [Toggle] _Reverse ("Push", float) = 0
    }
    SubShader
    {

        Tags { "Queue" = "Transparent"}

        CGPROGRAM
        #pragma surface surf NoLighting vertex:vert alpha:fade

        float _RecieveLight;

        half4 LightingNoLighting (SurfaceOutput s, half3 lightDir, half atten){
            half NdotL = dot(s.Normal, lightDir); //normal light value
            half4 c;
            c.rgb = s.Albedo;
            c.rbg *= _RecieveLight == 0 ? 1 : (NdotL * atten);
            c.a = s.Alpha;
            return c;
        }

        float4 _Color;
        float4 _RimColor;
        float _RimPower;
        half _Intensity;

        struct Input
        {
            float3 viewDir;
            float3 worldPos;
        };
        struct appdata {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float4 TEXCOORD : TEXCOORD0;
        };

        int _NodesLength;
        float _Trans;
        float _Warp;
        float3 _Affector;
        fixed _Reverse;

        void vert (inout appdata i){

            float3 worldPos = mul (unity_ObjectToWorld, i.vertex);
            float strength = _Warp - distance(worldPos, _Affector);
            float3 dir = //_//Reverse == 0 ? 
                //(normalize(_Affector - worldPos)) :
                (normalize(worldPos - _Affector));

            float3 warp = strength * dir;
            //warp *= (dot(dir, i.normal) + 1);
            i.vertex.xyz += mul (warp, unity_ObjectToWorld).xyz;
            
        }

        void surf (Input IN, inout SurfaceOutput o)
        {

            half rim = 1 - saturate(dot(normalize(IN.viewDir), o.Normal));

            o.Albedo = _Color.rgb;
            o.Emission = _RimColor.rgb * pow (rim, _RimPower) * _Intensity;
            o.Alpha = _Color.a;
            
        }
        ENDCG
    }
    FallBack "Diffuse"
}