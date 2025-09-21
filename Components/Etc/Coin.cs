using Godot;
using System;

public partial class Coin : Sprite2D
{
    private Player player;

    public override void _Ready()
    {
        player = GetTree().GetFirstNodeInGroup("Player") as Player;
    }

    public override void _PhysicsProcess(double delta)
    {
    }
    public void Activate()
    {

    }
    public void Deactivate()
    {
        
    }

}
