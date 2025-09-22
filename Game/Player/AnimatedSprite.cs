using Godot;

[GlobalClass]
public partial class AnimatedSprite : AnimatedSprite2D
{
    public int animationPriority = 0;
    public virtual void PlayAnimation(string animation, int priority = 0)
    {
    }
    
    private void _on_animation_finished()
    {
        animationPriority = 0;
    }
}
