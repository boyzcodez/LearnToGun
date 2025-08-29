using Godot;

[GlobalClass]
public partial class AnimatedSprite : AnimatedSprite2D
{
    private string Running = "";
    private Node Direction;
    private string directionString;
    public int animationValue = 0;

    public override void _Ready()
    {
        Direction = GetNode<Node>("Direction");
    }

    public override void _PhysicsProcess(double delta)
    {
        Physics(delta);
    }
    public virtual void Physics(double delta)
    {
        Godot.Vector2 direction = Input.GetVector("left", "right", "up", "down");
        if (direction != Godot.Vector2.Zero)
        {
            Running = "Run";
        }
        else
        {
            Running = "";
        }

        // Get the mouse position and calculate the direction based on the angle
        // And set which side of the six sided character is shown
        Godot.Vector2 mouse = GetLocalMousePosition();
        int a = (int)(Mathf.Snapped(mouse.Angle(), Mathf.Pi / 4.0f) / (Mathf.Pi / 4.0f));
        a = Mathf.Wrap(a, 0, 8);

        directionString = (string)Direction.Call("GetDirection", a);
        PlayAnimation(Running, 1);
    }
    public virtual void PlayAnimation(string animation = "", int value = 0)
    {
        if (value >= animationValue)
        {
            animationValue = value;
            Play(directionString + animation);
        }
            
    }
    private void _on_animation_finished()
    {
        animationValue = 0;
    }
}
