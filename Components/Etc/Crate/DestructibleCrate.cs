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

    private bool broken = false;
    public override void _Ready()
    {
        ysort = GetTree().GetFirstNodeInGroup("YSort") as Node2D;
        area = GetNode<Area2D>("DestructionArea");
        shardHandler = GetNode<ShardHandler>("ShardHandler");
        sprite = GetNode<Sprite2D>("Sprite2D");

        area.BodyEntered += Break;
        area.AreaEntered += Break;

        EventBus.EndRound += WorkAround;
    }
    public void Break(Node2D body)
    {
        WorkAround();
    }
    private void WorkAround()
    {
        if (broken) return;
        broken = true;
        
        shardHandler.Show();
        shardHandler.Trigger();
        sprite.Hide();

        // area.SetDeferred("monitoring", false);
        // area.SetDeferred("monitorable", false);
        area.QueueFree();


        var coin = coinScene.Instantiate<AnimatedSprite2D>();
        coin.GlobalPosition = GlobalPosition;
        ysort.CallDeferred("add_child", coin);
    }
    public override void _ExitTree()
    {
        EventBus.EndRound -= WorkAround;
    }

}
