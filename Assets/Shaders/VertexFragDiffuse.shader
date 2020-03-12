Shader "CStamp/Vert Frag Diffuse" {

    Properties{
        _MainTex ("Texture2D", 2D) = "white" {}
    }

    SubShader {

        pass {

            Tags {"LightMode" = "ForwardBase"}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

            #include "UNityCG.cginc"
            #include "UnityLightingCommon.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            //contains information about each 3D vertex provided to vertex shader
            struct appdata{
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
            };

            struct v2f{
                float2 uv : TEXCOORD0;
                fixed4 diff : COLOR0;
                float4 pos : SV_POSITION;
                SHADOW_COORDS(1)
            };

            //float4 because contains scaling data
            sampler2D _MainTex; //referenced by vertex shader
            float4 _MainTex_ST; //referenced by fragment shader

            //Projects 3D data into the 2D clipping space
            v2f vert (appdata v){
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                half3 worldNormal = UnityObjectToWorldNormal(v.normal); //put vectors in the same coordinate space
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz)); //light pos comes from #include statement
                o.diff = nl * _LightColor0; //also #included
                TRANSFER_SHADOW(o)
                return o;
            }

            //Claculate the color of each pixel that needs to appear in the final screen
            fixed4 frag (v2f i) : SV_TARGET{
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed shadow = SHADOW_ATTENUATION(i);
                col *= i.diff * shadow;
                return col;
            }

            ENDCG

        }

        pass{ //cast shadows

            Tags {"LightMode" = "ShadowCaster"}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster

            #include "UnityCG.cginc"

            struct appdata{
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
            };

            struct v2f {
                V2F_SHADOW_CASTER;
            };

            v2f vert (appdata v){
                v2f o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o);
                return o;
            }

            float4 frag (v2f i) : SV_TARGET{
                SHADOW_CASTER_FRAGMENT(i);
            }

            ENDCG

        }
    }


}