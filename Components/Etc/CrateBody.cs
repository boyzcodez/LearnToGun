using Godot;
using System;

[GlobalClass]
public partial class CrateBody : StaticBody2D
{
    public void Break()
    {
        GD.Print("this should break");
    }
}
