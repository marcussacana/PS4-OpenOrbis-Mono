#version 100

attribute vec3 Position;

uniform vec3 Offset;

void main(void) {
    gl_Position = vec4(Position + Offset, 1.0);
}