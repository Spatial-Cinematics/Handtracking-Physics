Shader "CStamp/Glass" {

    Properties{
        _MainTex ("Texture2D", 2D) = "white" {}
        _BumpMap ("Normalmap", 2D) = "bump" {}
        _Opacity ("Opacity", Range(0,1)) = 1
        _ScaleUV("Scale", Range(1, 5000)) = 1
    }

    SubShader {

        Tags {"Queue" = "Transparent"}

        //grabs all pixesl from the frame buffer - frame that's about to appear on the screen
        GrabPass {}

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
                float4 uvgrab : TEXCOORD1;
                float2 uvbump : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            sampler2D _GrabTexture;
            float4 _GrabTexture_TexelSize;
            sampler2D _MainTex; //referenced by vertex shader
            float4 _MainTex_ST; //referenced by fragment shader
            sampler2D _BumpMap; //referenced by vertex shader
            float4 _BumpMap_ST; //referenced by fragment shader
            float _ScaleUV;
            float _Opacity;

            //Projects 3D data into the 2D clipping space
            v2f vert (appdata v){
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uvgrab.xy = (float2(o.vertex.x, -o.vertex.y) + o.vertex.w) * 0.5;
                o.uvgrab.zw = o.vertex.zw;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uvbump = TRANSFORM_TEX(v.uv, _BumpMap);
                return o;
            }

            //Calculate the color of each pixel that needs to appear in the final screen
            fixed4 frag (v2f i) : SV_TARGET{

                half2 bump = UnpackNormal(tex2D( _BumpMap, i.uvbump)).xy;
                float2 offset = bump * _ScaleUV * _GrabTexture_TexelSize.xy;
                i.uvgrab.xy = offset * i.uvgrab.z + i.uvgrab.xy;

                fixed4 col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
                fixed4 tint = tex2D(_MainTex, i.uv);
                col *= tint * _Opacity;
                return col;
            }

            ENDCG

        }

    }


}