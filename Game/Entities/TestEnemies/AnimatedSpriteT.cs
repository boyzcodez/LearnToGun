using System;

public partial class AnimatedSpriteT : AnimatedSprite
{
    string[] hits = { "HitLeft", "HitRight"};
    Random random;
    public override void _Ready()
    {
        random = new Random();
    }

    public override void PlayAnimation(string animation = "", int value = 0)
    {
        int index = random.Next(hits.Length);
        Play(hits[index]);
    }
}
