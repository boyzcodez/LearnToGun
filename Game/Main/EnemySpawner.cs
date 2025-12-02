using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class EnemySpawner : Node2D
{
    [Export] private Node2D ysort;

    [Export] private Enemy[] DifficultyOne = [];
    [Export] private Enemy[] DifficultyTwo = [];
    [Export] private Enemy[] DifficultyThree = [];

    public Dictionary<string, Queue<Entity>> _pools = new();
    public List<Entity> currentEnemies = new();
    private int enemyAmount = 15;
    private int activeEnemies = 0;

    private RandomNumberGenerator rng = new RandomNumberGenerator();
    private List<Vector2> spawnpoints;
    private List<int> enemiesPerRound = new ();
    private int currentRound = 0;
    private int rounds = 0;

    


    public override void _Ready()
    {
        PreparePool(DifficultyOne);

        EventBus.EnemyDied += OnEnemyDied;
        EventBus.Reset += FullReset;
        EventBus.MapSwitch += Reset;

        EventBus.StartRound += CalcRound;

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



    private void CalcRound(List<Vector2> spots)
    {
        int room = EventBus.room;
        currentRound = 0;

        spawnpoints = spots;

        int totalEnemies = room * 3 + 5;

        int maxPerRound = Mathf.Max(1, spots.Count);

        rounds = Mathf.CeilToInt((float)totalEnemies / maxPerRound);

        enemiesPerRound = new List<int>();
        int remaining = totalEnemies;

        for (int i = 0; i < rounds; i++)
        {
            int spawnThisRound = Mathf.Min(maxPerRound, remaining);
            enemiesPerRound.Add(spawnThisRound);
            remaining -= spawnThisRound;
        }

        BeginRound();
    }

    private void BeginRound()
    {
        activeEnemies = 0;
        rng.Randomize();

        int enemyCount = enemiesPerRound[currentRound];

        List<Vector2> availableSpots = new List<Vector2>(spawnpoints);
        availableSpots = availableSpots.OrderBy(_ => rng.Randi()).ToList();

        List<Enemy> weightedPool = BuildWeightedPool();

        for (int i = 0; i < enemyCount; i++)
        {
            if (availableSpots.Count == 0) break;

            int index = rng.RandiRange(0, availableSpots.Count - 1);
            Vector2 spot = availableSpots[index];
            availableSpots.RemoveAt(index);

            Enemy chosen = weightedPool[rng.RandiRange(0, weightedPool.Count - 1)];

            SummonEnemy(spot, chosen.name);
            activeEnemies += 1;
        }

        currentRound++;
    }
    
    private List<Enemy> BuildWeightedPool()
    {
        List<Enemy> pool = new List<Enemy>();

        foreach (Enemy e in DifficultyOne)
        {
            int weight = Mathf.Max(1, 10 - e.value);

            for (int i = 0; i < weight; i++)
                pool.Add(e);
        } 

        return pool;
    }


    public void SummonEnemy(Vector2 spot, string enemy)
    {
        if (!_pools.TryGetValue(enemy, out var pool) || pool.Count == 0)
        {
            GD.PrintErr("No Such Enemy");
            return;
        }

        var selected = _pools[enemy].Dequeue();

        currentEnemies.Add(selected);

        selected.GlobalPosition = spot;
        selected.EmitSignal("Activation");
    }
    private void OnEnemyDied()
    {
        activeEnemies -= 1;

        GD.Print(activeEnemies);

        if (currentRound >= rounds && activeEnemies <= 0) EventBus.TriggerEndOfRound();
        else if  (activeEnemies == 0) BeginRound();
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
    private void FullReset()
    {
        activeEnemies = 0;

        foreach (var enemy in currentEnemies)
        {
            enemy.Visible = false;
            enemy.EmitSignal("Deactivation");
            _pools[enemy.name].Enqueue(enemy);
        }

        currentEnemies.Clear();
    }
}
