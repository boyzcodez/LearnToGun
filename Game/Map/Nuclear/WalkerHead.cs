using Godot;
using System.Collections.Generic;

public partial class WalkerHead : Node2D
{
    [Export] public int MapLength = 100;
    [Export] public int PathLength = 100;
    [Export] public TileMapLayer FloorMap;
    [Export] public TileMapLayer WallMap;

    public override void _Ready()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        FloorMap.Clear();
        WallMap.Clear();

        foreach (WalkerUnit walker in GetChildren())
        {
            walker.CalcPaht();
        }

        BuildWalls();
    }

    public void BuildWalls()
    {
        List<Vector2I> floorSet = new();
        foreach (WalkerUnit walker in GetChildren())
        {
            foreach (var pos in walker.carvedTiles)
            {
                if (!floorSet.Contains(pos)) floorSet.Add(pos);
            }
        }
            

        for (int x = -MapLength; x < MapLength; x++)
        {
            for (int y = -MapLength; y < MapLength; y++)
            {
                var location = new Vector2I(x, y);
                if (!floorSet.Contains(location)) WallMap.SetCell(location, 0, new Vector2I(1, 0));
            }
        }
    }

    public override void _Input(InputEvent input)
    {
        if (input.IsActionPressed("space"))
        {
            GenerateMap();
        }
    }



    public void DestroyWall(Vector2I pos)
    {
        WallMap.EraseCell(pos);
    }
}
