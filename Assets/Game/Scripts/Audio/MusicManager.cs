using Plugins.Audio.Core;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private SourceAudio _sourceAudio;
    [SerializeField] private MusicsSO _musicsDatabase;

    public void PlayBackgroundMusic(BackgroundMusic backgroundMusic)
    {
        var music = GetFromDatabase(backgroundMusic);

        _sourceAudio.Volume = music.volume;
        _sourceAudio.Pitch = music.pitch;

        _sourceAudio.Loop = music.loop;
        _sourceAudio.Mute = music.mute;

        _sourceAudio.RolloffMode = music.rolloffMode;
        _sourceAudio.OutputAudioMixerGroup = music.mixerGroup;
        
        music.snapshot.TransitionTo(0.5f);

        if (backgroundMusic == BackgroundMusic.NONE)
            return;

        _sourceAudio.Play(music.clip.Key);
    }

    public MusicData GetFromDatabase(BackgroundMusic backgroundMusic)
    {
        return _musicsDatabase.musics.Find(s => s.type == backgroundMusic);
    }
}
