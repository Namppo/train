Shader "Custom/EquirectangularProjection"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Off // 내부도 렌더링
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 dir : TEXCOORD0;
            };

            sampler2D _MainTex;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.dir = normalize(v.vertex.xyz); // 구면 좌표 변환
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float longitude = atan2(i.dir.z, i.dir.x) / UNITY_PI;
                float latitude = asin(i.dir.y) / UNITY_PI;
                float2 uv = float2(0.5 + longitude * 0.5, 0.5 + latitude);
                return tex2D(_MainTex, uv);
            }
            ENDCG
        }
    }
}
