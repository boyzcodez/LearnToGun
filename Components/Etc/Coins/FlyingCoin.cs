using Godot;
using System;
using System.Threading.Tasks;

public partial class FlyingCoin : AnimatedSprite2D
{
    private Player playr;
    private Vector2 vel;
    private float increment = 1.0f;
    private float speed = 200f;
    private bool active = true;
    public override void _Ready()
    {
        playr = GetTree().GetFirstNodeInGroup("Player") as Player;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!active) return;

        Rotation += (float)(delta * 16.0); // Adjust 1.0 to change rotation speed

        Vector2 dir = (playr.GlobalPosition - GlobalPosition).Normalized();
        vel = vel.MoveToward(dir * speed, (float)delta * 400f * increment);

        increment += 0.1f;
        GlobalPosition += vel * (float)delta;

        float distanceToPlayer = GlobalPosition.DistanceTo(playr.GlobalPosition);
        if (distanceToPlayer < 8f)
        {
            active = false;
            end();
        }
    }
    
    private async void end()
    {
        speed = 0f;
        EventBus.Money(1);
        Play("end");

        await ToSignal(this, "animation_finished");

        QueueFree();
    }
}
