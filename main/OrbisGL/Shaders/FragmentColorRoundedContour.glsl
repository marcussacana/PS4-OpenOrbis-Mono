#version 100

precision highp float;

varying vec2 UV;

uniform vec4 Color;
uniform vec2 Resolution;
uniform float Border;
uniform float ContourWidth;
uniform vec2 Margin;
uniform vec4 VisibleRect;

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
    float borderMax = max(Border, 0.15);
    // setup
    float t = -0.045 + (borderMax/10.);
    float iRadius = min(iResolution.x, iResolution.y) * (0.05 + t);
    vec2 halfRes = 0.5 * iResolution.xy;

    // compute box
    float d = udRoundBox( fragCoord.xy - halfRes, vec2(halfRes.x-Margin.x, halfRes.y-Margin.y), iRadius );

    d += ContourWidth;
    
    // check if the point is close to the contour
    float contour = smoothstep(0.0, ContourWidth, abs(d));
    
    // calculate the color and alpha
    vec3 c = mix(Color.xyz, vec3(0.0, 0.0, 0.0), contour);        
    float alpha = Color.w - smoothstep(0.0, 1.0, contour);

    fragColor = vec4(c, alpha);
    
    if ((UV.x < VisibleRect.x || UV.y < VisibleRect.y || UV.x > VisibleRect.z + VisibleRect.x || UV.y > VisibleRect.w + VisibleRect.y) && VisibleRect != vec4(0))
        discard;
}