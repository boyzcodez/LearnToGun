using Godot;
using System;

public partial class Spawn : AnimatedSprite2D
{
    [Export] private StateMachine stateMachine;
    [Export] private Hurtbox hurtbox;
    private void _on_animation_finished()
    {
        GetNode<Node2D>("Hide").Visible = true;
        hurtbox.Monitorable = true;
        stateMachine.SetActive();
    }
}
