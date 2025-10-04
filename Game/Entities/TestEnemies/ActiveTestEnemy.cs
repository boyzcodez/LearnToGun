using Godot;

public partial class ActiveTestEnemy : Entity
{
    [Export] public float ShootingRange = 400f;
    [Export] public float FireCooldown = 1.0f;
    [Export] public int FireTimes = 1;
    [Export] public float FireRate = 0.5f;
    [Export] public Guns gun;

    private Godot.Timer FireRateTimer;
    private Node2D player;
    private double fireTimer = 3.0;
    private bool isShooting = false;

    public override void _Ready()
    {
        FireRateTimer = GetNode<Godot.Timer>("FireRate");

        EventBus.Reset += Death;
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
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
