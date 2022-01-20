// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)
//粒子如果不在画布内，可以被剪掉

Shader "Custom/Particles/Additive" {
Properties {
    _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
    _MainTex ("Particle Texture", 2D) = "white" {}
    _InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0

	//记录边界范围
	_Area("Area", Vector) = (0,0,1,1)
	//值为1需要裁减 值为0不裁剪
	_IsClip("IsClip", Int) = 0
}

Category {
    Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
    Blend SrcAlpha One
    ColorMask RGB
    Cull Off Lighting Off ZWrite Off

    SubShader {
        Pass {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_particles
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _TintColor;

			float4 _Area;
			int _IsClip;

            struct appdata_t {
                float4 vertex : POSITION;//模型空间位置
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                #ifdef SOFTPARTICLES_ON
                float4 projPos : TEXCOORD2;
                #endif
				float2 worldPos:TEXCOORD3;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            float4 _MainTex_ST;

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                #ifdef SOFTPARTICLES_ON
                o.projPos = ComputeScreenPos (o.vertex);
                COMPUTE_EYEDEPTH(o.projPos.z);
                #endif
                o.color = v.color;
                o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xy;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
            float _InvFade;

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

                #ifdef SOFTPARTICLES_ON
                float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
                float partZ = i.projPos.z;
                float fade = saturate (_InvFade * (sceneZ-partZ));
                i.color.a *= fade;
                #endif

                fixed4 col = 2.0f * i.color * _TintColor * tex2D(_MainTex, i.texcoord);
                col.a = saturate(col.a); // alpha should not have double-brightness applied to it, but we can't fix that legacy behavior without breaking everyone's effects, so instead clamp the output to get sensible HDR behavior (case 967476)

                UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0,0,0,0)); // fog towards black due to our blend mode

				if(_IsClip == 1)
				{
					bool inArea = i.worldPos.x >= _Area.x && i.worldPos.x <= _Area.z && i.worldPos.y >= _Area.y && i.worldPos.y <= _Area.w;
					return inArea ? col : fixed4(0,0,0,0);
				}
                return col;
            }
            ENDCG
        }
    }
}
}
