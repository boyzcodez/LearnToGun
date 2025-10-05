using System;
using Godot;

public partial class ActiveTestEnemy : Entity
{
    [Export] public float ShootingRange = 400f;
    [Export] public float FireCooldown = 1.0f;
    [Export] public int FireTimes = 1;
    [Export] public float FireRate = 0.5f;
    [Export] public Guns gun;

    [Export] public float moveSpeed = 200f;
    [Export] public float MoveDistance = 100f;
    [Export] public float TargetOffset = 100f; // distance from player to candidate target points
    [Export] public float AngleJitterDeg = 10f; // small random jitter to avoid exact overlap
    private NavigationAgent2D NavAgent;
    private RayCast2D raycast;

    private Godot.Timer FireRateTimer;
    private Node2D player;
    private double fireTimer = 3.0;
    private bool isShooting = false;

    public override void _Ready()
    {
        player = GetTree().GetFirstNodeInGroup("Player") as Player;
        FireRateTimer = GetNode<Godot.Timer>("FireRate");
        NavAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        NavAgent.VelocityComputed += NavAgentComputed;

        EventBus.Reset += Death;
    }

    public override void _PhysicsProcess(double delta)
    {
        var distance = player.GlobalPosition - GlobalPosition;

        if (distance.Length() < MoveDistance)
        {
            GD.Print("i would shoot here");
            Velocity = Vector2.Zero;
        }
        else
        {
            var currentPosition = GlobalTransform.Origin;
            var nextPathPosition = NavAgent.GetNextPathPosition();
            var newVelocity = currentPosition.DirectionTo(nextPathPosition);
            NavAgent.Velocity = newVelocity;

            UpdateTargetPosition(player.GlobalTransform.Origin);

            
        } 
        MoveAndSlide();
    }
    public void UpdateTargetPosition(Vector2 target)
    {
        if (NavAgent == null) return;
        // Compute the direction from player to this enemy. The closest point on a
        // circle around the player is along that direction at radius TargetOffset.
        float offset = TargetOffset > 0f ? TargetOffset : MoveDistance;

        Vector2 dir = GlobalPosition - target;
        float angle;
        if (dir.LengthSquared() <= 0.0001f)
        {
            // If agent is exactly on the player, pick a random angle.
            angle = (float)GD.Randf() * (Mathf.Pi * 2f);
        }
        else
        {
            angle = dir.Angle();
        }

        // Apply small random jitter to reduce exact overlap between multiple agents.
        if (AngleJitterDeg != 0f)
        {
            float jitter = ((float)GD.Randf() * 2f - 1f) * AngleJitterDeg; // degrees in [-AngleJitterDeg, AngleJitterDeg]
            angle += Mathf.DegToRad(jitter);
        }

        Vector2 targetPos = target + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * offset;
        NavAgent.TargetPosition = targetPos;
    }
    public void NavAgentComputed(Vector2 safeVelocity)
    {
        Velocity = Velocity.MoveToward(safeVelocity * moveSpeed, 12f);
        Velocity = Velocity.Lerp(Velocity, 0.2f);
    }

    

    // private async void ShootAtPlayer()
    // {
    //     if (isShooting) return;
    //     isShooting = true;

    //     for (int i = 0; i < FireTimes; i++)
    //     {
    //         gun.Shoot();

    //         FireRateTimer.Start(FireRate);
    //         await ToSignal(FireRateTimer, "timeout");
    //     }

    //     fireTimer = FireCooldown;
    //     isShooting = false;
    // }

}
