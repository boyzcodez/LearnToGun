using System.Threading.Tasks;
using Godot;

[GlobalClass]
public partial class Player : Entity
{
    //[Export] public PlayerWeaponManager weaponManager;
    private const float SPEED = 120f;
    private const float DODGE_SPEED = 200f;
    private const float DODGE_DURATION = 0.5f;

    private bool isDodging = false;
    private Vector2 dodgeDirection;
    public float dodgeTime = 0f;
    private float dashCooldown = 0.5f;

    private Hurtbox hurtbox;
    private Node2D warpDashNode;
    private Timer dashTimer;
    private LookAt lookAt;

    private bool disabled = false;

    public override void _Ready()
    {

        dashTimer = GetNode<Timer>("DashCooldown");
        hurtbox = GetNode<Hurtbox>("Hurtbox");
        warpDashNode = GetNode<Node2D>("WarpDash");
        lookAt = GetNode<LookAt>("LookAt");
        //Input.SetMouseMode(Input.MouseModeEnum.Hidden);

        //EventBus.MapSwitch += PlayerReset;
        EventBus.Lock += Lock;
    }
    public override void _PhysicsProcess(double delta)
    {
        if (disabled || Dead) return;

        if (KnockbackTime > 0f)
        {
            KnockbackTime -= (float)delta;
            Velocity = KnockbackVelocity;
            if (KnockbackTime <= 0f)
            {
                Velocity = Vector2.Zero; // Stop movement after knockback

            }
        }
        else
        {
            if (dodgeTime > 0f)
            {
                DodgeLogic((float)delta);

            }
            else
            {
                Movement((float)delta);
            }
        }

        MoveAndSlide();
    }

    private void Movement(float delta)
    {
        Vector2 direction = Input.GetVector("left", "right", "up", "down");
        Velocity = Velocity.Lerp(direction * SPEED, 22.0f * delta);

        if (Input.IsActionJustPressed("dodge") && direction != Vector2.Zero && isDodging == false)
        {
            isDodging = true;
            hurtbox.immune = true;
            warpDashNode.CallDeferred("Activated");
            DodgeRoll(direction);
        }
    }

    private void DodgeRoll(Vector2 direction)
    {
        dodgeDirection = direction.Normalized();
        dodgeTime = DODGE_DURATION;
    }
    private void DodgeLogic(float delta)
    {
        float elapsedPercent = 1.0f - (dodgeTime / DODGE_DURATION);
        float currentSpeed = Mathf.Lerp(DODGE_SPEED, DODGE_SPEED * 0.5f, elapsedPercent);

        Velocity = dodgeDirection * currentSpeed;
        dodgeTime -= delta;

        if (dodgeTime <= 0f)
        {
            var dodgeSpeed = Mathf.Lerp(currentSpeed, SPEED, delta * 8f);
            Velocity = dodgeDirection * dodgeSpeed;
            // Velocity = Vector2.Zero; // Stop movement after dodge
            // dodgeDirection = Vector2.Zero;
            hurtbox.immune = false;
            warpDashNode.CallDeferred("Deactivated");
            dashTimer.Start(dashCooldown);
        }
    }
    public override async void Death()
    {
        EventBus.gameOn = false;

        Dead = true;
        KnockbackTime = 0f;
        lookAt.Hide();
        EventBus.PlayerDied();


        await ToSignal(GetTree().CreateTimer(3f), "timeout");
        
        GetNode<Hurtbox>("Hurtbox").ResetHealth();

        GlobalPosition = new Vector2(0, 0);
        Dead = false;
        lookAt.Show();
    }
    private void PlayerReset()
    {
        KnockbackTime = 0f;
        GlobalPosition = new Vector2(0, 0);
    }
    private void Lock()
    {
        if (disabled)
        {
            disabled = false;
        }
        else
        {
            disabled = true;
        }

    }
    // this function is hooked up through the engine
    private void _on_dash_cooldown_timeout()
    {
        isDodging = false;
    }
}
