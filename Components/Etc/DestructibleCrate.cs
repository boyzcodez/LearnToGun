using Godot;
using System;

[GlobalClass]
public partial class DestructibleCrate : Node2D
{
    [Export] public PackedScene shardScene;
    [Export] public PackedScene coinScene;
    private int numShards = 4;

    private Node2D ysort;
    public Area2D area;
    public override void _Ready()
    {
        ysort = GetTree().GetFirstNodeInGroup("YSort") as Node2D;
        area = GetNode<Area2D>("DestructionArea");

        area.BodyEntered += Break;
        area.AreaEntered += Break;
    }
    public void Break(Node2D body)
    {
        for (int i = 0; i < numShards; i++)
        {
            if (shardScene == null) continue;
            
            var shard = shardScene.Instantiate<Shard>();
            shard.GlobalPosition = GlobalPosition;

            ysort.CallDeferred("add_child", shard);
        }

        var coin = coinScene.Instantiate<AnimatedSprite2D>();
        coin.GlobalPosition = GlobalPosition;
        ysort.CallDeferred("add_child", coin);

        QueueFree();
    }

}
