using Godot;
using System;

public partial class Music : AudioStreamPlayer
{
    [Export] MusicResource[] CombatAlbum;
    [Export] MusicResource[] HubAlbum;
    [Export] MusicResource[] Specials;

    public float Volume = 0f;
    private bool isFading = false;

    private int index = 0;

    // public override void _Ready()
    // {
    //     if (Stream == null)
    //         PlayMusic(HubAlbum[0].stream);
        
    //     EventBus.SwitchGameState += SwitchAlbum;
    // }

    public void SwitchAlbum()
    {
        if (EventBus.gameOn)
        {
            index += 1;
            if (index > CombatAlbum.Length - 1) index = 0;
            PlayMusic(CombatAlbum[index].stream);
        } else
        {
            index += 1;
            if (index > CombatAlbum.Length - 1) index = 0;
            PlayMusic(HubAlbum[index].stream);
        }
    }
    public void Next()
    {
        if (EventBus.gameOn)
        {
            index += 1;
            if (index > CombatAlbum.Length - 1) index = 0;
            PlayMusic(CombatAlbum[index].stream);
        } else
        {
            index += 1;
            if (index > CombatAlbum.Length - 1) index = 0;
            PlayMusic(HubAlbum[index].stream);
        }
    }

    public async void PlayMusic(AudioStream music)
    {
        if (isFading)
            return;

        if (music == null || Stream == music)
            return;

        isFading = true;

        float fadeTime = 1.0f;

        // Fade out
        var tween = CreateTween();
        tween.TweenProperty(this, "volume_db", -80, fadeTime)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.InOut);

        await ToSignal(tween, Tween.SignalName.Finished);

        // Change stream & fade in
        Stream = music;
        Play();

        tween = CreateTween();
        VolumeDb = -80;
        tween.TweenProperty(this, "volume_db", Volume, fadeTime)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.InOut);

        await ToSignal(tween, Tween.SignalName.Finished);

        isFading = false;
    }

    private AudioStream GetStreamMusic(string tag, MusicResource[] music)
    {
        if (string.IsNullOrEmpty(tag))
        {
            GD.PrintErr("no tag provided, cannot get sound effect!");
            return null;
        }

        foreach (MusicResource sound in music)
        {
            if (sound.tag == tag)
            {
                return sound.stream;
            }
        }

        return null;
    }
    
}
