using Godot;
using System.Collections.Generic;

public partial class BulletPool : Node
{
    private Dictionary<string, Queue<Bullet>> _pools = new();
    private Dictionary<string, PackedScene> _sceneMap = new();
    private Dictionary<string, GunData> _data = new();

}
