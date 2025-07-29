using Godot;
using System;
using System.Threading.Tasks;

public partial class DamageNumbers : Node2D
{
    Font PixelFont = GD.Load<Font>("res://Assets/Fonts/PixelOperator8.ttf");

    public async void DisplayNumber(int damage, string damageType)
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
        number.LabelSettings.FontColor = DamageTypeColor(damageType);
        number.LabelSettings.OutlineColor = new Color("#ffffffff");
        number.LabelSettings.OutlineSize = 0;
        // number.Rotation = (float)GD.RandRange(-Math.PI / 4, Math.PI / 4); // Random rotation between -45 and 45 degrees

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

    public Color DamageTypeColor(string damageType)
    {
        return damageType switch
        {
            "Burn" => new Color("#ffaa49ff"), // OrangeRed
            "Cascade" => new Color("#1eff96ff"), // DodgerBlue
            "Obliterate" => new Color("#6fe2fcff"), // DarkRed
            _ => new Color("#FFFFFF") // Default to white for unknown types
        };
    }
}
