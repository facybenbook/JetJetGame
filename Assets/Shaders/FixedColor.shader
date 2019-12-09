Shader "JetJet/Fixed Unlit"
{
    Properties
    {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Main Texture", 2D) = "white" {}
    _SpecColor ("Specular Color", Color) = (1,1,1,1)
    _SpecShininess ("Specular Shininess", Range(1.0,100.0)) = 2.0
    _FresnelPower ("Fresnel Power", Range(0.0, 3.0)) = 1.4
    _FresnelScale ("Fresnel Scale", Range(0.0, 1.0)) = 1.0
    _FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Pass
        {
            Tags { "LightMode" = "ForwardBase" }
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float3 normal : NORMAL;
                    float2 texcoord : TEXCOORD0;
                    float4 posWorld : TEXCOORD1;
                };

                float4 _LightColor0;

                fixed4 _Color;
                sampler2D _MainTex;
                float4 _MainTex_ST;

                float4 _SpecColor;
                float _SpecShininess;

                float _FresnelPower;
                float _FresnelScale;
                float4 _FresnelColor;

                v2f vert (appdata IN)
                {
                    v2f OUT;
                    OUT.pos = UnityObjectToClipPos(IN.vertex);
                    OUT.posWorld = mul(unity_ObjectToWorld, IN.vertex);
                    OUT.normal = mul(float4(IN.normal, 0.0), unity_ObjectToWorld).xyz;
                    OUT.texcoord = IN.texcoord;
                    return OUT;
                }

                fixed4 frag (v2f IN) : COLOR
                {
                     fixed4 texColor = tex2D(_MainTex, IN.texcoord);
                    // return texColor;

                    float3 normalDir = normalize(IN.normal);
                    float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                    float3 viewDir = normalize(_WorldSpaceCameraPos - IN.posWorld.xyz);
                    float3 diffuse = _LightColor0.rgb * max(0.0, dot(normalDir, lightDir));

                    float3 specular;
                    if (dot(normalDir, lightDir) < 0.0)
                    {
                        specular = float3(0.0, 0.0, 0.0);
                    }
                    else
                    {
                        specular = _LightColor0.rgb * _SpecColor.rgb * pow(max(0.0, dot(reflect(-lightDir, normalDir), viewDir)), _SpecShininess);
                    }

                    float3 I = IN.posWorld - _WorldSpaceCameraPos.xyz;
                    float refl = _FresnelScale * pow(1.0 + dot(normalize(I), normalDir), _FresnelScale);

                    //float3 diffuseSpecular = diffuse + specular;

                    float4 finalColor = float4(diffuse,1) * texColor;

                    return lerp(finalColor, _FresnelColor, refl);

                    //return _Color * texColor * float4(diffuse, 1);
                    // return _Color;
                }
            ENDCG
        }
    }

}
