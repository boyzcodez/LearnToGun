using Godot;
using System;

[GlobalClass]
public partial class State : Node
{
    public Entity parent;
    public Player player;
    [Signal] public delegate void TransitionedEventHandler();

    public enum StateName
    {
        Idle,
        Follow,
        Attack,
        Surround,
        Nothing
    }

    [Export(PropertyHint.Enum, "Idle,Follow,Attack,Surround,Nothing")]
    public string NextState { get; set; } = "Attack";

    [Export(PropertyHint.Enum, "Idle,Follow,Attack,Surround,Nothing")]
    public string PrevState { get; set; } = "Idle";
    public override void _Ready()
    {
        parent = GetOwner<Entity>();
        player = GetTree().GetFirstNodeInGroup("Player") as Player;
    }
    public virtual void Enter()
    {
        // Optional override
    }
    public virtual void Exit()
    {
        // Optional override
    }
    public virtual void Update(double delta)
    {
        // Optional override
    }
    public virtual void PhysicsProcess(double delta)
    {
        // Optional override
    }
}
