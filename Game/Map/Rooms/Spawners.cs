using Godot;
using System;
using System.Collections.Generic;

public partial class Spawners : Node2D
{
    [Export] PortalMachine portal;
    private List<Vector2I> spawnLocations = new ();
    public override void _Ready()
    {
        foreach (Marker2D marker in GetChildren())
        {
            spawnLocations.Add((Vector2I)marker.GlobalPosition);
        }

        if (spawnLocations.Count <= 0)
        {
            EventBus.TriggerEndOfRound();
        } 
        else
        {
            EventBus.TriggerRound();
        }
    }

}
