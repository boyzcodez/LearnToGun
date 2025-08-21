using Godot;
using System.Threading.Tasks;

public partial class Burn : Node
{
    [Export] private int maxBuildUp = 15;
    [Export] private float burnTickInterval = 0.3f;
    [Export] private int burnStackAmount = 5;
    [Export]
    private DamageInfo burnDamageInfo = new()
    {
        damage = 5,
        knockbackForce = 0f,
        damageType = "Burn",
        typeDamage = 0
    };

    private Health healthComponent;
    private Timer burnTimer;
    private int buildUpCount;
    private int burnStacks;

    public override void _Ready()
    {
        healthComponent = GetParent<Health>();
        burnTimer = GetNode<Timer>("Timer");
    }

    public void BurnBuildUp(int damageAmount)
    {
        buildUpCount += damageAmount;

        if (buildUpCount < maxBuildUp) return;

        buildUpCount = 0;
        AddBurnStack();
    }

    private void AddBurnStack()
    {
        burnStacks += burnStackAmount;

        if (burnStacks == burnStackAmount)
        {
            StartBurnTimer();
        }
    }

    private void StartBurnTimer()
    {
        burnTimer.Start(burnTickInterval);
    }

    // Connected through editor signal
    private async Task _on_timer_timeout()
    {
        if (healthComponent != null)
        {
            await healthComponent.TakeDamage(burnDamageInfo);
        }

        burnStacks--;

        if (burnStacks > 0)
        {
            StartBurnTimer();
        }
    }
}

public class Help
{
    public static int Add(int a, int b) => a + b;
}
