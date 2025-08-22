using Godot;
using System;

public partial class MapHandler : Node2D
{
    [Export] private PackedScene startingMap;
    [Export] public PackedScene[] maps { get; set; } = Array.Empty<PackedScene>();
    private RandomNumberGenerator rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        EventBus.Reset += SpawnStartingMap;
        EventBus.MapSwitch += SpawnRoom;
        SpawnStartingMap();
    }
    private void SpawnStartingMap()
    {
        ClearMap();

        var instance = startingMap.Instantiate();
        CallDeferred(MethodName.AddChild, instance);
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
                AddChild(instance);
            }
        }

        ToSignal(GetTree().CreateTimer(0.5f), "timeout");

        EventBus.TriggerTransition();
        EventBus.TriggerWave();
    }
    private void ClearMap()
    {
        foreach (var child in GetChildren())
        {
            child.QueueFree();
        }
    }

}
