Shader "Hidden/Fog"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CameraDepthTexture ("Texture", 2D) = "white" {}
        _FogColor ("FogColor", Color) = (0.4, 0.1, 0.9)
        _FogDensity ("FogDensity", Float) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;

            fixed4 _FogColor;
            float _FogDensity;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float depth = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv));
                depth = Linear01Depth(depth);

                float fog = saturate(depth * pow(2, _FogDensity));
                // just invert the colors
                //col.rgb = 1 - col.rgb;
                return lerp(col, _FogColor, fog);
            }
            ENDCG
        }
    }
}
