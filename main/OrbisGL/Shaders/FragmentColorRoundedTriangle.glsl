#version 100

precision highp float;

varying vec2 UV;

uniform vec4 Color;
uniform vec2 Resolution;
uniform float Border;
uniform float Rotate;
uniform vec4 VisibleRect;

//Shadertoy Emulation
#define fragColor gl_FragColor
#define iResolution Resolution
#define fragCoord Resolution * UV

//stolen from https://www.shadertoy.com/view/tlVyWh

float cro( in vec2 a, in vec2 b ) { return a.x*b.y - a.y*b.x; }


vec3 sdgTriangle( in vec2 p, in vec2 v[3] )
{
    float gs = cro(v[0]-v[2],v[1]-v[0]);
    vec4 res;
    
    // edge 0
    {
    vec2  e = v[1]-v[0];
    vec2  w = p-v[0];
    vec2  q = w-e*clamp(dot(w,e)/dot(e,e),0.0,1.0);
    float d = dot(q,q);
    float s = gs*cro(w,e);
    res = vec4(d,q,s);
    }
    
    // edge 1
    {
	vec2  e = v[2]-v[1];
    vec2  w = p-v[1];
    vec2  q = w-e*clamp(dot(w,e)/dot(e,e),0.0,1.0);
    float d = dot(q,q);
    float s = gs*cro(w,e);
    res = vec4( (d<res.x) ? vec3(d,q) : res.xyz,
                (s>res.w) ?      s    : res.w );
    }
    
    // edge 2
    {
	vec2  e = v[0]-v[2];
    vec2  w = p-v[2];
    vec2  q = w-e*clamp(dot(w,e)/dot(e,e),0.0,1.0);
    float d = dot(q,q);
    float s = gs*cro(w,e);
    res = vec4( (d<res.x) ? vec3(d,q) : res.xyz,
                (s>res.w) ?      s    : res.w );
    }
    
    // distance and sign
    float d = sqrt(res.x)*sign(res.w);
    
    return vec3(d,res.yz/d);

}

#define AA 2

void main()
{
    vec3 tot = vec3(0.0);
    
    float proportion;
    
    if (iResolution.y > iResolution.x)
        proportion = (iResolution.x/iResolution.y) * iResolution.x;
    else
        proportion = (iResolution.y/iResolution.x) * iResolution.y;
    
    #if AA>1
    for( int m=0; m<AA; m++ )
    for( int n=0; n<AA; n++ )
    {
        // pixel coordinates
        vec2 o = vec2(float(m),float(n)) / float(AA) - 0.5;
        vec2 p = (-iResolution.xy + 2.0*(fragCoord+o))/proportion;
        #else    
        vec2 p = (-iResolution.xy + 2.0*fragCoord)/proportion;
        #endif

        // corner radious
        float ra = 0.1;
        
        float rd = ra/10.;
        
        float MinX = -1. + rd;
        float MaxX =  1. - rd;
        float MidY = 0.;
        float MidX = 0.;
        float MinY = -1.;
        float MaxY =  1.;
        
        vec2 v[3];        
        
        
        if (Rotate >= 3.5) {
            v[0] = vec2(MinX, MinY) * (1. - ra);
            v[1] = vec2(MaxX, MaxY) * (1. - ra);
            v[2] = vec2(MinX, MaxY) * (1. - ra);
        } else if (Rotate >= 3.) {
            v[0] = vec2(MaxX, MinY) * (1. - ra);
            v[1] = vec2(MaxX, MaxY) * (1. - ra);
            v[2] = vec2(MinX, MidY) * (1. - ra);
        } else if (Rotate >= 2.5){
            v[0] = vec2(MinX, MaxY) * (1. - ra);
            v[1] = vec2(MaxX, MinY) * (1. - ra);
            v[2] = vec2(MinX, MinY) * (1. - ra);
        } else if (Rotate >= 2.){
            v[0] = vec2(MinX, MaxY) * (1. - ra);
            v[1] = vec2(MaxX, MaxY) * (1. - ra);
            v[2] = vec2(MidX, MinY) * (1. - ra);
        } else if (Rotate >= 1.5){
            v[0] = vec2(MaxX, MinY) * (1. - ra);
            v[1] = vec2(MinX, MinY) * (1. - ra);
            v[2] = vec2(MaxX, MaxY) * (1. - ra);
        } else if (Rotate >= 1.){
            v[0] = vec2(MinX, MaxY) * (1. - ra);
            v[1] = vec2(MinX, MinY) * (1. - ra);
            v[2] = vec2(MaxX, MidY) * (1. - ra);
        } else if (Rotate >= 0.5){
            v[0] = vec2(MinX, MaxY) * (1. - ra);
            v[1] = vec2(MaxX, MinY) * (1. - ra);
            v[2] = vec2(MaxX, MaxY) * (1. - ra);
        } else {
            v[0] = vec2(MinX, MinY) * (1. - ra);
            v[1] = vec2(MaxX, MinY) * (1. - ra);
            v[2] = vec2(MidX, MaxY) * (1. - ra);
        }

        // sdf(p) and gradient(sdf(p))
        vec3  dg = sdgTriangle(p,v);
        float d = dg.x-ra;

        // coloring
        vec3 col = (d>0.0) ? vec3(0.) : vec3(1.);

 	    tot += col;
    #if AA>1
    }
    tot /= float(AA*AA);
    #endif

	fragColor = vec4(Color.xyz * tot, tot * Color.w);
    
    if ((UV.x < VisibleRect.x || UV.y < VisibleRect.y || UV.x > VisibleRect.z + VisibleRect.x || UV.y > VisibleRect.w + VisibleRect.y) && VisibleRect != vec4(0))
        discard;
}