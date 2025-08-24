using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerWeaponManager : Node
{
    [Export] public Node2D GunSocket; // Where the gun scene will be attached (e.g. hand position)
    [Export] public PackedScene[] GunScenes { get; set; } = Array.Empty<PackedScene>(); // List of gun scene prefabs

    private List<Gun> _guns = new();
    private int _currentGunIndex = 0;

    public override void _Ready()
    {
        // Instantiate all guns and keep them inactive except the first
        foreach (var gunScene in GunScenes)
        {
            var gun = gunScene.Instantiate<Gun>();
            gun.Visible = false;
            GunSocket.AddChild(gun);
            _guns.Add(gun);
        }

        if (_guns.Count > 0)
        {
            EquipGun(0);
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.WheelUp && mouseEvent.Pressed)
                SwitchGun(1);
            else if (mouseEvent.ButtonIndex == MouseButton.WheelDown && mouseEvent.Pressed)
                SwitchGun(-1);
        }
    }

    private void SwitchGun(int direction)
    {
        if (_guns.Count == 0) return;

        _guns[_currentGunIndex].Visible = false;

        _currentGunIndex = (_currentGunIndex + direction) % _guns.Count;
        if (_currentGunIndex < 0) _currentGunIndex = _guns.Count - 1;

        EquipGun(_currentGunIndex);
    }

    private void EquipGun(int index)
    {
        _guns[index].Visible = true;
    }

    public void Shoot(Node shooter)
    {
        if (_guns.Count > 0)
            _guns[_currentGunIndex].Shoot(shooter);
    }
}
