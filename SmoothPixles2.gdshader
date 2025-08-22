shader_type canvas_item;
render_mode unshaded;


uniform vec4 flash_color = vec4(1.0, 1.0, 1.0, 1.0); // color of flash
uniform float flash_strength : hint_range(0.0, 1.0) = 0.0;       // controlled by script

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
	vec4 texColor = texturePointSmooth(TEXTURE, UV, TEXTURE_PIXEL_SIZE);
	// Blend between texture and flash
	COLOR = mix(texColor, flash_color, flash_strength) * texColor.a;
}
