using Godot;

[GlobalClass]
public abstract partial class BaseHitEffect : Resource
{
    public abstract void Initialize();
    public abstract void Trigger();
}
