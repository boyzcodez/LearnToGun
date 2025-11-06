using Godot;

public partial class Look : Marker2D
{
    [Export] private float SnapDegrees = 10f;
    private WarpDash playerCenter;
    private RayCast2D raycast;
    [Export] private Guns guns;

    public override void _Ready()
    {
        raycast = GetNode<RayCast2D>("sight");
        playerCenter = GetTree().GetFirstNodeInGroup("PlayerCenter") as WarpDash;

        GetOwner<Entity>().Connect(Entity.SignalName.Activation, new Callable(this, nameof(Activate)));
        GetOwner<Entity>().Connect(Entity.SignalName.Deactivation, new Callable(this, nameof(Deactivate)));

        SetPhysicsProcess(false);
        if (guns != null) guns.SetProcess(false);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (playerCenter != null)
        {
            Vector2 directionToPlayer = (playerCenter.GlobalPosition - GlobalPosition).Normalized();
            float targetRotation = directionToPlayer.Angle();

            float angleDegrees = Mathf.RadToDeg(targetRotation);

            // // Snap to nearest increment (e.g., 10°)
            float snappedDegrees = Mathf.Round(angleDegrees / SnapDegrees) * SnapDegrees;

            // // Convert back to radians
            float snappedAngle = Mathf.DegToRad(snappedDegrees);


            Rotation = snappedAngle;
        }
        else
        {
            Vector2 direction = GetOwner<Entity>().Velocity.Normalized();
            float targetRotation = direction.Angle();
            Rotation = targetRotation;
        }
    }

    public void Activate()
    {
        SetPhysicsProcess(true);
        raycast.Enabled = true;
        if (guns != null) guns.SetProcess(true);
    }
    public void Deactivate()
    {
        SetPhysicsProcess(false);
        raycast.Enabled = false;
        if (guns != null) guns.SetProcess(false);
    }
}
