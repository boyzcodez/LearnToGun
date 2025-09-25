using Godot;
using System;

public partial class SceneTransition : CanvasLayer
{
    private Portal2 portal;
    private TransitionRect bg;
    private Speedlines speedlines;

    private bool rectUp = false;

    public override void _Ready()
    {
        portal = GetNode<Portal2>("PortalControl/Portal2");
        bg = GetNode<TransitionRect>("TransitionBG");
        speedlines = GetNode<Speedlines>("Speedlines");

        EventBus.Transition += Transition;
    }

    public void Transition()
    {
        if (rectUp)
            TransitionIn();
        else
            TransitionOut();
    }

    private void TransitionIn()
    {
        rectUp = true;

        bg.TransitionIn();
        portal.GrowPortal();
    }
    private void TransitionOut()
    {
        rectUp = false;

        bg.TransitionOut();
        portal.ShrinkPortal();
    }

}
