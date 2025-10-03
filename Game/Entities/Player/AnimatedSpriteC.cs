using Godot;
using System;

public partial class AnimatedSpriteC : AnimatedSprite
{
    
    private const string RunAnim = "Run";
    private Player player;

    private Direction directionNode;
    private string currentDirection;
    private string currentAnim = "";

    public override void _Ready()
    {
        directionNode = GetNode<Direction>("Direction");
        player = GetOwner<Player>();
    }
    public override void _Process(double delta)
    {
        HandleMovement();
        HandleAnimation();
    }

    protected void HandleMovement()
    {
        var inputDir = Input.GetVector("left", "right", "up", "down");
        currentAnim = inputDir != Vector2.Zero ? RunAnim : "";
    }

    protected void HandleAnimation()
    {
        Vector2 mouse = GetLocalMousePosition();
        int sectionIndex = (int)(Mathf.Snapped(mouse.Angle(), Mathf.Pi / 4.0f) / (Mathf.Pi / 4.0f));
        sectionIndex = Mathf.Wrap(sectionIndex, 0, 8);

        currentDirection = directionNode.GetDirection(sectionIndex);

        if (player.dodgeTime > 0) PlayAnimation("Glitch", 5);
        else PlayAnimation(currentAnim, 1);
    }

    public override void PlayAnimation(string animation, int priority = 0)
    {
        if (priority >= animationPriority)
        {
            animationPriority = priority;
            Play(currentDirection + animation);
        }
    }

}
