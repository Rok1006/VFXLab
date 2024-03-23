Shader "Custom/SimpleToonShade"
{
    Properties 
    {
        _surfaceColor ("surface color", Color) = (0.4, 0.1, 0.9)
        _gloss ("gloss", Range(0,1)) = 1
        _diffuseLightSteps ("diffuseLightStep", Int) = 3
        _specularLightSteps ("specularLightStep", Int) = 3

        [Header(LightPart)]
        _BrightColor("Light Color", Color) = (1, 1, 1, 1)
        [Header(ShadowPart)]
        _DarkColor("Dark Color", Color) = (1, 1, 1, 1)

    }
    SubShader
    {
        // this tag is required to use _LightColor0
        Tags { "LightMode"="ForwardBase" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // might be UnityLightingCommon.cginc for later versions of unity
            #include "Lighting.cginc"

            #define MAX_SPECULAR_POWER 256
            
            float3 _surfaceColor;
            float _gloss;
            int _diffuseLightSteps;
            int _specularLightSteps;
            float3 _BrightColor;
            float3 _DarkColor;

            struct MeshData
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD0;
                float3 posWorld : TEXCOORD1;
            };

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);

                return o;
            }

            float4 frag (Interpolators i) : SV_Target
            {
                float3 color = 0;

                float3 normal = normalize(i.normal);
                
                float3 lightDirection = _WorldSpaceLightPos0;
                float3 lightColor = _LightColor0; // includes intensity

                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld);
                float3 halfDirection = normalize(viewDirection + lightDirection);
                float lightDot = max(0, dot(normal, lightDirection));
                float3 highlight = lerp(_DarkColor, _BrightColor, lightDot);
                color += highlight ;

                float diffuseFalloff = max(0, dot(normal, lightDirection));
                
                float specularFalloff = max(0, dot(normal, halfDirection));
                specularFalloff = pow(specularFalloff, _gloss * MAX_SPECULAR_POWER + 0.0001) * _gloss;

                diffuseFalloff = floor(diffuseFalloff * _diffuseLightSteps) / _diffuseLightSteps;
                specularFalloff = floor(specularFalloff * _specularLightSteps) / _specularLightSteps;

                //float3 diffuse = diffuseFalloff * _surfaceColor * lightColor; //default
                //float3 diffuse = smoothstep(abs(diffuseFalloff) * _surfaceColor * lightColor, 0.3,diffuseFalloff/3); //1
                float3 diffuse = diffuseFalloff * _surfaceColor * lightColor; //1
                float3 specular = specularFalloff * lightColor;

                color += diffuse + specular;

                return float4(color, 1.0);
            }
            ENDCG
        }
    }
}
//Should acomplish: can apply texture/normal map, use point light instead, change type of shading, ambient light, outline


