using Godot;
using System;
//using System.Numerics;

public partial class Gun : Node2D
{
    private Hitbox hitbox;
    private Sprite2D rangeMarker;
    private GunSprite gunSprite;
    private CpuParticles2D gunParticles;
    [Export] BaseGun gun;

    public override void _Ready()
    {
        rangeMarker = GetNode<Sprite2D>("RangeMarker");
        gunSprite = GetNode<GunSprite>("GunSprite");
        hitbox = GetNode<Hitbox>("Hitbox");
        gunParticles = GetNode<CpuParticles2D>("ShotgunFireStrands");
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
            gunSprite.FireAnimation();
            gunParticles.Emitting = true;
        }
    }
    public void FireBeam(Vector2 from, Vector2 to)
    {
        var beamScene = GD.Load<PackedScene>("res://Game/Guns/Partical Effects/shotgun_beam.tscn");
        var beam = (Node2D)beamScene.Instantiate();
        GetTree().CurrentScene.AddChild(beam);

        beam.Position = from;

        Vector2 dir = to - from;
        float dist = dir.Length();
        float angle = dir.Angle();

        beam.Rotation = angle;
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
