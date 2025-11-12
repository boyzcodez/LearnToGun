using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

public partial class Wheel : Control
{
	private Control[] wheels = new Control[3];
	private int currentWheel = 0;
	private TextureRect highlighted = null;
	private List<WeaponContainer> icons = new();

	// highlight color and normal color
	private readonly Color normalColor = new Color(1, 1, 1, 1);
	private readonly Color highlightColor = new Color(1, 1, 0.6f, 1);

	public override void _Ready()
	{
		// Try to load the three child wheels by name. If a node is missing, leave null.
		wheels[0] = GetNodeOrNull<Control>("WeaponWheel1");
		wheels[1] = GetNodeOrNull<Control>("WeaponWheel2");
		wheels[2] = GetNodeOrNull<Control>("WeaponWheel3");

		foreach (var wheel in wheels)
		{
			foreach (WeaponContainer icon in wheel.GetChildren())
			{
				icons.Add(icon);
			}
		}

		// Start hidden; we'll show while Tab is held
		Visible = false;
		UpdateWheelVisibility();
	}

	public override void _Process(double delta)
	{
		bool tabPressed = Input.IsActionPressed("tab");

		if (tabPressed)
		{
			// show while held
			if (!Visible)
				Visible = true;

			// update highlight for current wheel each frame while visible
			UpdateHighlight();
		}
		else
		{
			// released this frame -> if it was visible, treat as selection
			if (Visible)
				OnTabReleased();

			Visible = false;
			ClearHighlight();
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		// Only handle wheel switching while visible
		if (!Visible)
			return;

		if (@event is InputEventMouseButton mb && mb.Pressed)
		{
			// Mouse wheel up -> previous wheel, wheel down -> next wheel
			if (mb.ButtonIndex == MouseButton.WheelUp)
			{
				CycleWheel(-1);
			}
			else if (mb.ButtonIndex == MouseButton.WheelDown)
			{
				CycleWheel(1);
			}
		}
	}

	private void CycleWheel(int direction)
	{
		int count = wheels.Count(w => w != null);
		if (count == 0)
			return;

		// find next non-null wheel index
		int next = currentWheel;
		for (int i = 0; i < wheels.Length; i++)
		{
			next = (next + direction + wheels.Length) % wheels.Length;
			if (wheels[next] != null)
				break;
		}

		currentWheel = next;
		UpdateWheelVisibility();
		ClearHighlight();
	}

	private void UpdateWheelVisibility()
	{
		for (int i = 0; i < wheels.Length; i++)
		{
			if (wheels[i] == null)
				continue;

			wheels[i].Visible = (i == currentWheel);
		}
	}

	private void UpdateHighlight()
	{
		var wheel = wheels[currentWheel];
		if (wheel == null)
			return;

		// collect TextureRect children (the weapon containers are TextureRect nodes)
		var texRects = wheel.GetChildren().OfType<TextureRect>().ToArray();

		if (texRects.Length == 0)
			return;

		Vector2 mousePos = GetViewport().GetMousePosition();

		// prefer exact hover (mouse inside rect) otherwise use closest center
		TextureRect best = null;
		float bestDist = float.MaxValue;

		foreach (var tr in texRects)
		{
			Rect2 rect = new Rect2(tr.GlobalPosition, tr.Size);
			if (rect.HasPoint(mousePos))
			{
				best = tr;
				break; // exact hovered
			}

			// distance to center
			Vector2 center = tr.GlobalPosition + tr.Size * 0.5f;
			float d = (center - mousePos).Length();
			if (d < bestDist)
			{
				bestDist = d;
				best = tr;
			}
		}

		if (best != highlighted)
		{
			// restore previous
			if (highlighted != null && IsInstanceValid(highlighted))
				highlighted.Modulate = normalColor;

			highlighted = best;

			if (highlighted != null && IsInstanceValid(highlighted))
				highlighted.Modulate = highlightColor;
		}
	}

	private void ClearHighlight()
	{
		if (highlighted != null && IsInstanceValid(highlighted))
			highlighted.Modulate = normalColor;

		highlighted = null;
	}

	private void OnTabReleased()
	{
		if (highlighted != null && IsInstanceValid(highlighted))
		{
			GD.Print($"Selected: {highlighted.Name}");

			if (icons.Contains(highlighted)) GD.Print("this worked 11111");
			else GD.Print("this worked 2");
		}
		else
		{
			GD.Print("Selected: (none)");
		}
	}
}
