using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System.Linq;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public readonly LinkedList<SoundEmitter> FrequentSoundEmitters = new();
    private readonly List<SoundEmitter> _activeSoundEmitters = new();
    
    private IObjectPool<SoundEmitter> _soundEmitterPool;

    [SerializeField] private SoundEmitter _soundEmitterPrefab;
    [SerializeField] private bool _collectionCheck = true;
    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] private int _maxPoolSize = 100;
    [SerializeField] private int _maxSoundInstances = 30;

    [SerializeField] private SoundsSO _soundsDatabase;

    private void Start()
    {
        InitializePool();
    }

    public SoundBuilder CreateSound() => new SoundBuilder(this);

    public bool CanPlaySound(SoundData data)
    {
        if (!data.frequentSound) return true;

        if (FrequentSoundEmitters.Count >= _maxSoundInstances)
        {
            try
            {
                FrequentSoundEmitters.First.Value.Stop();
                return true;
            }
            catch (Exception)
            {
                Debug.Log("SoundEmitter is already released");
            }

            return false;
        }

        return true;
    }

    public SoundEmitter Get()
    {
        return _soundEmitterPool.Get();
    }

    public void ReturnToPool(SoundEmitter soundEmitter)
    {
        _soundEmitterPool.Release(soundEmitter);
    }

    public void StopAll()
    {
        foreach (var soundEmitter in _activeSoundEmitters)
        {
            soundEmitter.Stop();
        }

        FrequentSoundEmitters.Clear();
    }

    public SoundData GetFromDatabase(SoundEffect soundEffect)
    {
        return _soundsDatabase.sounds.Find(s => s.type == soundEffect);
    }

    public SoundData GetFromDatabase(string soundKey)
    {
        return _soundsDatabase.sounds.Find(s => s.clip.Key == soundKey);
    }

    private void InitializePool()
    {
        _soundEmitterPool = new ObjectPool<SoundEmitter>
        (
            CreateSoundEmitter,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            _collectionCheck,
            _defaultCapacity,
            _maxPoolSize
        );
    }

    private SoundEmitter CreateSoundEmitter()
    {
        var soundEmitter = Instantiate(_soundEmitterPrefab);
        soundEmitter.gameObject.SetActive(false);
        return soundEmitter;
    }
    private void OnTakeFromPool(SoundEmitter soundEmitter)
    {
        soundEmitter.gameObject.SetActive(true);
        _activeSoundEmitters.Add(soundEmitter);
    }
    private void OnReturnedToPool(SoundEmitter soundEmitter)
    {
        if (soundEmitter.Node != null)
        {
            FrequentSoundEmitters.Remove(soundEmitter.Node);
            soundEmitter.Node = null;
        }
        soundEmitter.gameObject.SetActive(false);
        _activeSoundEmitters.Remove(soundEmitter);
    }
    private void OnDestroyPoolObject(SoundEmitter soundEmitter)
    {
        Destroy(soundEmitter.gameObject);
    }
}