Shader "CStamp/Water"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Tint ("Color Tint", Color) = (.3, .6, 1, 1)
        _Freq ("Frequency", Range (0, 5)) = 3
        _Speed ("Speed", Range (0,100)) = 10
        _Amp ("Amplitude", Range(0,1)) = 0.5
    }
    SubShader
    {

        Tags {"Queue" = "AlphaTest"}

        //ZWrite Off
        CGPROGRAM
        #pragma surface surf Lambert vertex:vert

        struct Input
        {
            float2 uv_MainTex;
            float3 vertColor;
        };

        float4 _Tint;
        float _Freq;
        float _Speed;
        float _Amp;

        struct appdata {
            float4 vertex : POSITION0;
            float3 normal : NORMAL;
            float4 texcoord : TEXCOORD0;
            float4 texcoord1 : TEXCOORD1;
            float4 texcoord2 : TEXCOORD2;
        };

        void vert (inout appdata v, out Input o){
            UNITY_INITIALIZE_OUTPUT(Input, o);
            float t = _Time * _Speed;
            float waveHeight = sin(t + v.vertex.x * _Freq) * _Amp;
            v.vertex.y = v.vertex.y + waveHeight;
            v.normal = normalize(float3(v.normal.x + waveHeight, v.normal.y, v.normal.z));
            o.vertColor = waveHeight + 2;
        }

        sampler2D _MainTex;

        void surf (Input IN, inout SurfaceOutput o)
        {
            float2 uv = IN.uv_MainTex + float2 (sin(cos(-_Time.x)), sin(_Time.x));
            float4 c = tex2D (_MainTex, uv);
            o.Albedo = c * IN.vertColor.rgb * _Tint;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
