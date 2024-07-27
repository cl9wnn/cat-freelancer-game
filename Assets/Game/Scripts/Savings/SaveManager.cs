using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class SaveManager : MonoBehaviour
{
    public static event Action OnSaveEvent;
    public static event Action OnLoadEvent;

    private void OnEnable()
    {
        YandexGame.GetDataEvent += GetLoad;
    }
    private void OnDisable()
    {
        YandexGame.GetDataEvent -= GetLoad;
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
