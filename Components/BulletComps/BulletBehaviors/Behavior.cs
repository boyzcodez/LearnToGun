using Godot;
using System;

[GlobalClass]
public abstract partial class Behavior : Resource
{
    public abstract void Initialize(Bullet bullet);
    public abstract void Update(Bullet bullet, double delta);
    public abstract void OnHit(Bullet bullet);
}
