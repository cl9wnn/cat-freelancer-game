using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using YG;
using UnityEngine.Analytics;


public class Settings : MonoBehaviour {
    public GameObject settingsPan;
    public Text resumeText;
    public Text volumeText;
    public Text settingsText;
    public Text languageText;
    public Text musicText;
    public Text aboutGameTitleText;
    public AudioSource audioSourceMusic;
    public Sprite onSprite;
    public Sprite offSprite;

    public Canvas StartCanvas;

    public bool IsMuted
    {
        get => audioSourceMusic.mute;
        set
        {
            audioSourceMusic.mute = value;
            buttonImage.sprite = IsMuted ? offSprite : onSprite;
        }
    }
    public Image buttonImage;
    public GameObject aboutGamePan;
    public AudioSource AudioOpenSettings;

    private Plot _plot;

    public void Awake()
    {
        _plot = GameSingleton.Instance.Plot;

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
        YandexGame.savesData.settingsData = new SettingsData(IsMuted);
    }
    private void Load()
    {
        var data = YandexGame.savesData.settingsData;

        if (data == null) return;

        IsMuted = data.isMuted;
    }

    private void Start()
    {
        StartCanvas.gameObject.SetActive(!_plot.isStart);
        if (_plot.isStart) audioSourceMusic.Play();
    }

    public void ChangeLanguage()
    {
        resumeText.text = LanguageSystem.lng.settings[0];
       // authorsText.text = LanguageSystem.lng.settings[1]; // В ЯИ были вырезаны
        volumeText.text = LanguageSystem.lng.settings[3];
        settingsText.text = LanguageSystem.lng.settings[4];
        languageText.text = LanguageSystem.lng.settings[5];
        musicText.text = LanguageSystem.lng.settings[6];
        aboutGameTitleText.text = LanguageSystem.lng.settings[7];
    }
     
    public void ShowAboutGamePan()
    {
        aboutGamePan.SetActive(true);
    }
    public void ExitAboutGamePan()
    {
        aboutGamePan.SetActive(false);

    }
    public void OpenAboutGame()
    {
        aboutGamePan.SetActive(true);

    }
    public void SwitchMusic()
    {
        IsMuted = !IsMuted;
    }
    public void Vka()
    {
     Application.OpenURL ("https://vk.com/cl9wn");
    }
    public void Telegram()
    {
        Application.OpenURL("https://t.me/cl_wn");
    }
    public void GooglePlay()
    {
        //Application.OpenURL("ссылка на игру");
    }
}