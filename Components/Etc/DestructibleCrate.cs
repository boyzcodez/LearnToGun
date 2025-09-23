using Godot;
using System;

[GlobalClass]
public partial class DestructibleCrate : Node2D
{
    [Export] public PackedScene shardScene;
    [Export] public PackedScene coinScene;
    private int numShards = 4;

    private ShardHandler shardHandler;
    private Sprite2D sprite;
    private Node2D ysort;
    public Area2D area;
    public override void _Ready()
    {
        ysort = GetTree().GetFirstNodeInGroup("YSort") as Node2D;
        area = GetNode<Area2D>("DestructionArea");
        shardHandler = GetNode<ShardHandler>("ShardHandler");
        sprite = GetNode<Sprite2D>("Sprite2D");

        area.BodyEntered += Break;
        area.AreaEntered += Break;

        EventBus.EndWave += WorkAround;
    }
    public void Break(Node2D body)
    {
        WorkAround();
    }
    private void WorkAround()
    {
        // for (int i = 0; i < numShards; i++)
        // {
        //     if (shardScene == null) continue;

        //     var shard = shardScene.Instantiate<Shard>();
        //     shard.GlobalPosition = GlobalPosition;

        //     ysort.CallDeferred("add_child", shard);
        // }
        shardHandler.Show();
        shardHandler.Trigger();
        sprite.Hide();

        var coin = coinScene.Instantiate<AnimatedSprite2D>();
        coin.GlobalPosition = GlobalPosition;
        ysort.CallDeferred("add_child", coin);
    }
    public override void _ExitTree()
    {
        EventBus.EndWave -= WorkAround;
    }

}
