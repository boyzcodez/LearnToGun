using Godot;
using System;
using System.Xml.Serialization;

public partial class UiManager : Control
{
    public Wheel wheel;
    private Label ammo;
    private TextureProgressBar health;
    private Label money;

    private int currentMoney = 0;

    private int _coins = 0;
    public int Coins
    {
        get => _coins;
        set => OnCoinsSet(value);
    }
    private int _fakeCoins = 0;
    public int FakeCoins
    {
        get => _fakeCoins;
        set => OnFakeCoinsSet(value);
    }

    private Tween tween;

    public override void _Ready()
    {
        ammo = GetNode<Label>("Ammo");
        money = GetNode<Label>("Money");
        health = GetNode<TextureProgressBar>("Healthbar");

        wheel = GetNode<Wheel>("Wheel");

        EventBus.UpdateAmmo += UpdateAmmo;
        EventBus.GainMoney += UpdateMoney;
        EventBus.UpdateHealth += UpdateHealth;
    }

    private void UpdateAmmo(int current, int max)
    {
        ammo.Text = current + " / " + max;
    }
    private void UpdateMoney(int amount)
    {
        // currentMoney += amount;
        // money.Text = currentMoney.ToString() + " coin";

        Coins += amount;
    }
    private void UpdateHealth(int current, int max)
    {
        health.MaxValue = max;
        health.Value = current;
    }

    private void OnCoinsSet(int newValue)
    {
        _coins = newValue;

        // tween fakeCoins → coins
        if (tween != null)
            tween.Kill();

        tween = CreateTween();
        tween.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
        tween.TweenProperty(this, "FakeCoins", Coins, 0.3f);
    }

    private void OnFakeCoinsSet(int newValue)
    {
        _fakeCoins = newValue;
        money.Text = _fakeCoins.ToString() + " coin";
    }
}
