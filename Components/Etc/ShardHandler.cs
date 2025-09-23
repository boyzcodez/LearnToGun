using Godot;
using System;

public partial class ShardHandler : Node2D
{
    public void Trigger()
    {
        foreach (var child in GetChildren())
        {
            if (child is Shard shard)
            {
                shard.GlobalPosition = GlobalPosition;
                shard.Start();
                shard.Activate();
            } 
        }
    }
}
