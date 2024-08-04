﻿using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using YG;
using UnityEngine.Analytics;
using UnityEngine.Audio;


public class Settings : MonoBehaviour 
{
    public GameObject settingsPan;
    public Text resumeText;
    public Text volumeText;
    public Text settingsText;
    public Text languageText;
    public Text musicText;
    public Text aboutGameTitleText;


    public Canvas StartCanvas;

    public GameObject aboutGamePan;

    private Plot _plot;

    public void Awake()
    {
        _plot = GameSingleton.Instance.Plot;    
    }

    private void Start()
    {
        StartCanvas.gameObject.SetActive(!_plot.isStart);
    }

    public void ChangeLanguage()
    {
        resumeText.text = LanguageSystem.lng.settings[0];
        volumeText.text = LanguageSystem.lng.settings[3];
        settingsText.text = LanguageSystem.lng.settings[4];
        languageText.text = LanguageSystem.lng.settings[5];
        musicText.text = LanguageSystem.lng.settings[6];
        aboutGameTitleText.text = LanguageSystem.lng.settings[7];
    }
     
    public void ShowAboutGamePan()
    {
        OpenAboutGame();
    }
    public void ExitAboutGamePan()
    {
        aboutGamePan.GetComponent<Image>().enabled = false;

        foreach (Transform child in aboutGamePan.transform)
        {
            child.gameObject.SetActive(false);
        }

    }
    public void OpenAboutGame()
    {
        aboutGamePan.GetComponent<Image>().enabled = true;

        foreach (Transform child in aboutGamePan.transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}