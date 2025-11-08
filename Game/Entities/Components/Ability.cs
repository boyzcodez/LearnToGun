using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

[GlobalClass]
public partial class Ability : Node2D
{
    [Export] AnimatedSprite animatedSprite;
    [Export] float rotateAmount = 0f;
    [Export] Marker2D lookat;
    [Export] public int Times = 1;
    [Export] public float RepeatWait = 0.5f;
    [Export] public float ShotWait = 0.1f;

    private List<Guns> guns = new();
    private Timer timer;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");

        GetOwner<Entity>().Connect(Entity.SignalName.Activation, new Callable(this, nameof(Activate)));
        GetOwner<Entity>().Connect(Entity.SignalName.Deactivation, new Callable(this, nameof(Deactivate)));

        foreach (var child in GetChildren())
        {
            if (child is Guns gun)
            {
                guns.Add(gun);
                gun.SetProcess(false);
            }
            
        }
    }


    public async void TriggerAbility()
    {
        if (animatedSprite != null)
        {
           animatedSprite.PlayAnimation("Ability", 8);

            await ToSignal(animatedSprite, "animation_finished"); 
        }
        
        for (int i = 0; i < Times; i++)
        {
            foreach (var gun in guns)
            {
                gun.Shoot();
            }

            timer.Start(RepeatWait);
            await ToSignal(timer, "timeout");
        }
    }

    private void Activate()
    {
        foreach (var gun in guns)
        {
            gun.SetProcess(true);
        }
    }
    private void Deactivate()
    {
        foreach (var gun in guns)
        {
            gun.SetProcess(false);
        }
    }
}
