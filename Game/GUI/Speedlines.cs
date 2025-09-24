using Godot;
using System;

public partial class Speedlines : TextureRect
{
    private Tween _tween;
    private ShaderMaterial _material;

    public override void _Ready()
    {
        _material = (ShaderMaterial)Material;
        Speed();
    }


    private void Speed()
    {
        _tween?.Kill();
        _tween = CreateTween();
        _tween.SetLoops();
        _tween.TweenProperty(_material, "shader_parameter/sample_radius", 1.0f, 0.5f);
        _tween.TweenProperty(_material, "shader_parameter/sample_radius", 0.6f, 0.5f);
    }
}
