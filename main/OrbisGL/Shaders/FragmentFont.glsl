#version 100

precision highp float;

varying lowp vec2 UV;

uniform lowp vec4 Color;
uniform sampler2D Texture;

void main(void) {
    vec4 Pixel = texture2D(Texture, UV);
    gl_FragColor = (Pixel * Color) / vec4(1);
}