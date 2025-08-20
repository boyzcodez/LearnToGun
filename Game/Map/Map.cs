using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;

public partial class Map : Node2D
{
    private PackedScene mapLine = ResourceLoader.Load<PackedScene>("uid://dtmy4qotne00s");
    [Export] public int PointCount = 8;
    [Export] public float MapRadius = 300f;
    [Export] public Texture2D PointTexture;
    [Export] public Godot.Color LineColor = new Godot.Color(1, 0, 0.5f);
    [Export] public float LineWidth = 2f;
    [Export] public Node2D Lines;

    private Line2D _line;
    private List<Vector2> _points = new List<Vector2>();
    private List<Vector2[]> _connections = new List<Vector2[]>();
    private List<Node2D> _pointSprites = new List<Node2D>();

    public override void _Ready()
    {
        _line = new Line2D();
        _line.DefaultColor = LineColor;
        _line.Width = LineWidth;
        AddChild(_line);

        RegenerateMap();
    }
    private void RegenerateMap()
    {
        ClearMap();
        GeneratePoints();
        GenerateConnections();
        DrawConnections();
        SpawnPointSprites();
    }
    private void ClearMap()
    {
        _points.Clear();
        _connections.Clear();
        _line.ClearPoints();

        foreach (var sprite in _pointSprites)
        {
            if (sprite != null && IsInstanceValid(sprite))
                sprite.QueueFree();
        }
        _pointSprites.Clear();
    }
    private void GeneratePoints()
    {
        _points.Clear();
        var rng = new Random();

        for (int i = 0; i < PointCount; i++)
        {
            double angle = rng.NextDouble() * Math.Tau;
            double radius = rng.NextDouble() * MapRadius;
            Vector2 pos = new Vector2(
                (float)(Math.Cos(angle) * radius),
                (float)(Math.Sin(angle) * radius)
            );
            _points.Add(pos);
        }
    }
    private void GenerateConnections()
    {
        _connections.Clear();

        for (int i = 0; i < _points.Count; i++)
        {
            float minDist = float.MaxValue;
            int closestIndex = -1;

            for (int j = 0; j < _points.Count; j++)
            {
                if (i == j) continue;

                float dist = _points[i].DistanceTo(_points[j]);
                if (dist < minDist)
                {
                    minDist = dist;
                    closestIndex = j;
                }
            }

            if (closestIndex != -1)
            {
                Vector2 start = _points[i];
                Vector2 end = _points[closestIndex];
                _connections.Add(new Vector2[] { start, end });
            }
        }
    }
    private void DrawConnections()
    {
        _line.ClearPoints();
        // foreach (var line in Lines.GetChildren())
        // {
        //     line.QueueFree();
        // }

        foreach (var pair in _connections)
        {
            //var newMapLine = mapLine.Instantiate() as Line2D;

            _line.AddPoint(pair[0]);
            _line.AddPoint(pair[1]);
            //_line.AddPoint(Vector2.Inf);
            //Lines.AddChild(newMapLine);

            //_line.AddPoint(Vector2.Inf);
        }
    }
    private void SpawnPointSprites()
    {
        foreach (var pos in _points)
        {
            var sprite = new Sprite2D();
            sprite.Texture = PointTexture;
            sprite.Position = pos;
            sprite.Centered = true;
            AddChild(sprite);

            _pointSprites.Add(sprite);
        }
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("space"))
            RegenerateMap();
    }

}
