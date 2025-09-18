using Godot;
using System;

public partial class MapHandler : Node2D
{
    [Export] private PackedScene startingMap;
    [Export] public PackedScene[] maps { get; set; } = Array.Empty<PackedScene>();
    [Export] private Node2D ysort;
    private RandomNumberGenerator rng = new RandomNumberGenerator();
    private Node2D child;

    public override void _Ready()
    {
        EventBus.Reset += SpawnStartingMap;
        EventBus.MapSwitch += SpawnRoom;
        SpawnStartingMap();
    }
    private void SpawnStartingMap()
    {
        ClearMap();

        var instance = startingMap.Instantiate() as Node2D;
        ysort.CallDeferred(MethodName.AddChild, instance);
        child = instance;
    }
    private void SpawnRoom()
    {
        ClearMap();
        rng.Randomize();

        if (maps.Length > 0)
        {
            // Pick a random index
            int index = rng.RandiRange(0, maps.Length - 1);

            // Get the scene and instance it
            var scene = maps[index];
            if (scene != null)
            {
                var instance = scene.Instantiate();
                ysort.AddChild(instance);
            }
        }

        ToSignal(GetTree().CreateTimer(0.5f), "timeout");

        EventBus.TriggerTransition();
        EventBus.TriggerWave();
    }
    private void ClearMap()
    {
        if (child == null) return;
        child.QueueFree();
    }

}
