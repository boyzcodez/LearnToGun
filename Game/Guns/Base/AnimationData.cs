using Godot;
using System;

public partial class AnimationData : Resource
{
    [Export] public Texture2D SpriteSheet {get;set;}
    [Export] public int HorizontalFrames {get;set;}
    [Export] public int VerticalFrames {get;set;}
    [Export] public int FrameRate {get;set;} = 7;
}
