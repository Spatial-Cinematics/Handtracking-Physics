Shader "CStamp/HelloShader" {

    Properties {
        _myColor ("Example Color", Color) = (1,1,1,1) //Properties
        _myEmission ("Example Emission", Color) = (1,1,1,1)
    }
    SubShader {
        CGPROGRAM     //processing
            
            //Compiler directive - Tell Unity how the code should be used
            //Type of shader, name of function containing shader, lighting mode           
            #pragma surface surf Lambert
            
            struct Input { // input data required by the function (verte, normal, uv, etc.)
                float2 uvMainTex;
            };
            
            fixed4 _myColor;
            fixed4 _myEmission;
            
            void surf (Input IN, inout SurfaceOutput o){
                o.Albedo = _myColor.rgb;
                o.Emission = _myEmission.xyz;
            }
        ENDCG
    
    }

    FallBack "Diffuse" //fallback for inferior GPUs
 }