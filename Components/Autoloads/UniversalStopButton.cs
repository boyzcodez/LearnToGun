using Godot;
using System.Collections.Generic;

public static class UniversalStopButton
{
    /// <summary>
    /// Fully disables a node and its children:
    /// - Stops processing (ProcessMode.Disabled)
    /// - Disables all CollisionShape2D / CollisionPolygon2D
    /// - Disables Area2D monitoring
    /// </summary>
    public static void DisableNode(Node node)
    {
        if (node == null) return;

        node.ProcessMode = Node.ProcessModeEnum.Disabled;

        // Recursively disable colliders and areas
        foreach (Node child in node.GetChildren())
        {
            if (child is CollisionShape2D shape)
                shape.Disabled = true;

            if (child is CollisionPolygon2D poly)
                poly.Disabled = true;

            if (child is Area2D area)
            {
                area.Monitoring = false;
                area.Monitorable = false;
            }

            // Recursive call for grandchildren
            DisableNode(child);
        }
    }

    /// <summary>
    /// Re-enables a node and its children.
    /// </summary>
    public static void EnableNode(Node node)
    {
        if (node == null) return;

        node.ProcessMode = Node.ProcessModeEnum.Inherit;

        foreach (Node child in node.GetChildren())
        {
            if (child is CollisionShape2D shape)
                shape.Disabled = false;

            if (child is CollisionPolygon2D poly)
                poly.Disabled = false;

            if (child is Area2D area)
            {
                area.Monitoring = true;
                area.Monitorable = true;
            }

            EnableNode(child);
        }
    }
}
