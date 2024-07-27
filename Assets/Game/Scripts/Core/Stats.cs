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
    private void Update()
    {
        if (Application.isFocused)
        {
            totalPlayTime += Time.deltaTime;
            
            totalTimeText.text = FormatTime(totalPlayTime);
        }
    }
    private string FormatTime(float totalTimeInSeconds)
    {
        int totalSeconds = (int)totalTimeInSeconds;
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        if (hours > 0)
        {
            return $"{hours} {LanguageSystem.lng.time[0]} {minutes} {LanguageSystem.lng.time[2]}";
        }
        else if (minutes > 0)
        {
            return $"{minutes} {LanguageSystem.lng.time[2]} {seconds} {LanguageSystem.lng.time[7]}";
        }
        else
        {
            return $"{seconds} {LanguageSystem.lng.time[7]}";
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
