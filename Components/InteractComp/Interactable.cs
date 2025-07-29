using Godot;
using System;

public partial class Interactable : Area2D
{
    [Export] private string interactionName = "Interact";
    [Export] private bool isInteractable = true;
    public Callable interactFunction;
    public override void _Ready()
    {
        interactFunction = new Callable(this, nameof(IfNothing));
    }
    public void interact()
    {
        isInteractable = false;
        interactFunction.Call();
    }
    private void IfNothing()
    {
        GD.Print("Nothing to do here.");
    }

}
