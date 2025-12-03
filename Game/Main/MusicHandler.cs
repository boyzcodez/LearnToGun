using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class MusicHandler : Node
{
    [ExportCategory("Albums")]
    [Export] public AudioStream[] HubAlbum;
    [Export] public AudioStream[] CombatAlbum;

    [ExportCategory("Settings")]
    [Export] public float FadeTime = 2.0f;
    [Export] public float VolumeDb = 0.0f;

    private AudioStreamPlayer _playerA;
    private AudioStreamPlayer _playerB;
    private AudioStreamPlayer _currentPlayer;
    private AudioStream[] _currentAlbum;
    private Random _rng = new Random();

    private int _lastSongIndex = -1; // prevents repetition

    public enum AlbumType
    {
        Hub,
        Combat
    }

    public AlbumType CurrentAlbumType { get; private set; }

    public override void _Ready()
    {
        _playerA = GetNode<AudioStreamPlayer>("PlayerA");
        _playerB = GetNode<AudioStreamPlayer>("PlayerB");

        _playerA.VolumeDb = -80;
        _playerB.VolumeDb = -80;
        
        _currentPlayer = _playerA;

        SetAlbum(AlbumType.Hub);
        PlayNextSong();

        EventBus.SwitchGameState += CheckState;
    }
    private void CheckState()
    {
        if (EventBus.gameOn)
        {
            SwitchAlbum(AlbumType.Combat);
        }
        else
        {
            SwitchAlbum(AlbumType.Hub);
        }
    }

    private AudioStream GetRandomSong()
    {
        if (_currentAlbum.Length == 0) return null;

        int index;
        do
        {
            index = _rng.Next(_currentAlbum.Length);
        } 
        while (index == _lastSongIndex && _currentAlbum.Length > 1);

        _lastSongIndex = index;
        return _currentAlbum[index];
    }

    public async void PlayNextSong()
    {
        AudioStream nextSong = GetRandomSong();
        if (nextSong == null) return;

        AudioStreamPlayer nextPlayer = (_currentPlayer == _playerA) ? _playerB : _playerA;
        nextPlayer.Stream = nextSong;
        nextPlayer.VolumeDb = -80;
        nextPlayer.Play();

        await FadeCross(_currentPlayer, nextPlayer);
        _currentPlayer = nextPlayer;

        _ = MonitorSongEnd();
    }

    private async Task MonitorSongEnd()
    {
        // Smooth monitored fade trigger
        while (_currentPlayer.Playing)
        {
            float pos = _currentPlayer.GetPlaybackPosition();
            float duration = (float)_currentPlayer.Stream.GetLength();

            // start early fade if near end
            if (duration - pos <= FadeTime + 0.5f)
            {
                PlayNextSong();
                return;
            }

            await ToSignal(GetTree().CreateTimer(0.25f), "timeout");
        }
    }

    private async Task FadeCross(AudioStreamPlayer from, AudioStreamPlayer to)
    {
        float step = FadeTime / 40f;
        for (int i = 0; i < 40; i++)
        {
            from.VolumeDb = Mathf.Lerp(VolumeDb, -80f, i / 40f);
            to.VolumeDb = Mathf.Lerp(-80f, VolumeDb, i / 40f);
            await ToSignal(GetTree().CreateTimer(step), "timeout");
        }

        from.Stop();
        from.VolumeDb = -80;
        to.VolumeDb = VolumeDb;
    }

    private void SetAlbum(AlbumType album)
    {
        CurrentAlbumType = album;
        _currentAlbum = album switch
        {
            AlbumType.Hub => HubAlbum,
            AlbumType.Combat => CombatAlbum,
            _ => HubAlbum
        };

        _lastSongIndex = -1;
    }

    public async void SwitchAlbum(AlbumType album)
    {
        if (album == CurrentAlbumType)
            return;

        SetAlbum(album);

        AudioStreamPlayer oldPlayer = _currentPlayer;

        AudioStreamPlayer nextPlayer = (_currentPlayer == _playerA) ? _playerB : _playerA;
        nextPlayer.Stream = GetRandomSong();
        nextPlayer.VolumeDb = -80;
        nextPlayer.Play();

        await FadeCross(oldPlayer, nextPlayer);
        _currentPlayer = nextPlayer;
    }
}
