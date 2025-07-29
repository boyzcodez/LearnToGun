shader_type canvas_item;
render_mode unshaded;

uniform float time : hint_range(0.0, 1.0);
uniform vec4 color_tint : source_color;

vec4 texturePointSmooth(sampler2D smp, vec2 uv, vec2 pixel_size)
{
	vec2 ddx = dFdx(uv);
	vec2 ddy = dFdy(uv);
	vec2 lxy = sqrt(ddx * ddx + ddy * ddy);

	vec2 uv_pixels = uv / pixel_size;

	vec2 uv_pixels_floor = round(uv_pixels) - vec2(0.5f);
	vec2 uv_dxy_pixels = uv_pixels - uv_pixels_floor;

	uv_dxy_pixels = clamp((uv_dxy_pixels - vec2(0.5f)) * pixel_size / lxy + vec2(0.5f), 0.0f, 1.0f);

	uv = uv_pixels_floor * pixel_size;

	return textureGrad(smp, uv + uv_dxy_pixels * pixel_size, ddx, ddy);
}

void fragment() {
	vec2 uv = UV;
	vec4 tex = texture(TEXTURE, uv);
	
	vec2 offset = vec2(0.005, 0.0);
	float r = texturePointSmooth(TEXTURE, uv - offset, TEXTURE_PIXEL_SIZE).r;
	float g = texturePointSmooth(TEXTURE, uv, TEXTURE_PIXEL_SIZE).g;
	float b = texturePointSmooth(TEXTURE, uv + offset, TEXTURE_PIXEL_SIZE).b;
	
	vec4 glitch_color = vec4(r, g, b, tex.a);
	COLOR = glitch_color * color_tint;
}

