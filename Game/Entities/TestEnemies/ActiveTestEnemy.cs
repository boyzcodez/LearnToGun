using Godot;

public partial class ActiveTestEnemy : Entity
{
    private StateMachine stateMachine;
    private Hurtbox hurtbox;
    private Node2D neccissary;

    public override void _Ready()
    {
        neccissary = GetNode<Node2D>("Hide");

        stateMachine = GetNode<StateMachine>("StateMachine");
        hurtbox = GetNode<Hurtbox>("Hurtbox");

        neccissary.Visible = true;
    }

    public void Activate()
    {
        neccissary.Visible = true;
        SetDeferred("process_mode", (int)Node.ProcessModeEnum.Inherit);
    }
    public void Deactivate()
    {
        neccissary.Visible = false;
        SetDeferred("process_mode", (int)Node.ProcessModeEnum.Disabled);
    }

}
