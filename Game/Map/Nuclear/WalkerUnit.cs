using Godot;
using System.Collections.Generic;

public partial class WalkerUnit : Node2D
{
    public enum Dirs{LEFT,RIGHT,UP,DOWN}
    private int PathLength = 100;
    const int tileSize = 32;
    

    public void CalcPaht()
    {
        List<int> PathSteps = new();
        for (int i = 0; i < PathLength; i++)
        {
            var stepsi = GD.RandRange(0, Dirs.GetNames(typeof(Dirs)).Length - 1);
            PathSteps.Add(stepsi);
        }

        Vector2I location = (Vector2I)GetParent<WalkerHead>().GlobalPosition;
        TileMapLayer tm = GetParent<WalkerHead>().map;

        foreach (int dir in PathSteps)
        {
            var ModifierDirection = Vector2I.Zero;

            switch (dir)
            {
                case 0:
                    ModifierDirection = Vector2I.Left;
                    break;
                case 1:
                    ModifierDirection = Vector2I.Right;
                    break;
                case 2:
                    ModifierDirection = Vector2I.Up;
                    break;
                case 3:
                    ModifierDirection = Vector2I.Down;
                    break;
            }
            location += ModifierDirection;
            tm.SetCell(location, 0, new Vector2I(0, 0));
        }

    }

}
