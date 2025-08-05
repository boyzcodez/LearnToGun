shader_type canvas_item;

uniform float time;
uniform vec4 beamColor : source_color = vec4(1.0, 0.2, 1.0, 1.0);
uniform float scroll_speed = 2.0;
uniform sampler2D noise_tex : source_color;

void fragment() {
	vec2 uv = UV;
	
	uv.y += time * scroll_speed;
	
	vec4 tex_color = texture(TEXTURE, uv);
	
	float noise_val = texture(noise_tex, uv).r;
	tex_color.rgb *= mix(1.0, noise_val, 0.4);
	
	COLOR = beamColor * tex_color;
}

