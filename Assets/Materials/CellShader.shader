Shader "Unlit/CellShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Albedo Color",  Color) = (.0, .0, .0, 1)
        _ShadowColor("Shadow Color",  Color) = (.0, .0, .0, 1)
        _RimColor("Rim Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _RimPower("Rim Power", Range(0.0, 10.0)) = 3.0
    }
    SubShader
    {
        Pass
        {
            Tags { "LightMode" = "ForwardBase"  "SHADOWSUPPORT" = "true" "RenderType" = "Opaque" }
            AlphaTest Greater 0.5
            LOD 100
            CGPROGRAM
            #pragma target 3.0
            #pragma multi_compile_fog multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                //UNITY_FOG_COORDS(1)
                SHADOW_COORDS(1)
                float4 posWorld : TEXCOORD3;
                float3 normal : TEXCOORD4;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 _ShadowColor;
            float4 _RimColor;
            float _RimPower;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //UNITY_TRANSFER_FOG(o, o.pos);
                UNITY_TRANSFER_SHADOW(o, o.pos)
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half NdotL = max(dot(i.normal, normalize(_WorldSpaceLightPos0)), 0.0f);
                //UnityLight light = CreateLight(i);
                half diff = saturate(NdotL);

                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);

                fixed shadow = SHADOW_ATTENUATION(i);
                fixed3 lighting = diff * shadow;

                fixed3 col = _Color * lighting + (1 - lighting ) * _ShadowColor;

                col = tex2D(_MainTex, i.uv);
                //clip(col.g);
                /*if (_RimPower > 0 )
                {
                    float3 rim = pow(1.0 - saturate(dot(viewDirection, i.normal)), 1 / _RimPower);

                    col = (1 - rim) * col + rim * _RimColor;
                }*/
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return fixed4(col, 1.0f);
            }
            ENDCG
        }

        //Pass
        //{
        //    Tags { "LightMode" = "ForwardAdd"  }
        //    ZWrite Off Blend SrcAlpha OneMinusSrcAlpha
        //    AlphaTest Greater 0.5

        //    CGPROGRAM
        //    #pragma target 3.0
        //    #pragma vertex vert
        //    #pragma fragment frag

        //    #pragma multi_compile_fwdadd_fullshadows 

        //    #include "UnityCG.cginc"
        //    #include "Lighting.cginc"
        //    #include "AutoLight.cginc"

        //    struct appdata
        //    {
        //        float4 vertex : POSITION;
        //        float3 normal : NORMAL;
        //        float2 uv : TEXCOORD0;
        //    };

        //    struct v2f
        //    {
        //        float2 uv : TEXCOORD0;
        //        float4 pos : SV_POSITION;
        //        //UNITY_FOG_COORDS(1)
        //        SHADOW_COORDS(1)
        //        float3 posWorld : TEXCOORD2;
        //        float3 normal : TEXCOORD3;
        //    };

        //    sampler2D _MainTex;
        //    float4 _MainTex_ST;
        //    float4 _Color;
        //    float4 _ShadowColor;
        //    float4 _RimColor;
        //    float _RimPower;

        //    float map(float s, float a1, float a2, float b1, float b2)
        //    {
        //        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        //    }

        //    v2f vert(appdata v)
        //    {
        //        v2f o;
        //        o.pos = UnityObjectToClipPos(v.vertex);
        //        o.posWorld = mul(unity_ObjectToWorld, v.vertex).xyz;
        //        o.normal = UnityObjectToWorldNormal(v.normal);
        //        o.uv = TRANSFORM_TEX(v.uv, _MainTex);
        //        //UNITY_TRANSFER_FOG(o,o.vertex);
        //        TRANSFER_SHADOW(o)
        //        return o;
        //    }

        //    fixed4 frag(v2f i) : SV_Target
        //    {

        //        half NdotL = max(dot(i.normal, normalize(i.posWorld - _WorldSpaceLightPos0)) + 0.172f , 0.00f);
        //        
        //        half diff = DotClamped(i.normal, normalize(i.posWorld - _WorldSpaceLightPos0));

        //        UNITY_LIGHT_ATTENUATION(atten, i, i.posWorld)

        //        float3 lightVec = _WorldSpaceLightPos0.xyz - i.posWorld;

        //        diff = map(diff, 0, 1, 0.15f, 1.0f);
        //        fixed3 lighting = ((atten - (atten * sign(NdotL))) + (0.8f - NdotL) /** sign(NdotL)*/) / 2;

        //        fixed3 col = (lighting) * _LightColor0;

        //        return fixed4(col, 1.0f);
        //    }
        //    ENDCG
        //}
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
