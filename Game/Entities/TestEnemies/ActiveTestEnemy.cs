using System;
using Godot;

public partial class ActiveTestEnemy : Entity
{
    [Export] public float Speed = 100f;
    [Export] public float ShootingRange = 400f;
    [Export] public float FireCooldown = 1.0f;
    [Export] public Gun gun;

    [Export] private Area2D separationArea;
    private NavigationAgent2D navAgent;
    private Node2D player;
    private double fireTimer = 3.0;

    public override void _Ready()
    {
        navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        player = GetTree().GetFirstNodeInGroup("Player") as Node2D;

        EventBus.Reset += Death;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (player == null) return;

        if (KnockbackTime > 0f)
        {
            KnockbackTime -= (float)delta;
            if (KnockbackTime <= 0f)
            {
                Velocity = Vector2.Zero; // Stop movement after knockback
            }

            Velocity = KnockbackVelocity;
            MoveAndSlide();

            return;
        }

        fireTimer -= delta;

        float distanceToPlayer = GlobalPosition.DistanceTo(player.GlobalPosition);
        if (distanceToPlayer > ShootingRange)
        {
            navAgent.TargetPosition = player.GlobalPosition;
            MoveAlongPath(delta);
            return;
        }

        bool hasLineOfSight = HasLineOfSight(player.GlobalPosition);
        if (hasLineOfSight)
        {
            Velocity = Vector2.Zero;
            ShootAtPlayer();
        }
        else
        {
            Vector2 offsetTarget = player.GlobalPosition + (GlobalPosition.DirectionTo(player.GlobalPosition)).Orthogonal() * 100;
            navAgent.TargetPosition = offsetTarget;
            MoveAlongPath(delta);
        }
    }
    private void MoveAlongPath(double delta)
    {
        if (navAgent.IsNavigationFinished()) return;

        Vector2 nextPos = navAgent.GetNextPathPosition();
        Vector2 dir = (nextPos - GlobalPosition).Normalized() * Speed;

        //dir = AvoidWalls(dir); // dont know if needed
        //dir = SeparateFromEnemies(dir); // dont know if needed

        Velocity = Velocity.Lerp(dir, 0.2f);
        MoveAndSlide();
    }
    private bool HasLineOfSight(Vector2 target)
    {
        var spaceState = GetWorld2D().DirectSpaceState;
        var query = PhysicsRayQueryParameters2D.Create(GlobalPosition, target);
        query.Exclude = new Godot.Collections.Array<Rid> { GetRid() };

        var result = spaceState.IntersectRay(query);
        if (result.Count == 0) return true;

        if (result.TryGetValue("collider", out var colliderVar))
        {
            // Convert the Variant into a GodotObject
            var colliderObj = colliderVar.AsGodotObject();

            // Compare against your player node
            if (colliderObj == player)
                return true;
        }

        return false;
    }
    private void ShootAtPlayer()
    {
        if (fireTimer > 0) return;

        GD.Print("Shooting now");
        gun.Shoot();
        fireTimer = FireCooldown;
    }



    // dont know if needed
    // private Vector2 AvoidWalls(Vector2 desiredVel)
    // {
    //     var space = GetWorld2D().DirectSpaceState;

    //     var query = PhysicsRayQueryParameters2D.Create(
    //         GlobalPosition,
    //         GlobalPosition + desiredVel.Normalized() * 20f
    //     );
    //     query.Exclude = new Godot.Collections.Array<Rid> { GetRid() };

    //     var result = space.IntersectRay(query);
    //     if (result.Count > 0)
    //     {
    //         Vector2 normal = ((Vector2)result["normal"]).Normalized();
    //         desiredVel = desiredVel.Slide(normal);
    //     }

    //     return desiredVel;
    // }

    // // dont know if needed
    // private Vector2 SeparateFromEnemies(Vector2 desiredVel)
    // {
    //     float separationRadius = 32f;
    //     Vector2 push = Vector2.Zero;

    //     foreach (var body in separationArea.GetOverlappingBodies())
    //     {
    //         if (body is Hurtbox other && other != this.GetNode<Hurtbox>("Hurtbox"))
    //         {
    //             float dist = GlobalPosition.DistanceTo(other.GlobalPosition);
    //             if (dist < separationRadius && dist > 0)
    //             {
    //                 Vector2 away = (GlobalPosition - other.GlobalPosition).Normalized();
    //                 push += away * (separationRadius - dist);
    //             }
    //         }
    //     }

    //     if (push != Vector2.Zero)
    //         desiredVel += push.Normalized() * 50f;

    //     return desiredVel;
    // }

}
