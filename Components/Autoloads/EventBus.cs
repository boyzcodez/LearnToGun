using System;
using System.Collections.Generic;
using System.Numerics;

public static class EventBus
{
    // Player / Game Stuff
    public static event Action Reset;
    public static event Action Lock;
    public static event Action ClearBullets;
    public static event Action<float> ScreenShake;
    public static event Action SwitchGameState;

    // Map and room stuff
    public static event Action Transition;
    public static event Action MapSwitch;

    // Enemy and wave stuff
    public static event Action StartWave;
    public static event Action EndWave;
    public static event Action EndRound;
    public static event Action EnemyDied;
    public static event Action StartRound;

    // Numbers for ui and level ups
    public static event Action<int> GainExp;
    public static event Action<int> GainMoney;
    public static event Action<int, int> UpdateAmmo;
    public static event Action<int, int> UpdateHealth;

    
    // Game tracker and stuff
    public static int dangerValue = 0;
    public static int room = 0;
    public static bool gameOn = false;


    // Player and game stuff
    public static void PlayerDied() =>
        Reset?.Invoke();
    public static void TriggerScreenShake(float amount) =>
        ScreenShake?.Invoke(amount);
    public static void TriggerClearBullets() =>
        ClearBullets?.Invoke();
    public static void TriggerSwitchGameState()
    {
        if (gameOn == true) gameOn = false;
        else gameOn = true;

        SwitchGameState?.Invoke();
    }
        


    // Map and room stuff
    public static void TriggerLock() =>
        Lock?.Invoke();
    public static void TriggerTransition() =>
        Transition?.Invoke();
    public static void TriggerMapSwitch() =>
        MapSwitch?.Invoke();
    
    
    // Enemy and Waves
    public static void OnEnemyDied() =>
        EnemyDied?.Invoke();
    public static void TriggerRound() =>
        StartRound?.Invoke();
    public static void TriggerWave() =>
        StartWave?.Invoke();    
    public static void TriggerEndOfRound() =>
        EndRound?.Invoke();
    public static void TriggerEndOfWave() =>
        EndWave?.Invoke();


    // UI
    public static void Ammo(int current, int max) =>
        UpdateAmmo?.Invoke(current, max);
    public static void Money(int amount) =>
        GainMoney?.Invoke(amount);
    public static void Exp(int amount) =>
        GainExp?.Invoke(amount);   
    public static void Health(int current, int max) =>
        UpdateHealth?.Invoke(current, max); 
}
