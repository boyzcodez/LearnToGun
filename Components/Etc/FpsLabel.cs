using Godot;
using System;

public partial class FpsLabel : Label
{
    public override void _Process(double delta)
    {
        Text = "fps: " + Engine.GetFramesPerSecond().ToString();
    }

}
