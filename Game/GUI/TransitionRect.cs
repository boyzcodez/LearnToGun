using Godot;
using System;

public partial class TransitionRect : ColorRect
{
    private Tween tween;
    private Material myMaterial;
    private bool rectUp = false;

    public override void _Ready()
    {
        myMaterial = this.Material;
        //EventBus.Transition += Transition;
    }

    public void Transition()
    {
        if (rectUp)
            TransitionIn();
        else
            TransitionOut();
    }
    public async void TransitionOut()
    {

        if (tween != null)
            tween.Kill();

        tween = CreateTween();

        tween.TweenProperty(
        myMaterial, "shader_parameter/progress", 1f, 2f
        ).SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);

        await ToSignal(tween, "finished");

        EventBus.TriggerMapSwitch();
    }
    public void TransitionIn()
    {

        if (tween != null)
            tween.Kill();

        tween = CreateTween();

        tween.TweenProperty(
        myMaterial, "shader_parameter/progress", 0f, 2f
        ).SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);

        //EventBus.TriggerLock();
    }
}
