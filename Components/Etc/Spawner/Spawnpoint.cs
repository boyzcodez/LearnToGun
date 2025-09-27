using Godot;
using System;

public partial class Spawnpoint : AnimatedSprite2D
{
    [Export(PropertyHint.Enum, "PistolEnemy,ShotgunEnemy")]
    public string Enemy { get; set; } = "PistolEnemy";
    private EnemySpawner spawner;

    public override void _Ready()
    {
        spawner = GetTree().GetFirstNodeInGroup("Spawner") as EnemySpawner;
        AnimationFinished += Summon;
    }
    public void Summon()
    {
        spawner.SummonEnemy(GlobalPosition, Enemy);
    }
}
