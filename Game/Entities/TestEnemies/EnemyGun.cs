using Godot;
using System;

[GlobalClass]
public partial class EnemyGun : Node2D
{
    [Export] PackedScene bullet;
    private GunSprite gunSprite;
    private Marker2D mark;

    private float time = 0f;

    // public override void _Ready()
    // {
    //     gunSprite = GetNode<GunSprite>("GunSprite");
    //     mark = GetNode<Marker2D>("MarkSpot");
    // }
    // public void Shoot()
    // {
    //     gunSprite?.FireAnimation();

    //     var instance = bullet?.Instantiate() as BasicBullet;

    //     instance.GlobalPosition = mark.GlobalPosition;
    //     instance.direction = Vector2.Right.Rotated(GlobalRotation);

    //     GetTree().CurrentScene.CallDeferred("add_child", instance);
    // }

    // remove the shoot countdown
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

        // time += (float)delta;
        // if (time >= 4f)
        // {
        //     time = 0f;
        //     Shoot();
        // }
    }
}
