Shader "CStamp/Leaves"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }
    SubShader
    {

        Tags {
            "Queue" = "Transparent"
        }

        Cull Off

        CGPROGRAM
        //#pragma surface surf Lambert (included in Unity)
        // Custom lighting:
        #pragma surface surf BasicLambert alpha:fade

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

        sampler2D _MainTex;

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
