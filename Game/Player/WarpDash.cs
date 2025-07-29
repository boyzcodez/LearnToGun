using Godot;
using System;

public partial class WarpDash : Node2D
{
    private float ghostTimer = 0.0f;
    private const float ghostInterval = 0.5f;
    private bool isWarping = false;

    private Timer warpTimer;
    private AnimatedSprite2D animatedSprite;
    [Export] private PackedScene ghostScene;


    public void SpawnGhost()
    {
        var ghost = ghostScene.Instantiate<GhostSprite>();

        var tintColors = new Color[]
        {
            new Color("#1eff96ff"), // DodgerBlue
            new Color("#57dafeff"), // DarkRed
            new Color("#ff3ed2ff")  // OrangeRed
        };
        var tint = tintColors[(int)GD.RandRange(0, tintColors.Length - 1)];

        ghost.Setup(
            animatedSprite.SpriteFrames.GetFrameTexture(animatedSprite.Animation, animatedSprite.Frame),
            GlobalPosition,
            tint,
            animatedSprite.FlipH
        );

        GetTree().CurrentScene.AddChild(ghost);
    }

    public override void _Ready()
    {
        warpTimer = GetNode<Timer>("Timer");
        animatedSprite = GetParent().GetNode<AnimatedSprite2D>("AnimatedSprite");
        warpTimer.Connect("timeout", new Callable(this, nameof(OnWarpTimeout)));
    }

    private void OnWarpTimeout()
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
    }
    public void Deactivated()
    {
        isWarping = false;
    }
}
