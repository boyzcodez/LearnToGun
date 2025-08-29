using Godot;

public partial class Look : Marker2D
{
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
