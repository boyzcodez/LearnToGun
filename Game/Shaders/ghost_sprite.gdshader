shader_type canvas_item;

uniform float time : hint_range(0.0, 1.0);
uniform vec4 color_tint : source_color;

void fragment() {
	vec2 uv = UV;
	vec4 tex = texture(TEXTURE, uv);

	// RGB split glitch (Sandevistan style)
	vec2 offset = vec2(0.005, 0.0);
	float r = texture(TEXTURE, uv - offset).r;
	float g = texture(TEXTURE, uv).g;
	float b = texture(TEXTURE, uv + offset).b;

	vec4 glitch_color = vec4(r, g, b, tex.a);
	COLOR = glitch_color * color_tint;
}

