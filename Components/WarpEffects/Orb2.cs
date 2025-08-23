using Godot;
using System;

public partial class Orb2 : Sprite2D
{
    private readonly RandomNumberGenerator _rng = new RandomNumberGenerator();
    private Vector2 _origin;

    public override void _Ready()
    {
        _rng.Randomize();
        _origin = Position;
        StartWobbleLoop();
    }

    private async void StartWobbleLoop()
    {
        while (IsInsideTree())
        {
            // choose a random direction & distance
            float angle = _rng.RandfRange(0f, (float)(Math.PI * 2.0));
            float distance = _rng.RandfRange(1f, 7f);
            Vector2 target = _origin + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;

            // tween toward the target
            float duration = _rng.RandfRange(0.2f, 0.4f);
            var tween = CreateTween()
                .SetTrans(Tween.TransitionType.Sine)
                .SetEase(Tween.EaseType.InOut);
            tween.TweenProperty(this, "position", target, duration);
            await ToSignal(tween, "finished");

            // tween back to origin
            var tweenBack = CreateTween()
                .SetTrans(Tween.TransitionType.Sine)
                .SetEase(Tween.EaseType.InOut);
            tweenBack.TweenProperty(this, "position", _origin, duration);
            await ToSignal(tweenBack, "finished");

            // small random pause before repeating
            await ToSignal(GetTree().CreateTimer(_rng.RandfRange(0.05f, 0.2f)), "timeout");
        }
    }
}
