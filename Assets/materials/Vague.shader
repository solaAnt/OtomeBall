Shader "Sprites/Vague"
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
            float4 c=float4(0.0h,0.0h,0.0h,0.0h);
                     	
  	 
            c+=tex2D(_MainTex, float2(IN.texcoord.x+0.05,IN.texcoord.y-0.05))*0.02;
            c+=tex2D(_MainTex, float2(IN.texcoord.x+0.04,IN.texcoord.y-0.04))*0.05;
            c+=tex2D(_MainTex, float2(IN.texcoord.x+0.03,IN.texcoord.y-0.03))*0.1;
            c+=tex2D(_MainTex, float2(IN.texcoord.x+0.02,IN.texcoord.y-0.02))*0.12;
            c+=tex2D(_MainTex, float2(IN.texcoord.x+0.01,IN.texcoord.y-0.01))*0.15;
            
            c+=tex2D(_MainTex, float2(IN.texcoord.x-0.05,IN.texcoord.y+0.05))*0.02;
            c+=tex2D(_MainTex, float2(IN.texcoord.x-0.04,IN.texcoord.y+0.04))*0.05;
            c+=tex2D(_MainTex, float2(IN.texcoord.x-0.03,IN.texcoord.y+0.03))*0.1;
            c+=tex2D(_MainTex, float2(IN.texcoord.x-0.02,IN.texcoord.y+0.02))*0.12;
            c+=tex2D(_MainTex, float2(IN.texcoord.x-0.01,IN.texcoord.y+0.01))*0.15;
            
          	c+=tex2D(_MainTex, IN.texcoord+0.05)*0.02;
          	c+=tex2D(_MainTex, IN.texcoord+0.04)*0.05;
          	c+=tex2D(_MainTex, IN.texcoord+0.03)*0.1;
          	c+=tex2D(_MainTex, IN.texcoord+0.02)*0.15;
          	c+=tex2D(_MainTex, IN.texcoord+0.01)*0.2;  
	
           	c+=tex2D(_MainTex, IN.texcoord-0.05)*0.02;
          	c+=tex2D(_MainTex, IN.texcoord-0.04)*0.05;
          	c+=tex2D(_MainTex, IN.texcoord-0.03)*0.1;
          	c+=tex2D(_MainTex, IN.texcoord-0.02)*0.15;
          	c+=tex2D(_MainTex, IN.texcoord-0.01)*0.2;    
          	
          	c+=tex2D(_MainTex,IN.texcoord)*0.3;
      		c*=c.a;   
          	
          	return c/3;       
        }
    ENDCG
    }
}
}