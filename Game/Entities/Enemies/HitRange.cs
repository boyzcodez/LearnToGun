using Godot;
using System;

public partial class HitRange : Area2D
{
    private DamageData damageData = new DamageData(0, 200f, "");
    private Hurtbox ownerHurtbox;
    public override void _Ready()
    {
        ownerHurtbox = GetOwner<Entity>().GetNode<Hurtbox>("Hurtbox");
        AreaEntered += RangeEntered;
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

}
