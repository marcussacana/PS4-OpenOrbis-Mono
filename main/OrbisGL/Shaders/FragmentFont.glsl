#version 100

precision highp float;

varying lowp vec2 UV;

uniform lowp vec4 Color;
uniform lowp vec4 BackColor;
uniform sampler2D Texture;
uniform vec4 VisibleRect;

void main(void) {
    vec4 Pixel = texture2D(Texture, UV);
    gl_FragColor = (Pixel * Color) / vec4(1);

    if (BackColor != vec4(0))
        gl_FragColor = mix(BackColor, gl_FragColor, gl_FragColor.a);

    if ((UV.x < VisibleRect.x || UV.y < VisibleRect.y || UV.x > VisibleRect.z + VisibleRect.x || UV.y > VisibleRect.w + VisibleRect.y) && VisibleRect != vec4(0))
        discard;
}