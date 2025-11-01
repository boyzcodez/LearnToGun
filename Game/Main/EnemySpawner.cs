using Godot;
using System.Collections.Generic;

public partial class EnemySpawner : Node2D
{
    [Export] private Node2D ysort;

    [Export] private Enemy[] DifficultyOne = [];
    [Export] private Enemy[] DifficultyTwo = [];
    [Export] private Enemy[] DifficultyThree = [];

    public Dictionary<string, Queue<Entity>> _pools = new();
    public List<Entity> currentEnemies = new();
    private int enemyAmount = 20;
    private int activeEnemies = 0;

    public override void _Ready()
    {
        PreparePool(DifficultyOne);
        EventBus.EnemyDied += OnEnemyDied;
        EventBus.Reset += Reset;
        EventBus.MapSwitch += Reset;
    }


    private void PreparePool(Enemy[] difficulty)
    {
        foreach (var enemy in difficulty)
        {
            var pool = new Queue<Entity>();
            _pools[enemy.name] = pool;

            for (int i = 0; i < enemyAmount; i++)
            {
                var instance = enemy.enemyScene.Instantiate<Entity>();

                instance.name = enemy.name;

                ysort.CallDeferred("add_child", instance);

                pool.Enqueue(instance);
            }
        }
    }
    public void SummonEnemy(Vector2 spot, string enemy)
    {
        if (!_pools.TryGetValue(enemy, out var pool) || pool.Count == 0)
        {
            GD.PrintErr("No Such Enemy");
            return;
        }
        if (EventBus.gameOn) activeEnemies += 1;

        var selected = _pools[enemy].Dequeue();

        currentEnemies.Add(selected);

        selected.GlobalPosition = spot;
        selected.EmitSignal("Activation");
    }
    private void OnEnemyDied()
    {
        activeEnemies -= 1;
        if (activeEnemies == 0) EventBus.TriggerEndOfWave();
    }
    private void Reset()
    {
        activeEnemies = 0;

        foreach (var enemy in currentEnemies)
        {
            enemy.Visible = false;
            _pools[enemy.name].Enqueue(enemy);
        }

        currentEnemies.Clear();
    }
}
