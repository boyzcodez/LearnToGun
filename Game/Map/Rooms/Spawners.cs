using Godot;
using System;
using System.Collections.Generic;

public partial class Spawners : Node2D
{
    [Export] PortalMachine portal;
    private List<Vector2> spawnLocations = new ();
    public override void _Ready()
    {
        foreach (Marker2D marker in GetChildren())
        {
            spawnLocations.Add(marker.GlobalPosition);
        }

        if (spawnLocations.Count <= 0)
        {
            EventBus.TriggerEndOfRound();
        } 
        else
        {
            EventBus.TriggerRound(spawnLocations);
        }
    }

}
