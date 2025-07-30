using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public partial class InteractComponent : Node2D
{
    private Label interactionLabel;
    private List<Interactable> interactables = new();
    private bool canInteract = true;

    public override void _Ready()
    {
        interactionLabel = GetNode<Label>("InteractLabel");
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("interact") && canInteract)
        {
            if (interactables.Count > 0)
            {
                canInteract = false;
                interactionLabel.Hide();

                var interactable = interactables[0];
                interactable?.Call("interact");

                canInteract = true;
            }
        }
    }
    public override void _Process(double delta)
    {
        if (interactables.Count > 0 && canInteract)
        {
            interactables.Sort(SortByNearest);
            var nearest = interactables[0];

            if ((bool)nearest.Get("isInteractable"))
            {
                interactionLabel.Text = nearest.Get("interactionName").ToString();
                interactionLabel.Show();
            }
        }    
        else
        {
            interactionLabel.Hide();
        }
        
    }
    private int SortByNearest(Area2D a, Area2D b)
    {
        float distA = GlobalPosition.DistanceTo(a.GlobalPosition);
        float distB = GlobalPosition.DistanceTo(b.GlobalPosition);
        return distA.CompareTo(distB);
    }
    public override void _EnterTree()
    {
        var interactArea = GetNode<Area2D>("InteractRange");
        interactArea.Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
        interactArea.Connect("area_exited", new Callable(this, nameof(OnAreaExited)));
    }
    private void OnAreaEntered(Node body)
    {
        interactables.Add(body as Interactable);
    }
    private void OnAreaExited(Node body)
    {
        interactables.Remove(body as Interactable);
    }
}
