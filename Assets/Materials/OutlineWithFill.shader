Shader "Unlit/OutlineWithFill"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineWidth ("Outline Width", Range(0, 4)) = 1
        _FillColor ("Fill Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask RGBA

        CGINCLUDE
        #include "UnityCG.cginc"

        sampler2D _MainTex;
        float4 _MainTex_ST;

        fixed4 _OutlineColor;
        float _OutlineWidth;

        fixed4 _FillColor;

        struct v2f
        {
            float4 pos : SV_POSITION;
            float2 uv : TEXCOORD0;
        };

        v2f vertOutline(appdata_base v, float2 offset)
        {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.pos.xy += offset * 2.0 * o.pos.w * _OutlineWidth / _ScreenParams.xy;
            o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
            return o;
        }

        v2f vert(appdata_base v)
        {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
            return o;
        }

        fixed4 fragOutline(v2f i) : SV_Target
        {
            fixed alpha = tex2D(_MainTex, i.uv).a;
            fixed4 col = _OutlineColor;
            col.a *= alpha;
            return col;
        }

        fixed4 fragFill(v2f i) : SV_Target
        {
            fixed alpha = tex2D(_MainTex, i.uv).a;
            fixed4 col = _FillColor;
            col.a *= alpha;
            return col;
        }
        ENDCG

        Pass
        {
            Name "OUTLINE1"
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGB

            CGPROGRAM
            #pragma vertex vertOutline1
            #pragma fragment fragOutline

            v2f vertOutline1(appdata_base v)
            {
                return vertOutline(v, float2(1, 1));
            }
            ENDCG
        }

        Pass
        {
            Name "OUTLINE2"
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGB

            CGPROGRAM
            #pragma vertex vertOutline2
            #pragma fragment fragOutline

            v2f vertOutline2(appdata_base v)
            {
                return vertOutline(v, float2(-1, 1));
            }
            ENDCG
        }

        Pass
        {
            Name "OUTLINE3"
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGB

            CGPROGRAM
            #pragma vertex vertOutline3
            #pragma fragment fragOutline

            v2f vertOutline3(appdata_base v)
            {
                return vertOutline(v, float2(1, -1));
            }
            ENDCG
        }

        Pass
        {
            Name "OUTLINE4"
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGB

            CGPROGRAM
            #pragma vertex vertOutline4
            #pragma fragment fragOutline

            v2f vertOutline4(appdata_base v)
            {
                return vertOutline(v, float2(-1, -1));
            }
            ENDCG
        }


        // ---- Fill Pass (中央部分をFillColorで描画する) ----
        Pass
        {
            Name "FILL"
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGB

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragFill
            ENDCG
        }
    }
}