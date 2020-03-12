Shader "CStamp/BasicLambert"
{
    Properties
    {
        _MainTex ("_MainTex", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _SpecColor("Specular Color", Color) = (1,1,1,1)
        _Decal ("Decal Texture", 2D) = "white" {}
        [Toggle] _ShowDecal ("Show Decal", Float) = 0
        _Spec("Specular", Range (0,1)) = 0.5
        _Gloss("Gloss", Range(0,1)) = 0.5 //power you apply to specular
    }
    SubShader
    {

        Tags {
            "Queue" = "Geometry"
        }

        CGPROGRAM
        //#pragma surface surf Lambert (included in Unity)
        // Custom lighting:
        #pragma surface surf BasicLambert

        half4 LightingBasicLambert (SurfaceOutput s, half3 lightDir, half atten){
            //attenuation for light is its intensity: loss of intensity as it travels
            half NdotL = dot(s.Normal, lightDir); //normal light value
            half4 c; //color that should appear on the surface
            c.rgb = s.Albedo * _LightColor0.rbg * (NdotL * atten);
            c.a = s.Alpha;
            return c;
        }

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;
        half _Spec;
        fixed _Gloss;
        sampler2D _Decal;
        sampler2D _MainTex;
        float _ShowDecal;
        //_SpecColor is defined by Unity

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            //o.Albedo = _Color.rgb;
            //apply specular and gloss to surface output
            o.Specular = _Spec;
            o.Gloss = _Gloss;
           
           fixed4 main = tex2D (_MainTex, IN.uv_MainTex);
           fixed4 decal = tex2D (_Decal, IN.uv_MainTex) * _ShowDecal;
           fixed3 albedo = decal.r > 0.9 ? decal.rgb : main.rgb;

           o.Albedo = albedo * _Color;
           
        }
        ENDCG
    }
    FallBack "Diffuse"
}
