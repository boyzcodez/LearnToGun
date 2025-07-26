using Godot;
using System;

public partial class Player : Entity
{
    private const float SPEED = 100f;
    private const float DODGE_SPEED = 180f;
    private const float DODGE_DURATION = 0.4f;

    private bool isDodging = false;
    private Vector2 dodgeDirection;
    private float dodgeTime = 0f;
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

        if (Input.IsActionJustPressed("dodge"))
        {
            isDodging = true;
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
            isDodging = false;
            Velocity = Vector2.Zero; // Stop movement after dodge
            dodgeDirection = Vector2.Zero;
        }
    }

}
