Shader "CStamp/Properties" {

    Properties {
        _myColor ("Example Color", Color) = (1,1,1,1) //Properties
        _myRange ("Example Range", Range(0,5)) = 1
        _myTex ("Example Texture", 2D) = "white" {}
        _myCube ("Example Cube", CUBE) = "" {}
        _myFloat ("Example Float", Float) = 0.5
        _myVector ("Example Vector", Vector) = (0.5,1,1,1)
    }
    SubShader {
        CGPROGRAM     //processing
            
            //Compiler directive - Tell Unity how the code should be used
            //Type of shader, name of function containing shader, lighting mode           
            #pragma surface surf Lambert

            fixed4 _myColor;
            half _myRange;
            sampler2D _myTex;
            samplerCUBE _myCube;
            float _myFloat;
            float4 _myVector;

            struct Input { // input data required by the function (verte, normal, uv, etc.)
                float2 uv_myTex;
                float3 worldRefl;
            };
                        
            void surf (Input IN, inout SurfaceOutput o){
                //grab all of the UV values
                o.Albedo = (tex2D(_myTex, IN.uv_myTex) * _myRange * _myColor).rgb;
                o.Emission = texCUBE(_myCube, IN.worldRefl).rgb;
            }
        ENDCG
    
    }

    FallBack "Diffuse" //fallback for inferior GPUs
 }