using Godot;

[GlobalClass]
public partial class DeleteTimer : Behavior
{
    [Export] private float deleteTime = 4f;
    private float timer = 0f;
    public override void Initialize(Bullet bullet)
    {
        timer = 0f;
    }
    public override void Update(Bullet bullet, double delta)
    {
        timer += (float)delta;
        if (timer >= deleteTime) bullet.Deactivate();
    }
    public override void OnHit(Bullet bullet)
    {
    }
}


        