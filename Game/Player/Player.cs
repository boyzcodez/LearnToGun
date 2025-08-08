using Godot;
using System;

[GlobalClass]
public partial class Player : Entity
{
    private const float SPEED = 100f;
    private const float DODGE_SPEED = 180f;
    private const float DODGE_DURATION = 0.3f;

    private bool isDodging = false;
    private Vector2 dodgeDirection;
    private float dodgeTime = 0f;
    private float dashCooldown = 0.3f;

    private Hurtbox hurtbox;
    private Node2D warpDashNode;
    private Timer dashTimer;

    public override void _Ready()
    {
        
        dashTimer = GetNode<Timer>("DashCooldown");
        hurtbox = GetNode<Hurtbox>("Hurtbox");
        warpDashNode = GetNode<Node2D>("WarpDash");
        //Input.SetMouseMode(Input.MouseModeEnum.Hidden);

        dashTimer.Connect("timeout", new Callable(this, nameof(OnTimerTimeout)));
    }
    public override void _PhysicsProcess(double delta)
    {
        if (KnockbackTime > 0f)
        {
            KnockbackTime -= (float)delta;
            if (KnockbackTime <= 0f)
            {
                Velocity = Vector2.Zero; // Stop movement after knockback

            }
            return; // Skip normal movement logic during knockback
        }


        if (dodgeTime > 0f)
        {
            DodgeLogic((float)delta);

        }
        else
        {
            Movement((float)delta);
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
            hurtbox.Monitorable = false;
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
            Velocity = Vector2.Zero; // Stop movement after dodge
            dodgeDirection = Vector2.Zero;
            hurtbox.Monitorable = true; // Re-enable hurtbox monitoring
            warpDashNode.CallDeferred("Deactivated");
            dashTimer.Start(dashCooldown);
        }
    }
    private void OnTimerTimeout()
    {
        isDodging = false;
    }

}
