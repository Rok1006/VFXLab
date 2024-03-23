#undef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR

#ifndef SHADERGRAPH_PREVIEW
    #if VERSION_GREATER_EQUAL(9, 0)
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #if (SHADERPASS != SHADERPASS_FORWARD)
            #undef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
        #endif
    #else
        #ifndef SHADERPASS_FORWARD
            #undef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
        #endif
    #endif
#endif
void MainLight_float (float3 WorldPos, out float3 Direction, out float3 Color,
    out float DistanceAtten, out float ShadowAtten){
 
#ifdef SHADERGRAPH_PREVIEW
    Direction = normalize(float3(1,1,-0.4));
    Color = float4(1,1,1,1);
    DistanceAtten = 1;
    ShadowAtten = 1;
#else
    float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
    Light mainLight = GetMainLight(shadowCoord);
 
    Direction = mainLight.direction;
    Color = mainLight.color;
    DistanceAtten = mainLight.distanceAttenuation;
    ShadowAtten = mainLight.shadowAttenuation;
#endif
 
}
// void MainLight_half(float3 WorldPos, out float3 Direction, out float3 Color, out float DistanceAtten, out float ShadowAtten)
// {
//     #ifndef SHADERGRAPH_PREVIEW
//         Direction = half3(0.5, 0.5, 0);
//         Color = 1;
//         DistanceAtten = 1;
//         ShadowAtten = 1;
//     #else
//         #if SHADOWS_SCREEN
//             float4 clipPos = TransformWorldToHClip(WorldPos);
//             float4 shadowCoord = ComputeScreenPos(clipPos);
//         #else
//             float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
//         #endif
//             Light mainLight = GetMainLight(shadowCoord);
//             Direction = mainLight.direction;
//             Color = mainLight.color;
//             DistanceAtten = mainLight.distanceAttenuation;
 
//         #if !defined(_MAIN_LIGHT_SHADOWS) || defined(_RECEIVE_SHADOWS_OFF)
//             ShadowAtten = 1.0h;
//         #endif
 
//         #if SHADOWS_SCREEN
//             ShadowAtten = SampleScreenSpaceShadowmap(shadowCoord);
//         #else
//             ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
//             float shadowStrength = GetMainLightShadowStrength();
//             ShadowAtten = SampleShadowmap(shadowCoord, TEXTURE2D_ARGS(_MainLightShadowmapTexture,
//             sampler_MainLightShadowmapTexture),
//             shadowSamplingData, shadowStrength, false);
//         #endif
//     #endif
// }

