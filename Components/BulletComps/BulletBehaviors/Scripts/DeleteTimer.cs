using Godot;

[GlobalClass]
public partial class DeleteTimer : Behavior
{
    [Export] private float deleteTime = 4f;
    public override void Initialize(Bullet bullet)
    {
    }
    public override void Update(Bullet bullet, double delta)
    {
        bullet._timer += (float)delta;
        if (bullet._timer >= deleteTime) bullet.Deactivate(); 
    }
    public override void OnHit(Bullet bullet)
    {
    }
}


        