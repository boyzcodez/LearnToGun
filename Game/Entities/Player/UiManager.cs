using Godot;
using System;

public partial class UiManager : Control
{
    public Wheel wheel;
    private Label ammo;
    private Label health;
    private Label money;

    private int currentMoney = 0;

    public override void _Ready()
    {
        ammo = GetNode<Label>("Ammo");
        money = GetNode<Label>("Money");

        wheel = GetNode<Wheel>("Wheel");

        EventBus.UpdateAmmo += UpdateAmmo;
        EventBus.GainMoney += UpdateMoney;
    }

    private void UpdateAmmo(int current, int max)
    {
        ammo.Text = current + " / " + max;
    }
    private void UpdateMoney(int amount)
    {
        currentMoney += amount;
        money.Text = currentMoney.ToString() + " coin";
    }
}
