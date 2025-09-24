using Godot;
using System;

public partial class Spawnpoints : Node2D
{
    public override void _Ready()
    {
        foreach (AnimatedSprite2D child in GetChildren())
        {
            child.Play("default");
        }
    }
}