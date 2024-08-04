using UnityEngine;

public class SoundBuilder
{
    private readonly SoundManager _soundManager;
    private SoundData _soundData;
    private Vector3 _position = Vector3.zero;
    private bool _randomPitch;

    public SoundBuilder(SoundManager soundManager)
    {
        _soundManager = soundManager;
    }

    public SoundBuilder WithSoundData(SoundData soundData)
    {
        _soundData = soundData;
        return this;
    }

    public SoundBuilder WithSoundData(SoundEffect soundEffect)
    {
        _soundData = _soundManager.GetFromDatabase(soundEffect);
        return this;
    }

    public SoundBuilder WithSoundData(string soundKey)
    {
        _soundData = _soundManager.GetFromDatabase(soundKey);
        return this;
    }


    public SoundBuilder WithPosition(Vector3 position)
    {
        _position = position;
        return this;
    }

    public SoundBuilder WithRandomPitch()
    {
        _randomPitch = true;
        return this;
    }

    public void Play()
    {
        if (_soundData == null)
        {
            Debug.LogError("SoundData is null");
            return;
        }

        if (!_soundManager.CanPlaySound(_soundData)) return;

        var soundEmitter = _soundManager.Get();
        soundEmitter.Initialize(_soundData);
        soundEmitter.transform.position = _position;
        soundEmitter.transform.parent = _soundManager.transform;

        if (_randomPitch)
        {
            soundEmitter.WithRandomPitch();
        }

        if (_soundData.frequentSound)
        {
            soundEmitter.Node = _soundManager.FrequentSoundEmitters.AddLast(soundEmitter);
        }
        soundEmitter.Play();
    }
}
