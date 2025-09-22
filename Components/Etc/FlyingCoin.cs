using Godot;
using System;

public partial class FlyingCoin : AnimatedSprite2D
{
    private Player playr;
    private Vector2 vel;
    public override void _Ready()
    {
        playr = GetTree().GetFirstNodeInGroup("Player") as Player;
    }

    public override void _PhysicsProcess(double delta)
    {
        Rotation += (float)(delta * 16.0); // Adjust 1.0 to change rotation speed

        Vector2 dir = (playr.GlobalPosition - GlobalPosition).Normalized();
        vel = vel.MoveToward(dir * 200f, (float)delta * 400f);

        GlobalPosition += vel * (float)delta;

        float distanceToPlayer = GlobalPosition.DistanceTo(playr.GlobalPosition);
        if (distanceToPlayer < 20f)
        {
            EventBus.Money(1);
            QueueFree();
        } 
    }
}
