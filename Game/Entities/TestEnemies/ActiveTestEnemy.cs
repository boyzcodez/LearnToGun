using Godot;

public partial class ActiveTestEnemy : Entity
{
    [Export] public float ShootingRange = 400f;
    [Export] public float WalkingRange = 400f;
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

    private AnimatedSprite animatedSprite;
    private CollisionShape2D collisionShape;
    public EnemyHurtbox hurtbox;
    private Timer FireRateTimer;
    private Node2D player;
    private double fireTimer = 3.0;
    private bool isShooting = false;
    private bool waitDeactivation = true;

    public override void _Ready()
    {
        player = GetTree().GetFirstNodeInGroup("Player") as Player;
        FireRateTimer = GetNode<Timer>("FireRate");
        NavAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        collisionShape = GetNode<CollisionShape2D>("CollisionShape");
        hurtbox = GetNode<EnemyHurtbox>("EnemyHurtbox");
        animatedSprite = GetNode<AnimatedSprite>("AnimatedSpriteE");

        lineOfSight.TargetPosition = new Vector2(ShootingRange/lineOfSight.Scale.X, 0);

        Connect(SignalName.Activation, new Callable(this, nameof(Activate)));
        Connect(SignalName.Deactivation, new Callable(this, nameof(Deactivate)));

        SetProcess(false);
        Visible = false;
    }

    public override void _Process(double delta)
    {
        if (player == null || NavAgent == null) return;

        if (KnockbackTime > 0f)
        {
            KnockbackTime -= (float)delta;
            if (KnockbackTime <= 0f)
            {
                Velocity = Vector2.Zero; // Stop movement after knockback
            }

            Velocity = Velocity.Lerp(KnockbackVelocity, 1.0f - (float)Mathf.Exp(-35f * GetPhysicsProcessDeltaTime()));;
            MoveAndSlide();

            return;
        }

        if (Dead && waitDeactivation)
        {
            waitDeactivation = false;
            EmitSignal("Deactivation");
            return;
        }

        fireTimer -= delta;

        // --- Core AI Logic ---
        float distanceToPlayer = GlobalPosition.DistanceTo(player.GlobalPosition);

        // Update line of sight check — uses ShootingRange for the raycast length internally
        bool hasLineOfSight = HasLineOfSight(player.GlobalPosition);
        bool canShoot = hasLineOfSight || !needsLineOfSight;

        // --- Movement Logic ---
        bool shouldMove = distanceToPlayer > WalkingRange;

        if (shouldMove || hasLineOfSight == false)
        {
            NavAgent.TargetPosition = player.GlobalPosition;
            MoveAlongPath(delta);
        }
        else
        {
            // Stop movement when within walking range
            Velocity = Vector2.Zero;
        }

        // --- Shooting Logic ---
        bool inShootingRange = distanceToPlayer <= ShootingRange;

        if (inShootingRange && canShoot && fireTimer <= 0)
        {
            fireTimer = FireCooldown + FireRate * FireTimes;
            TriggerAction(Trigger);
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
                AbilityAtPlayer();
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
            if (Dead) break;
            gun.Shoot();

            FireRateTimer.Start(FireRate);
            await ToSignal(FireRateTimer, "timeout");
        }

        isShooting = false;
    }
    private async void AbilityAtPlayer()
    {
        if (animatedSprite != null && !Dead)
        {
            animatedSprite.PlayAnimation("AbilityStart", 8);

            await ToSignal(animatedSprite, "animation_finished");
        }
        
        if (!Dead) animatedSprite.PlayAnimation("Ability", 8);

        for (int i = 0; i < FireTimes; i++)
        {
            if (Dead) break;
            ability.TriggerAbility();

            FireRateTimer.Start(FireRate);
            await ToSignal(FireRateTimer, "timeout");
        }

        if (!Dead) animatedSprite.PlayAnimation("AbilityEnd", 8);
    }

    public void Activate()
    {
        //ZIndex = 0;

        collisionShape.Disabled = false;
        SetProcess(true);
        Visible = true;
        if (gun != null) gun.Visible = true;

        hurtbox.Monitorable = true;
        hurtbox.Monitoring = true;

        hurtbox.ResetHealth();

        Dead = false;
        waitDeactivation = true;
    }
    public void Deactivate()
    {
        collisionShape.SetDeferred("disabled", true);
        SetProcess(false);

        hurtbox.SetDeferred("monitoring", false);
        hurtbox.SetDeferred("monitorable", false);
    }

    public override void Death()
    {
        if (Dead) return;
        Dead = true;
        EventBus.OnEnemyDied();
        //ZIndex = -1;

        animatedSprite.Deactivate();
        animatedSprite.PlayAnimation("Death", 10);

        if (gun != null) gun.Visible = false;
    }


}
