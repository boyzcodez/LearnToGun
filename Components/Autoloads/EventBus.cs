using Godot;
using System;

public static class EventBus
{
    public static event Action PlayerDied;
    public static event Action<int> GainExp;

    public static void Reset() =>
        PlayerDied?.Invoke();
    public static void Exp(int amount) =>
        GainExp?.Invoke(amount);
}
