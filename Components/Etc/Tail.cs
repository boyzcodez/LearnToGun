using Godot;
using System;

public partial class Tail : Line2D
{
    private AnimatedSprite2D parent;
    private int length = 10;
    private Vector2 offset;

    public override void _Ready()
    {
        parent = GetOwner<AnimatedSprite2D>();
        offset = Position;
        TopLevel = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        AddPoint(parent.GlobalPosition, 0);
        if (GetPointCount() > length) RemovePoint(GetPointCount() - 1);
    }



}
