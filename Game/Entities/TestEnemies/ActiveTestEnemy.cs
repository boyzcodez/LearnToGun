using Godot;

public partial class ActiveTestEnemy : Entity
{
    [Export] public float ShootingRange = 400f;
    [Export] public float FireCooldown = 1.0f;
    [Export] public int FireTimes = 1;
    [Export] public float FireRate = 0.5f;
    [Export] public float moveSpeed = 200f;

    [Export(PropertyHint.Enum, "Shoot,Ability,Nothing")]
    public string Trigger { get; set; } = "Shoot";
    [Export] public bool needsLineOfSight = true;
    [Export] public Guns gun;
    [Export] public Ability ability;
    [Export] public RayCast2D lineOfSight;


    private NavigationAgent2D NavAgent;

    private CollisionShape2D collisionShape;
    private Hurtbox hurtbox;
    private Timer FireRateTimer;
    private Node2D player;
    private double fireTimer = 3.0;
    private bool isShooting = false;

    public override void _Ready()
    {
        player = GetTree().GetFirstNodeInGroup("Player") as Player;
        FireRateTimer = GetNode<Timer>("FireRate");
        NavAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        collisionShape = GetNode<CollisionShape2D>("CollisionShape");
        hurtbox = GetNode<Hurtbox>("Hurtbox");

        lineOfSight.TargetPosition = new Vector2(ShootingRange/lineOfSight.Scale.X, 0);

        EventBus.Reset += Death;
        Connect(SignalName.Activation, new Callable(this, nameof(Activate)));
        Connect(SignalName.Deactivation, new Callable(this, nameof(Deactivate)));

        collisionShape.Disabled = true;
        SetProcess(false);
        Visible = false;

        hurtbox.Monitorable = false;
        hurtbox.Monitoring = false;
    }

    public override void _Process(double delta)
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
            NavAgent.TargetPosition = player.GlobalPosition;
            MoveAlongPath(delta);
            return;
        }

        bool hasLineOfSight = HasLineOfSight(player.GlobalPosition);
        if (hasLineOfSight || needsLineOfSight == false)
        {
            Velocity = Vector2.Zero;
            if (fireTimer < 0)
            {
                fireTimer = FireCooldown + FireRate * FireTimes;
                TriggerAction(Trigger);
            }

        }
        else
        {
            Vector2 offsetTarget = player.GlobalPosition + (GlobalPosition.DirectionTo(player.GlobalPosition)).Orthogonal() * 100;
            NavAgent.TargetPosition = offsetTarget;
            MoveAlongPath(delta);
        }
    }
    private void MoveAlongPath(double delta)
    {
        if (NavAgent.IsNavigationFinished()) return;

        Vector2 nextPos = NavAgent.GetNextPathPosition();
        Vector2 dir = (nextPos - GlobalPosition).Normalized() * moveSpeed;

        //dir = AvoidWalls(dir); // dont know if needed
        //dir = SeparateFromEnemies(dir); // dont know if needed

        Velocity = Velocity.Lerp(dir, 0.2f);
        MoveAndSlide();
    }
    private bool HasLineOfSight(Vector2 target)
    {
        // var spaceState = GetWorld2D().DirectSpaceState;
        // var query = PhysicsRayQueryParameters2D.Create(gun.GlobalPosition, target);
        // query.Exclude = new Godot.Collections.Array<Rid> { GetRid() };

        // var result = spaceState.IntersectRay(query);
        // if (result.Count == 0) return true;

        // if (result.TryGetValue("collider", out var colliderVar))
        // {
        //     // Convert the Variant into a GodotObject
        //     var colliderObj = colliderVar.AsGodotObject();

        //     // Compare against your player node
        //     if (colliderObj == player)
        //         return true;
        // }

        // return false;

        // Get direction enemy is facing
        // Vector2 direction = Velocity.Normalized();
        // if (direction == Vector2.Zero)
        //     return false;

        // // Set up ray start & end
        // Vector2 start = GlobalPosition + new Vector2(0, -6);
        // Vector2 end = start + direction * RayLength;

        // var spaceState = GetWorld2D().DirectSpaceState;

        // var query = PhysicsRayQueryParameters2D.Create(start, end);
        // query.CollideWithAreas = false;
        // query.CollideWithBodies = true;

        // // Optional: if you use layer masks, apply them here
        // // query.CollisionMask = 1 << 1; // example if "Walls" is on layer 1

        // var result = spaceState.IntersectRay(query);

        // return result.Count < 0; // true if we hit something

        if (!lineOfSight.IsColliding())
        {
            return true;
        } 
        else
        {
            var distanceToPlayer = lineOfSight.GlobalPosition.DistanceTo(player.GlobalPosition);
            var distanceToWall = lineOfSight.GlobalPosition.DistanceTo(lineOfSight.GetCollisionPoint());

            if (distanceToWall < distanceToPlayer)
            {
                return false;
            }
            else return true;
        }
    }

    private void TriggerAction(string trigger)
    {
        switch (trigger)
        {
            case "Shoot":
                ShootAtPlayer();
                break;
            case "Ability":
                ability.TriggerAbility();
                break;
            case "Nothing":
                GD.Print("I will do nothing");
                break;
        }
    }



    private async void ShootAtPlayer()
    {
        if (isShooting) return;
        isShooting = true;

        for (int i = 0; i < FireTimes; i++)
        {
            gun.Shoot();

            FireRateTimer.Start(FireRate);
            await ToSignal(FireRateTimer, "timeout");
        }

        isShooting = false;
    }

    public void Activate()
    {
        collisionShape.Disabled = false;
        SetProcess(true);
        Visible = true;

        hurtbox.Monitorable = true;
        hurtbox.Monitoring = true;
    }
    public void Deactivate()
    {
        collisionShape.SetDeferred("Disabled", true);
        SetProcess(false);
        Visible = false;

        hurtbox.SetDeferred("monitoring", false);
        hurtbox.SetDeferred("monitorable", false);
    }

    public override void Death()
    {
        EventBus.OnEnemyDied(name, this);
        hurtbox.ResetHealth();
        KnockbackVelocity = Vector2.Zero;
        KnockbackTime = 0f;
        Dead = true;


    }


}
