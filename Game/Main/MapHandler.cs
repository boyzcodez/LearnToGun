using Godot;
using System;
using System.Collections.Generic;

public partial class MapHandler : Node2D
{
    [Export] private PackedScene startingMap;
    [Export] public PackedScene[] maps { get; set; } = Array.Empty<PackedScene>();
    [Export] private Node2D ysort;

    private Queue<PackedScene> preloadQueue = new();
    private Queue<string> preloadPaths = new();
    private Random random = new();

    private const int PRELOAD_BUFFER_SIZE = 3;

    private RandomNumberGenerator rng = new RandomNumberGenerator();
    private Node2D child;
    private Player player;

    public override void _Ready()
    {
        player = GetTree().GetFirstNodeInGroup("Player") as Player;

        EventBus.Reset += SpawnStartingMap;
        EventBus.MapSwitch += SpawnRoom;
        SpawnStartingMap();

        for (int i = 0; i < PRELOAD_BUFFER_SIZE; i++)
        {
            QueueRandomRoom();
        }

        ProcessLoading();
    }
    private void QueueRandomRoom()
    {
        if (maps.Length == 0) return;

        int index = random.Next(maps.Length);
        var scene = maps[index];

        string path = scene.ResourcePath;
        ResourceLoader.LoadThreadedRequest(path);
        preloadPaths.Enqueue(path);
    }

    public async void ProcessLoading()
    {
        while (preloadPaths.Count > 0)
        {
            string path = preloadPaths.Peek();

            var status = ResourceLoader.LoadThreadedGetStatus(path);
            if (status == ResourceLoader.ThreadLoadStatus.Loaded)
            {
                var packed = (PackedScene)ResourceLoader.LoadThreadedGet(path);
                preloadQueue.Enqueue(packed);
                preloadPaths.Dequeue();
            }
            else
            {
                await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
            }
        }
    }
    public Node SpawnNextRoom()
    {
        if (preloadQueue.Count == 0)
        {
            GD.PrintErr("Preload queue empty! Spawning failed");
            return null;
        }

        var packed = preloadQueue.Dequeue();
        var instance = packed.Instantiate();

        QueueRandomRoom();
        ProcessLoading();

        return instance;
    }
    private void SpawnStartingMap()
    {
        ClearMap();

        var instance = startingMap.Instantiate() as Node2D;
        instance.GlobalPosition = player.GlobalPosition;
        ysort.CallDeferred(MethodName.AddChild, instance);
        child = instance;
    }
    private void SpawnRoom()
    {
        ClearMap();
        rng.Randomize();

        EventBus.room += 1;

        if (EventBus.room == 1) EventBus.gameOn = true;

        Node nextRoom = SpawnNextRoom();
        ysort.CallDeferred("add_child", nextRoom);
        child = nextRoom as Node2D;

        // if (maps.Length > 0)
        // {
        //     // Pick a random index
        //     int index = rng.RandiRange(0, maps.Length - 1);

        //     // Get the scene and instance it
        //     var scene = maps[index];
        //     if (scene != null)
        //     {
        //         var instance = scene.Instantiate() as Node2D;
        //         instance.GlobalPosition = player.GlobalPosition;
        //         child = instance;
        //         ysort.AddChild(instance);
        //     }
        // }

        GC.Collect();

        //ToSignal(GetTree().CreateTimer(0.5f), "timeout");

        //EventBus.TriggerTransition();
        //EventBus.TriggerWave();
    }
    private void ClearMap()
    {
        if (child == null) return;
        child.QueueFree();
    }

}
