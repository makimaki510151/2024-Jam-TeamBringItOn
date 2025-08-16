Shader "UI/AlphaGradient"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _OverlayTex ("Overlay Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _TransparentCenter ("Transparent Center", Range(0,1)) = 0.5
        _TransparentRange ("Transparent Range", Range(0.01,1)) = 0.3
        _FadeEdge ("Fade Edge Width", Range(0.01,1)) = 0.2

        _OverlaySpeedTop ("Overlay Speed Top", Float) = -0.2   // 上装飾のスクロール（下方向）
        _OverlaySpeedBottom ("Overlay Speed Bottom", Float) = 0.2 // 下装飾のスクロール（上方向）
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _OverlayTex;
            float4 _MainTex_ST;
            float4 _OverlayTex_ST;

            float4 _Color;
            float _TransparentCenter;
            float _TransparentRange;
            float _FadeEdge;
            float _OverlaySpeedTop;
            float _OverlaySpeedBottom;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // 背景
                fixed4 baseCol = tex2D(_MainTex, uv);

                // 上半分用スクロール
                float2 uvTop = uv;
                uvTop.y += _Time.y * _OverlaySpeedTop;
                fixed4 topCol = tex2D(_OverlayTex, uvTop);

                // 下半分用スクロール
                float2 uvBottom = uv;
                uvBottom.y += _Time.y * _OverlaySpeedBottom;
                fixed4 bottomCol = tex2D(_OverlayTex, uvBottom);

                // 透明度計算（中央透明、上下不透明）
                float dist = abs(uv.y - _TransparentCenter);
                float inner = _TransparentRange / 2.0;
                float fadeAlpha = saturate((dist - inner) / _FadeEdge);

                // 上半分 or 下半分を切り替えて適用
                fixed4 overlayCol = (uv.y > _TransparentCenter) ? topCol : bottomCol;

                // 合成
                fixed4 finalCol = baseCol + overlayCol;
                finalCol *= _Color;
                finalCol.a *= fadeAlpha;

                return finalCol;
            }
            ENDCG
        }
    }
}