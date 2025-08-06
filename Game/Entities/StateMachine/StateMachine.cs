using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class StateMachine : Node
{
    private State currentState;
    private Dictionary<string, State> states = new();
    [Export] private State initialState;
    [Export] private bool Active = false;

    public override void _Ready()
    {
        if (Active != true)
            SetInactive();

        foreach (State child in GetChildren())
        {

            string key = child.Name.ToString().ToLower();
            states[key] = child;

            child.Connect("Transitioned", new Callable(this, nameof(OnChildTransition)));

        }

        if (initialState != null)
        {
            initialState.Enter();
            currentState = initialState;
        }
    }
    public override void _Process(double delta)
    {
        currentState?.Update(delta);
    }
    public override void _PhysicsProcess(double delta)
    {
        currentState?.PhysicsProcess(delta);
    }
    private void OnChildTransition(State state, string newStateName)
    {
        if (state != currentState)
            return;

        string key = newStateName.ToLower();
        if (!states.TryGetValue(key, out State newState))
            return;

        currentState?.Exit();
        newState.Enter();
        currentState = newState;
        
    }
    public void SetActive()
    {
        SetProcess(true);
        SetPhysicsProcess(true);
    }
    public void SetInactive()
    {
        SetProcess(false);
        SetPhysicsProcess(false);
    }
}
