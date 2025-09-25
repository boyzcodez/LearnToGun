using Godot;

public partial class Portal2 : Sprite2D
{
    private Tween tween;
    public async void GrowPortal()
    {
        if (tween != null)
            tween.Kill();
        tween = CreateTween();

        tween.TweenProperty(this, "scale", new Vector2(1, 1), 0.3f).SetEase(Tween.EaseType.Out);

        // await ToSignal(tween, Tween.SignalName.Finished);

        await ToSignal(tween, "finished");

        //EventBus.TriggerTransition();
    }
    public async void ShrinkPortal()
    {
        if (tween != null)
            tween.Kill();
        tween = CreateTween();

        tween.TweenProperty(this, "scale", new Vector2(0, 0), 0.3f).SetEase(Tween.EaseType.Out);

        // await ToSignal(tween, Tween.SignalName.Finished);

        await ToSignal(tween, "finished");

        //EventBus.TriggerTransition();
    }
}
