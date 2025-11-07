using System.Threading.Tasks;
using Godot;

[GlobalClass]
public partial class Explosion : Behavior
{
    [Export] public float ExplosionRadius = 64f;
    [Export] public uint HurtboxLayer = 3; // 1-based layer index (3 -> 1 << 2)
    [Export] public int MaxResults = 64;
    public override void Initialize(Bullet bullet)
    {
    }
    public override void Update(Bullet bullet, double delta)
    {
    }
    public override void OnHit(Bullet bullet)
    {
        var space = bullet.GetWorld2D().DirectSpaceState;
        if (space == null) return;

        // Temporary shape for the overlap query
        var shape = new CircleShape2D { Radius = ExplosionRadius };

        var query = new PhysicsShapeQueryParameters2D
        {
            Shape = shape,
            Transform = new Transform2D(0, bullet.GlobalPosition),
            CollisionMask = (uint)(1 << (int)(HurtboxLayer - 1)),
            CollideWithAreas = true,
            CollideWithBodies = true
        };

        var results = space.IntersectShape(query, MaxResults);

        foreach (Godot.Collections.Dictionary result in results)
        {
            if (!result.ContainsKey("collider"))
                continue;

            // We assume every collider on the queried layer is a Hurtbox.
            var hurtbox = result["collider"].As<Hurtbox>();
            if (hurtbox == null)
                continue;

            if (hurtbox.immune)
                continue;

            hurtbox.TakeDamage(bullet.DamageData, bullet.Direction);
        }

        // Return/deactivate the bullet (using your pooling API)
        bullet.Deactivate();
    }
}
