#version 100

precision highp float;

varying vec2 UV;

uniform vec4 Color;
uniform vec2 Resolution;
uniform float AntiAliasing;
uniform vec4 VisibleRect;

//Shadertoy Emulation
#define fragColor gl_FragColor
#define iResolution Resolution
#define fragCoord Resolution * UV

void main(void)
{
    float res = max(iResolution.x, iResolution.y);
    float border = 0.05/(res/(AntiAliasing*10.));
    vec2 m = UV - vec2(0.5, 0.5);
    float dist = 0.5*0.5 - (m.x * m.x + m.y * m.y);
    float t = mix( dist / border, 1., max(0., sign(dist - border)) );
    fragColor = mix(vec4(0.), Color, t);
    
    
    if ((UV.x < VisibleRect.x || UV.y < VisibleRect.y || UV.x > VisibleRect.z + VisibleRect.x || UV.y > VisibleRect.w + VisibleRect.y) && VisibleRect != vec4(0))
        discard;
}