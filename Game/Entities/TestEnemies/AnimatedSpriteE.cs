using Godot;
using System;

public partial class AnimatedSpriteE : AnimatedSprite
{
    private Player player;
    private Entity self;
    private Look look;
    private string direction = "Front";

    public override void _Ready()
    {
        self = GetOwner<Entity>();
        look = GetNode<Look>("Look");
        player = GetTree().GetFirstNodeInGroup("Player") as Player;
    }
    public override void _Process(double delta)
    {
        if (self.Velocity != Vector2.Zero)
            PlayAnimation("Run", 1);
        else
            PlayAnimation("Idle", 1);

        // if (look.Rotation > -1.5f && look.Rotation < 1.5f)
        //     FlipH = false;
        // else
        //     FlipH = true;
        Vector2 dir = (player.GlobalPosition - GlobalPosition).Normalized();
        FlipH = dir.X < 0;

        if (dir.Y > 0)
        {
            direction = "Front";
            look.ShowBehindParent = false;
        }
        else
        {
            direction = "Back";
            look.ShowBehindParent = true;
        }
        //direction = dir.Y > 0 ? "Front" : "Back";

        
    }
    public override void PlayAnimation(string animation = "", int value = 0)
    {
        if (value >= animationPriority)
        {
            animationPriority = value;
            Play(animation + direction);
        }
            
    }
}
