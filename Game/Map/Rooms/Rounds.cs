using Godot;
using System.Collections.Generic;

public partial class Rounds : Node2D
{
    private int rounds;
    private int currentRound = 0;
    private List<Spawnpoints> list = new();
    // public override void _Ready()
    // {
    //     rounds = GetChildCount();

    //     foreach (Spawnpoints point in GetChildren())
    //     {
    //         list.Add(point);
    //     }

    //     TriggerRound();
    //     EventBus.EndWave += TriggerRound;
    // }
    // public void TriggerRound()
    // {
    //     if (currentRound >= rounds)
    //     {
    //         EventBus.TriggerEndOfRound();
    //     }
    //     else
    //     {
    //         list[currentRound].SpawnEnemies();
    //         currentRound += 1;
    //     }
    // }
    // public override void _ExitTree()
    // {
    //     EventBus.EndWave -= TriggerRound;
    // }

}
