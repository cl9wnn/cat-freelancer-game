using Plugins.Audio.Core;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private SourceAudio _sourceAudio;
    [SerializeField] private MusicsSO _music;

    public void PlayBackgroundMusic(BackgroundMusic backgroundMusic)
    {
        var music = GetFromDatabase(backgroundMusic);

        _sourceAudio.Volume = music.volume;
        _sourceAudio.Pitch = music.pitch;

        _sourceAudio.Loop = music.loop;
        _sourceAudio.Mute = music.mute;

        _sourceAudio.RolloffMode = music.rolloffMode;
        _sourceAudio.AudioMixerGroup = music.mixerGroup;
        
        music.snapshot.TransitionTo(0.5f);

        _sourceAudio.Play(music.clip.Key);
    }

    public MusicData GetFromDatabase(BackgroundMusic backgroundMusic)
    {
        return _music.musics.Find(s => s.type == backgroundMusic);
    }
}
