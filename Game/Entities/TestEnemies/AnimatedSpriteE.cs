using Godot;
using System;

public partial class AnimatedSpriteE : AnimatedSprite
{
    private Entity self;
    private Look look;

    public override void _Ready()
    {
        self = GetOwner<Entity>();
        look = GetNode<Look>("Look");
    }
    public override void Physics(double delta)
    {
        if (self.Velocity != Vector2.Zero)
            PlayAnimation("Run", 1);
        else
            PlayAnimation("Idle", 1);

        if (look.Rotation > -1.5f && look.Rotation < 1.5f)
            FlipH = false;
        else
            FlipH = true;
    }
    public override void PlayAnimation(string animation = "", int value = 0)
    {
        if (value >= animationPriority)
        {
            animationPriority = value;
            Play(animation);
        }
            
    }
}
