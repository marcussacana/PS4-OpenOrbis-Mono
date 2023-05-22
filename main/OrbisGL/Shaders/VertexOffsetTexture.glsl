attribute vec3 Position;
attribute vec2 uv;

uniform vec3 Offset;
 
varying lowp vec2 UV;

void main(void) {
    gl_Position = vec4(Position + Offset, 1.0);
    UV = uv;
}