Shader "CStamp/Blending" {

    Properties {
        _MainTex ("Texture", 2D) = "black" {}
    }
    SubShader{
        Tags {"Queue" = "Transparent"}
        Blend one one
        pass {
            SetTexture [_MainTex] {Combine texture}
        }
    }

}