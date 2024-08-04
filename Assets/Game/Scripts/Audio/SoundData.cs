using Plugins.Audio.Utils;
using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class SoundData
{
    public AudioDataProperty clip;
    public SoundEffect type;

    public AudioMixerGroup mixerGroup;

    [Range(0f, 1f)] public float volume;
    [Range(0f, 3f)] public float pitch;

    public bool loop;
    public bool mute;
    public bool frequentSound;

    public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;
}
