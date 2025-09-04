using Godot;

public partial class Effects : AnimatedSprite2D
{
    public override void _Ready()
    {
        EventBus.ClearBullets += ClearEffect;
    }
    private void ClearEffect()
    {
        Play("default");
    }

}
