shader_type canvas_item;

uniform float pixel_size : hint_range(1.0, 16.0) = 1.0;
uniform int mix_sampling : hint_range(-1, 2) = 0;
uniform bool flip_h = false;
uniform bool flip_v = false;

uniform vec4 outline_color : source_color = vec4(1.0, 0.0, 0.0, 1.0);
uniform bool enable_outline = true;
uniform float outline_thickness : hint_range(1.0, 4.0) = 1.0; // Thickness in pixels

varying float theta;
varying vec2 block_size;

// Rotated neighbor offsets for outline detection
vec2 get_rotated_neighbor_offset(int index, vec2 pixel_offset) {
    vec2 offsets[8] = vec2[](
        vec2(0.0, pixel_offset.y),   // up
        vec2(0.0, -pixel_offset.y),  // down
        vec2(pixel_offset.x, 0.0),   // right
        vec2(-pixel_offset.x, 0.0),  // left
        vec2(pixel_offset.x, pixel_offset.y),    // up-right
        vec2(pixel_offset.x, -pixel_offset.y),   // down-right
        vec2(-pixel_offset.x, pixel_offset.y),   // up-left
        vec2(-pixel_offset.x, -pixel_offset.y)   // down-left
    );
    return offsets[index];
}

void vertex() {
    theta = atan(MODEL_MATRIX[0][1], MODEL_MATRIX[0][0]);
    block_size = vec2(
        pixel_size * TEXTURE_PIXEL_SIZE.x / length(MODEL_MATRIX[0].xy),
        pixel_size * TEXTURE_PIXEL_SIZE.y / length(MODEL_MATRIX[1].xy)
    );
}

void fragment() {
    mat2 rotation = mat2(vec2(cos(theta), sin(theta)), vec2(-sin(theta), cos(theta)));
    mat2 inverse_rotation = mat2(vec2(cos(theta), -sin(theta)), vec2(sin(theta), cos(theta)));

    // Snap UV to rotated pixel grid
    vec2 snapped_uv = inverse_rotation *
        (floor(rotation * ((UV - 0.5) / block_size)) + 0.5) * block_size + 0.5;

    // Mirror if needed
    if (flip_v) snapped_uv.y = 1.0 - snapped_uv.y;

    vec4 color = texture(TEXTURE, snapped_uv);

    if (mix_sampling > 0) {
        vec4 min_color = texture(TEXTURE, snapped_uv + get_rotated_neighbor_offset(0, block_size));
        float min_difference = length(color - min_color);
        for (int i = 1; i < 4 * mix_sampling; i++) {
            vec4 neighbor_color = texture(TEXTURE, snapped_uv + get_rotated_neighbor_offset(i, block_size));
            float difference = length(color - neighbor_color);
            if (difference < min_difference) {
                min_color = neighbor_color;
                min_difference = difference;
            }
        }
        color = min_color;
    }

    float cut_x = max(abs(rotation[0][0] + rotation[0][1]), abs(inverse_rotation[0][0] + inverse_rotation[0][1])) * block_size.x * 0.5;
    float cut_y = max(abs(rotation[1][0] + rotation[1][1]), abs(inverse_rotation[1][0] + inverse_rotation[1][1])) * block_size.y * 0.5;
    if (snapped_uv.x < cut_x || snapped_uv.x > 1.0 - cut_x || snapped_uv.y < cut_y || snapped_uv.y > 1.0 - cut_y)
        color.a = 0.0;
    else
        color.a = step(0.5, color.a);

    // ----- PIXEL-PERFECT OUTLINE LOGIC -----
    if (enable_outline && color.a < 0.5) {
        bool neighbor_opaque = false;

        // Pixel offset adjusted for rotation and outline thickness
        vec2 pixel_offset = outline_thickness * block_size;

        for (int i = 0; i < 8; i++) {
            vec2 neighbor_uv = snapped_uv + get_rotated_neighbor_offset(i, pixel_offset);
            vec4 neighbor_color = texture(TEXTURE, neighbor_uv);
            if (neighbor_color.a > 0.5) {
                neighbor_opaque = true;
                break;
            }
        }

        if (neighbor_opaque) {
            color = outline_color;
            color.a = 1.0;
        }
    }

    COLOR = color;
}
