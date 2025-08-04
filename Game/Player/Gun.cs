using Godot;
using System;
//using System.Numerics;

public partial class Gun : Node2D
{
    private Hitbox hitbox;
    private Sprite2D rangeMarker;
    private Sprite2D gunSprite;
    [Export] BaseGun gun;

    public override void _Ready()
    {
        rangeMarker = GetNode<Sprite2D>("RangeMarker");
        gunSprite = GetNode<Sprite2D>("GunSprite");
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
    public override void _PhysicsProcess(double delta)
    {
        var angle = this.GlobalRotation;
        if (Math.Abs(angle) > Math.PI / 2)
        {
            Scale = new Vector2(1, -1);
        }
        else
        {
            Scale = new Vector2(1, 1);
        }
    }

}
