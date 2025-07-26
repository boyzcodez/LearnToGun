using Godot;
using System;
using System.Numerics;

public partial class AnimatedSprite : AnimatedSprite2D
{
    private string Running = "";
    private Node Direction;

    public override void _Ready()
    {
        Direction = GetNode<Node>("Direction");
    }

    public override void _PhysicsProcess(double delta)
    {

        if (Input.IsActionPressed("run"))
        {
            Running = "Run";
        } else {
            Running = "";
        }

        // Get the mouse position and calculate the direction based on the angle
        // And set which side of the six sided character is shown
        Godot.Vector2 mouse = GetLocalMousePosition();
        int a = (int)(Mathf.Snapped(mouse.Angle(), Mathf.Pi / 4.0f) / (Mathf.Pi / 4.0f));
        a = Mathf.Wrap(a, 0, 8);

        string directionString = (string)Direction.Call("GetDirection", a);
        Play(directionString + Running);
    }
}
