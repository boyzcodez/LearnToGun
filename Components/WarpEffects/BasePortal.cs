using Godot;
using System;

public partial class BasePortal : CanvasGroup
{
    [Export]
    public Color[] Palette { get; set; } = new Color[]
    {
        Colors.Violet,
        Colors.Cyan,
        Colors.Lime,
        Colors.Fuchsia,
        Colors.Orange
    };

    [Export]
    public string ShaderLineParam { get; set; } = "line_color";

    [Export]
    public float TransitionDuration { get; set; } = 2.0f;

    [Export]
    public float HoldDuration { get; set; } = 1.0f;

    private readonly RandomNumberGenerator _rng = new();
    private int _currentIndex = -1;

    public override void _Ready()
    {
        _rng.Randomize();
        if (Palette == null || Palette.Length == 0)
            return;

        _currentIndex = _rng.RandiRange(0, Palette.Length - 1);
        var mat = GetShaderMaterial();
        if (mat != null)
            mat.SetShaderParameter(ShaderLineParam, Palette[_currentIndex]);

        StartColorCycle();
    }

    private ShaderMaterial GetShaderMaterial()
    {
        // Prefer this node's material (CanvasItem.Material), otherwise search children for first ShaderMaterial
        if (Material is ShaderMaterial sm)
            return sm;

        foreach (var child in GetChildren())
        {
            if (child is CanvasItem ci && ci.Material is ShaderMaterial csm)
                return csm;
        }

        return null;
    }

    private async void StartColorCycle()
    {
        if (Palette == null || Palette.Length == 0)
            return;

        while (IsInsideTree())
        {
            // pick a different random color
            int next;
            if (Palette.Length == 1)
                next = 0;
            else
            {
                do
                {
                    next = _rng.RandiRange(0, Palette.Length - 1);
                } while (next == _currentIndex);
            }

            _currentIndex = next;
            var targetColor = Palette[_currentIndex];

            var mat = GetShaderMaterial();
            if (mat == null)
                return;

            // Tween the shader parameter smoothly
            var tween = CreateTween()
                .SetTrans(Tween.TransitionType.Sine)
                .SetEase(Tween.EaseType.InOut);

            // The property path "shader_parameter/<name>" lets Tween set shader params on ShaderMaterial
            tween.TweenProperty(mat, $"shader_parameter/{ShaderLineParam}", targetColor, TransitionDuration);

            await ToSignal(tween, "finished");

            // hold the color for a bit before switching again
            await ToSignal(GetTree().CreateTimer(HoldDuration), "timeout");
        }
    }
}
