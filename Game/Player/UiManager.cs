using Godot;
using System;

public partial class UiManager : Control
{
    private Label ammo;
    private Label health;
    private Label money;

    public override void _Ready()
    {
        ammo = GetNode<Label>("Ammo");

        EventBus.UpdateAmmo += UpdateAmmo;
    }

    private void UpdateAmmo(int current, int max)
    {
        ammo.Text = current + " / " + max;
    }
}
