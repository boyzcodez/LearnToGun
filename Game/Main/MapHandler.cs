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

    private const int PRELOAD_BUFFER_SIZE = 1;

    private Node2D child;
    private Player player;

    public override void _Ready()
    {
        player = GetTree().GetFirstNodeInGroup("Player") as Player;

        EventBus.Reset += SpawnStartingMap;
        EventBus.MapSwitch += SpawnRoom;
        SpawnStartingMap();

        // for (int i = 0; i < PRELOAD_BUFFER_SIZE; i++)
        // {
        //     QueueRandomRoom();
        // }

        // ProcessLoading();
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
    public Node2D SpawnNextRoom()
    {
        if (preloadQueue.Count == 0)
        {
            GD.PrintErr("Preload queue empty! Spawning failed");
            return null;
        }

        var packed = preloadQueue.Dequeue();
        var instance = packed.Instantiate() as Node2D;

        QueueRandomRoom();
        ProcessLoading();

        return instance;
    }
    private void SpawnStartingMap()
    {
        ClearMap();

        EventBus.room = 0;
    }
    private void SpawnRoom()
    {
        ClearMap();

        EventBus.room += 1;

        if (EventBus.room == 1) EventBus.gameOn = true;

        // Node2D nextRoom = SpawnNextRoom();
        var nextRoom = maps[0].Instantiate() as Node2D;
        nextRoom.GlobalPosition = new Vector2(0, -500);
        
        ysort.CallDeferred("add_child", nextRoom);
        child = nextRoom;

        player.GlobalPosition = nextRoom.GlobalPosition;

        GC.Collect();
    }
    private void ClearMap()
    {
        if (!IsInstanceValid(child) || child == null) return;
        child.QueueFree();
    }

}
