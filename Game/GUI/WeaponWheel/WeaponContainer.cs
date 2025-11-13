using Godot;
using System;

public partial class WeaponContainer : TextureRect
{
    [Export] private Texture2D fail;
    private TextureRect icon = null;
    public GunData gunData = null;
    public int index = 0;

    public override void _Ready()
    {
        icon = GetNode<TextureRect>("Icon");
    }
    public void SetContainerData(GunData newGunData, int newIndex)
    {
        gunData = newGunData;
        index = newIndex;

        if (gunData.icon != null) icon.Texture = gunData.icon;
        else icon.Texture = fail;
    }

}
