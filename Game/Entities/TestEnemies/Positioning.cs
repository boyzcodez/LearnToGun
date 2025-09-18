using Godot;
using System;

public partial class Positioning : Node2D
{
    // [Export] public float Speed = 100f;
    // [Export] public float ShootingRange = 400f;
    // [Export] public float FireCooldown = 1.0f;

    // private NavigationAgent2D navAgent;
    // private Node2D player;
    // private Entity entity;
    // private double fireTimer = 0.0;

    // public override void _Ready()
    // {
    //     navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
    //     player = GetTree().GetFirstNodeInGroup("Player") as Node2D;
    //     entity = GetOwner<Entity>();
    // }

    // public override void _PhysicsProcess(double delta)
    // {
    //     if (player == null) return;

    //     fireTimer -= delta;

    //     float distanceToPlayer = GlobalPosition.DistanceTo(player.GlobalPosition);
    //     if (distanceToPlayer > ShootingRange)
    //     {
    //         navAgent.TargetPosition = player.GlobalPosition;
    //         MoveAlongPath(delta);
    //         return;
    //     }

    //     bool hasLineOfSight = HasLineOfSight(player.GlobalPosition);
    // }
    // private void MoveAlongPath(double delta)
    // {
    //     if (navAgent.IsNavigationFinished()) return;

    //     Vector2 nextPos = navAgent.GetNextPathPosition();
    //     Vector2 dir = (nextPos - GlobalPosition).Normalized();
    //     entity.Velocity = dir * Speed;
    // }
    // private bool HasLineOfSight(Vector2 target)
    // {
    //     var spaceState = GetWorld2D().DirectSpaceState;
    //     var query = PhysicsRayQueryParameters2D.Create(GlobalPosition, target);
    //     query.Exclude = new Godot.Collections.Array<Rid> { GetRid() };
    // }



}
