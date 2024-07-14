using System;
using Unity.Services.Core;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class GameSingleton : MonoBehaviour
{
    private static GameSingleton _instance;
    public static GameSingleton Instance => ReturnObject(ref _instance);

    private Game _game;
    public Game Game => ReturnObject(ref _game);
    
    private Boost _boost;
    public Boost Boost => ReturnObject(ref _boost);

    private Fortune _fortune;
    public Fortune Fortune => ReturnObject(ref _fortune);

    private ObjectPool _pool;
    public ObjectPool Pool => ReturnObject(ref _pool);

    private static T ReturnObject<T>(ref T component) where T : Component
    {
        if (!component) component = FindAnyObjectByType<T>();
        return component;
    }
}
