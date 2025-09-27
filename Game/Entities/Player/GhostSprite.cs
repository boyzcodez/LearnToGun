using Godot;
using System;

public partial class GhostSprite : Node2D
{
    private Sprite2D sprite;
    public void Setup(Texture2D texture, Vector2 position, Color tintColor, bool flip_h)
    {

        if (sprite == null)
            sprite = GetNode<Sprite2D>("Sprite");
        
        sprite.FlipH = flip_h;
        sprite.Texture = texture;
        GlobalPosition = position;

        var mat = sprite.Material.Duplicate() as ShaderMaterial;
        sprite.Material = mat;
        mat.SetShaderParameter("color_tint", tintColor);

        var tween = CreateTween();
        tween.TweenProperty(this, "modulate:a", 0.0f, 0.25f);
        tween.TweenCallback(new Callable(this, "queue_free")).SetDelay(0.05f);
    }
}
