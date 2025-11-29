using Godot;
using System;

[GlobalClass]
public partial class MusicData : Node
{
    [Export] string tag {get;set;}
    [Export] AudioStream stream {get;set;}
}
