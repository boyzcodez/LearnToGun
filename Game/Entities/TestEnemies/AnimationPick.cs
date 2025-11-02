using Godot;
using System;

public static class AnimationPick
{
    public struct AnimationResult
    {
        public string AnimationName;
        public bool FlipH;
        public bool ShowBehindParent;
    }

    public static AnimationResult GetAnimationFromRotation(float rotation, DirectionMode mode)
    {
        rotation = Mathf.PosMod(rotation, Mathf.Tau);
        float deg = Mathf.RadToDeg(rotation);

        string anim = "Front";
        bool flipH = false;
        bool showBehindParent = false;

        if (mode == DirectionMode.TwoDirections)
        {
            // Front (down) = 0–180°, Back (up) = 180–360°
            if (deg >= 0 && deg < 180)
                anim = "Front";
            else
                anim = "Back";

            // Flip horizontally if facing left (90–270°)
            if (deg >= 90 && deg < 270)
                flipH = true;

            // Show gun behind parent when facing back
            showBehindParent = anim == "Back";
        }
        else // FourDirections
        {
            // Quadrants based on your rotation setup:
            // RightFront = 315–45°
            // Front = 45–135°
            // RightBack = 135–225°
            // Back = 225–315°

            if (deg >= 45 && deg < 135)
            {
                anim = "Front";
            }
            else if (deg >= 135 && deg < 225)
            {
                anim = "RightBack";
            }
            else if (deg >= 225 && deg < 315)
            {
                anim = "Back";
            }
            else // (deg >= 315 or deg < 45)
            {
                anim = "RightFront";
            }

            // Flip for left-facing angles (90–270°)
            if (deg >= 90 && deg < 270)
                flipH = true;

            // Show gun behind parent when facing away
            showBehindParent = anim == "Back" || anim == "RightBack";
        }

        return new AnimationResult
        {
            AnimationName = anim,
            FlipH = flipH,
            ShowBehindParent = showBehindParent
        };
    }
}
