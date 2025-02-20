using Plugins.Audio.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SourceAudio))]
public class SoundEmitter : MonoBehaviour
{
    private SourceAudio _sourceAudio;
    
    private Coroutine _playingCoroutine;

    public SoundData Data { get; private set; }
    public LinkedListNode<SoundEmitter> Node { get; set; }

    private void Awake()
    {
        _sourceAudio = gameObject.GetComponent<SourceAudio>();
    }

    public void Initialize(SoundData data)
    {
        Data = data;

        _sourceAudio.Volume = data.volume;
        _sourceAudio.Pitch = data.pitch;

        _sourceAudio.Loop = data.loop;
        _sourceAudio.Mute = data.mute;

        _sourceAudio.RolloffMode = data.rolloffMode;
        _sourceAudio.OutputAudioMixerGroup = data.mixerGroup;
    }

    public void Play()
    {
        if (_playingCoroutine != null)
        {
            StopCoroutine(_playingCoroutine);
        }

        _sourceAudio.Play(Data.clip.Key);
        _playingCoroutine = StartCoroutine(WaitForSoundEnd());
    }

    private IEnumerator WaitForSoundEnd()
    {
        yield return new WaitWhile(() => !_sourceAudio.IsPlaying);
        yield return new WaitWhile(() => _sourceAudio.IsPlaying);
        GameSingleton.Instance.SoundManager.ReturnToPool(this);
    }

    public void Stop()
    {
        if (_playingCoroutine != null)
        {
            StopCoroutine(_playingCoroutine);
            _playingCoroutine = null;
        }

        _sourceAudio.Stop();
        GameSingleton.Instance.SoundManager.ReturnToPool(this);
    }

    public void WithRandomPitch(float min = -0.05f, float max = 0.05f)
    {
        _sourceAudio.Pitch += Random.Range(min, max);
    }
}
