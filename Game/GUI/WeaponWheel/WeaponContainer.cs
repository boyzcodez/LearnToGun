using Godot;
using System;

public partial class WeaponContainer : TextureRect
{
    private TextureRect icon = null;
    public GunData gunData = null;

    public override void _Ready()
    {
        icon = GetNode<TextureRect>("Icon");
    }

}
