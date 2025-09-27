using Godot;
using System;

public partial class Coin : Sprite2D
{
    public float pickupRange = 20f;
    private NavigationAgent2D navAgent;
    private Player player;
    private Line2D trail;

    public override void _Ready()
    {
        player = GetTree().GetFirstNodeInGroup("Player") as Player;
        navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        trail = GetNode<Line2D>("Trail");
    }

    public override void _PhysicsProcess(double delta)
    {
    }
    public void Activate()
    {
        Show();
        SetPhysicsProcess(true);
        trail.SetPhysicsProcess(true); 
    }
    public void Deactivate()
    {
        Hide();
        SetPhysicsProcess(false); 
        trail.SetPhysicsProcess(false);
    }

}
