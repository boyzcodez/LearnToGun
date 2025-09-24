using Godot;
using System;

[GlobalClass]
public partial class Enemy : Resource
{
    [Export] public string name { get; set; }
    [Export] public PackedScene enemyScene { get; set; }
    [Export] public int value { get; set; } = 1;
}
