Shader "CStamp/Borderlands"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Main Texture", 2D) = "white" {}
        _RampTex ("Ramp Texture", 2D) = "white" {}
        _Normal ("Normal Map", 2D) = "white" {}
        _normInt ("Normal Intensity", Range(0,1)) = 1
        _OutlineColor("Outline Color", Color) = (0,0,0,0)
        _OutlinePower("Outline Power", Range (.0002 , .02)) = .01
    }
    SubShader
    {

        Tags {"Queue" = "Transparent"}

        ZWrite Off
        CGPROGRAM
        #pragma surface surf Lambert
        #pragma vertex vert

        struct Input
        {
            float2 uv_MainTex;
        };

        float _OutlinePower;
        float4 _OutlineColor;

        void vert (inout appdata_full v){
            v.vertex.xyz += v.normal * _OutlinePower;
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Emission = _OutlineColor.rgb;
            o.Albedo = _OutlineColor.rgb;
        }

        ENDCG

        ZWrite On
        CGPROGRAM
        #pragma surface surf ToonRamp

        fixed4 _Color;
        sampler2D _MainTex;
        sampler2D _RampTex;
        sampler2D _Normal;
        half _normInt;

        half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, half atten){
            float diff = dot (s.Normal, lightDir);
            float h = diff * 0.5 + 0.5;
            float2 rh = h;
            float3 ramp = tex2D(_RampTex, rh).rgb;

            float4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * ramp;
            c.a = s.Alpha;
            return c;
        }

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_MainTex));
            o.Normal *= float3(_normInt, _normInt, 1);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
