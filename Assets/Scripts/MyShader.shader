Shader "Custom/MyShader" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,0.5)
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

            float4 _Color;
            // Main texture
            sampler2D _MainTex;
            float4 _MainTex_ST;

            // VertexInput
            struct VertIn {
                float4 position : POSITION;
                float4 color : COLOR;
            };

            // Vertex Output
            struct VertOut {
                float4 position : POSITION;
                float4 color : COLOR;
            };

            VertOut vert (VertIn i) {
                VertOut o;
                // Transforms a point from object space to the camera’s clip space in homogeneous coordinates. This is 
                // the equivalent of mul(UNITY_MATRIX_MVP, float4(pos, 1.0)), and should be used in its place.
                o.position = UnityObjectToClipPos(i.position);
                o.color = i.color;
                return o;
            }

            struct FragOut {
            	float4 color : COLOR;
            };
            
            FragOut frag (VertOut i) : SV_Target {
                FragOut o;
                // sample the texture
                o.color = tex2D(_MainTex, i.color);
                o.color = (i.color);
                return o;
            }
            ENDCG
        }
    }
}
// https://docs.unity3d.com/Manual/ShaderTut2.html
//The vertex and fragment programs here don’t do anything fancy; vertex program uses the TRANSFORM_TEX macro from UnityCG.cginc
// to make sure texture scale and offset is applied correctly, and fragment program just samples the texture and multiplies by the color property.