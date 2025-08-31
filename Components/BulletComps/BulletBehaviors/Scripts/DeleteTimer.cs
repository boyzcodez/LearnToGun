using Godot;

[GlobalClass]
public partial class DeleteTimer : Behavior
{
    [Export] private float deleteTime = 4f;
    public override void Initialize(Bullet bullet)
    {
        bullet.timer = 0f;
    }
    public override void Update(Bullet bullet, double delta)
    {
        bullet.timer += (float)delta;
        if (bullet.timer >= deleteTime) bullet.Deactivate(); 
    }
    public override void OnHit(Bullet bullet)
    {
    }
}


        