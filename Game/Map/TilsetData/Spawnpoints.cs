using Godot;
using System;

public partial class Spawnpoints : Node2D
{
    public void SpawnEnemies()
    {
        foreach (AnimatedSprite2D child in GetChildren())
        {
            child.Play("default");
        }
    }
}