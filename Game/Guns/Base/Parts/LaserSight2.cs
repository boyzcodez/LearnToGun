using Godot;

public partial class LaserSight2 : Node2D
{
    public bool LaserEnabled { get; set; } = false;
    [Export] private float maxLineLengthPx = 640.0f;
    [Export] private float visualOffsetPx = 0.0f;

    private ShaderMaterial shaderMaterial;
    private AnimatedSprite2D reddot;
    private Sprite2D lineSprite;
    private RayCast2D raycast;

    public override void _Ready()
    {
        reddot = GetNode<AnimatedSprite2D>("Reddot");
        lineSprite = GetNode<Sprite2D>("LineSprite");
        raycast = GetNode<RayCast2D>("Raycast");
        shaderMaterial = lineSprite.Material as ShaderMaterial;

        // Automatically use texture width if available
        if (lineSprite?.Texture != null)
        {
            maxLineLengthPx = lineSprite.Texture.GetWidth();
        }
    }

    public override void _Process(double delta)
    {
        if (lineSprite == null || raycast == null)
            return;

        float hitLength = maxLineLengthPx;

        // If RayCast hits something, measure the distance to the collision point
        if (raycast.IsColliding())
        {
            Vector2 origin = raycast.GlobalPosition;
            Vector2 collisionPoint = raycast.GetCollisionPoint();
            hitLength = origin.DistanceTo(collisionPoint) + visualOffsetPx;

            reddot.Position = ToLocal(raycast.GetCollisionPoint());
        }

        hitLength = Mathf.Clamp(hitLength, 0.0f, maxLineLengthPx);

        // Convert pixel length to normalized 0–1 ratio for the shader
        float visibleRatio = hitLength / maxLineLengthPx;

        // Apply to shader
        if (lineSprite.Material != null)
        {
            shaderMaterial.SetShaderParameter("visible_length", visibleRatio);
        }
    }
    public void ToggleLaser(bool toggle)
    {
        LaserEnabled = toggle;

        if (toggle == true)
        {
            Show();
        }
        else if (toggle == false)
        {
            Hide();
        }
    }
}