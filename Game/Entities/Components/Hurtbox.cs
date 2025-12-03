using Godot;

[GlobalClass]
public partial class Hurtbox : Area2D
{
    [Signal]
    public delegate void HitEventHandler();

    [Export] private BaseHitEffect[] hitEffects = [];
    [Export] private bool isImmune = false;
    [Export] public int maxHealth = 100;
    [Export] private float knockbackResist = 0f;
    [Export] private HitFlash hitFlash;
    [Export] public AnimatedSprite animationSprite;
    [Export] private int xpAmount = 1;

    public int currentHealth;
    public bool immune = false;
    private Entity owner;

    public override void _Ready()
    {
        owner = GetOwner<Entity>();

        currentHealth = maxHealth;
        immune = isImmune;

        OnInit();
    }

    public void TakeDamage(DamageData damageData, Vector2 direction = default)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damageData.Damage;
        owner.Knockback(direction, damageData.Knockback - knockbackResist);

        EmitSignal(SignalName.Hit);
        XpHandler.AddXP(damageData.WeaponName, xpAmount);


        if (currentHealth <= 0)
        {
            immune = true;
            OnDeath();
        }
        else
        {
            OnHit();
            Effects();
        }
    }

    public virtual void OnInit()
    {
    }
    public virtual void OnHit()
    {
    }
    public virtual void OnDeath()
    {
        owner.Death();
    }
    public virtual void ResetHealth()
    {
        immune = false;
        currentHealth = maxHealth;
    }

    private void Effects()
    {
        if (hitFlash != null) hitFlash.Blink();
        if (animationSprite != null) animationSprite.PlayAnimation("Hit", 2);
    }
}
