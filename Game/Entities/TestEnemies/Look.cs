using Godot;

public partial class Look : Marker2D
{
    [Export] private float SnapDegrees = 10f;
    private WarpDash playerCenter;

    public override void _Ready()
    {
        playerCenter = GetTree().GetFirstNodeInGroup("PlayerCenter") as WarpDash;
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
            Rotation = targetRotation;
        }
        else
        {
            Vector2 direction = GetOwner<Entity>().Velocity.Normalized();
            float targetRotation = direction.Angle();
            Rotation = targetRotation;
        }
    }
}
