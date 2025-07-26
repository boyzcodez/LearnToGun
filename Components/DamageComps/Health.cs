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

    public void TakeDamage(int damage, Vector2 knockbackDirection = default, float knockbackForce = 0f)
    {
        currentHealth -= damage;
        owner.Knockback(knockbackDirection, knockbackForce);
        if (currentHealth <= 0)
        {
            GD.Print("dead");
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
