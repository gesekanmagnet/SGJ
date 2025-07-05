Shader "Custom/SpriteOutline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineSize ("Outline Size", Float) = 1.0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
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
            fixed4 _OutlineColor;
            float _OutlineSize;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 offset = float2(_OutlineSize / _ScreenParams.x, _OutlineSize / _ScreenParams.y);

                float alpha = tex2D(_MainTex, i.uv).a;
                float outline = 0.0;

                // Sample alpha around the pixel to determine if outline should be shown
                outline += tex2D(_MainTex, i.uv + offset).a;
                outline += tex2D(_MainTex, i.uv - offset).a;
                outline += tex2D(_MainTex, i.uv + float2(offset.x, -offset.y)).a;
                outline += tex2D(_MainTex, i.uv + float2(-offset.x, offset.y)).a;

                if (alpha == 0 && outline > 0)
                {
                    return _OutlineColor;
                }

                return tex2D(_MainTex, i.uv) * _Color;
            }
            ENDCG
        }
    }
}