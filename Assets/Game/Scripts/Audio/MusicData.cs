using Plugins.Audio.Utils;
using System;
using UnityEngine.Audio;
using UnityEngine;

[Serializable]
public class MusicData
{
    public AudioDataProperty clip;
    public BackgroundMusic type;

    public AudioMixerGroup mixerGroup;
    public AudioMixerSnapshot snapshot;

    [Range(0f, 1f)] public float volume;
    [Range(0f, 3f)] public float pitch;

    public bool loop;
    public bool mute;

    public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;
}