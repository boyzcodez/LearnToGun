using Godot;
using System;

public partial class Direction : Node
{
    private string currentDirection = "Front";
    private Marker2D lookAt;
    private AnimatedSprite2D animatedSprite;

    private readonly (bool showBehind, bool flipH, string dir)[] sectionMap =
    {
        (false, false, "RightFront"), // 0
        (false, false, "RightFront"), // 1
        (false, true,  "Front"),      // 2
        (false, true,  "RightFront"), // 3
        (false, true,  "RightFront"), // 4
        (true,  true,  "RightBack"),  // 5
        (true,  false, "Back"),       // 6
        (true,  false, "RightBack"),  // 7
    };

    public override void _Ready()
    {
        lookAt = GetNode<Marker2D>("../../LookAt");
        animatedSprite = GetNode<AnimatedSprite2D>("..");
    }

    public string GetDirection(int section)
    {
        if (section >= 0 && section < sectionMap.Length)
        {
            var settings = sectionMap[section];
            lookAt.ShowBehindParent = settings.showBehind;
            animatedSprite.FlipH = settings.flipH;
            return settings.dir;
        }

        return currentDirection;
    }
}
