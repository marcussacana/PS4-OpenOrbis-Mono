#version 100

precision highp float;

varying vec2 UV;

uniform vec4 Color;
uniform vec2 Resolution;
uniform vec3 CircleConfig;

//Shadertoy Emulation
#define fragColor gl_FragColor
#define iResolution Resolution
#define fragCoord Resolution * UV

void main(void)
{
    //Angle can be from -3,14159 to 3,14159 (Ohh, Pi, it's you again!)
    // Parameters 
    float StartAngle = min(CircleConfig.y, CircleConfig.x);
    float EndAngle = max(CircleConfig.x, CircleConfig.y);
    float Thickness = CircleConfig.z;

    float fade = 4.0 / iResolution.y;

    // -1 -> 1 local space, adjusted for aspect ratio
    //vec2 uv = fragCoord / iResolution.xy * 2.0 - 1.0;
    vec2 uv = (UV * 2.) - 1.;

    // Calculate distance and angle
    float distance = length(uv) + 0.01;
    float angle = atan(uv.y, uv.x);

    // Check if angle is within range
    if (angle >= StartAngle && angle <= EndAngle) {
        // Fill circle with white
        vec3 color = vec3(smoothstep(1.0 - Thickness - fade, 1.0 - Thickness, distance));
        color *= vec3(smoothstep(1.0 + fade, 1.0, distance));

        // Set output color
        fragColor = vec4(color, color); //vec4(color, smoothstep(1.0 + fade, 1.0, distance));
        fragColor.rgb *= Color.xyz;

        // Apply anti-aliasing to start and end edges
        float startEdge = smoothstep(StartAngle - fade, StartAngle + fade, angle);
        float endEdge = smoothstep(EndAngle + fade, EndAngle - fade, angle);
        fragColor.a *= min(startEdge, endEdge);
    } else {
        discard;
    }
}