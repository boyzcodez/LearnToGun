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
    private int enemyAmount = 5;
    private int activeEnemies = 0;

    public override void _Ready()
    {
        PreparePool(DifficultyOne);
        EventBus.EnemyDied += OnEnemyDied;
        EventBus.Reset += Reset;
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
                instance.SetDeferred("process_mode", (int)Node.ProcessModeEnum.Disabled);
                instance.Hide();

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

        if (!currentEnemies.Contains(selected)) currentEnemies.Add(selected);

        selected.GlobalPosition = spot;
        selected.SetDeferred("process_mode", (int)Node.ProcessModeEnum.Inherit);
        selected.Show();
    }
    private void OnEnemyDied(string name, Entity enemy)
    {
        if (EventBus.gameOn) activeEnemies -= 1;

        enemy.SetDeferred("process_mode", (int)Node.ProcessModeEnum.Disabled);
        enemy.Hide();
        enemy.GlobalPosition = new Vector2(500, 0);
        _pools[name].Enqueue(enemy);

        if (currentEnemies.Contains(enemy)) currentEnemies.Remove(enemy);

        if (activeEnemies <= 0 && EventBus.gameOn) EventBus.TriggerEndOfWave();
    }
    private void Reset()
    {
        activeEnemies = 0;
    }
}
