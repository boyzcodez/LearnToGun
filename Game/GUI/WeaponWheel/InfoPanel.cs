using Godot;
using System;

public partial class InfoPanel : Panel
{
    // Cache node references (optional but faster)
    private Label _name;
    private Label _level;
    private Label _ammo;
    private ProgressBar _xpBar;

    public override void _Ready()
    {
        _name  = GetNode<Label>("VBoxContainer/Header/NameLevel/Name");
        _level = GetNode<Label>("VBoxContainer/Header/NameLevel/LVL");
        _ammo  = GetNode<Label>("BoxContainer/Ammo");
        _xpBar = GetNode<ProgressBar>("XP");
    }

    public void UpdateInfo(GunData gunData)
    {
        if (gunData == null)
        {
            Visible = false;
            return;
        }

        Visible = true;

        // --- Name & Level -----------------------------------------------------
        _name.Text = gunData.GunName;
        _level.Text = gunData.LVL;

        // --- Ammo -------------------------------------------------------------
        _ammo.Text = $"{gunData.CurrentAmmo} / {gunData.MaxAmmo}";

        // --- XP Bar -----------------------------------------------------------
        _xpBar.MaxValue = gunData.maxXP;
        _xpBar.Value    = gunData.currentXP;

        //_xpBar.TooltipText = $"{gunData.XpCurrent} / {gunData.XpRequired} XP";
    }
}
