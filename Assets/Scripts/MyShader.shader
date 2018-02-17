Shader "Custom/MyShader" {
    Properties {
        _Color ("Main Color", Color) = (256,1,1,0.5)
        _MainTex ("Texture", 2D) = "white" { }
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // For TRANSFORM_TEXT	
            #include "UnityCG.cginc"

            fixed4 _Color;
            // Main texture
            sampler2D _MainTex;
            float4 _MainTex_ST;

            // VertexInput
            struct appdata {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            // Vertex Output
            struct v2f {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v) {
                v2f o;
                // Transforms a point from object space to the camera’s clip space in homogeneous coordinates. This is 
                // the equivalent of mul(UNITY_MATRIX_MVP, float4(pos, 1.0)), and should be used in its place.
                o.pos = UnityObjectToClipPos(v.pos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return (col * _Color);
            }
            ENDCG
        }
    }
}
// https://docs.unity3d.com/Manual/ShaderTut2.html
//The vertex and fragment programs here don’t do anything fancy; vertex program uses the TRANSFORM_TEX macro from UnityCG.cginc
// to make sure texture scale and offset is applied correctly, and fragment program just samples the texture and multiplies by the color property.