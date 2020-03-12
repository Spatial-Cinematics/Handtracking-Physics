Shader "CStamp/Screen Grab" {

    Properties{
        _MainTex ("Texture2D", 2D) = "white" {}
        _ScaleUVX("UV Scalar", Range(1, 10)) = 1
    }

    SubShader {

        Tags {"Queue" = "Transparent"}

        //grabs all pixesl from the frame buffer - frame that's about to appear on the screen
        GrabPass {

        }

        pass {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UNityCG.cginc"

            //contains information about each 3D vertex provided to vertex shader
            struct appdata{
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f{
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _GrabTexture;
            //float4 because contains scaling data
            sampler2D _MainTex; //referenced by vertex shader
            float4 _MainTex_ST; //referenced by fragment shader
            float _ScaleUVX;

            //Projects 3D data into the 2D clipping space
            v2f vert (appdata v){
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.x = sin(o.uv.x * _ScaleUVX);
                return o;
            }

            //Claculate the color of each pixel that needs to appear in the final screen
            fixed4 frag (v2f i) : SV_TARGET{
                fixed4 col = tex2D(_GrabTexture, i.uv);
                return col;
            }

            ENDCG

        }

    }


}