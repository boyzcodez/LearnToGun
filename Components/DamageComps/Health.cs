using Godot;
using System;
using System.Threading.Tasks;

[GlobalClass]
public partial class Health : Node2D
{
    private DamageNumbers damageNumber;
    private Entity owner;
    private Node ObliterateComponent;
    private Node BurnComponent;
    private Node2D CascadeComponent;
    private int currentHealth;
    [Export] private int maxHealth = 100;
    

    public override void _Ready()
    {
        owner = GetParent<Entity>();
        damageNumber = GetNode<DamageNumbers>("DamageNumbers");
        ObliterateComponent = GetNode("Obliterate");
        BurnComponent = GetNode("Burn");
        CascadeComponent = GetNode<Node2D>("Cascade");
        currentHealth = maxHealth;
    }

    public async Task TakeDamage(DamageInfo damageInfo, Vector2 knockbackDirection = default)
    {
        int repeats = Math.Max(1, damageInfo.repeatCount);

        for (int i = 0; i < repeats; i++)
        {
            currentHealth -= damageInfo.damage;
            owner.Knockback(knockbackDirection, damageInfo.knockbackForce);
            damageNumber.DisplayNumber(damageInfo.damage, damageInfo.damageType);

            TriggerDamageType(damageInfo.damageType, damageInfo.typeDamage);

            if (currentHealth <= 0)
            {
                GD.Print("dead");
            }

            if (i < repeats - 1)
                await ToSignal(GetTree().CreateTimer(0.12f), "timeout"); // 120ms delay between numbers
        }
        
    }

    private void TriggerDamageType(string damageType, int typeAmount)
    {
        if (typeAmount <= 0)
        {
            return;
        }
        // Handle different damage types here
        switch (damageType)
        {
            case "Obliterate":
                ObliterateComponent.Call("ObliterateBuildUp", typeAmount);
                break;
            case "Cascade":
                CascadeComponent.Call("CascadeBuildUp", typeAmount);
                break;
            case "Burn":
                BurnComponent.Call("BurnBuildUp", typeAmount);
                break;
            case "None":
                GD.Print("No damage type specified");
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
