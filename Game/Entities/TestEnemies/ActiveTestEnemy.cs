using Godot;
using System;

public partial class ActiveTestEnemy : Entity
{
    public override void _Ready()
    {
        UniversalStopButton.DisableNode(this);
    }

    // public void SetEnemyActive(bool active)
    // {
    //     ProcessMode = active
    //         ? ProcessModeEnum.Inherit
    //         : ProcessModeEnum.Disabled;
    // }
}
