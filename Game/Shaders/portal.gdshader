shader_type canvas_item;

uniform float time : hint_range(0.0, 100.0) = 0.0;
uniform vec4 main_color : source_color = vec4(0.8, 0.2, 1.0, 1.0);
uniform float radius = 0.4;
uniform float glow_strength = 0.1;

float circle(vec2 uv, vec2 pos, float r) {
    return smoothstep(r, r - 0.01, length(uv - pos));
}

void fragment() {
    vec2 uv = (FRAGCOORD.xy / SCREEN_PIXEL_SIZE.xy) * 2.0 - 1.0;
    uv.x *= SCREEN_PIXEL_SIZE.x / SCREEN_PIXEL_SIZE.y;

    // Base portal circle
    float base = circle(uv, vec2(0.0), radius);

    // Add orbiting circles
    float rings = 0.0;
    for (int i = 0; i < 8; i++) {
        float angle = time * 0.6 + float(i) * 0.8;
        vec2 pos = vec2(cos(angle), sin(angle)) * (radius + 0.1);
        rings += circle(uv, pos, 0.08);
    }

    // Combine
    float portal = base + rings;

    // Glow effect
    float glow = smoothstep(0.0, 0.02, portal);

    COLOR = vec4(main_color.rgb, glow);
}
