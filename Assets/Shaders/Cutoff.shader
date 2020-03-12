Shader "CStamp/Cutoff"
{
    Properties {
        _RimColor ("Rim Color", Color) = (0, .5, .5, .5)
        _RimPower ("Rim Power", Range(.5, 8)) = 3
    }

    SubShader{

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input {
            float3 viewDir;
            float3 worldPos;
        };

        float4 _RimColor;
        float _RimPower;

        void surf(Input IN, inout SurfaceOutput o){
            half rim = saturate(dot(normalize(IN.viewDir), o.Normal));
            //o.Emission = _RimColor.rgb * rim > .5 ? 1 : rim > .3 ? float3(0,1,0) : 0
            //o.Emission = IN.worldPos.y > 1 ? float3(0,1,0) : 0; 
            o.Emission = frac(IN.worldPos.y * 5) > .4 ?
                float3(0,1,0) * rim : float3(1,0,0) * rim;

        }
        ENDCG
    }
    Fallback "Diffuse"
}