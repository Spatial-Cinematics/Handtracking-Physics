Shader "CStamp/VF Color" {

    Properties{
        _MainTex ("Texture2D", 2D) = "white" {}
    }

    SubShader {

        pass {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

            #include "UNityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            //contains information about each 3D vertex provided to vertex shader
            struct appdata{
                float4 vertex : POSITION;
            };

            struct v2f{
                float4 pos : SV_POSITION; //vertices that have been processed from world space into clipping space
                float4 color : COLOR;
                SHADOW_COORDS(1)
            };

            //float4 because contains scaling data
            sampler2D _MainTex; //referenced by vertex shader
            float4 _MainTex_ST; //referenced by fragment shader

            //Projects 3D data into the 2D clipping space
            v2f vert (appdata v){
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                TRANSFER_SHADOW(o)
                return o;
            }

            //Calculate the color of each pixel that needs to appear in the final screen
            //Job of the fragment shader to return a color
            //this does a lot more work than vert shader - working over larger set of data
            fixed4 frag (v2f i) : SV_TARGET{
                fixed4 col = (1,1,0,0);
                fixed shadow = SHADOW_ATTENUATION(i);
                col.r = i.pos.x/1000;
                col.g = i.pos.y/1000;
                col.rgb *=  shadow;
                return col;
            }

            ENDCG

        }

    }


}