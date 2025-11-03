using Godot;
using System;

public enum DirectionMode
{
    TwoDirections,
    FourDirections
}

public partial class AnimatedSpriteE : AnimatedSprite
{
    [Export]
    public DirectionMode directionMode = DirectionMode.TwoDirections;

    private Player player;
    private Entity self;
    private Look look;
    private string vel = "Idle";
    private string direction = "Front";

    public override void _Ready()
    {
        self = GetOwner<Entity>();
        look = GetNode<Look>("Look");
        player = GetTree().GetFirstNodeInGroup("Player") as Player;

        GetOwner<Entity>().Connect(Entity.SignalName.Activation, new Callable(this, nameof(Activate)));
        GetOwner<Entity>().Connect(Entity.SignalName.Deactivation, new Callable(this, nameof(Deactivate)));

        SetProcess(false);
        Stop();
    }
    public override void _Process(double delta)
    {
        if (self.Velocity != Vector2.Zero)
            vel = "Run";
        else
            vel = "Idle";

        // // if (look.Rotation > -1.5f && look.Rotation < 1.5f)
        // //     FlipH = false;
        // // else
        // //     FlipH = true;
        // Vector2 dir = (player.GlobalPosition - GlobalPosition).Normalized();
        // FlipH = dir.X < 0;

        // if (dir.Y > 0)
        // {
        //     direction = "Front";
        //     look.ShowBehindParent = false;
        // }
        // else
        // {
        //     direction = "Back";
        //     look.ShowBehindParent = true;
        // }
        //direction = dir.Y > 0 ? "Front" : "Back";
        var result = AnimationPick.GetAnimationFromRotation(look.Rotation, directionMode);
        FlipH = result.FlipH;
        look.ShowBehindParent = result.ShowBehindParent;
        direction = result.AnimationName;
        PlayAnimation(vel, 1);
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
