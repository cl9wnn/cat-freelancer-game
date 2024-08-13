using UnityEngine;
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
    public Text saveDataText;

    public Canvas StartCanvas;

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
        saveDataText.text = LanguageSystem.lng.settings[0];
        resumeText.text = LanguageSystem.lng.settings[8];
        volumeText.text = LanguageSystem.lng.settings[3];
        settingsText.text = LanguageSystem.lng.settings[4];
        languageText.text = LanguageSystem.lng.settings[5];
        musicText.text = LanguageSystem.lng.settings[6];
        aboutGameTitleText.text = LanguageSystem.lng.settings[7];
    }
}