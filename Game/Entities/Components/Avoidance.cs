using Godot;
using System;

public partial class Avoidance : Node2D
{
    [Export] public float AvoidForce = 150f;
    [Export] public float RayLength = 32f;
    private Entity parent;
    private Vector2 velocity;

    public override void _Ready()
    {
        parent = GetOwner<Entity>();
    }

    public void MoveDirection(Vector2 target)
    {
        velocity = target + GetAvoidanceVector() * AvoidForce;
        parent.Velocity = velocity;
    }

    private Vector2 GetAvoidanceVector()
    {
        Vector2 avoidance = Vector2.Zero;
        Vector2[] directions = new Vector2[]
        {
            Vector2.Right, Vector2.Left,
            Vector2.Up, Vector2.Down,
            new Vector2(1,1).Normalized(),
            new Vector2(-1,1).Normalized(),
            new Vector2(1,-1).Normalized(),
            new Vector2(-1,-1).Normalized(),
        };

        var space = parent.GetWorld2D().DirectSpaceState;

        foreach (var dir in directions)
        {
            var result = space.IntersectRay(
                new PhysicsRayQueryParameters2D()
                {
                    From = parent.GlobalPosition,
                    To = parent.GlobalPosition + dir * RayLength,
                    CollisionMask = 1 << 0,
                    Exclude = new Godot.Collections.Array<Rid> { parent.GetRid() }
                });

            if (result.Count > 0)
            {
                // Add opposite of direction to avoid wall
                avoidance -= dir;
            }
        }

        if (avoidance != Vector2.Zero)
            avoidance = avoidance.Normalized();

        return avoidance;
    }
}
