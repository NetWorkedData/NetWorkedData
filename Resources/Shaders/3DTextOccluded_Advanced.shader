Shader "BasicToolBox/3DTextOccluded_Advanced" {
    Properties {
        _Color ("Text Color", Color) = (1,1,1,1)
        _OccludeColor ("Occlusion Color", Color) = (0,0,0,0)
        _MainTex ("Font Texture", 2D) = "white" {}
    }
    SubShader {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Lighting Off Cull Back ZWrite Off Fog { Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            ZTest Greater
            Color [_OccludeColor]
            SetTexture [_MainTex] {
                combine primary, texture * primary
            }
        }

        Pass {
            Color [_Color]
            SetTexture [_MainTex] {
                combine primary, texture * primary
            }
        }
    }
    FallBack "Diffuse"
}