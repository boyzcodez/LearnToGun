using Godot;
using System;

public partial class ShotgunBeam : Node2D
{
    private float timeAlive = 0.0f;
    [Export] private float duration = 0.2f;
    [Export] private Color beamColor = new Color(1.0f, 0.2f, 1.0f);

    private Sprite2D beam;

    public override void _Ready()
    {
        beam = GetNode<Sprite2D>("Sprite2D");
        if (beam.Material is ShaderMaterial shaderMaterial)
        {
            shaderMaterial.SetShaderParameter("beamColor", beamColor);
        }
    }

    public override void _Process(double delta)
    {
        timeAlive += (float)delta;
        if (beam.Material is ShaderMaterial shaderMaterial)
        {
            shaderMaterial.SetShaderParameter("time", timeAlive);
        }

        if (timeAlive >= duration)
            QueueFree();
    }


}
