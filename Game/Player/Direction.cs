using Godot;
using System;

public partial class Direction : Node
{
    private string currentDirection = "Front";
    private Marker2D lookAt;
    private AnimatedSprite2D animatedSprite;

    public override void _Ready()
    {
        lookAt = GetNode<Marker2D>("../../LookAt");
        animatedSprite = GetNode<AnimatedSprite2D>("..");
    }

    public string GetDirection(int section)
    {
        switch (section)
        {
            case 0:
                lookAt.ShowBehindParent = false;
                animatedSprite.FlipH = false;
                return "RightFront";
            case 1:
                lookAt.ShowBehindParent = false;
                animatedSprite.FlipH = false;
                return "RightFront";
            case 2:
                lookAt.ShowBehindParent = false;
                animatedSprite.FlipH = true;
                return "Front";
            case 3:
                lookAt.ShowBehindParent = false;
                animatedSprite.FlipH = true;
                return "RightFront";
            case 4:
                lookAt.ShowBehindParent = false;
                animatedSprite.FlipH = true;
                return "RightFront";
            case 5:
                lookAt.ShowBehindParent = true;
                animatedSprite.FlipH = true;
                return "RightBack";
            case 6:
                lookAt.ShowBehindParent = true;
                animatedSprite.FlipH = false;
                return "Back";
            case 7:
                lookAt.ShowBehindParent = true;
                animatedSprite.FlipH = false;
                return "RightBack";
            default: return currentDirection;
        }
    }
}
