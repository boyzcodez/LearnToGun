using Godot;
using System;

public partial class ActiveTestEnemy : Entity
{
    private Interactable interactable;

    public override void _Ready()
    {
        interactable = GetNode<Interactable>("Interactable");
        interactable.interactFunction = new Callable(this, nameof(HandleInteraction));
        EventBus.Reset += Death;
    }
    private void HandleInteraction()
    {
        GetNode<StateMachine>("StateMachine").SetActive();
    }
}
