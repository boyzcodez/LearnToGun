using Godot;
using System;
using System.Collections.Generic;

public partial class DodgeFlag : AnimatedSprite2D
{
    [Export]
    private EnemyGun enemyGun;
    [Export]
    public string[] AllowedAnimationTypes { get; set; } = { "Normal", "Swift" };
    [Export]
    public string[] AllowedDirections { get; set; } = { "QA", "NA", "LA" };

    private Random random = new Random();

    public void PlayRandomAnimation()
    {
        if (IsPlaying())
            return;

        string type = AllowedAnimationTypes[random.Next(AllowedAnimationTypes.Length)];
        string direction = AllowedDirections[random.Next(AllowedDirections.Length)];
        string animationName = $"{type}{direction}";

        if (SpriteFrames.HasAnimation(animationName))
        {
            Play(animationName);
        }
    }

    // this function is hooked up through the engine
    public void _on_animation_finished()
    {
        if (enemyGun != null)
        {
            //enemyGun.Shoot();
        }
    }
}
