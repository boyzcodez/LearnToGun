using Godot;

[GlobalClass]
public partial class Hurtbox : Area2D
{
    [Export] private bool isImmune = false;
    [Export] private int maxHealth = 100;
    [Export] private HitFlash hitFlash;
    private int currentHealth;
    public bool immune = false;
    private Entity owner;

    public override void _Ready()
    {
        owner = GetOwner<Entity>();
        currentHealth = maxHealth;
        immune = isImmune;
    }

    public void TakeDamage(DamageData damageData, Vector2 direction = default)
    {
        currentHealth -= damageData.Damage;
        owner.Knockback(direction, damageData.Knockback);

        if (hitFlash != null) hitFlash.Blink();

        GD.Print("took " + damageData.Damage + " damage");
        if (currentHealth <= 0)
        {
            owner.Death();
        }
    }
}
