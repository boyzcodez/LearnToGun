using Godot;

public partial class TransitionRect : ColorRect
{
    private Tween tween;
    private Material myMaterial;
    private bool rectUp = false;

    public override void _Ready()
    {
        myMaterial = this.Material;
        EventBus.Transition += Transition;
    }

    private void Transition()
    {
        if (rectUp)
            TransitionIn();
        else
            TransitionOut();
    }
    private void TransitionOut()
    {
        rectUp = true;

        if (tween != null)
            tween.Kill();

        tween = CreateTween();

        tween.TweenProperty(
        myMaterial, "shader_parameter/progress", 1f, 3f
        ).SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);
    }
    private void TransitionIn()
    {
        rectUp = false;

        if (tween != null)
            tween.Kill();

        tween = CreateTween();

        tween.TweenProperty(
        myMaterial, "shader_parameter/progress", 0f, 3f
        ).SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("space"))
        {
            Transition();
        }
    }

}
