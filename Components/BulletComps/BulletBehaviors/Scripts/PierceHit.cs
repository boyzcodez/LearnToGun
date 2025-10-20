using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class PierceHit : Behavior
{
    [Export] public int MaxPierces = 3;
    private HashSet<Hurtbox> _piercedTargets;
    private int _currentPierces;
    private bool _isActive;
    public override void Initialize(Bullet bullet)
    {
        _piercedTargets = new HashSet<Hurtbox>();
        _currentPierces = 0;
        _isActive = true;
    }
    public override void Update(Bullet bullet, double delta)
    {
    }
    public override void OnHit(Bullet bullet)
    {
        if (!_isActive) return;
        var overlaps = bullet.GetOverlappingAreas();

        foreach (Hurtbox hurtbox in overlaps)
        {
            if (_piercedTargets.Contains(hurtbox)) return;
            else
            {
                _piercedTargets.Add(hurtbox);
                _currentPierces++;
                hurtbox.TakeDamage(bullet.DamageData, bullet.Direction);
            }
                
        }

        if (_currentPierces >= MaxPierces)
        {
            bullet.Deactivate();
        }
    }
}
