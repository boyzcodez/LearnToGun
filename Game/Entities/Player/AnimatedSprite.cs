using Godot;

[GlobalClass]
public partial class AnimatedSprite : AnimatedSprite2D
{
    [Signal]
    public delegate void AnimationDoneEventHandler();
    public int animationPriority = 0;
    public virtual void PlayAnimation(string animation, int priority = 0)
    {
    }

    private void _on_animation_finished()
    {
        animationPriority = 0;
        EmitSignal(SignalName.AnimationDone);
    }
    public void Activate()
    {
        SetProcess(true);
    }
    public void Deactivate()
    {
        SetProcess(false);
    }
}
