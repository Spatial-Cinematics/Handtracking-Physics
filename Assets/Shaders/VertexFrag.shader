Shader "CStamp/Basic Vertex Fragment" {

    Properties{
        _MainTex ("Texture2D", 2D) = "white" {}
    }

    SubShader {

        pass {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UNityCG.cginc"

            //contains information about each 3D vertex provided to vertex shader
            struct appdata{
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f{
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            //float4 because contains scaling data
            sampler2D _MainTex; //referenced by vertex shader
            float4 _MainTex_ST; //referenced by fragment shader

            //Projects 3D data into the 2D clipping space
            v2f vert (appdata v){
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            //Claculate the color of each pixel that needs to appear in the final screen
            fixed4 frag (v2f i) : SV_TARGET{
                fixed4 col = tex2D(_MainTex, i.uv);
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }

            ENDCG

        }

    }


}