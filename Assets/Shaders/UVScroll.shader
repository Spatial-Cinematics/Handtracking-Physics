Shader "CStamp/UVScroll"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        
        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o){
            float2 newuv = IN.uv_MainTex + float2(_Time.y, 0);
            o.Albedo = tex2D (_MainTex, newuv);
        }
        
        ENDCG

    }
}
