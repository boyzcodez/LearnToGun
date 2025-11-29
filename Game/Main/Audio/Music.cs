using Godot;
using System;

public partial class Music : AudioStreamPlayer
{
    [Export] MusicResource[] CombatAlbum;
    [Export] MusicResource[] HubAlbum;
    [Export] MusicResource[] Specials;
}
