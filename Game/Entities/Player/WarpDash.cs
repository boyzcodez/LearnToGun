using Godot;
using System;

public partial class WarpDash : Node2D
{
    private float ghostTimer = 0.0f;
    private const float ghostInterval = 0.5f;
    public bool isWarping = false;

    private Timer warpTimer;
    private AnimatedSprite animatedSprite;
    private Node2D ysort;
    [Export] private PackedScene ghostScene;


    public void SpawnGhost()
    {
        var ghost = ghostScene.Instantiate<GhostSprite>();

        var tintColors = new Color[]
        {
            new Color("#18ffbaff"), // DodgerBlue
            new Color("#4791ffff"), // DarkRed
            new Color("#ff497dff")  // OrangeRed
        };
        var tint = tintColors[(int)GD.RandRange(0, tintColors.Length - 1)];

        ghost.Setup(
            animatedSprite.SpriteFrames.GetFrameTexture(animatedSprite.Animation, animatedSprite.Frame),
            GlobalPosition,
            tint,
            animatedSprite.FlipH
        );

        ysort.AddChild(ghost);
    }

    public override void _Ready()
    {
        warpTimer = GetNode<Timer>("Timer");
        animatedSprite = GetParent().GetNode<AnimatedSprite>("AnimatedSprite");
        ysort = GetTree().GetFirstNodeInGroup("YSort") as Node2D;
    }

    // this function is hooked up through the engine
    private void _on_timer_timeout()
    {
        SpawnGhost();

        if (isWarping)
        {
            warpTimer.Start();
        }
    }

    public void Activated()
    {
        isWarping = true;
        warpTimer.Start();
        //animatedSprite.PlayAnimation("Glitch", 3);

    }
    public void Deactivated()
    {
        isWarping = false;
    }
}
