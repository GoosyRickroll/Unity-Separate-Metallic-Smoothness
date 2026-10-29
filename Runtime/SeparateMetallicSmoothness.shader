Shader "Sheepish90/Separate Metallic & Smoothness"
{
    Properties
    {
        _Color ("Color Tint", Color) = (1,1,1,1)
        _MainTex ("Albedo", 2D) = "white" {}
        _MetallicMap ("Metallic Map", 2D) = "black" {}
        _MetallicStrength ("Metallic Strength", Range(0,2)) = 1
        [HideInInspector] _MetallicBlack ("Metallic Black Point", Range(0,1)) = 0
        [HideInInspector] _MetallicWhite ("Metallic White Point", Range(0,1)) = 1
        [HideInInspector] _MetallicGamma ("Metallic Gamma", Range(0.1,5)) = 1
        _SmoothnessMap ("Smoothness Map", 2D) = "white" {}
        _SmoothnessStrength ("Smoothness Strength", Range(0,3)) = 1
        [HideInInspector] _SmoothnessBlack ("Smoothness Black Point", Range(0,1)) = 0
        [HideInInspector] _SmoothnessWhite ("Smoothness White Point", Range(0,1)) = 1
        [HideInInspector] _SmoothnessGamma ("Smoothness Gamma", Range(0.1,5)) = 1
        [Normal] _BumpMap ("Normal Map", 2D) = "bump" {}
        _BumpScale ("Normal Strength", Range(0,2)) = 1
        _OcclusionMap ("Occlusion Map", 2D) = "white" {}
        _OcclusionStrength ("Occlusion Strength", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 300
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0
        sampler2D _MainTex, _MetallicMap, _SmoothnessMap, _BumpMap, _OcclusionMap;
        fixed4 _Color;
        half _MetallicStrength, _SmoothnessStrength, _BumpScale, _OcclusionStrength;
        half _MetallicBlack, _MetallicWhite, _MetallicGamma;
        half _SmoothnessBlack, _SmoothnessWhite, _SmoothnessGamma;
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_MetallicMap;
            float2 uv_SmoothnessMap;
            float2 uv_BumpMap;
            float2 uv_OcclusionMap;
        };
        half ApplyLevels(half value, half blackPoint, half whitePoint, half gamma)
        {
            half normalized = saturate((value - blackPoint) / max(whitePoint - blackPoint, 0.001h));
            return pow(normalized, 1.0h / max(gamma, 0.1h));
        }
        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = (tex2D(_MainTex, IN.uv_MainTex) * _Color).rgb;
            half metallic = tex2D(_MetallicMap, IN.uv_MetallicMap).r;
            half smoothness = tex2D(_SmoothnessMap, IN.uv_SmoothnessMap).r;
            o.Metallic = saturate(ApplyLevels(metallic, _MetallicBlack, _MetallicWhite, _MetallicGamma) * _MetallicStrength);
            o.Smoothness = saturate(ApplyLevels(smoothness, _SmoothnessBlack, _SmoothnessWhite, _SmoothnessGamma) * _SmoothnessStrength);
            o.Normal = UnpackScaleNormal(tex2D(_BumpMap, IN.uv_BumpMap), _BumpScale);
            half ao = tex2D(_OcclusionMap, IN.uv_OcclusionMap).g;
            o.Occlusion = lerp(1.0h, ao, _OcclusionStrength);
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
    CustomEditor "SeparateMetallicSmoothnessGUI"
}
