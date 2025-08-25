using Godot;
using System.Collections.Generic;

public partial class WeaponWheel : Control
{
//     [Export] public PlayerWeaponManager WeaponManager;
//     [Export] public float Radius = 150f;

//     private List<Gun> _guns = new();
//     private int _highlightedIndex = -1;

//     public override void _Ready()
//     {
//         Hide();
//         if (WeaponManager != null)
//             //_guns = WeaponManager.GetGuns();
//     }

//     public override void _Process(double delta)
//     {
//         if (!Visible) return;

//         // Get mouse vector relative to center
//         Vector2 center = GetViewportRect().Size / 2f;
//         Vector2 dir = GetGlobalMousePosition() - center;

//         if (dir.Length() > 20) // deadzone
//         {
//             float angle = Mathf.Atan2(dir.Y, dir.X);
//             if (angle < 0) angle += Mathf.Tau; // normalize 0..2π

//             // Determine which sector mouse is in
//             int sectorCount = _guns.Count;
//             float sectorAngle = Mathf.Tau / sectorCount;
//             _highlightedIndex = (int)(angle / sectorAngle);
//         }
//     }

//     public void Open()
//     {
//         //_guns = WeaponManager.GetGuns();
//         Show();
//         GetTree().Paused = true;
//     }

//     public void Close()
//     {
//         Hide();
//         GetTree().Paused = false;

//         if (_highlightedIndex >= 0 && _highlightedIndex < _guns.Count)
//         {
//             //WeaponManager.SwitchTo(_highlightedIndex);
//         }
//     }

//     public override void _Draw()
//     {
//         if (!Visible) return;

//         Vector2 center = GetViewportRect().Size / 2f;
//         int count = _guns.Count;
//         float sectorAngle = Mathf.Tau / count;

//         for (int i = 0; i < count; i++)
//         {
//             float angle = (i + 0.5f) * sectorAngle;
//             Vector2 pos = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * Radius;

//             // Draw icon
//             var tex = _guns[i].gunData.GunSprite;
//             if (tex != null)
//             {
//                 Rect2 rect = new Rect2(pos - tex.GetSize() / 2, tex.GetSize());
//                 DrawTexture(tex, rect.Position);

//                 if (i == _highlightedIndex)
//                 {
//                     DrawCircle(pos, tex.GetSize().X / 2 + 10, new Color(1, 1, 0, 0.5f));
//                 }
//             }
//         }
//     }
}

