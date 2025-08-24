using System.Threading.Tasks;
using Godot;
using Godot.Collections;

public partial class EntityHandler : Node2D
{
    private float _dangerValue = EventBus.dangerValue;
    private int _currentEnemyCount = 0;
    private const int MIN_ENEMIES = 3;
    private int rounds = 0;
    private PackedScene[] _enemyTypes;
    private Array<Entity> enemies = new();

    public override void _Ready()
    {
        EventBus.StartWave += CalculateRounds;
        EventBus.EnemyDied += OnEnemyDied;
        EventBus.Reset += ClearEnemies;
        LoadEnemyTypes();
    }

    private void LoadEnemyTypes()
    {
        _enemyTypes = new PackedScene[]
        {
            GD.Load<PackedScene>("uid://bhb68kqkfragl"),
            GD.Load<PackedScene>("uid://bhb68kqkfragl"),
            GD.Load<PackedScene>("uid://bhb68kqkfragl")
        };
    }

    private async void CalculateRounds()
    {
        await ToSignal(GetTree().CreateTimer(3f), "timeout");

        rounds = Mathf.CeilToInt(3 + _dangerValue * 2);

        SpawnWave();
    }
    private async void SpawnWave()
    {
        rounds--;
        int enemiesToSpawn = Mathf.CeilToInt(4 + _dangerValue * 2);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int enemyType = (int)(_dangerValue / 2) % _enemyTypes.Length;
            var enemy = _enemyTypes[enemyType].Instantiate<Entity>();

            enemies.Add(enemy);

            CallDeferred(MethodName.AddChild, enemy);
            //AddChild(enemy);
            _currentEnemyCount++;

            enemy.Position = new Vector2(
                GD.RandRange(-100, 100),
                GD.RandRange(-100, 100)
            );

            await ToSignal(GetTree().CreateTimer(0.2f), "timeout");
        }
    }

    private void OnEnemyDied()
    {
        _currentEnemyCount--;
        if (rounds <= 0 && _currentEnemyCount <= 0)
        {
            GD.Print("All enemies defeated!");
        }
        else if (_currentEnemyCount <= MIN_ENEMIES && rounds > 0)
        {
            SpawnWave();
        }
    }
    private void ClearEnemies()
    {
        foreach (var entity in enemies)
        {
            if (entity != null)
                entity.QueueFree();
        }
        enemies.Clear();
        _currentEnemyCount = 0;
        rounds = 0;
    }
}
