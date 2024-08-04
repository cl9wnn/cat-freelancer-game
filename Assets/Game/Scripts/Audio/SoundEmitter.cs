using Plugins.Audio.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SourceAudio))]
public class SoundEmitter : MonoBehaviour
{
    private AudioSource _audioSource;
    private SourceAudio _source;
    
    private Coroutine _playingCoroutine;

    public SoundData Data { get; private set; }
    public LinkedListNode<SoundEmitter> Node { get; set; }

    private void Awake()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
        _source = gameObject.GetComponent<SourceAudio>();
    }

    public void Initialize(SoundData data)
    {
        Data = data;
        _audioSource.outputAudioMixerGroup = data.mixerGroup;
        _audioSource.loop = data.loop;
        _audioSource.playOnAwake = data.playOnAwake;
        
        _audioSource.mute = data.mute;
        
        _audioSource.rolloffMode = data.rolloffMode;
    }

    public void Play()
    {
        if (_playingCoroutine != null)
        {
            StopCoroutine(_playingCoroutine);
        }

        _source.Play(Data.clip.Key);
        _playingCoroutine = StartCoroutine(WaitForSoundEnd());
    }

    private IEnumerator WaitForSoundEnd()
    {
        yield return new WaitWhile(() => !_audioSource.isPlaying);
        yield return new WaitWhile(() => _audioSource.isPlaying);
        GameSingleton.Instance.SoundManager.ReturnToPool(this);
    }

    public void Stop()
    {
        if (_playingCoroutine != null)
        {
            StopCoroutine(_playingCoroutine);
            _playingCoroutine = null;
        }

        _source.Stop();
        GameSingleton.Instance.SoundManager.ReturnToPool(this);
    }

    public void WithRandomPitch(float min = -0.05f, float max = 0.05f)
    {
        _audioSource.pitch += Random.Range(min, max);
    }
}
