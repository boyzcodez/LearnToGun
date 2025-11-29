using Godot;
using System;

[GlobalClass]
public partial class MusicResource : Resource
{
    [Export] string tag {get;set;}
    [Export] AudioStream stream {get;set;}
}
