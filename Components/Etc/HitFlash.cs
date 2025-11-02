using Godot;

public partial class HitFlash : Node
{
    private AnimatedSprite parent;
    private Tween tween;

    public override void _Ready()
    {
        parent = GetParent<AnimatedSprite>();
    }

    public void Blink()
    {
        if (parent == null)
            return;

        if (tween != null && tween.IsValid())
            tween.Kill();

        tween = CreateTween();

        tween.TweenMethod(
            Callable.From<float>(SetShader_BlinkIntensity),
            1.5f,   // from
            0.0f,   // to
            0.3f    // duration
        );
    }

    private void SetShader_BlinkIntensity(float newValue)
    {
        if (parent.Material is ShaderMaterial shaderMaterial)
        {
            shaderMaterial.SetShaderParameter("blink_intensity", newValue);
        }
    }
}
