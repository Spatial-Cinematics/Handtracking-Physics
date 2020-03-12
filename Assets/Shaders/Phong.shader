Shader "CStamp/BasicPhong"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _SpecColor("Specular Color", Color) = (1,1,1,1)
        _Spec("Specular", Range (0,1)) = 0.5
        _Gloss("Gloss", Range(0,1)) = 0.5 //power you apply to specular
    }
    SubShader
    {

        Tags {
            "Queue" = "Geometry"
        }

        CGPROGRAM
        //#pragma surface surf BlinnPhong
        #pragma surface surf BasicBlinn

        half4 LightingBasicBlinn (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten){

            half3 h = normalize (lightDir + viewDir);

            //diffuse value for the color
            half diff = max (0, dot(s.Normal, lightDir)); 

            //fall off of the specular component
            float nh = max (0, dot (s.Normal, h));
            float spec = pow (nh, 48);

            half4 c;
            c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * atten;
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
        //_SpecColor is defined by Unity

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            o.Albedo = _Color.rgb;
            //apply specular and gloss to surface output
            o.Specular = _Spec;
            o.Gloss = _Gloss;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
