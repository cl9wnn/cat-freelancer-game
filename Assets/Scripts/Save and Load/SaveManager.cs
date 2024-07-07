using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class SaveManager : MonoBehaviour
{
    private static SaveManager instance;
    public static SaveManager Instance => ReturnObject(ref instance);

    public static event Action OnSaveEvent;
    public static event Action OnLoadEvent;

    private void OnEnable() => YandexGame.GetDataEvent += GetLoad;
    private void OnDisable() => YandexGame.GetDataEvent -= GetLoad;

    private static T ReturnObject<T>(ref T component) where T : Component
    {
        if (!component) component = FindAnyObjectByType<T>();
        return component;
    }

    private void Awake()
    {
        if (YandexGame.SDKEnabled)
            GetLoad();
    }

    public void Save()
    {
        OnSaveEvent?.Invoke();

        YandexGame.SaveProgress();
    }

    public void ResetSaves()
    {
        YandexGame.ResetSaveProgress();
        YandexGame.SaveProgress();
    }
    public void GetLoad()
    {
        OnLoadEvent?.Invoke();
    }
}
