using Godot;

[GlobalClass]
public partial class Hurtbox : Area2D
{
    [Signal]
    public delegate void HitEventHandler();

    [Export] private BaseHitEffect[] hitEffects = [];
    [Export] private bool isImmune = false;
    [Export] private int maxHealth = 100;
    [Export] private float knockbackResist = 0f;
    [Export] private HitFlash hitFlash;
    [Export] private int xpAmount = 1;

    private CpuParticles2D particles;
    private AnimatedSprite2D animation;

    private int currentHealth;
    public bool immune = false;
    private Entity owner;

    public override void _Ready()
    {
        owner = GetOwner<Entity>();

        particles = GetNode<CpuParticles2D>("HitParticle");
        animation = GetNode<AnimatedSprite2D>("HitAnimation");

        currentHealth = maxHealth;
        immune = isImmune;
    }

    public void TakeDamage(DamageData damageData, Vector2 direction = default)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damageData.Damage;
        owner.Knockback(direction, damageData.Knockback - knockbackResist);

        EmitSignal(SignalName.Hit);
        XpHandler.AddXP(damageData.WeaponName, xpAmount);

        GD.Print("took " + damageData.Damage + " damage, current health " + currentHealth);


        if (currentHealth <= 0)
        {
            owner.Death();
        }
        else
        {
            Effects();
            foreach (var effect in hitEffects)
            {
                effect.Trigger();
            }
        }
    }

    private void Effects()
    {
        animation.Play("default");
        particles.Emitting = true;
        if (hitFlash != null) hitFlash.Blink();
    }
    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
