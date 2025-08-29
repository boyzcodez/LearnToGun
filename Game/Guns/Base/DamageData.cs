using Godot;

public class DamageData
{
    public int Damage { get; }
    public float Knockback { get; }
    public Node Source { get; }

    public DamageData(int damage, float knockback)
    {
        Damage = damage;
        Knockback = knockback;
        //Source = source;
    }
}
