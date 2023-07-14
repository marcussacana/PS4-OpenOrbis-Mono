#version 100

uniform lowp vec4 Color;

varying highp vec2 UV;

uniform highp vec4 VisibleRect;

void main(void) {
    gl_FragColor = Color;
    
    if ((UV.x < VisibleRect.x || UV.y < VisibleRect.y || UV.x > VisibleRect.z + VisibleRect.x || UV.y > VisibleRect.w + VisibleRect.y) && VisibleRect != vec4(0))
        discard;
}