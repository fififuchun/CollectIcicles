Shader "Unlit/Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0, 0, 0, 1)
        _OutlineColor ("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineSize ("Outline Size", Range(0,0.1)) = 0.003
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        CGINCLUDE
        float _OutlineSize;
        float4 _OutlineColor;
        float4 _Color;
        #include "UnityCG.cginc"
        
        struct appdata
        {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
        };

        struct v2f
        {
            float4 pos : SV_POSITION;
            float3 normal : TEXCOORD1;
        };
        ENDCG

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 L = _WorldSpaceLightPos0.xyz;
                return dot(i.normal, L) * _Color;
            }
            ENDCG
        }

        Pass
        {
            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            v2f vert(appdata v)
            {
                v2f o;

                // MVP座標変換
                o.pos = UnityObjectToClipPos(v.vertex);

                // 法線の座標変換 : モデル空間 -> カメラ空間
                float3 norm = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);

                // 投影
                float2 offset = TransformViewToProjection(norm.xy);

                // クリップ空間で頂点を動かす
                o.pos.xy += offset * o.pos.w * _OutlineSize;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }
}