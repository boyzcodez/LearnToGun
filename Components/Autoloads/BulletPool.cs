using Godot;
using System.Collections.Generic;

public partial class BulletPool : Node
{
    private Dictionary<string, Queue<Bullet>> _pools = new();
    private List<Bullet> _enemyBullets = new();

    public override void _Ready()
    {
        EventBus.ClearBullets += ClearBullets;
    }

    public void PreparePool(string key, GunData gunData, int amount)
    {
        if (_pools.TryGetValue(key, out var pool))
        {
            if (pool.Count > 0 && pool.Peek().SceneFilePath != gunData.BulletScene.ResourcePath)
            {
                foreach (var bullet in pool)
                {
                    bullet.QueueFree();
                    if (_enemyBullets.Contains(bullet)) _enemyBullets.Remove(bullet);
                }
                pool.Clear();
            }
        }
        else
        {
            pool = new Queue<Bullet>();
            _pools[key] = pool;
        }


        for (int i = pool.Count; i < amount; i++)
        {
            var bullet = gunData.BulletScene.Instantiate<Bullet>();
            bullet.Init(new DamageData(gunData.Damage, gunData.Knockback), key, gunData.BulletSpeed, this);
            CallDeferred("add_child", bullet);
            pool.Enqueue(bullet);

            if (gunData.isEnemy) _enemyBullets.Add(bullet);
        }
    }
    public Bullet GetBullet(string key, GunData gunData)
    {
        if (!_pools.TryGetValue(key, out var pool) || pool.Count == 0)
        {
            GD.PrintErr("No Bullets to use");
            PreparePool(key, gunData, 1);
        }

        var bullet = _pools[key].Dequeue();
        //bullet.Visible = true;
        //UniversalStopButton.EnableNode(bullet);
        return bullet;
    }
    public void ReturnBullet(string key, Bullet bullet)
    {
        //bullet.Visible = false;
        //UniversalStopButton.DisableNode(bullet);
        _pools[key].Enqueue(bullet);
    }
    //     public void DeleteBullets(string oldKey)
    // {
    //     if (_pools.ContainsKey(oldKey))
    //     {
    //         foreach (var bullet in _pools[oldKey])
    //             bullet.QueueFree();

    //         _pools.Remove(oldKey);
    //     }

    //     // Reset the enemy bullet list safely
    //     _enemyBullets.RemoveAll(b => b.PoolKey == oldKey);
    // }
    public void ClearBullets()
    {
        EventBus.TriggerScreenShake(0.5f);

        foreach (var bullet in _enemyBullets)
        {
            if (IsInstanceValid(bullet) && bullet.Active)
                bullet.Deactivate();
        }
    }

}
