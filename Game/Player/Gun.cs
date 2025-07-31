using Godot;
using System;

public partial class Gun : Node2D
{
    private Hitbox hitbox;
    private Sprite2D rangeMarker;
    [Export] BaseGun gun;

    public override void _Ready()
    {
        rangeMarker = GetNode<Sprite2D>("RangeMarker");
        hitbox = GetNode<Hitbox>("Hitbox");
        SetGun();
    }
    public void SetGun()
    {
        if (gun != null)
        {
            hitbox.SetGun(gun);
            rangeMarker.Position = new Vector2(gun.RangeX, 0);
        }
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("attack"))
        {
            hitbox.ApplyDamage();
        }
    }
}
