using Godot;
using System;

[GlobalClass]
public partial class Health : Node2D
{
    private Entity owner;
    [Export]
    private int maxHealth = 100;
    private int currentHealth;

    public override void _Ready()
    {
        owner = GetParent<Entity>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(DamageInfo damageInfo, Vector2 knockbackDirection = default, float knockbackForce = 0f)
    {
        currentHealth -= damageInfo.damage;
        owner.Knockback(knockbackDirection, knockbackForce);
        TriggerDamageType("Obliterate", damageInfo.damage);
        if (currentHealth <= 0)
        {
            GD.Print("dead");
        }
    }

    private void TriggerDamageType(string damageType, int damageAmount)
    {
        // Handle different damage types here
        switch (damageType)
        {
            case "Obliterate":
                GD.Print("Obliterate damage");
                break;
            case "Cascade":
                GD.Print("Cascade damage");
                break;
            case "Burn":
                GD.Print("Burn damage");
                break;
            default:
                GD.Print("Unknown damage type");
                break;
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
