using Godot;
using System;

[GlobalClass]
public partial class EnemyGun : Node2D
{
    private Hitbox hitbox;
    private GunSprite gunSprite;
    public override void _Ready()
    {
        hitbox = GetNode<Hitbox>("Hitbox");
        gunSprite = GetNode<GunSprite>("GunSprite");
    }
    public void Shoot()
    {
        hitbox?.ApplyDamage();
        gunSprite?.FireAnimation();
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
