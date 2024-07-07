using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using YG;
public class Stats : MonoBehaviour
{
    public Text[] stringNames;
    public Text[] statsText;
    public Text totalTimeText;
    private float totalPlayTime = 0f;
    private float sessionStartTime;


    private void Awake()
    {
        if (YandexGame.SDKEnabled)
            Load();
    }
    private void OnEnable()
    {
        SaveManager.OnSaveEvent += Save;
        SaveManager.OnLoadEvent += Load;
    }
    private void OnDisable()
    {
        SaveManager.OnSaveEvent -= Save;
        SaveManager.OnLoadEvent -= Load;
    }
   
    private void Save()
    {
        YandexGame.savesData.statsData = new StatsData(totalPlayTime);
    }
    private void Load()
    {
        var data = YandexGame.savesData.statsData;

        if (data == null) return;

        totalPlayTime = data.totalPlayTime;
    }


    void Start()
    {
        sessionStartTime = Time.time;
    }
    void Update()
    {
        if ((int)totalPlayTime < 60)
        {
            totalTimeText.text = (int)totalPlayTime + LanguageSystem.lng.time[7];
        }
        else if ((int)totalPlayTime < 3600)
        {
            totalTimeText.text = (int)(totalPlayTime / 60) + LanguageSystem.lng.time[2];
        }
        else totalTimeText.text = (int)(totalPlayTime / 3600) + LanguageSystem.lng.time[0];

        if (Application.isFocused)
        {
            float sessionTime = Time.time - sessionStartTime;
            totalPlayTime += sessionTime;
            sessionStartTime = Time.time; 
        }
    }
    public void ChangeLanguage()
    {
        for (int i = 0; i < stringNames.Length; i++)
        {
            stringNames[i].text = LanguageSystem.lng.statsString[i];
        }
        
        for (int i = 0; i < 3; i++)
        {
            statsText[i].text = LanguageSystem.lng.statsText[i];
        }
    }
}
