Shader "UI/AlphaGradient"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color Tint", Color) = (1,1,1,1)
        _TopAlpha ("Top Alpha", Range(0,1)) = 1
        _BottomAlpha ("Bottom Alpha", Range(0,1)) = 0
        _FadeStart ("Fade Start (0=bottom, 1=top)", Range(0,1)) = 0.0
        _FadeRange ("Fade Range", Range(0.01,1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
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
            float4 _MainTex_ST;
            fixed4 _Color;

            float _TopAlpha;
            float _BottomAlpha;
            float _FadeStart;
            float _FadeRange;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv) * _Color;

                // グラデーションの影響範囲を計算
                float fadeT = saturate((1.0 - i.uv.y - _FadeStart) / _FadeRange);

                // アルファ値をグラデーションで補間
                float alpha = lerp(_BottomAlpha, _TopAlpha, fadeT);
                texColor.a *= alpha;

                return texColor;
            }
            ENDCG
        }
    }
}