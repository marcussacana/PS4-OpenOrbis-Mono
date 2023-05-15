namespace Orbis
{
    internal static class ShaderSource
    {

public const string Vertex =
@"
attribute vec3 Position;

void main(void) {
    gl_Position = vec4(Position, 1.0);
}
";

public const string UVVertex =
@"
attribute vec3 Position;
attribute vec2 uv;
 
varying lowp vec2 UV;

void main(void) {
    gl_Position = vec4(Position, 1.0);
    UV = uv;
}
";

public const string FragmentColor =
@"
uniform lowp vec4 Color;
 
void main(void) {
    gl_FragColor = Color;
}
";

public const string FragmentTexture =
@"
varying lowp vec2 UV;

uniform sampler2D Texture;
 
void main(void) {
    gl_FragColor = texture2D(Texture, UV);
}
";
    }
}
