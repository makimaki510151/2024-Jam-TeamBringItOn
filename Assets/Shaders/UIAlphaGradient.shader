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

        _OverlayScrollSpeedTop ("Overlay Scroll Speed Top", Float) = -0.2
        _OverlayScrollSpeedBottom ("Overlay Scroll Speed Bottom", Float) = 0.2
        _OverlayIntensity ("Overlay Intensity", Range(0,5)) = 2.0
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
            float _OverlayScrollSpeedTop;
            float _OverlayScrollSpeedBottom;
            float _OverlayIntensity;

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

                // フェード計算（中央透明・上下不透明）
                float dist = abs(uv.y - _TransparentCenter);
                float inner = _TransparentRange / 2.0;
                float fadeAlpha = saturate((dist - inner) / _FadeEdge);

                // 装飾UV（上下別スクロール）
                float2 uvTop = uv + float2(0, _Time.y * _OverlayScrollSpeedTop);
                float2 uvBottom = uv + float2(0, _Time.y * _OverlayScrollSpeedBottom);

                fixed4 overlayTop = tex2D(_OverlayTex, uvTop);
                fixed4 overlayBottom = tex2D(_OverlayTex, uvBottom);

                // 上下どちらかに応じて選択
                fixed4 overlayCol = (uv.y > _TransparentCenter) ? overlayTop : overlayBottom;

                // 濃さ強調（RGBもαも強め）
                float overlayAlpha = saturate(overlayCol.a * _OverlayIntensity);
                fixed3 overlayRGB = overlayCol.rgb * _OverlayIntensity;

                // 背景と lerp（ステッカー感）
                fixed3 finalRGB = lerp(baseCol.rgb, overlayRGB, overlayAlpha);

                // フェード適用
                fixed4 finalCol = fixed4(finalRGB, baseCol.a * fadeAlpha);

                // Tint 反映
                finalCol *= _Color;

                return finalCol;
            }
            ENDCG
        }
    }
}