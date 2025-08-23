using Godot;
using System;

public partial class Spawn : AnimatedSprite2D
{
    [Export] private StateMachine stateMachine;
    private void _on_animation_finished()
    {
        GetNode<Node2D>("Hide").Visible = true;
        stateMachine.SetActive();
    }
}
