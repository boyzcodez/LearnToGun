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

        parent.PlayAnimation("Hit", 2);

        if (tween != null && tween.IsValid())
            tween.Kill();

        tween = CreateTween();

        tween.TweenMethod(
            Callable.From<float>(SetShader_BlinkIntensity),
            1.0f,   // from
            0.0f,   // to
            0.4f    // duration
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
