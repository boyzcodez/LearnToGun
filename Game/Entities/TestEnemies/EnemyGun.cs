using Godot;
using System;

[GlobalClass]
public partial class EnemyGun : Node2D
{
    [Export] Gun gun;
    private RayCast2D rayCast;

    private float time = 0f;
    private bool canShoot = false;

    public override void _Ready()
    {
        rayCast = GetNode<RayCast2D>("RayCast");
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

        if (canShoot == false)
        {
            time += (float)delta;

            if (time >= 4f)
            {
                canShoot = true;
                time = 0;
            }
        }
        else if (!rayCast.IsColliding() && canShoot)
        {
            canShoot = false;
            if (gun != null) gun.Shoot();
        }


    }
}
