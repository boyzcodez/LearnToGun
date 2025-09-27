using Godot;
using System;

public partial class Shadow : Sprite2D
{
    private Tween tween;
    private Vector2 originalScale;

    public override void _Ready()
    {
        originalScale = Scale;
    }

    public void TriggerShrink(float shrinkFactor = 0.7f, float duration = 0.3f)
    {
        if (tween != null && tween.IsRunning())
            tween.Kill();

        tween = CreateTween();
        tween.TweenProperty(this, "scale", originalScale * shrinkFactor, duration)
             .SetTrans(Tween.TransitionType.Sine)
             .SetEase(Tween.EaseType.InOut);

        tween.TweenCallback(Callable.From(() =>
        {
            Tween growTween = CreateTween();
            growTween.TweenProperty(this, "scale", originalScale, duration)
                     .SetTrans(Tween.TransitionType.Sine)
                     .SetEase(Tween.EaseType.InOut);
        }));
    }
}
