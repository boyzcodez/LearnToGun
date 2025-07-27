using Godot;
using System;
using System.Threading.Tasks;

public partial class DamageNumbers : Node2D
{
    Font PixelFont = GD.Load<Font>("res://Assets/Fonts/PixelOperator8.ttf");

    public async void DisplayNumber(int damage)
    {
        var number = new Label
        {
            Position = Vector2.Zero, // Centered at this node's position
            Text = damage.ToString(),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            LabelSettings = new LabelSettings()
        };

        number.LabelSettings.Font = PixelFont;
        number.LabelSettings.FontSize = 8;
        number.LabelSettings.OutlineColor = new Color("#000");
        number.LabelSettings.OutlineSize = 0;

        CallDeferred("add_child", number);
        await ToSignal(number, Label.SignalName.Resized);

        // Center the label at the parent origin
        number.Position = -number.Size / 2.0f + new Vector2(0, -10);

        var tween = CreateTween();
        tween.SetParallel(true);

        tween.TweenProperty(number, "modulate", new Color(1, 1, 1, 0), 0.5f);
        tween.TweenProperty(number, "position", number.Position + new Vector2(0, -50), 0.5f);

        await ToSignal(tween, Tween.SignalName.Finished);
        number.QueueFree();
    }
}
