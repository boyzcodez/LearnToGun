using Godot;
using System;

[GlobalClass]
public partial class Follow : State
{
    [Export] private float howClose = 20f;
    [Export] private float speed = 60f;
    [Export] private float prevRange = 900f;
    [Export] private float nextRange = 100f;
    public override void PhysicsProcess(double delta)
    {
        var direction = player.GlobalPosition - parent.GlobalPosition;

        if (direction.Length() > howClose)
        {
            avoidance.MoveDirection(direction.Normalized() * speed);
        }
        else
            parent.Velocity = Vector2.Zero;

        if (direction.Length() < nextRange && NextState != "Nothing")
        {
            EmitSignal("Transitioned", this, NextState);
        }
        else if (direction.Length() > prevRange && PrevState != "Nothing")
        {
            EmitSignal("Transitioned", this, PrevState);
        }
    }
}
