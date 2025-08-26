using Godot;

public partial class Look : Marker2D
{
    private Player player;

    public override void _Ready()
    {
        player = GetTree().GetFirstNodeInGroup("Player") as Player;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (player != null)
        {
            Vector2 directionToPlayer = (player.GlobalPosition - GlobalPosition).Normalized();
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
