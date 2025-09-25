shader_type canvas_item;

uniform float progress : hint_range(0.0, 1.0) = 0.0;
uniform vec2 center = vec2(0.5, 0.5);

void fragment() {
    float dist = distance(UV, center);
    float radius = progress;

    if (dist > radius) {
        discard; // Completely hide pixel outside the circle
    }
}