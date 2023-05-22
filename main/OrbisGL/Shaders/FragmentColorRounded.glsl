#version 100

precision highp float;

varying vec2 UV;

uniform vec4 Color;
uniform vec2 Resolution;
uniform float Border;

//Shadertoy Emulation
#define fragColor gl_FragColor
#define iResolution Resolution
#define fragCoord Resolution * UV

//stolen from https://iquilezles.org/articles/distfunctions
float udRoundBox( vec2 p, vec2 b, float r )
{
    return length(max(abs(p)-b+r,0.0))-r;
}

void main(void)
{
    // setup
    float t = -0.045 + (Border/10.);
    float iRadius = min(iResolution.x, iResolution.y) * (0.05 + t);
    vec2 halfRes = 0.5 * iResolution.xy;

    // compute box
    float b = udRoundBox( fragCoord.xy - halfRes, halfRes, iRadius );
    
	vec3 c = mix( Color.xyz, vec3(0.0, 0.0, 0.0), smoothstep(0.0, 1.0, b) );        
    float alpha = Color.w - smoothstep(0.0, 1.0, b);
    if(alpha < 0.1)
        discard;   
    fragColor = vec4( c, alpha);
}