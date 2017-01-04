Shader "Sprites/Gray"
{
Properties
{
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color ("Tint", Color) = (1,1,1,1)
    [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    _MinX ("MinX", Float) = 0.5
    _Gc ("Gc", Float) = 0
    _ClipTex("ClipTex Texture", 2D) = "white" {}
}
 
SubShader
{
    Tags
    { 
        "Queue"="Transparent" 
        "IgnoreProjector"="True" 
        "RenderType"="Transparent" 
        "PreviewType"="Plane"
        "CanUseSpriteAtlas"="True"
    }
    
    Cull Off
    Lighting Off
    ZWrite Off
    Fog { Mode Off }
    Blend One OneMinusSrcAlpha
 
    Pass
    {
    CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile DUMMY PIXELSNAP_ON
        #include "UnityCG.cginc"
 		float _MinX;
 		float _Gc;
 		
 		
        struct appdata_t
        {
            float4 vertex   : POSITION;
            float4 color    : COLOR;
            float2 texcoord : TEXCOORD0;
        };
 
        struct v2f
        {
            float4 vertex   : SV_POSITION;
            fixed4 color    : COLOR;
            half2 texcoord  : TEXCOORD0;
        };
 
        fixed4 _Color;
 
        v2f vert(appdata_t IN)
        {
            v2f OUT;
            OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
            OUT.texcoord = IN.texcoord;
            OUT.color = IN.color * _Color;
            #ifdef PIXELSNAP_ON
            OUT.vertex = UnityPixelSnap (OUT.vertex);
            #endif
 
            return OUT;
        }
 
        sampler2D _MainTex;
        sampler2D _ClipTex;
 
        fixed4 frag(v2f IN) : SV_Target
        {
            fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;
			if(IN.texcoord.x > _MinX)
            {
            	c.rgba *= _Gc;
            	return c;
            }

        	float gray = dot(c.rgb, float3(0.299, 0.587, 0.114));
        	c.rgb = float3(gray, gray, gray)*c.a;

            return c;
        }
    ENDCG
    }
}
}