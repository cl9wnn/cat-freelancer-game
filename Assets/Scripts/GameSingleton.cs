﻿using System;
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

    private SaveManager _saveManager;
    public SaveManager SaveManager => ReturnObject(ref _saveManager);

    private NavigationBar _navigationManager;
    public NavigationBar NavigationManager => ReturnObject(ref _navigationManager);

    private Achievements _achievements;
    public Achievements Achievements => ReturnObject(ref _achievements);

    private Settings _settings;
    public Settings Settings => ReturnObject(ref _settings);

    private SpawnDown _spawnDown;
    public SpawnDown SpawnDown => ReturnObject(ref _spawnDown);

    private LanguageSystem _languageSystem;
    public LanguageSystem LanguageSystem => ReturnObject(ref _languageSystem);

    private Plot _plot;
    public Plot Plot => ReturnObject(ref _plot);

    private Timer _timer;
    public Timer Timer => ReturnObject(ref _timer);

    private Stats _stats;
    public Stats Stats => ReturnObject(ref _stats);

    private SkinPC _skins;
    public SkinPC Skins => ReturnObject(ref _skins);

    private static T ReturnObject<T>(ref T component) where T : Component
    {
        if (!component) component = FindAnyObjectByType<T>(FindObjectsInactive.Include);
        return component;
    }
}
