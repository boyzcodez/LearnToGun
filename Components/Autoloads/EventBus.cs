using System;

public static class EventBus
{
    public static event Action Reset;
    public static event Action Lock;
    public static event Action Transition;
    public static event Action MapSwitch;
    public static event Action StartWave;
    public static event Action EnemyDied;
    public static event Action<int> GainExp;
    public static event Action<float> ScreenShake;

    public static int dangerValue = 0;

    public static void PlayerDied()
    {
        Reset?.Invoke();
    }
    public static void Exp(int amount) =>
        GainExp?.Invoke(amount);
    public static void TriggerScreenShake(float amount) =>
        ScreenShake?.Invoke(amount);
    public static void TriggerLock() =>
        Lock?.Invoke();
    public static void TriggerTransition() =>
        Transition?.Invoke();
    public static void TriggerMapSwitch() =>
        MapSwitch?.Invoke();
    public static void TriggerWave() =>
        StartWave?.Invoke();
        public static void OnEnemyDied() =>
        EnemyDied?.Invoke();
}
