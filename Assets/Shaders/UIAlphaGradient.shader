Shader "UI/AlphaGradient"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}         // 背景画像
        _OverlayTex ("Overlay Texture", 2D) = "white" {}   // 重ねる画像
        _Color ("Tint", Color) = (1,1,1,1)

        _TransparentCenter ("Transparent Center", Range(0,1)) = 0.5
        _TransparentRange ("Transparent Range", Range(0.01,1)) = 0.3
        _FadeEdge ("Fade Edge Width", Range(0.01,1)) = 0.2
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

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
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); // UV は Main/Overlay 共通
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // 背景・オーバーレイテクスチャ取得
                fixed4 baseCol = tex2D(_MainTex, uv);
                fixed4 overlayCol = tex2D(_OverlayTex, uv);

                // 透明度の共通計算（中心が透明）
                float dist = abs(uv.y - _TransparentCenter);
                float inner = _TransparentRange / 2.0;
                float outer = inner + _FadeEdge;
                float fadeAlpha = saturate((dist - inner) / _FadeEdge); // 0=透明, 1=不透明

                // 背景 + オーバーレイ合成（両方にフェードを適用）
                fixed4 finalCol = baseCol + overlayCol;
                finalCol *= _Color;
                finalCol.a *= fadeAlpha;

                return finalCol;
            }
            ENDCG
        }
    }
}