using Godot;
using System;

public partial class HitRange : Area2D
{
    [Export] private int Damage = 1;
    [Export] private float knockback = 0f;
    private DamageData damageData;
    private Hurtbox ownerHurtbox;
    public override void _Ready()
    {
        ownerHurtbox = GetOwner<Entity>().GetNode<Hurtbox>("Hurtbox");
        AreaEntered += RangeEntered;

        damageData = new DamageData(Damage, knockback, "");

        GetParent<Entity>().Connect(Entity.SignalName.Activation, new Callable(this, nameof(Activate)));
        GetParent<Entity>().Connect(Entity.SignalName.Deactivation, new Callable(this, nameof(Deactivate)));

        Monitorable = false;
        Monitoring = false;
    }
    private void RangeEntered(Node body)
    {
        if (body is Hurtbox hurtbox)
        {
            Vector2 direction = (hurtbox.GlobalPosition - GlobalPosition).Normalized();

            if (hurtbox.immune)
            {
                ownerHurtbox.TakeDamage(damageData, -direction);
            }
            else
            {
                hurtbox.TakeDamage(damageData, direction);
            }
        }

    }

    public void Activate()
    {
        Monitorable = true;
        Monitoring = true;
    }
    public void Deactivate()
    {
        SetDeferred("monitoring", false);
        SetDeferred("monitorable", false);
    }

}
