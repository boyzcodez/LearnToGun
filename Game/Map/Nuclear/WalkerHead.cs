using Godot;
using System;

public partial class WalkerHead : Node2D
{
    [Export] public TileMapLayer map;
    public override void _Ready()
    {
        foreach (WalkerUnit walker in GetChildren())
        {
            walker.CalcPaht();
        }
        
    }
}
