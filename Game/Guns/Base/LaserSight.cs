using Godot;
using System;

public partial class LaserSight : Node2D
{
     [Export] public float MaxLength { get; set; } = 300f;
    [Export] public Color LaserColor { get; set; } = new Color(1, 0.2f, 0.2f);
    [Export] public float Thickness { get; set; } = 1f;
    [Export] public bool PixelSnap { get; set; } = true;
    [Export] public bool ShowImpactDot { get; set; } = true;

    // Detects walls (layer 1/2) and enemy hurtboxes (layer 3)
    [Export(PropertyHint.Layers2DPhysics)]
    public uint CollisionMask { get; set; } = (1u << 0) | (1u << 1) | (1u << 3);

    [Export] public bool IsActive { get; set; } = true;

    private Vector2 _localHitPoint = Vector2.Zero;

    public override void _Process(double delta)
    {
        if (!IsActive)
        {
            QueueRedraw(); // Clears the beam when disabled
            return;
        }

        UpdateLaser();
        QueueRedraw();
    }

    private void UpdateLaser()
    {
        var space = GetWorld2D().DirectSpaceState;

        Vector2 start = GlobalPosition;
        Vector2 dir = Vector2.Right.Rotated(GlobalRotation);
        Vector2 end = start + dir * MaxLength;

        var query = PhysicsRayQueryParameters2D.Create(start, end);
        query.CollideWithBodies = true;
        query.CollideWithAreas = true;
        query.CollisionMask = CollisionMask;

        var result = space.IntersectRay(query);

        Vector2 hitGlobal = result.Count > 0
            ? (Vector2)result["position"]
            : end;

        if (PixelSnap)
            hitGlobal = hitGlobal.Floor();

        _localHitPoint = ToLocal(hitGlobal);
    }

    public override void _Draw()
    {
        if (!IsActive)
            return;

        // Draw crisp pixel-perfect laser
        DrawLine(Vector2.Zero, _localHitPoint, LaserColor, Thickness, antialiased: false);

        // Draw a tiny impact pixel or square
        if (ShowImpactDot)
            DrawRect(new Rect2(_localHitPoint - Vector2.One * 0.5f, Vector2.One), LaserColor);
    }

    // Toggle on/off
    public void Toggle() => IsActive = !IsActive;
    public void Enable() => IsActive = true;
    public void Disable() => IsActive = false;
}
