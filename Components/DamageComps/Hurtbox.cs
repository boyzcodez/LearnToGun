using Godot;

[GlobalClass]
public partial class Hurtbox : Area2D
{
    [Export] private int maxHealth = 100;
    private int currentHealth;
    private Entity owner;

    public override void _Ready()
    {
        owner = GetOwner<Entity>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(DamageData damageData, Vector2 direction = default)
    {
        currentHealth -= damageData.Damage;
        owner.Knockback(direction, damageData.Knockback);

        GD.Print("took " + damageData.Damage + " damage");
        if (currentHealth <= 0)
        {
            owner.Death();
        }
    }
}
