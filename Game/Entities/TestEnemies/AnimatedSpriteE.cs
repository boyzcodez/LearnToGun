using Godot;
using System;

public partial class AnimatedSpriteE : AnimatedSprite2D
{
    private Entity self;
    private Look look;

    public override void _Ready()
    {
        self = GetOwner<Entity>();
        look = GetNode<Look>("Look");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (self.Velocity != Vector2.Zero)
            Play("run");
        else
            Play("idle");

        if (look.Rotation > -1.5f && look.Rotation < 1.5f)
            FlipH = false;
        else
            FlipH = true;
    }


}
