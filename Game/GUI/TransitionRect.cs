using System.Threading.Tasks;
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
    private async void TransitionOut()
    {
        rectUp = true;

        if (tween != null)
            tween.Kill();

        tween = CreateTween();

        tween.TweenProperty(
        myMaterial, "shader_parameter/progress", 1f, 2f
        ).SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);

        await ToSignal(GetTree().CreateTimer(2.5f), "timeout");

        System.GC.Collect();

        EventBus.TriggerMapSwitch();
    }
    private void TransitionIn()
    {
        rectUp = false;

        if (tween != null)
            tween.Kill();

        tween = CreateTween();

        tween.TweenProperty(
        myMaterial, "shader_parameter/progress", 0f, 2f
        ).SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);

        EventBus.TriggerLock();
    }
}
