using Godot;
using System;

[GlobalClass]
public partial class MusicResource : Resource
{
    [Export] public string tag {get;set;}
    [Export] public AudioStream stream {get;set;}
}
