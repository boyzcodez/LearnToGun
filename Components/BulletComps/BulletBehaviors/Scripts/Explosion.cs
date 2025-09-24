using System.Threading.Tasks;
using Godot;

[GlobalClass]
public partial class Explosion : Behavior
{
    [Export] private PackedScene _explosion;
    public override void Initialize(Bullet bullet)
    {
    }
    public override void Update(Bullet bullet, double delta)
    {
    }
    public override void OnHit(Bullet bullet)
    {
        if (_explosion != null)
        {
            var explosion = _explosion.Instantiate<Area2D>();
            explosion.GlobalPosition = bullet.GlobalPosition;
            bullet.GetTree().CurrentScene.CallDeferred("add_child", explosion);
        }
        bullet.Deactivate();
    }
}
