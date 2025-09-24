shader_type canvas_item;

uniform vec4 color_center : source_color = vec4(1.0, 1.0, 1.0, 1.0); // center color
uniform vec4 color_edge   : source_color = vec4(0.0, 0.0, 0.0, 1.0); // edge color
uniform float radius : hint_range(0.0, 2.0) = 1.0; // how far center color reaches
uniform float softness : hint_range(0.0, 1.0) = 1.0; // smooth falloff

void fragment() {
    // UV is 0→1 across rect, so center it at (0.5, 0.5)
    vec2 centered = UV - vec2(0.5);
    
    // Distance from center, normalized (0 = center, 1 = edge)
    float dist = length(centered) * 2.0;
    
    // Apply radius and softness
    float t = smoothstep(radius, radius - softness, dist);
    
    // Blend between center and edge
    COLOR = mix(color_center, color_edge, clamp(t, 0.0, 1.0));
}
