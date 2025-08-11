using Godot;
using System.Threading.Tasks;

public partial class Obliterate : Node
{
    [Export]
    private DamageInfo obliterateDamage = new()
    {
        damage = 1000,
        knockbackForce = 0f,
        damageType = "Obliterate",
        typeDamage = 0
    };
    private Health healthComponent;
    private int buildUp;
    private const int MaxBuildUp = 15;

    public override void _Ready() => healthComponent = GetParent<Health>();

    public void ObliterateBuildUp(int amount)
    {
        if ((buildUp += amount) < MaxBuildUp) return;
        buildUp = 0;
        TriggerObliteration();
    }

    private Task TriggerObliteration() =>
        healthComponent?.TakeDamage(obliterateDamage) ?? Task.CompletedTask;
}
