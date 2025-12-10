using Godot;
using System;

public partial class Dust : CpuParticles2D
{
    public override void _Ready()
    {
        Emitting = true;
    }

    private void _on_finished()
    {
        QueueFree();
    }
}
