using Godot;

public partial class Effects : AnimatedSprite2D
{
    private AnimationPlayer animationPlayer;
    public override void _Ready()
    {
        EventBus.ClearBullets += ClearEffect;
        EventBus.Reset += ClearEffect;
        EventBus.MapSwitch += ClearEffect;

        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }
    private void ClearEffect()
    {
        animationPlayer.Play("Shockwave");
        Play("default");
    }

}
