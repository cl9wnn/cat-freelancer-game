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

    public bool loop;
    public bool playOnAwake;
    public bool frequentSound;

    public bool mute;

    public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;
}
