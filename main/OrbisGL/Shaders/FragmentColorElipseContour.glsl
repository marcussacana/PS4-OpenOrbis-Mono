#version 100

precision highp float;

varying vec2 UV;

uniform vec4 Color;
uniform vec2 Resolution;
uniform float ContourWidth;
uniform vec4 VisibleRect;

//Shadertoy Emulation
#define fragColor gl_FragColor
#define iResolution Resolution
#define fragCoord Resolution * UV

//stolen from https://www.shadertoy.com/view/ssdSD2
void main(void)
{
    // Parameters
    float thickness = ContourWidth/100.;
    float fade = 4.0 / iResolution.y;

    // -1 -> 1 local space, adjusted for aspect ratio
    vec2 uv = (UV * 2.) - 1.;

    // Calculate distance and fill circle with white
    float distance = 1.0 - length(uv);
    vec3 colorMod = vec3(smoothstep(0.0, fade, distance));
    colorMod *= vec3(smoothstep(thickness + fade, thickness, distance));

    // Set output color
    fragColor = vec4(colorMod, colorMod);
    fragColor.rgb *= Color.xyz;
    
    if ((UV.x < VisibleRect.x || UV.y < VisibleRect.y || UV.x > VisibleRect.z + VisibleRect.x || UV.y > VisibleRect.w + VisibleRect.y) && VisibleRect != vec4(0))
        discard;
}