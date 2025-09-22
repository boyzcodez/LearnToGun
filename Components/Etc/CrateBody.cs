using Godot;
using System;

[GlobalClass]
public partial class CrateBody : Area2D
{
    public void Break()
    {
        GD.Print("this should break");
    }
}
