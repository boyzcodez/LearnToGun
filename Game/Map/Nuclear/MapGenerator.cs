using Godot;
using System;
using System.Collections.Generic;

public partial class MapGenerator : Node
{
    [Export] public TileMapLayer FloorLayer;
    [Export] public TileMapLayer WallLayer;
    [Export] public TileMapLayer UnderWallLayer;

    [Export] public int FloorTileId = 0;
    [Export] public int WallTileId = 1;
    [Export] public int UnderWallTileId = 2;

    [Export] public int Width = 80;
    [Export] public int Height = 80;

    [Export] public int RoomCount = 5;
    [Export] public int WalkStepsPerRoom = 250;
    [Export] public int WalkRadius = 2;

    private bool[,] map;
    private Random rng = new Random();

    public override void _Ready()
    {
        GenerateMap();
    }

    // ------------------------------------------------------------
    // MAIN GENERATION
    // ------------------------------------------------------------
    public void GenerateMap()
    {
        map = new bool[Width, Height];
        List<Vector2I> roomCenters = new();

        // --- Generate organic rooms ---
        for (int i = 0; i < RoomCount; i++)
        {
            Vector2I start = new(
                rng.Next(5, Width - 5),
                rng.Next(5, Height - 5)
            );

            roomCenters.Add(start);
            DrunkWalk(start, WalkStepsPerRoom, WalkRadius);
        }

        // --- Connect rooms with corridors ---
        for (int i = 0; i < roomCenters.Count - 1; i++)
        {
            CarveCorridor(roomCenters[i], roomCenters[i + 1]);
        }

        // --- Paint tiles ---
        PaintTiles();
    }

    // ------------------------------------------------------------
    // DRUNK WALK ROOM CREATION
    // ------------------------------------------------------------
    private void DrunkWalk(Vector2I start, int steps, int radius)
    {
        int x = start.X;
        int y = start.Y;

        for (int i = 0; i < steps; i++)
        {
            // Carve circle
            for (int rx = -radius; rx <= radius; rx++)
            {
                for (int ry = -radius; ry <= radius; ry++)
                {
                    int px = x + rx;
                    int py = y + ry;

                    if (px > 1 && px < Width - 1 && py > 1 && py < Height - 1)
                        map[px, py] = true;
                }
            }

            // Step
            int dir = rng.Next(4);
            switch (dir)
            {
                case 0: x++; break;
                case 1: x--; break;
                case 2: y++; break;
                case 3: y--; break;
            }

            // Clamp
            x = Mathf.Clamp(x, 1, Width - 2);
            y = Mathf.Clamp(y, 1, Height - 2);
        }
    }

    // ------------------------------------------------------------
    // CORRIDORS BETWEEN ROOMS
    // ------------------------------------------------------------
    private void CarveCorridor(Vector2I a, Vector2I b)
    {
        int x = a.X;
        int y = a.Y;

        // Horizontal tunnel
        while (x != b.X)
        {
            if (x >= 1 && x < Width - 1 && y >= 1 && y < Height - 1)
                map[x, y] = true;

            x += Math.Sign(b.X - x);
        }

        // Vertical tunnel
        while (y != b.Y)
        {
            if (x >= 1 && x < Width - 1 && y >= 1 && y < Height - 1)
                map[x, y] = true;

            y += Math.Sign(b.Y - y);
        }
    }

    // ------------------------------------------------------------
    // TILE PAINTING
    // ------------------------------------------------------------
    private void PaintTiles()
    
    {
        FloorLayer.Clear();
        WallLayer.Clear();
        UnderWallLayer.Clear();

        //int tilesPerRow = 3; // your number of columns

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Vector2I cell = new(x, y);

                if (map[x, y])
                {
                    int floorTile = GetRandomTileId(0); // row 0 = floor
                    FloorLayer.SetCell(cell, floorTile);
                }
                else
                {
                    int wallTile = GetRandomTileId(1); // row 1 = walls
                    int underTile = GetRandomTileId(2); // row 2 = under walls

                    WallLayer.SetCell(cell, wallTile);
                    UnderWallLayer.SetCell(cell, underTile);
                }
            }
        }
}

    private int GetRandomTileId(int rowIndex)
    {
        int tilesPerRow = 3; // adjust to how many columns you have
        int col = rng.Next(tilesPerRow);
        return col + rowIndex * tilesPerRow;
    }
}
