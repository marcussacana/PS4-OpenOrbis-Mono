#version 100

precision highp float;

varying vec2 UV;

uniform vec4 Color;
uniform vec2 Resolution;
uniform float ContourWidth;

//Shadertoy Emulation
#define fragColor gl_FragColor
#define iResolution Resolution
#define fragCoord Resolution * UV

void main(void)
{
    // Parameters
    float thickness = ContourWidth/100.;
    // Control with mouse
    //thickness = iMouse.x / iResolution.x;
    float fade = 0.005;

    // -1 -> 1 local space, adjusted for aspect ratio
    vec2 uv = (UV * 2.) - 1.;
    //float aspect = iResolution.x / iResolution.y;
    //uv.x *= aspect;

    // Calculate distance and fill circle with white
    float distance = 1.0 - length(uv);
    vec3 colorMod = vec3(smoothstep(0.0, fade, distance));
    colorMod *= vec3(smoothstep(thickness + fade, thickness, distance));

    // Set output color
    fragColor = vec4(colorMod, colorMod);
    fragColor.rgb *= Color.xyz;
}